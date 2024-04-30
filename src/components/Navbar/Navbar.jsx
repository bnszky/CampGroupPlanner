import { Box, Avatar, Tooltip, AppBar, Toolbar, Menu, MenuItem, IconButton, Button, Typography } from '@mui/material';
import * as React from 'react'
import MenuIcon from '@mui/icons-material/Menu';
import { useState } from 'react';

const pages = ['Home', 'Articles']
const profileOptions = ['Account', 'Favourite Posts']
const isLoggedIn = true;

function Navbar() {

    const [anchorElNav, setAnchorElNav] = useState(null);
    const [anchorElUser, setAnchorElUser] = useState(null);

    const handleOpenNavMenu = (event) => {
        setAnchorElNav(event.currentTarget);
    };
    const handleOpenUserMenu = (event) => {
        setAnchorElUser(event.currentTarget);
      };

    const handleCloseNavMenu = () => {
        setAnchorElNav(null);
    };
    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
      };

    return <Box sx={{flexGrow: 1, marginBottom: 10}}><AppBar>
        
        <Toolbar>
            <Typography variant="h6">
                Article Platform
            </Typography>

            {/* Burger menu dropdown */}

            <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'left',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'left',
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: 'block', md: 'none' },
              }}
            >
              {pages.map((page) => (
                <MenuItem key={page} onClick={handleCloseNavMenu}>
                  <Typography textAlign="center">{page}</Typography>
                </MenuItem>
              ))}
            </Menu>
            </Box>
            
            {/* Menu for larger screens */}

            <Box ml={5} sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex'}}}>
                {pages.map(page => (
                    <Button key={page} color="inherit">{page}</Button>
                ))}
            </Box>

            {/* Profile Settings */}
            
            {isLoggedIn && <Box sx={{ flexGrow: 0, marginRight: 2}}>
            <Tooltip title="Profile actions">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar alt="profile image" src="https://www.iprcenter.gov/image-repository/blank-profile-picture.png/@@images/image.png" />
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: '40px' }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
            >
              {profileOptions.map((option) => (
                <MenuItem key={option} onClick={handleCloseUserMenu}>
                  <Typography textAlign="center">{option}</Typography>
                </MenuItem>
              ))}
            </Menu>
          </Box>}

            {/* Login and Register buttons */}

            {isLoggedIn ? 
            
            (<Box>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}}>Logout</Button>
            </Box>) : 
            
            (<Box>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}}>Login</Button>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}}>Register</Button>
            </Box>)}

        </Toolbar>

    </AppBar></Box>;
}
export default Navbar;