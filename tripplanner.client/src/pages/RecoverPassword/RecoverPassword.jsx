import React, { useState } from 'react';
import { Grid, Box, Typography } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import ResendEmail from '../../components/ResendEmail/ResendEmail';


function RecoverPassword() {
  const [email, setEmail] = useState('');

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
            <ResendEmail email={email} isRecoverPassword={true}/>
          </Grid>
        </Grid>
      </Grid>
    </Box>
  );
}

export default RecoverPassword;
