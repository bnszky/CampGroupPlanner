import * as React from 'react'
import { Box, Typography, TextField, IconButton, InputAdornment } from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import toCamelCase from '../../functions/toCamelCase';

function TextInput({fieldName, onValueChange, required, multiline, error, errorMessage, disabled, value, type}) {

    const [showPassword, setShowPassword] = React.useState(false);

    const handleValueChange = (event) => {
        onValueChange(event.target.value);
    };

    const togglePasswordVisibility = () => {
        setShowPassword(!showPassword);
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
            type={showPassword ? 'text' : type}  // Show text instead of password if showPassword is true
            InputProps={{
                endAdornment: type === 'password' && (
                    <InputAdornment position="end">
                        <IconButton onClick={togglePasswordVisibility} edge="end">
                            {showPassword ? <VisibilityOff /> : <Visibility />}
                        </IconButton>
                    </InputAdornment>
                ),
            }}
        />
    </Box>
}

export default TextInput;