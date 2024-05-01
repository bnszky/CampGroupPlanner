import * as React from 'react'
import { Grid, Box, Typography, Divider, Button } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import AttractionsMap from '../../components/AttractionsMap/AttractionsMap';
import LocationInput from '../../components/LocationInput/LocationInput';
import InteractiveAttractionMap from '../../components/InteractiveAttractionMap/InteractiveAttractionMap';
import SelectInput from '../../components/SelectInput/SelectInput';

function CreateAttraction({regions}) {

    const [attraction, setAttraction] = React.useState({
        name: '',
        description: '',
        image: '',
        longitude: 0,
        latitude: 0
    })

    function updateName(value){
        setAttraction(att => ({
            ...att, ...{'name': value}
        }))
    }

    function updateDescription(value){
        setAttraction(att => ({
            ...att, ...{'description': value}
        }))
    }

    function updateImage(value){
        setAttraction(att => ({
            ...att, ...{'image': value}
        }))
    }

    function handleChangePosition(position){
        console.log(position);
        setAttraction(att => ({
            ...att,
            'longitude': position.lng,
            'latitude': position.lat
        }))
    }

    return <Box 
    alignItems='center'
    component='form'
    noValidate>
        <Typography variant='h4' textAlign='center'>Create Attraction</Typography>

        <Grid display='flex' container justifyContent='center'>
            <Grid item xs={12} md={6} p={5}>
                <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
                    <TextInput fieldName='name' onValueChange={updateName} required/>
                    <TextInput fieldName='description' onValueChange={updateDescription} multiline/>
                    <ImageInput fieldName='image' onImageChange={updateImage}/>

                    <LocationInput latitude={attraction.latitude} longitude={attraction.longitude}/>

                    <SelectInput fieldName='region' onValueChange={a => a} items={regions}/>
            </Grid>
            </Grid>
            <Grid item xs={12} md={6}>
                <Grid container display='flex' justifyContent='center' alignItems='center' sx={{height: 700}}>
                    <Box sx={{height: 600, width: 600}}>
                        <InteractiveAttractionMap attraction={attraction} onChangePosition={handleChangePosition}/>
                    </Box>
                </Grid>
            </Grid>
            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary">Create</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>;
}

export default CreateAttraction;