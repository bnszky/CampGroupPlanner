import { Select, MenuItem, Box, Grid, InputLabel, Button, Typography, TextField, Divider } from '@mui/material';
import * as React from 'react'
import './CreateArticle.css'
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import SelectInput from '../../components/SelectInput/SelectInput';
import ArticleItem from '../../components/ArticleItem/ArticleItem';
import { Alert, Switch } from '@mui/material';
import SwitchInput from '../../components/SwitchInput/SwitchInput';
import { fetchRegions } from '../../functions/api';
import { useState, useEffect } from 'react';
import useDataCreate from '../../hooks/useDataCreate';

function CreateArticle({initialData}) {

    const {
        data: article,
        setData: setArticle,
        errors,
        errorMsg,
        createdMsg,
        handleSubmit,
    } = useDataCreate(
        initialData || {
            title: '',
            description: '',
            createdAt: new Date(),
            imageFile: null,
            sourceLink: '',
            positioningRate: 0,
            isVisible: false,
            regionName: null,
        },
        '/api/articles',
        '/articles',
        'Article'
    );

    const [regions, setRegions] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const data = await fetchRegions();
            setRegions(data);
        };

        fetchData();
    }, []);

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
                        value={article.title}
                        onValueChange={value => updateArticle('title', value)} 
                        error={errors?.Title} 
                        errorMessage={errors?.Title} 
                        required
                    />

                    <TextInput 
                        fieldName='description'
                        value={article.description} 
                        onValueChange={value => updateArticle('description', value)} 
                        error={errors?.Description} 
                        errorMessage={errors?.Description} 
                        multiline
                    />

                    <TextInput 
                        fieldName='source' 
                        value={article.sourceLink}
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
                        value={article.regionName}
                        onValueChange={value => updateArticle('regionName', value)} 
                        items={regions} 
                        error={errors?.Region} 
                        errorMessage={errors?.Region}
                    />

                    <TextInput 
                        fieldName='rate' 
                        value={article.positioningRate}
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