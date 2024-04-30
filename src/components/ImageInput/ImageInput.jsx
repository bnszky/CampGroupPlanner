import * as React from 'react'
import { Box, TextField, Typography } from '@mui/material';
import toCamelCase from '../../functions/toCamelCase';

function ImageInput({fieldName, onImageChange}) {

    const handleFileChange = (event) => {
        onImageChange(URL.createObjectURL(event.target.files[0]));
    };

    return <Box>
        <Typography variant='body1'>{toCamelCase(fieldName)}</Typography>
        <TextField
            sx={{width: 400}}
            type="file"
            variant="outlined"
            id={fieldName}
            inputProps={{ accept: '.jpg,.img' }}
            onChange={handleFileChange}
        />
    </Box>
}

export default ImageInput;