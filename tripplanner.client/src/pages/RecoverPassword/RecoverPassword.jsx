import React, { useState } from 'react';
import { Grid, Box, Typography, Button, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../components/AuthProvider/AuthContext';

const redirectPath = "/";

function RecoverPassword() {
  const navigate = useNavigate();
  const { recoverPassword } = useAuth();
  const [email, setEmail] = useState('');
  const [errorMsg, setErrorMsg] = useState('');
  const [successMsg, setSuccessMsg] = useState('');

  const handleSubmit = async () => {
    const response = await recoverPassword(email);
    if (response.isOk) {
      setSuccessMsg(response.msg);
      setErrorMsg('');
      navigate(redirectPath, {
        state: { infoMsg: { type: 'success', msg: response.msg } },
      });
    } else {
      setErrorMsg(response.msg);
      setSuccessMsg('');
    }
  };

  return (
    <Box alignItems='center' component='form' noValidate>
      <Typography variant='h4' textAlign='center'>Recover Password</Typography>
      <Grid display='flex' container justifyContent='center'>
        <Grid item xs={12} md={6} p={5}>
          <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
            <TextInput
              fieldName='email'
              value={email}
              onValueChange={val => setEmail(val)}
              type='email'
              required
            />
            {errorMsg && <Alert variant="outlined" severity="error" sx={{ width: 350, mt: 3, whiteSpace: 'pre-line' }}>{errorMsg}</Alert>}
            {successMsg && <Alert variant="outlined" severity="success" sx={{ width: 350, mt: 3 }}>{successMsg}</Alert>}
            <Button sx={{ width: 300, height: 50, mt: 3 }} size='large' variant="contained" color="secondary" onClick={handleSubmit}>Recover Password</Button>
          </Grid>
        </Grid>
      </Grid>
    </Box>
  );
}

export default RecoverPassword;
