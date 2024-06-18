import React, { useState } from 'react';
import { Grid, Button, Alert } from '@mui/material';
import { useAuth } from '../../components/AuthProvider/AuthContext';

function ResendEmail({email, isRecoverPassword}) {
  const { recoverPassword, resendConfirmationLink } = useAuth();
  const [errorMsg, setErrorMsg] = useState('');
  const [successMsg, setSuccessMsg] = useState('');
  const [isSent, setIsSent] = useState(false);

  const handleSubmit = async () => {
    var response;

    if(isRecoverPassword) response = await recoverPassword(email);
    else response = await resendConfirmationLink(email);

    if (response.isOk) {
      setSuccessMsg(response.msg);
      setErrorMsg('');
    } else {
      setErrorMsg(response.msg);
      setSuccessMsg('');
    }

    setIsSent(true);
  };

  return (
          <Grid container direction='column' sx={{my: 2}} display='flex' alignItems='center'>
            {errorMsg && <Alert variant="outlined" severity="error" sx={{ width: 350, mt: 3, whiteSpace: 'pre-line' }}>{errorMsg}</Alert>}
            {successMsg && <Alert variant="outlined" severity="success" sx={{ width: 350, mt: 3 }}>{successMsg}</Alert>}
            <Button sx={{ width: 300, height: 50, mt: 3 }} size='large' variant="contained" color="secondary" onClick={handleSubmit}>Send link to email {isSent && 'again'}</Button>
          </Grid>
  );
}

export default ResendEmail;
