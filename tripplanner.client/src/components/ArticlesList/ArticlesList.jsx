import { Grid, Button, Typography, Stack, Chip } from '@mui/material';
import * as React from 'react'
import { useState, useEffect } from 'react';
import ArticleItem from '../ArticleItem/ArticleItem';
import { useAuth } from '../AuthProvider/AuthContext';

function ArticlesList({articles, regionName, handleDelete, handleEdit}) {

    const {isAdmin} = useAuth();

    const nextItemsNumber = 3; 

    const [articlesToShow, setArticlesToShow] = useState(articles.slice(0, nextItemsNumber))

    const [variants, setVariants] = useState(['outlined', 'outlined', 'outlined'])

    function setVariant(index){
        let _variants = ['outlined', 'outlined', 'outlined'];

        if(index > 2){
            index = 0;
        }
        _variants[index] = 'contained';

        setVariants(_variants);
    }

    function sortArticles(sortingType){

        switch(sortingType){
            case 1:
                articles = articles.sort((a, b) => b.positioningRate - a.positioningRate)
                break;
            case 2:
                articles = articles.sort((a, b) => a.positioningRate - b.positioningRate)
                break;
            default:
                articles = articles.sort((a, b) => b.editedAt - a.editedAt)
                break;
        }

        setVariant(sortingType);

        setArticlesToShow(articles.slice(0, articlesToShow.length))
    }

    useEffect(() => {
        sortArticles(0);
    }, []);

    return <>
    {regionName != null 
    ? <Typography variant='h3' mb={5}>Latest news from {regionName}</Typography> 
    : <Typography variant='h3' mb={5}>Latest news</Typography> 
    }
    {articles.length > 0 && isAdmin && <Stack direction="row" spacing={2} p={2}>
        <Typography variant="subtitle1">Sort by</Typography>
        <Chip label="Last edited" color="primary" variant={variants[0]} onClick={() => sortArticles(0)} />
        <Chip label="Highest" color="primary" variant={variants[1]} onClick={() => sortArticles(1)}/>
        <Chip label="Lowest" color="primary" variant={variants[2]} onClick={() => sortArticles(2)} />
    </Stack>}
    <Grid container alignItems='center' justifyContent='center' spacing={15}>
        {articlesToShow.map(article => (
            <Grid item xs={12} sm={6} md={4} key={article.id}>
                <ArticleItem key={article.id} article={article} handleDelete={() => handleDelete(article.id)} handleEdit={() => handleEdit(article.id)}/>
            </Grid>
        ))}
    </Grid>
    {(articlesToShow.length < articles.length) &&
        <Grid container my={5} alignItems='center' justifyContent='center'>    
            <Button sx={{width: '300px'}} variant='outlined' color='primary'  onClick={() => setArticlesToShow(articles.slice(0, articlesToShow.length + nextItemsNumber))}>Show more</Button>
        </Grid>
    }
    </>;
}

export default ArticlesList;