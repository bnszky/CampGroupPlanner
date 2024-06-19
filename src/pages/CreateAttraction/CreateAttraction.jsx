import * as React from 'react'
import { Grid, Box, Typography, Divider, Button, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import LocationInput from '../../components/LocationInput/LocationInput';
import InteractiveAttractionMap from '../../components/InteractiveAttractionMap/InteractiveAttractionMap';
import SelectInput from '../../components/SelectInput/SelectInput';

import { fetchRegions } from '../../functions/api';
import useDataCreate from '../../hooks/useDataCreate';

function CreateAttraction({initialAttractionData}) {

    const [regions, setRegions] = React.useState([]);
    React.useEffect(() => {async function fetchData() {
         const data = await fetchRegions();
         setRegions(data)
      }
      fetchData();}, [])

    const {
        data: attraction,
        setData: setAttraction,
        errors,
        errorMsg,
        createdMsg,
        handleSubmit,
    } = useDataCreate(
        initialAttractionData || {
            name: '',
            description: '',
            imageURL: '',
            longitude: 0,
            latitude: 0,
            regionName: '',
        },
        '/api/attraction',
        '/attraction',
        'Attraction'
    );

    function updateAttraction(key, value) {
        setAttraction(att => ({
          ...att,
          [key]: value
        }));
    }

    function updateImage(value){
        setAttraction(att => ({
            ...att, ...{'imageURL': URL.createObjectURL(value)}
        }))
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
        <Typography variant='h4' textAlign='center'>{initialAttractionData ? "Edit" : "Create"} Attraction</Typography>

        <Grid display='flex' container justifyContent='center'>
            <Grid item xs={12} md={6} p={5}>
                <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
                    <TextInput fieldName='name' error={errors?.name} errorMessage={errors?.name} value={attraction.name} onValueChange={val => updateAttraction('name', val)} required disabled={initialAttractionData}/>
                    <TextInput fieldName='description' error={errors?.description} errorMessage={errors?.description} value={attraction.description} onValueChange={val => updateAttraction('description', val)} multiline/>
                    <ImageInput fieldName='image' error={errors?.image} errorMessage={errors?.image} onImageChange={updateImage}/>

                    <LocationInput error={errors?.latitude || errors?.longitude} errorMessage={errors?.longitude || errors?.latitude} latitude={attraction.latitude} longitude={attraction.longitude}/>

                    <SelectInput fieldName='region' error={errors?.regionName} errorMessage={errors?.regionName} onValueChange={val => updateAttraction('regionName', val)} value={attraction.region} items={regions}/>

                    {errorMsg && <Alert variant="outlined" severity="error" sx={{width: 350, mt: 3}}>
                    {errorMsg}
                    </Alert>}

                    {createdMsg && <Alert variant="outlined" severity="success" sx={{width: 350, mt: 3}}>
                    {createdMsg}
                    </Alert>}
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
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary" onClick={handleSubmit}>{initialAttractionData ? "Edit" : "Create"}</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>;
}

export default CreateAttraction;