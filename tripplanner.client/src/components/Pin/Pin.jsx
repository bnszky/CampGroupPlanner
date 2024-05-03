import { Typography, Grid } from '@mui/material';
import * as React from 'react'
import { Marker, Popup } from 'react-leaflet';

function Pin({attraction}) {
    return <>
        <Marker position={[attraction.longitude, attraction.latitude]}>
        <Popup>
            <Grid align="center">
                <img src={attraction.image} alt={`image ${attraction.name}`} style={{width: 150, height: 100, margin: 0}}/>
                <Typography variant='body1' align='center'>{attraction.name}</Typography>
            </Grid>
        </Popup>
        </Marker>
    </>;
}

export default Pin