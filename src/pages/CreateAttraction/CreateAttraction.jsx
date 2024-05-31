import * as React from 'react'
import { Grid, Box, Typography, Divider, Button, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import LocationInput from '../../components/LocationInput/LocationInput';
import InteractiveAttractionMap from '../../components/InteractiveAttractionMap/InteractiveAttractionMap';
import SelectInput from '../../components/SelectInput/SelectInput';

function CreateAttraction({initialAttractionData}) {

    const [attraction, setAttraction] = React.useState(initialAttractionData || {
        id: null,
        name: '',
        description: '',
        imageURL: '',
        longitude: 0,
        latitude: 0,
        region: ''
    })

    const fetchRegions = async () => {
        try {
            const response = await fetch(`/api/region/names`);
            if (!response.ok) {
                throw new Error('Failed to fetch regions');
            }
            const data = await response.json();
            setRegions(data);
        } catch (error) {
            console.error(error.message);
        }
    }

    const [regions, setRegions] = React.useState([]);
    React.useEffect(() => {async function fetchData() {
         await fetchRegions();
      }
      fetchData();}, [regions])
    const [errorMsg, setErrorMsg] = React.useState(null);
    const [errors, setErrors] = React.useState({});
    const [createdMsg, setCreatedMsg] = React.useState(null);
    const [imageFile, setImageFile] = React.useState(null);

    const handleSubmit = async () => {
        const attractionData = new FormData();
        attractionData.append('name', attraction.name);
        attractionData.append('description', attraction.description);

        if (imageFile) attractionData.append('image', imageFile);

        attractionData.append('latitude', attraction.latitude.toString().replace('.', ','));
        attractionData.append('longitude', attraction.longitude.toString().replace('.', ','));
        attractionData.append('regionName', attraction.region);

        try {
            var response;

            if(initialAttractionData){
                response = await fetch(`/api/attraction/${attraction.id}`, {
                    method: 'PUT',
                    body: attractionData
                });
            }
            else {
                response = await fetch(`/api/attraction`, {
                    method: 'POST',
                    body: attractionData
                });
            }

            if (!response.ok) {

                const result = await response.json();

                if (result.errors) {
                    var _errors = {};
                    console.log(result.errors);
                    for (const errorKey in result.errors) {
                        if (result.errors.hasOwnProperty(errorKey)) {
                            _errors[errorKey] = result.errors[errorKey][0];
                        }
                    }
                    setErrors(_errors);
                }
                if (result.title) {
                    setErrorMsg(result.title);
                }
                return;
            }

            setErrors({});
            setErrorMsg('');
            setCreatedMsg('Attraction created successfully');
        } catch (error) {
            console.error(error);
            setErrorMsg('An unexpected error occurred');
        }
    };

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
            ...att, ...{'imageURL': URL.createObjectURL(value)}
        }))
        setImageFile(value);
    }

    function updateRegion(value){
        setAttraction(att => ({
            ...att, ...{'region': value}
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
                    <TextInput fieldName='name' error={errors?.name} errorMessage={errors?.name} value={attraction.name} onValueChange={updateName} required disabled={initialAttractionData}/>
                    <TextInput fieldName='description' error={errors?.description} errorMessage={errors?.description} value={attraction.description} onValueChange={updateDescription} multiline/>
                    <ImageInput fieldName='image' error={errors?.image} errorMessage={errors?.image} onImageChange={updateImage}/>

                    <LocationInput error={errors?.latitude || errors?.longitude} errorMessage={errors?.longitude || errors?.latitude} latitude={attraction.latitude} longitude={attraction.longitude}/>

                    <SelectInput fieldName='region' error={errors?.regionName} errorMessage={errors?.regionName} onValueChange={updateRegion} value={attraction.region} items={regions}/>

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