import * as React from 'react'
import { Grid, Box, Typography, Divider, Button, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import Carousel from 'react-material-ui-carousel';
import Image from 'mui-image';
import CountrySelect from '../../components/CountrySelect/CountrySelect';
import InputItemList from '../../components/InputItemList/InputItemList';
import InputAddText from '../../components/InputAddText/InputAddText';
import InputAddImage from '../../components/InputAddImage/InputAddImage';
import InputFetchText from '../../components/InputFetchText/InputFetchText';
import { fetchAndConvertImage } from '../../functions/imageConvert';
import useDataCreate from '../../hooks/useDataCreate';

function CreateRegion({regionData = null}) {
    
    const [images, setImages] = React.useState([]);

    const {
        data: region,
        setData: setRegion,
        errors,
        errorMsg,
        createdMsg,
        handleSubmit,
    } = useDataCreate(
        regionData || {
            name: '',
            country: '',
            cities: [],
            description: '',
            images: []
        },
        '/api/Region',
        '/region',
        'Region',
        async (formData) => {
            const imageFiles = await Promise.all(images.map(url => fetchAndConvertImage(url)));
            imageFiles.forEach((file) => {
                formData.append('images', file, file.name);
            });
        }
    );

    async function fetchListFromApi(regionName, type) {
        const url = `/api/Region/${type}/${regionName}`;
    
        try {
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
    
            const data = await response.json();
    
            if (!Array.isArray(data)) {
                throw new Error('Data is not an array');
            }
    
            return data;
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }

    async function fetchDescription(regionName){
        const url = `/api/Region/description/${regionName}`;

        try {
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
    
            const data = await response.text();
    
            updateRegion('description', data);
            console.log(data)
        } catch (error) {
            console.error('Fetch error:', error);
        }
    }
    
    function updateRegion(key, value){
        setRegion(reg => ({
            ...reg, ...{[key]: value}
        }))
    }

    function onImageListChange(_images){
        setImages([..._images]);
    }

    function onCitiesListChange(_cities){
        setRegion(reg => ({
            ...reg, ...{'cities': [..._cities]}
        }));
    }

    return <Box 
    alignItems='center'
    component='form'
    noValidate>
        <Typography variant='h4' textAlign='center'>{regionData ? "Edit" : "Create"} Region</Typography>

        <Grid display='flex' container justifyContent='center'>

            <Grid item xs={12} md={6} my={2}>
                <Grid container direction='column' display='flex' alignItems='center'>
                    <TextInput fieldName='name' onValueChange={val => updateRegion("name", val)} value={region.name} error={errors?.Name} errorMessage={errors?.Name} required disabled={regionData}/>
                    <TextInput fieldName='description' onValueChange={val => updateRegion("description", val)} value={region.description} error={errors?.Description} errorMessage={errors?.Description} multiline/>
                    <Button variant='outlined' color='secondary' sx={{my: 2}} onClick={() => fetchDescription(region.name)}>Import example description</Button>

                    <CountrySelect updateCountry={val => updateRegion("country", val)} value={region.country} error={errors?.Country} errorMessage={errors?.Country}/>

                    <InputItemList name="Cities" onItemListChange={onCitiesListChange} getInitialItems={() => region.cities}>
                        <InputAddText name="City" error={errors?.Cities} errorMessage={errors?.Cities}/>
                        <InputFetchText name="Fetch cities" fetchData={() => fetchListFromApi(region.name, 'cities')}/>
                    </InputItemList>

                    <InputItemList name="Images" onItemListChange={onImageListChange} getInitialItems={() => region.images}>
                        <InputAddImage name="Image" error={errors?.Images} errorMessage={errors?.Images}/>
                        <InputFetchText name="Fetch images" fetchData={() => fetchListFromApi(region.name, 'images')}/>
                    </InputItemList>

                    {errorMsg && <Alert variant="outlined" severity="error" sx={{width: 350, mt: 3}}>
                    {errorMsg}
                    </Alert>}

                    {createdMsg && <Alert variant="outlined" severity="success" sx={{width: 350, mt: 3}}>
                    {createdMsg}
                    </Alert>}
                </Grid>
            </Grid>

            <Grid item xs={12} md={6} my={4} display='flex' alignItems='center' justifyContent='center'>
                {(images.length > 0) ? (<Box sx={{width: 500, height: 500}}><Carousel>
                    {images.map((image, key) => {
                        return <Image key={key} height={500} style={{ maxWidth: 500 }} duration={0} src={image} alt='image'/>
                    })}
                </Carousel></Box>) : (
                    <Typography variant='h3' textAlign='center'>No Image</Typography>
                )}
            </Grid>

            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary" onClick={handleSubmit}>{regionData ? "Edit" : "Create"}</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>;
}

export default CreateRegion;