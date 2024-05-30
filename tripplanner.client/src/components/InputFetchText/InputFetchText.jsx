import * as React from 'react'
import { Fab, Box, Typography, Alert } from '@mui/material';
import DownloadIcon from '@mui/icons-material/Download';

function InputFetchText({name, handleFetchData, fetchData}) {

    const [error, setError] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState(null);

    return <Box sx={{width: 400}} my={2} display='flex' justifyContent='space-between'>

    <Fab color="warning" sx={{width: 150, borderRadius: 2, padding: 2}} onClick={async () => {
            try{
                var list = await fetchData();
                handleFetchData(list)
                setError(false);
                setErrorMessage(null);
            }
            catch(error){
                setError(true);
                setErrorMessage(error.message);
            }
        }}>
        <DownloadIcon />
        <Typography variant='body1'>{name}</Typography>
    </Fab>

    {error && <Alert severity="error" sx={{mx: 1}}>{errorMessage}</Alert>}

    </Box>;
}

export default InputFetchText;