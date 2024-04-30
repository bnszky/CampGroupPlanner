import { Typography, Box, Link } from '@mui/material';
import * as React from 'react'

function Footer(){

    function getCurrentYear(){
        return new Date().getFullYear();
    }

    return <Box sx={{
        width: "100%",
        backgroundColor: "primary.main",
    }}>
        <Typography variant='body1' p={2} sx={{color: 'white'}}>&copy; {getCurrentYear()} <Link href="https://github.com/bnszky" target="_blank" sx={{textDecoration: 'none', color: 'white'}}>Bnszky</Link></Typography>
    </Box>;
}

export default Footer