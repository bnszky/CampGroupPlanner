import * as React from 'react';
import {Switch, Box, Typography, Badge} from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';

export default function SwitchInput({value, onValueChange}) {
  return (
    <Box sx={{width: 400}}>
        <Typography variant='body1'>
            Visibility: 
            <Switch checked={value} onChange={evt => onValueChange(evt.target.checked)}/>
            <Badge
            sx={{mx: 2}}
            badgeContent={
                value ? <Visibility sx={{color: 'primary.main'}}/> : <VisibilityOff />
            }
            />
        </Typography>
    </Box>
  );
}