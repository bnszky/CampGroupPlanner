import * as React from 'react'
import { Box, Typography, TextField } from '@mui/material';
import toCamelCase from '../../functions/toCamelCase';

function TextInput({fieldName, onValueChange, required, multiline}) {

    const handleValueChange = (event) => {
        onValueChange(event.target.value);
    };

    return <Box>
        <Typography variant='body1'>{toCamelCase(fieldName)}</Typography>
        <TextField
            sx={{width: 400}}
            required={required}
            multiline={multiline}
            rows={{multiline} && 5}
            id={fieldName}
            label={toCamelCase(fieldName)}
            onChange={handleValueChange}
            placeholder={toCamelCase(fieldName)}
        />
    </Box>
}

export default TextInput;