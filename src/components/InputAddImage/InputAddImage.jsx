import * as React from 'react'
import { TextField, Fab, Box } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

function InputAddImage({name, handleAdd, error, errorMessage}) {

    const [currentText, setCurrentText] = React.useState('');

    const handleFileChange = (event) => {
        handleAdd(URL.createObjectURL(event.target.files[0]));
    };

    return <Box sx={{width: 400}} my={2} display='flex' justifyContent='space-between'>

    <TextField
        error={error}
        helperText={errorMessage}
        sx={{width: 400}}
        type="file"
        variant="outlined"
        id={name}
        inputProps={{ accept: '.jpg,.img' }}
        onChange={handleFileChange}
    />

    </Box>;
}

export default InputAddImage;