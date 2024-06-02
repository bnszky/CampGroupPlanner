import { Select, MenuItem, Box, Grid, InputLabel, Button, Typography, TextField, Divider } from '@mui/material';
import * as React from 'react'
import './CreateArticle.css'
import { Description, PhotoCamera } from '@mui/icons-material';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import SelectInput from '../../components/SelectInput/SelectInput';
import ArticleItem from '../../components/ArticleItem/ArticleItem';
import { Alert, Switch } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import SwitchInput from '../../components/SwitchInput/SwitchInput';
import { fetchRegions } from '../../api';
import { useState, useEffect } from 'react';

function CreateArticle({initialData}) {

    const [article, setArticle] = React.useState(
        {
            title: '',
            description: "",
            createdAt: new Date(),
            imageFile: null,
            sourceLink: '',
            isVisible: false,
            regionName: null
        }
    )

    const [errors, setErrors] = React.useState({});
    const [errorMsg, setErrorMsg] = React.useState();
    const [createdMsg, setCreatedMsg] = React.useState();
    const navigate = useNavigate();
    const [regions, setRegions] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const data = await fetchRegions();
            setRegions(data);
        };

        fetchData();
    }, []);

    const handleSubmit = async () => {
        const formData = new FormData();
        formData.append('title', article.title);
        formData.append('description', article.description);
        formData.append('sourceLink', article.sourceLink);
        if (article.imageFile) {
            formData.append('imageFile', article.imageFile);
        }
        formData.append('regionName', article.regionName);
        formData.append('isVisible', article.isVisible);
        formData.append('positioningRate', article.positioningRate);

        try {
            var response;

            if(initialData){
                response = await fetch(`/api/articles/${article.id}`, {
                    method: 'PUT',
                    body: formData
                });
            }
            else {
                response = await fetch(`/api/articles`, {
                    method: 'POST',
                    body: formData
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
            setCreatedMsg('Article created/edited successfully');
            navigate(
                '/articles',
                {state: { infoMsg: {type: 'success', msg: `Article ${article.title} ${initialData ? 'edited' : 'created'} successfully`}} }
            );
        } catch (error) {
            console.error(error);
            setErrorMsg('An unexpected error occurred');
        }
    };

    function updateArticle(key, value) {
        setArticle(article => ({
          ...article,
          [key]: value
        }));
      }

    function updateImageURL(value){
        setArticle(article => ({
            ...article, ...{'imageFile': value, 'imageURL': URL.createObjectURL(value)}
        }))
    }

    return <Box 
    alignItems='center'
    component='form'
    onSubmit={handleSubmit}
    noValidate>
        <Typography variant='h4' textAlign='center'>{initialData ? 'Edit' : 'Create'} Article</Typography>

        <Grid display='flex' container justifyContent='center'>
            <Grid item xs={12} md={6} p={5}>
                <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
                    <TextInput 
                        fieldName='title' 
                        onValueChange={value => updateArticle('title', value)} 
                        error={errors?.Title} 
                        errorMessage={errors?.Title} 
                        required
                    />

                    <TextInput 
                        fieldName='description' 
                        onValueChange={value => updateArticle('description', value)} 
                        error={errors?.Description} 
                        errorMessage={errors?.Description} 
                        multiline
                    />

                    <TextInput 
                        fieldName='source' 
                        onValueChange={value => updateArticle('sourceLink', value)} 
                        error={errors?.SourceLink} 
                        errorMessage={errors?.SourceLink} 
                        required
                    />

                    <ImageInput 
                        fieldName='image' 
                        onImageChange={value => updateImageURL(value)} 
                        error={errors?.ImageURL} 
                        errorMessage={errors?.ImageURL}
                    />

                    <SelectInput 
                        fieldName='region' 
                        onValueChange={value => updateArticle('regionName', value)} 
                        items={regions} 
                        error={errors?.Region} 
                        errorMessage={errors?.Region}
                    />

                    <TextInput 
                        fieldName='rate' 
                        onValueChange={value => updateArticle('positioningRate', value)} 
                        error={errors?.PositioningRate} 
                        errorMessage={errors?.PositioningRate} 
                        required
                    />

                    <SwitchInput 
                        value={article.isVisible} 
                        onValueChange={value => updateArticle('isVisible', value)}
                    />

                    {errorMsg && <Alert variant="outlined" severity="error" sx={{width: 350, mt: 3}}>
                    {errorMsg}
                    </Alert>}

                    {createdMsg && <Alert variant="outlined" severity="success" sx={{width: 350, mt: 3}}>
                    {createdMsg}
                    
                    </Alert>}
                </Grid>
            </Grid>
            <Grid item xs={12} md={6}>
                <Grid container display='flex' justifyContent='center' alignItems='center' sx={{height: 800}}>
                    <ArticleItem article={article} handleDelete={() => a} handleEdit={() => a}/>
                </Grid>
            </Grid>
            <Divider color="black" width="100%" sx={{margin: 5}}/>
            <Grid item xs={12}>
                <Grid container display='flex' alignItems='center' justifyContent='center'>
                    <Button sx={{ width: 300, height: 50}} size='large' variant="contained" color="secondary" onClick={handleSubmit}>{initialData ? 'Edit' : 'Create'}</Button>
                </Grid>
            </Grid>
        </Grid>
    </Box>
}

export default CreateArticle;