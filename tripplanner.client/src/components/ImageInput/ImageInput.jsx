import * as React from 'react'
import { Box, TextField, Typography } from '@mui/material';
import toCamelCase from '../../functions/toCamelCase';

function ImageInput({fieldName, onImageChange, error, errorMessage}) {

    const handleFileChange = (event) => {
        onImageChange(event.target.files[0]);
    };

    return <Box>
        <Typography variant='body1'>{toCamelCase(fieldName)}</Typography>
        <TextField
            error={error}
            helperText={errorMessage}
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