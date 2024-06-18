import React, { useState, useEffect } from 'react';
import { Box, Typography, Button, Alert } from '@mui/material';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../components/AuthProvider/AuthContext';

const redirectPath = "/";

function ConfirmEmail() {
  const navigate = useNavigate();
  const location = useLocation();
  const { confirmEmail, resendConfirmationLink } = useAuth();
  const [errorMsg, setErrorMsg] = useState('');
  const [successMsg, setSuccessMsg] = useState('');

  const query = new URLSearchParams(location.search);
  const email = decodeURIComponent(query.get('email'));
  const token = decodeURIComponent(query.get('token'));

  useEffect(() => {
    const confirmUserEmail = async () => {
      const response = await confirmEmail(email, token);
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

    confirmUserEmail();
  }, [email, token, navigate, confirmEmail]);

  const handleResend = async () => {
    console.log("se")
    const response = await resendConfirmationLink(email);
    if (response.isOk) {
      setSuccessMsg(response.msg);
      setErrorMsg('');
    } else {
      setErrorMsg(response.msg);
      setSuccessMsg('');
    }
  };

  return (
    <Box alignItems='center' component='form' noValidate>
      <Typography variant='h4' textAlign='center'>Confirm Email</Typography>
      {errorMsg && (
        <>
          <Alert variant="outlined" severity="error" sx={{ width: 350, mt: 3, whiteSpace: 'pre-line' }}>{errorMsg}</Alert>
          <Button sx={{ width: 300, height: 50, mt: 3 }} size='large' variant="contained" color="secondary" onClick={handleResend}>Resend Confirmation Link</Button>
        </>
      )}
      {successMsg && <Alert variant="outlined" severity="success" sx={{ width: 350, mt: 3 }}>{successMsg}</Alert>}
    </Box>
  );
}

export default ConfirmEmail;
