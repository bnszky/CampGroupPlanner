import * as React from 'react'
import { Box, Typography, TextField } from '@mui/material';
import toCamelCase from '../../functions/toCamelCase';

function TextInput({fieldName, onValueChange, required, multiline, error, errorMessage, disabled, value}) {

    const handleValueChange = (event) => {
        onValueChange(event.target.value);
    };

    return <Box>
        <Typography variant='body1'>{toCamelCase(fieldName)}</Typography>
        <TextField
            error={error}
            helperText={errorMessage}
            sx={{width: 400}}
            required={required}
            multiline={multiline}
            rows={{multiline} && 5}
            id={fieldName}
            label={toCamelCase(fieldName)}
            onChange={handleValueChange}
            placeholder={toCamelCase(fieldName)}
            disabled={disabled}
            value={value}
        />
    </Box>
}

export default TextInput;