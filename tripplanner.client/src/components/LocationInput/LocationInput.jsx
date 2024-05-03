import * as React from 'react'
import { Box, Typography, TextField } from '@mui/material';

function LocationInput({longitude, latitude}) {

    return <Box>
        <Typography variant='body1'>Location</Typography>
        <Box>
            <TextField
                sx={{width: 200}}
                id='longitude'
                label="Longitude"
                value={longitude}
                disabled
            />
            <TextField
                sx={{width: 200}}
                id='latitude'
                label="Latitude"
                value={latitude}
                disabled
            />
        </Box>
    </Box>
}

export default LocationInput;