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
import { Update } from '@mui/icons-material';

function CreateRegion({regionData = null}) {

    const [images, setImages] = React.useState([])
    const [errors, setErrors] = React.useState(null)
    const [errorMsg, setErrorMsg] = React.useState(null)
    const [createdMsg, setCreatedMsg] = React.useState(null)

    const initialRegionState = {
        name: "",
        country: "",
        cities: [],
        description: '',
        images: []
    };

    const [region, setRegion] = React.useState(regionData || initialRegionState)

    const fetchAndConvertImage = async (url) => {
        try {
          const response = await fetch(url);
          const contentType = response.headers.get('content-type');
    
          if (!response.ok) {
            throw new Error('Network response was not ok');
          }
    
          if (contentType !== 'image/jpeg' && contentType !== 'image/png') {
            throw new Error('Image format not supported. Only .jpg and .png are allowed.');
          }
    
          const blob = await response.blob();
          const extension = contentType.split('/')[1]; // Extract the file extension
          const fileName = `image.${extension}`; // Create a file name with the correct extension
          const file = new File([blob], fileName, { type: contentType });
          return file;
        } catch (error) {
          console.error('Error fetching or converting image:', error);
          throw error;
        }
    };

    async function handleSubmit() {
        event.preventDefault();
    
        console.log(region);
        console.log(region.country);
    
        const formData = new FormData();
        formData.append('name', region.name);
        formData.append('country', region.country);
        formData.append('description', region.description);
    
        // Append city names
        region.cities.forEach((city, index) => {
            formData.append(`cities[${index}]`, city);
        });
    
        var _images = [];
        for(var imageUrl of images){
            const res = await fetchAndConvertImage(imageUrl)
            _images.push(res)
        }

        console.log(_images);

        // Append image files
        _images.forEach((file, index) => {
            console.log(file);
            formData.append(`Images`, file, file.name);
        });

        console.log(formData);
    
        var response;
        if(regionData == null) {
            response = await fetch('/api/Region', {
                method: 'POST',
                body: formData
            });
        } else {
            response = await fetch(`/api/Region/${region.name}`, {
                method: 'PUT',
                body: formData
            });
        }
    
        setErrors({});
        setErrorMsg(null);
        setCreatedMsg(null);
        if (!response.ok) {
            if (response.status === 400) {
                const errorData = await response.json();
                console.log(errorData);
                setErrors(errorData.errors);
                if (errorData?.title) {
                    setErrorMsg(`Couldn't create region: ${errorData.title}`);
                } else {
                    setErrorMsg(`Couldn't create region: ${errorData[0]}`);
                }
            } else {
                console.error('Failed to create region', response);
                setErrorMsg(`Couldn't create region: Unknown problem. Try again`);
            }
        } else {
            console.log('Region created successfully');
            setCreatedMsg('Region created successfully');
        }
    }    

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
            throw error;
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
    
            updateDescription(data);
            console.log(data)
        } catch (error) {
            console.error('Fetch error:', error);
            updateDescription(error);
        }
    }
    
    function updateName(value){
        setRegion(reg => ({
            ...reg, ...{'name': value}
        }))
    }

    function updateCountry(value){
        setRegion(reg => ({
            ...reg, ...{'country': value}
        }))
    }

    function updateDescription(value){
        setRegion(reg => ({
            ...reg, ...{'description': value}
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
                    <TextInput fieldName='name' onValueChange={updateName} value={region.name} error={errors?.Name} errorMessage={errors?.Name} required disabled={regionData}/>
                    <TextInput fieldName='description' onValueChange={updateDescription} value={region.description} error={errors?.Description} errorMessage={errors?.Description} multiline/>
                    <Button variant='outlined' color='secondary' sx={{my: 2}} onClick={() => fetchDescription(region.name)}>Import example description</Button>

                    <CountrySelect updateCountry={updateCountry} value={region.country} error={errors?.Country} errorMessage={errors?.Country}/>

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