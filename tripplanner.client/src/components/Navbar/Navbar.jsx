import { Box, Avatar, Tooltip, AppBar, Toolbar, Menu, MenuItem, IconButton, Button, Typography } from '@mui/material';
import * as React from 'react'
import MenuIcon from '@mui/icons-material/Menu';
import { useState } from 'react';
import { useAuth } from '../AuthProvider/AuthContext';
import { useNavigate } from 'react-router-dom';

const pages = [
  { name: 'Articles', link: '/articles' },
  { name: 'Regions', link: '/region' },
  { name: 'Attractions', link: '/attraction' },
]

const adminPages = [
  { name: 'Create Article', link: '/articles/create' },
  { name: 'Create Region', link: '/region/create' },
  { name: 'Create Attraction', link: '/attraction/create' }
]

const userPages = [
  { name: 'Your Reviews', link: '/review/user' },
]

const adminUsername = "admin@admin.com";
const adminPassword = "Admin123!";

function Navbar() {
    const {isLoggedIn, username, isAdmin, logout, login} = useAuth();

    const [anchorElNav, setAnchorElNav] = useState(null);
    const navigate = useNavigate();

    const handleOpenNavMenu = (event) => {
        setAnchorElNav(event.currentTarget);
    };

    const handleCloseNavMenu = () => {
        setAnchorElNav(null);
    };

    return <Box sx={{flexGrow: 1, marginBottom: 10}}><AppBar>
        
        <Toolbar>
            <Typography variant="h6">
                Article Platform
            </Typography>

            {/* Burger menu dropdown */}

            <Box sx={{ flexGrow: 1, display: { md: 'flex', lg: 'none' } }}>
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
                display: { md: 'block', lg: 'none' },
              }}
            >
              {pages.map((page) => (
                <MenuItem key={page.name}>
                  <Button textAlign="center" color='inherit' onClick={handleCloseNavMenu} href={page.link}>{page.name}</Button>
                </MenuItem>
              ))}
              {isLoggedIn && userPages.map((page) => (
                <MenuItem key={page.name}>
                  <Button textAlign="center" color='inherit' onClick={handleCloseNavMenu} href={page.link}>{page.name}</Button>
                </MenuItem>
              ))}
              {isAdmin && adminPages.map((page) => (
                <MenuItem key={page.name}>
                  <Button textAlign="center" color='inherit' onClick={handleCloseNavMenu} href={page.link}>{page.name}</Button>
                </MenuItem>
              ))}
            </Menu>
            </Box>
            
            {/* Menu for larger screens */}

            <Box ml={5} sx={{ flexGrow: 1, display: { xs: 'none', lg: 'flex'}}}>
                {pages.map(page => (
                    <Button key={page.name} color="inherit" href={page.link}>{page.name}</Button>
                ))}
                {isLoggedIn && userPages.map(page => (
                    <Button key={page.name} color="inherit" href={page.link}>{page.name}</Button>
                ))}
                {isAdmin && adminPages.map(page => (
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
              </Box>
            )}

            {/* Login and Register buttons */}

            {isLoggedIn ? 
            
            (<Box>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}} onClick={logout}>Logout</Button>
            </Box>) : 
            
            (<Box>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}} onClick={() => navigate("/login")}>Login</Button>
                <Button color="inherit" variant="outlined" sx={{marginLeft: 2}} onClick={() => navigate("/register")}>Register</Button>
            </Box>)}

        </Toolbar>

    </AppBar></Box>;
}
export default Navbar;