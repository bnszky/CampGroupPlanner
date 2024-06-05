import { Box, Avatar, Tooltip, AppBar, Toolbar, Menu, MenuItem, IconButton, Button, Typography } from '@mui/material';
import * as React from 'react'
import MenuIcon from '@mui/icons-material/Menu';
import { useState } from 'react';
import { useAuth } from '../AuthProvider/AuthContext';

const profileOptions = ['Account', 'Favourite Posts']
const credentials = {
  "email": "admin@admin.com",
  'username': "admin@admin.com",
  "password": "Admin123!",
}

function Navbar({pages}) {
    const {isLoggedIn, username, isAdmin, logout, login} = useAuth();

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
                <MenuItem key={page.name}>
                  <Button textAlign="center" color='inherit' onClick={handleCloseNavMenu} href={page.link}>{page.name}</Button>
                </MenuItem>
              ))}
            </Menu>
            </Box>
            
            {/* Menu for larger screens */}

            <Box ml={5} sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex'}}}>
                {pages.map(page => (
                    <Button key={page.name} color="inherit" href={page.link}>{page.name}</Button>
                ))}
            </Box>

            {/* Profile Settings */}
            
            {isLoggedIn && (
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <Tooltip title={username} sx={{ marginLeft: 1, color: isAdmin ? 'red' : 'black' }}>
                  <IconButton sx={{p: 0}}>
                    <Avatar
                      alt="profile image"
                      src="/img/no-profile-image.png"
                    />
                  </IconButton>
                </Tooltip>
                <Typography variant="body1" sx={{ marginLeft: 1, color: isAdmin ? 'red' : 'black' }}>
                  {username}
                </Typography>
              </Box>
            )}

            {/* Login and Register buttons */}

            {isLoggedIn ? 
            
            (<Box>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}} onClick={logout}>Logout</Button>
            </Box>) : 
            
            (<Box>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}} onClick={() => login(credentials)}>Login</Button>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}}>Register</Button>
            </Box>)}

        </Toolbar>

    </AppBar></Box>;
}
export default Navbar;