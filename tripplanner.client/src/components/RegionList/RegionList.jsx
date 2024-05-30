import { Grid, Typography } from '@mui/material';
import * as React from 'react'
import RegionMinimalItem from '../RegionMinimalItem/RegionMinimalItem';

function RegionList({regions, handleDelete, handleEdit, handleVisit}) {

    return <>
    <Typography variant='h3' mb={5}>Regions</Typography> 
    <Grid container alignItems='center' justifyContent='center' spacing={15}>
        {regions.map(region => (
            <Grid item xs={12} sm={6} md={4} key={region.id}>
                <RegionMinimalItem key={region.id} region={region} handleDelete={() => handleDelete(region.name)} handleEdit={() => handleEdit(region.name)} handleVisit={() => handleVisit(region.name)}/>
            </Grid>
        ))}
    </Grid>
    </>;
}

export default RegionList;