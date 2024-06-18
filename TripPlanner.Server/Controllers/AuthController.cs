using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IAuthService authService, IEmailService emailService, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _emailService = emailService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Couldn't log in because user is already logged in");
                return BadRequest("User already logged in.");
            }
            if(!await _emailService.IsEmailValid(model.Email))
            {
                _logger.LogInformation("Email is not valid: {ModelEmail}", model.Email);
                return BadRequest("Invalid email address.");
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
                    return BadRequest("Couldn't send confirmation link to this email");
                }
            }

            _logger.LogError(result.Errors.ToString(), "Registration errors");
            return BadRequest(result.Errors);
        }

        [HttpGet("resend-confirmation-link")]
        public async Task<IActionResult> ResendConfirmationLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError("User with this email not found");
                return NotFound("User with this email not found");
            }

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
                return BadRequest("Couldn't send confirmation link to this email");
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string email,  string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                _logger.LogError("Invalid request for confirmation email");
                return BadRequest("Invalid email confirmation request.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                _logger.LogError("User with this email not found");
                return NotFound("User with this email not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("Email {UserEmail} successfully confirmed", user.Email);
                return Ok("Email confirmed successfully.");
            }

            _logger.LogError("Email {UserEmail} confirmation failed", user.Email);
            return BadRequest("Email confirmation failed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Couldn't log in because user is already logged in");
                return BadRequest("User already logged in.");
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if(user == null) { user = await _userManager.FindByEmailAsync(model.Username); }
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    _logger.LogError("Couldn't log in. Email {UserEmail} is not confirmed", user.Email);
                    return BadRequest("Email is not confirmed.");
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _authService.GenerateJwtToken(user, userRoles);
                _logger.LogInformation("User with email: {userEmail} has been successfully logged in", user.Email);
                return Ok(new { token });
            }

            return Unauthorized();
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
                return Unauthorized();
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
                _logger.LogError("User with this email not found");
                return NotFound("User with this email not found");
            }

            try
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordLink = $"{_configuration["Ports:Client"]}/reset-password?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(resetToken)}";
                await _emailService.SendMessageByEmailAsync(user.Email, "Reset your password by following this link", resetPasswordLink);
                _logger.LogDebug("Succesffully reset password link has been sent: {ResetPasswordLink} to the user with email: {ModelEmail}", resetPasswordLink, email);
                return Ok("Password reset link has been sent to your email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't send reset password link to this email");
                return BadRequest("Couldn't send reset password link to this email");
            }

        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] RegisterModel model, string token)
        {
            var user = await _userManager.FindByIdAsync(model.Email);
            if (user == null)
            {
                _logger.LogError("User with this email not found");
                return NotFound("User with this email not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Password reset successful for user with email: {ModelEmail}", model.Email);
                return Ok("Password reset successful.");
            }

            _logger.LogError(result.Errors.ToString(), "Reset password errors");
            return BadRequest(result.Errors);
        }

    }

}
