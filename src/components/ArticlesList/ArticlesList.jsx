import { Grid, Button, Typography } from '@mui/material';
import * as React from 'react'
import { useState } from 'react';
import ArticleItem from '../ArticleItem/ArticleItem';

function ArticlesList({articles, regionName}) {

    const nextItemsNumber = 3; 

    const [articlesToShow, setArticlesToShow] = useState(articles.slice(0, nextItemsNumber))

    return <>
    {regionName != null 
    ? <Typography variant='h3' mb={5}>Latest news from {regionName}</Typography> 
    : <Typography variant='h3' mb={5}>Latest news</Typography> 
    }
    <Grid container alignItems='center' justifyContent='center' spacing={15}>
        {articlesToShow.map(article => (
            <Grid item xs={12} sm={6} md={4} key={article.id}>
                <ArticleItem key={article.id} article={article}/>
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