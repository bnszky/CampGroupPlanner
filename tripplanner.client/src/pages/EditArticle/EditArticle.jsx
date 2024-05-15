import { Box, Grid, Typography, Divider, Button } from '@mui/material';
import * as React from 'react';
import { Alert } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import TextInput from '../../components/TextInput/TextInput';
import ImageInput from '../../components/ImageInput/ImageInput';
import SelectInput from '../../components/SelectInput/SelectInput';
import ArticleItem from '../../components/ArticleItem/ArticleItem';

function EditArticle({regions}) {
    const { id } = useParams();
    const [article, setArticle] = React.useState({
        title: '',
        description: '',
        imageURL: '',
        sourceLink: ''
    });
    const [errors, setErrors] = React.useState({});
    const [errorMsg, setErrorMsg] = React.useState();
    const [updatedMsg, setUpdatedMsg] = React.useState();
    const navigate = useNavigate();

    React.useEffect(() => {
        fetch(`/api/articles/${id}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch article');
                }
                return response.json();
            })
            .then(data => {
                setArticle(data);
            })
            .catch(error => {
                console.error('Error fetching article:', error);
                navigate(
                    '/articles',
                    { state: { infoMsg: { type: 'error', msg: `Article ${id} doesn't exist in the db` } } }
                );
            });
    }, [id]);

    async function handleSubmit(event) {
        event.preventDefault();

        const response = await fetch(`/api/articles/${id}`, {
            method: 'PUT',
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
                if (errorData.title) {
                    setErrorMsg(`Couldn't update article: ${errorData.title}`);
                } else {
                    setErrorMsg(`Couldn't update article: ${errorData[0]}`);
                }
            } else {
                console.error('Failed to update article', response);
                setErrorMsg(`Couldn't update article: ${response}`);
            }
        } else {
            console.log('Article updated successfully');
            setUpdatedMsg('Article updated successfully');
            navigate(
                '/articles',
                { state: { infoMsg: { type: 'success', msg: `Article ${article.title} updated successfully` } } }
            );
        }
    }

    function handleChange(field, value) {
        setArticle(prevState => ({
            ...prevState,
            [field]: value
        }));
    }

    return (
        <Box
            alignItems='center'
            component='form'
            onSubmit={handleSubmit}
            noValidate
        >
            <Typography variant='h4' textAlign='center'>Edit Article</Typography>

            <Grid display='flex' container justifyContent='center'>
                <Grid item xs={12} md={6} p={5}>
                    <Grid container direction='column' spacing={2} display='flex' alignItems='center'>
                        <TextInput fieldName='title' onValueChange={value => handleChange('title', value)} value={article.title} error={errors?.title} errorMessage={errors?.title} required />
                        <TextInput fieldName='description' onValueChange={value => handleChange('description', value)} value={article.description} error={errors?.description} errorMessage={errors?.description} multiline />
                        <TextInput fieldName='source' onValueChange={value => handleChange('sourceLink', value)} value={article.sourceLink} error={errors?.sourceLink} errorMessage={errors?.sourceLink} required disabled/>
                        <ImageInput fieldName='image' onImageChange={value => handleChange('imageURL', value)} value={article.imageURL} error={errors?.imageURL} errorMessage={errors?.imageURL} />
                        <SelectInput fieldName='region' onValueChange={value => handleChange('region', value)} value={article.region} items={regions} error={errors?.region} errorMessage={errors?.region} />

                        {errorMsg && <Alert variant="outlined" severity="error" sx={{ width: 350, mt: 3 }}>
                            {errorMsg}
                        </Alert>}

                        {updatedMsg && <Alert variant="outlined" severity="success" sx={{ width: 350, mt: 3 }}>
                            {updatedMsg}
                        </Alert>}
                    </Grid>
                </Grid>
                <Grid item xs={12} md={6}>
                    <Grid container display='flex' justifyContent='center' alignItems='center' sx={{ height: 700 }}>
                        <ArticleItem article={article} handleDelete={() => { }} handleEdit={() => { }} />
                    </Grid>
                </Grid>
                <Divider color="black" width="100%" sx={{ margin: 5 }} />
                <Grid item xs={12}>
                    <Grid container display='flex' alignItems='center' justifyContent='center'>
                        <Button sx={{ width: 300, height: 50 }} size='large' variant="contained" type='submit' color="secondary">Save Changes</Button>
                    </Grid>
                </Grid>
            </Grid>
        </Box>
    );
}

export default EditArticle;
