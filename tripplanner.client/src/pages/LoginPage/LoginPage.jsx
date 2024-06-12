import React, { useState } from 'react';
import { Grid, Box, Typography, Button, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import { useAuth } from '../../components/AuthProvider/AuthContext';
import { useNavigate } from 'react-router-dom';

const redirectPath = "/articles";

function LoginPage() {

  const navigate = useNavigate();

  const { login, isLoggedIn } = useAuth();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errorMsg, setErrorMsg] = useState('');
  const [successMsg, setSuccessMsg] = useState('');

  if(isLoggedIn){
    navigate(redirectPath, {
        state: { infoMsg: { type: 'error', msg: `You are already logged in` } },
    });
  }

  const handleSubmit = async () => {
    const response = await login(username, password);
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
    <Box
      alignItems='center'
      component='form'
      noValidate
    >
      <Typography variant='h4' textAlign='center'>Login</Typography>

      <Grid display='flex' container justifyContent='center'>
        <Grid item xs={12} md={6} p={5}>
          <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
            <TextInput
              fieldName='username'
              value={username}
              onValueChange={val => setUsername(val)}
              required
            />
            <TextInput
              fieldName='password'
              value={password}
              onValueChange={val => setPassword(val)}
              type='password'
              required
            />
            {errorMsg && <Alert variant="outlined" severity="error" sx={{ width: 350, mt: 3 }}>{errorMsg}</Alert>}
            {successMsg && <Alert variant="outlined" severity="success" sx={{ width: 350, mt: 3 }}>{successMsg}</Alert>}
            <Button sx={{ width: 300, height: 50, mt: 3 }} size='large' variant="contained" color="secondary" onClick={handleSubmit}>Login</Button>
          </Grid>
        </Grid>
      </Grid>
    </Box>
  );
}

export default LoginPage;
