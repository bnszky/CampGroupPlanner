import * as React from 'react';
import Grid from '@mui/material/Grid';
import Carousel from 'react-material-ui-carousel';
import { Typography, Box } from '@mui/material';
import Image from 'mui-image';

function RegionInfo({region}) {

    const cities = region.cities.join(", ");

    return   <Grid container spacing={10} alignItems="center">
        <Grid item xs={12} md={7}>
            <Grid container direction="row" justifyContent="space-around" p={2}>
                <Grid item>
                    <Typography variant="h2">
                        {region.name}
                    </Typography>
                </Grid>
                <Grid item>
                    <Grid container direction="column" justifyContent="center">
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
            <Box sx={{width: 500, height: 500}}><Carousel>
                    {region.images == null || region.images.length <= 0
                    ? <Image key={0} height={500} style={{ maxWidth: 500 }} duration={0} src={"/img/no-image.png"} alt='image'/>
                    : region.images.map((image, key) => {
                        return <Image key={key} height={500} style={{ maxWidth: 500 }} duration={0} src={image} alt='image'/>
                    })}
            </Carousel></Box>
        </Grid>
        </Grid>;
}

export default RegionInfo;