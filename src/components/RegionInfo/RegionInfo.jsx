import * as React from 'react';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import Carousel from 'react-material-ui-carousel';
import { Typography } from '@mui/material';
import Image from 'mui-image';

function RegionInfo({region}) {

    const imageComponents = region.images.map((srcLink, key) => {
        return <Image 
        key={key} 
        width="auto"
        height="500px"
        duration={0}
        src={srcLink} 
        alt='images for region ${region.name}'/>
    }) 

    const cities = region.cities.join(", ");

    return   <Grid container spacing={10} alignItems="center">
        <Grid item xs={12} md={7}>
            <Grid container direction="row" justifyContent="space-around" allignItem="flex-start" p={2}>
                <Grid item>
                    <Typography variant="h2">
                        {region.name}
                    </Typography>
                </Grid>
                <Grid item>
                    <Grid container direction="column" justifyContent="center" allignItem="flex-end">
                        <Typography variant="h5">Country: {region.country}</Typography>
                        <Typography variant="subtitle">Cities: {cities}</Typography>
                    </Grid>
                </Grid>
            </Grid>
            <Typography variant="body1" sx={{ fontSize: 20 }}>
                {region.description}
            </Typography>
        </Grid>
        <Grid item xs={12} md={5}>
            <Carousel>
                {imageComponents}
            </Carousel>
        </Grid>
        </Grid>;
}

export default RegionInfo;