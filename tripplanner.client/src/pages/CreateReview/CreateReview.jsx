import * as React from 'react'
import { Grid, Box, Typography, Divider, Button, Rating, Stack } from '@mui/material';
import TextInput from '../../components/TextInput/TextInput';
import RegionMinimalItem from '../../components/RegionMinimalItem/RegionMinimalItem';

function CreateReview({region, author}) {

    const [review, setReview] = React.useState({
        title: '',
        text: '',
        rate: '',
        author: {author}
    })

    function updateTitle(value){
        setReview(rev => ({
            ...rev, ...{'title': value}
        }))
    }

    function updateText(value){
        setReview(rev => ({
            ...rev, ...{'text': value}
        }))
    }

    function updateRate(value){
        setReview(rev => ({
            ...rev, ...{'rate': value}
        }))
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
                        <Rating size='large' name="rating" defaultValue={1} precision={0.5} onChange={updateRate}/>
                    </Stack>
                    <Typography variant='h4' textAlign='center' my={2}>Comment</Typography>
                    <TextInput fieldName='title' onValueChange={updateTitle} required/>
                    <TextInput fieldName='text' onValueChange={updateText} multiline/>
                </Grid>
            </Grid>

            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary">Add</Button>
                </Grid>
            </Grid>

        </Grid>
    </Box>;
}

export default CreateReview;