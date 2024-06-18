﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TripPlanner.Server.Messages;
using TripPlanner.Server.Models;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IResponseService _responseService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IAuthService authService, IEmailService emailService, ILogger<AuthController> logger, IConfiguration configuration, IResponseService responseService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _emailService = emailService;
            _logger = logger;
            _configuration = configuration;
            _responseService = responseService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString(), "Registration errors");
                return BadRequest(ModelState);
            }

            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation(ResponseMessages.UserAlreadyLoggedIn);
                return _responseService.Get(ResponseMessages.UserAlreadyLoggedIn, StatusCodes.Status400BadRequest);
            }
            if(!await _emailService.IsEmailValid(model.Email))
            {
                _logger.LogInformation("Email is not valid: {ModelEmail}", model.Email);
                return _responseService.Get(ResponseMessages.InvalidEmail, StatusCodes.Status400BadRequest);
            }

            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                try
                {
                    var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = $"{_configuration["Ports:Client"]}/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(confirmationToken)}";
                    await _emailService.SendMessageByEmailAsync(user.Email, "Confirm your email by follow this link", confirmationLink);
                    _logger.LogDebug("Succesffully confirmation link has been sent: {ConfirmationLink} to the user with email: {ModelEmail}", confirmationLink, user.Email);
                    return Ok("Confirmation link has been sent to your email.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't send confirmation link to this email");
                    return _responseService.Get(ResponseMessages.EmailSendFailed, StatusCodes.Status500InternalServerError);
                }
            }

            _logger.LogError(result.Errors.ToString(), "Registration errors");
            return _responseService.Get(ResponseMessages.UnexpectedError, StatusCodes.Status500InternalServerError);
        }

        [HttpGet("resend-confirmation-link")]
        public async Task<IActionResult> ResendConfirmationLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError(ResponseMessages.UserNotFound);
                return _responseService.Get(ResponseMessages.UserNotFound, StatusCodes.Status404NotFound);
            }

            try
            {
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"{_configuration["Ports:Client"]}/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(confirmationToken)}";
                await _emailService.SendMessageByEmailAsync(user.Email, "Confirm your email by follow this link", confirmationLink);
                _logger.LogDebug("Succesffully confirmation link has been sent: {ConfirmationLink} to the user with email: {ModelEmail}", confirmationLink, user.Email);
                return Ok(ResponseMessages.EmailSendSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ResponseMessages.EmailSendFailed);
                return _responseService.Get(ResponseMessages.EmailSendFailed, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email,  string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                _logger.LogError("Invalid request for confirmation email");
                return _responseService.Get(ResponseMessages.EmailConfirmationFailed, StatusCodes.Status400BadRequest);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                _logger.LogError(ResponseMessages.UserNotFound);
                return _responseService.Get(ResponseMessages.UserNotFound, StatusCodes.Status404NotFound);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email {UserEmail} successfully confirmed", user.Email);
                return Ok(ResponseMessages.EmailConfirmed);
            }

            _logger.LogError("Email {UserEmail} confirmation failed", user.Email);
            return _responseService.Get(ResponseMessages.EmailConfirmationFailed, StatusCodes.Status400BadRequest);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation(ResponseMessages.UserAlreadyLoggedIn);
                return _responseService.Get(ResponseMessages.UserAlreadyLoggedIn, StatusCodes.Status401Unauthorized);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null) { user = await _userManager.FindByEmailAsync(model.Username); }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    _logger.LogError("Couldn't log in. Email {UserEmail} is not confirmed", user.Email);
                    return _responseService.Get(ResponseMessages.EmailNotConfirmed, StatusCodes.Status403Forbidden);
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _authService.GenerateJwtToken(user, userRoles);
                _logger.LogInformation("User with email: {userEmail} has been successfully logged in", user.Email);
                return Ok(new { token });
            }

            _logger.LogError(ResponseMessages.InvalidLogin);
            return _responseService.Get(ResponseMessages.InvalidLogin, StatusCodes.Status401Unauthorized);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            await _authService.AddTokenToBlacklistAsync(token);
            _logger.LogInformation("Successfully logged out");
            _logger.LogDebug("Token: {Token} has been added to the blacklist", token);
            return Ok();
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError(ResponseMessages.UserNotFound);
                return _responseService.Get(ResponseMessages.UserNotFound, StatusCodes.Status404NotFound);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var isAdmin = roles.Contains("Admin");
            return Ok(new { username = user.UserName, isAdmin });
        }

        [HttpGet("recover-password")]
        public async Task<IActionResult> RecoverPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                _logger.LogError(ResponseMessages.UserNotFound);
                return _responseService.Get(ResponseMessages.UserNotFound, StatusCodes.Status404NotFound);
            }

            try
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordLink = $"{_configuration["Ports:Client"]}/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(resetToken)}";
                await _emailService.SendMessageByEmailAsync(user.Email, "Reset your password by following this link", resetPasswordLink);
                _logger.LogDebug("Succesffully reset password link has been sent: {ResetPasswordLink} to the user with email: {ModelEmail}", resetPasswordLink, email);
                return Ok(ResponseMessages.EmailSendSuccessful);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't send reset password link to this email");
                return _responseService.Get(ResponseMessages.EmailSendFailed, StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] RegisterModel model, string token)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(ModelState.ToString(), "Reset password errors");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Email);
            if (user == null)
            {
                _logger.LogError(ResponseMessages.UserNotFound);
                return _responseService.Get(ResponseMessages.UserNotFound, StatusCodes.Status404NotFound);
            }

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successful for user with email: {ModelEmail}", model.Email);
                return Ok(ResponseMessages.PasswordResetSuccessful);
            }

            _logger.LogError(result.Errors.ToString(), "Reset password errors");
            return _responseService.Get(ResponseMessages.PasswordResetFailed, StatusCodes.Status400BadRequest);
        }

    }

}
