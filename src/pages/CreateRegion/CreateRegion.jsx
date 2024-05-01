import * as React from 'react'
import { Grid, Box, Typography, Divider, Button } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import Carousel from 'react-material-ui-carousel';
import Image from 'mui-image';
import CountrySelect from '../../components/CountrySelect/CountrySelect';
import InputItemList from '../../components/InputItemList/InputItemList';
import InputAddText from '../../components/InputAddText/InputAddText';
import InputAddImage from '../../components/InputAddImage/InputAddImage';

function CreateRegion() {

    const [images, setImages] = React.useState([])

    const [region, setRegion] = React.useState({
        name: "",
        country: "",
        cities: [],
        description: '',
        images: []
    })

    function updateName(value){
        setRegion(reg => ({
            ...reg, ...{'name': value}
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

    return <Box 
    alignItems='center'
    component='form'
    noValidate>
        <Typography variant='h4' textAlign='center'>Create Region</Typography>

        <Grid display='flex' container justifyContent='center'>

            <Grid item xs={12} md={6} my={2}>
                <Grid container direction='column' display='flex' alignItems='center'>
                    <TextInput fieldName='name' onValueChange={updateName} required/>
                    <TextInput fieldName='description' onValueChange={updateDescription} multiline/>

                    <CountrySelect/>

                    <InputItemList name="Cities" onItemListChange={(a) => a}>
                        <InputAddText name="City"/>
                    </InputItemList>

                    <InputItemList name="Images" onItemListChange={onImageListChange}>
                        <InputAddImage name="Image"/>
                    </InputItemList>
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
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary">Create</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>;
}

export default CreateRegion;