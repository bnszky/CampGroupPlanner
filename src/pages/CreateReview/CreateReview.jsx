import * as React from 'react'
import { Grid, Box, Typography, Divider, Button, Rating, Stack, CircularProgress, Alert } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import RegionMinimalItem from '../../components/RegionMinimalItem/RegionMinimalItem';
import useDataFeed from '../../hooks/useDataFeed';
import { useNavigate, useParams } from 'react-router-dom';
import useDataCreate from '../../hooks/useDataCreate';
import { useAuth } from '../../components/AuthProvider/AuthContext';

function CreateReview() {

    const navigate = useNavigate();
    const { regionName } = useParams();

    const {isLoggedIn, isLoading: isAuthLoading} = useAuth();

    if(!isAuthLoading && !isLoggedIn){
        navigate(`/region/${regionName}`, {
            state: { infoMsg: { type: 'error', msg: `You must be logged in to write a review` } },
        });
    }

    const {isLoading, error, data: region} = useDataFeed(`/api/region/${regionName}/mini`, '/region/edit', '/api/region', 'region')
    const {
        data: review,
        setData: setReview,
        errors,
        errorMsg,
        createdMsg,
        handleSubmit,
    } = useDataCreate(
        {
            title: '',
            text: '',
            rate: 1.0,
            regionId: null,
            regionName: regionName
        },
        '/api/review',
        `/region/${regionName}`,
        'Review'
    );

    function updateReview(key, value){
        setReview(prevReview => ({
            ...prevReview,
            [key]: value
        }));
    }

    if(isLoading) {
        return <CircularProgress/>
    }

    if(error){
        navigate('/articles', {
            state: { infoMsg: { type: 'error', msg: `Couldn't find region with this name` } },
        });
    }

    return <Box 
    alignItems='center'
    component='form'
    noValidate>
        <Typography variant='h4' textAlign='center'>Add Review</Typography>

        <Grid display='flex' container justifyContent='center'>

            <Grid item xs={12} md={6} my={4} display='flex' alignItems='center' justifyContent='center'>
                <RegionMinimalItem region={region}/>
            </Grid>

            <Grid item xs={12} md={6} my={4}>
                <Grid container direction='column' display='flex' alignItems='center'>
                    <Typography variant='h4' width={400} textAlign='center' p={3}>What do you think about {region.name}?</Typography>
                    <Stack direction='row'>
                        <Typography variant="h5" textAlign='left' mx={2}>Rate this place: </Typography> 
                        <Rating size='large' name="rating" value={review.rate} precision={1} onChange={(event, newValue) => {
                            updateReview('rate', newValue);
                        }}/>
                    </Stack>
                    <Typography variant='h4' textAlign='center' my={2}>Comment</Typography>
                    <TextInput fieldName='title' onValueChange={val => updateReview('title', val)} value={review.title} error={errors?.title} errorMessage={errors?.title} required/>
                    <TextInput fieldName='text' onValueChange={val => updateReview('text', val)} value={review.text} error={errors?.title} errorMessage={errors?.title} multiline/>

                    {errorMsg && <Alert variant="outlined" severity="error" sx={{width: 350, mt: 3}}>
                    {errorMsg}
                    </Alert>}

                    {createdMsg && <Alert variant="outlined" severity="success" sx={{width: 350, mt: 3}}>
                    {createdMsg}
                    </Alert>}
                </Grid>
            </Grid>

            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary" onClick={() => {
                        review.regionId = region.id;
                        updateReview('regionId', region.id);
                        handleSubmit();
                    }}>Add</Button>
                </Grid>
            </Grid>

        </Grid>
    </Box>;
}

export default CreateReview;