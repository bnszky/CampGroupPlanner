import React, { useState, useEffect } from 'react';
import { Grid, Box, Typography, Button, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../components/AuthProvider/AuthContext';

const redirectPath = "/";

function ResetPassword() {
  const navigate = useNavigate();
  const location = useLocation();
  const { resetPassword, validateToken } = useAuth();
  const [password, setPassword] = useState('');
  const [repeatPassword, setRepeatPassword] = useState('');
  const [errorMsg, setErrorMsg] = useState('');
  const [successMsg, setSuccessMsg] = useState('');

  const query = new URLSearchParams(location.search);
  const email = decodeURIComponent(query.get('email'));
  const token = decodeURIComponent(query.get('token'));

  useEffect(() => {
    const validateAndNavigate = async () => {
        const isTokenCorrect = await validateToken(email, token);
        if (!isTokenCorrect) {
          navigate(redirectPath, {
            state: { infoMsg: { type: 'error', msg: "Couldn't allow, invalid token" } },
          });
        }
    };

    validateAndNavigate();
  }, [token]);

  const handleSubmit = async () => {
    const response = await resetPassword(email, token, password, repeatPassword);
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
      <Typography variant='h4' textAlign='center'>Reset Password</Typography>
      <Grid display='flex' container justifyContent='center'>
        <Grid item xs={12} md={6} p={5}>
          <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
            <TextInput
              fieldName='password'
              value={password}
              onValueChange={val => setPassword(val)}
              type='password'
              required
            />
            <TextInput
              fieldName='confirm password'
              value={repeatPassword}
              onValueChange={val => setRepeatPassword(val)}
              type='password'
              required
            />
            {errorMsg && <Alert variant="outlined" severity="error" sx={{ width: 350, mt: 3, whiteSpace: 'pre-line' }}>{errorMsg}</Alert>}
            {successMsg && <Alert variant="outlined" severity="success" sx={{ width: 350, mt: 3 }}>{successMsg}</Alert>}
            <Button sx={{ width: 300, height: 50, mt: 3 }} size='large' variant="contained" color="secondary" onClick={handleSubmit}>Reset Password</Button>
          </Grid>
        </Grid>
      </Grid>
    </Box>
  );
}

export default ResetPassword;
