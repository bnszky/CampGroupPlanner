import { Select, MenuItem, Box, Grid, InputLabel, Button, Typography, TextField, Divider } from '@mui/material';
import * as React from 'react'
import './CreateArticle.css'
import { Description, PhotoCamera } from '@mui/icons-material';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import SelectInput from '../../components/SelectInput/SelectInput';
import ArticleItem from '../../components/ArticleItem/ArticleItem';

function CreateArticle({regions}) {

    const [article, setArticle] = React.useState(
        {
            id: 1,
            title: '',
            description: "",
            createdAt: new Date(),
            imgUrl: "",
            sourceLink: ''
        }
    )

    function updateTitle(value){
        setArticle(article => ({
            ...article, ...{'title': value}
        }))
    }

    function updateDescription(value){
        setArticle(article => ({
            ...article, ...{'description': value}
        }))
    }

    function updateSourceLink(value){
        setArticle(article => ({
            ...article, ...{'sourceLink': value}
        }))
    }

    function updateImgUrl(value){
        setArticle(article => ({
            ...article, ...{'imgUrl': value}
        }))
    }

    return <Box 
    alignItems='center'
    component='form'
    noValidate>
        <Typography variant='h4' textAlign='center'>Create Article</Typography>

        <Grid display='flex' container justifyContent='center'>
            <Grid item xs={12} md={6} p={5}>
                <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
                    <TextInput fieldName='title' onValueChange={title => updateTitle(title)} required/>
                    <TextInput fieldName='description' onValueChange={description => updateDescription(description)} multiline/>
                    <TextInput fieldName='source' onValueChange={sourceLink => updateSourceLink(sourceLink)} required/>

                    <ImageInput fieldName='image' onImageChange={imgUrl => updateImgUrl(imgUrl)}/>

                    <SelectInput fieldName='region' onValueChange={a => a} items={regions}/>
                </Grid>
            </Grid>
            <Grid item xs={12} md={6}>
                <Grid container display='flex' justifyContent='center' alignItems='center' sx={{height: 700}}>
                    <ArticleItem article={article}/>
                </Grid>
            </Grid>
            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary">Create</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>
}

export default CreateArticle;