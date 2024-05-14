import { Select, MenuItem, Box, Grid, InputLabel, Button, Typography, TextField, Divider } from '@mui/material';
import * as React from 'react'
import './CreateArticle.css'
import { Description, PhotoCamera } from '@mui/icons-material';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import SelectInput from '../../components/SelectInput/SelectInput';
import ArticleItem from '../../components/ArticleItem/ArticleItem';
import { Alert } from '@mui/material';
import { useNavigate } from 'react-router-dom';

function CreateArticle({regions}) {

    const [article, setArticle] = React.useState(
        {
            title: '',
            description: "",
            createdAt: new Date(),
            imageURL: "",
            sourceLink: ''
        }
    )

    const [errors, setErrors] = React.useState({});
    const [errorMsg, setErrorMsg] = React.useState();
    const [createdMsg, setCreatedMsg] = React.useState();
    const navigate = useNavigate();

    async function handleSubmit(){
        event.preventDefault();

        console.log(article);

        const response = await fetch('/api/articles', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(article)
        });

        setErrors({});
        setErrorMsg(null);
        if (!response.ok) {
            if (response.status === 400) {
                const errorData = await response.json();
                console.log(errorData);
                setErrors(errorData.errors);
                if(errorData?.title){
                    setErrorMsg(`Couldn't create article: ${errorData.title}`);
                }
                else {
                    setErrorMsg(`Couldn't create article: ${errorData[0]}`)
                }
            } else {
                console.error('Failed to create article', response);
                setErrorMsg(`Couldn't create article: ${response}`)
            }
        } else {
            console.log('Article created successfully');
            setCreatedMsg('Article created successfully');
            navigate(
                '/articles',
                {state: { infoMsg: {type: 'success', msg: `Article ${article.title} created successfully`}} }
            );
        }
    }

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

    function updateImageURL(value){
        setArticle(article => ({
            ...article, ...{'imageURL': value}
        }))
    }

    return <Box 
    alignItems='center'
    component='form'
    onSubmit={handleSubmit}
    noValidate>
        <Typography variant='h4' textAlign='center'>Create Article</Typography>

        <Grid display='flex' container justifyContent='center'>
            <Grid item xs={12} md={6} p={5}>
                <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
                    <TextInput fieldName='title' onValueChange={title => updateTitle(title)} error={errors?.Title} errorMessage={errors?.Title} required/>
                    <TextInput fieldName='description' onValueChange={description => updateDescription(description)} error={errors?.Description} errorMessage={errors?.Description} multiline/>
                    <TextInput fieldName='source' onValueChange={sourceLink => updateSourceLink(sourceLink)} error={errors?.SourceLink} errorMessage={errors?.SourceLink} required/>

                    <ImageInput fieldName='image' onImageChange={imageURL => updateImageURL(imageURL)} error={errors?.ImageURL} errorMessage={errors?.ImageURL}/>

                    <SelectInput fieldName='region' onValueChange={a => a} items={regions} error={errors?.Region} errorMessage={errors?.Region}/>

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
                    <ArticleItem article={article} handleDelete={() => a} handleEdit={() => a}/>
                </Grid>
            </Grid>
            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" type='submit' color="secondary">Create</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>
}

export default CreateArticle;