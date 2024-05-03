import * as React from 'react'
import { TextField, Fab, Box } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

function InputAddText({name, handleAdd, error, errorMessage}) {

    const [currentText, setCurrentText] = React.useState('');

    function handleChange(event){
        setCurrentText(event.target.value);
    }

    return <Box sx={{width: 400}} my={2} display='flex' justifyContent='space-between'>

    <TextField 
    error={error}
    helperText={errorMessage}
    id={name}
    label={name}
    sx={{width: 300}}
    value={currentText}
    onChange={handleChange}/>

    <Fab color="secondary" aria-label="add" onClick={() => {
        handleAdd(currentText)
        setCurrentText('');
        }}>
        <AddIcon />
    </Fab>

    </Box>;
}

export default InputAddText;