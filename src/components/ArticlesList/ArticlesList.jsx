import { Grid, Button, Typography } from '@mui/material';
import * as React from 'react'
import { useState } from 'react';
import ArticleItem from '../ArticleItem/ArticleItem';

function ArticlesList({articles, regionName}) {

    const nextItemsNumber = 3; 

    const [articlesToShow, setArticlesToShow] = useState(articles.slice(0, nextItemsNumber))

    return <>
    <Typography variant='h3' mb={5}>Latest news from {regionName}</Typography>
    <Grid container alignItems='center' justifyContent='center' spacing={15}>
        {articlesToShow.map(article => <ArticleItem key={article.id} article={article}/>)}
    </Grid>
    {(articlesToShow.length < articles.length) &&
        <Grid container my={5} alignItems='center' justifyContent='center'>    
            <Button sx={{width: '300px'}} variant='outlined' color='primary'  onClick={() => setArticlesToShow(articles.slice(0, articlesToShow.length + nextItemsNumber))}>Show more</Button>
        </Grid>
    }
    </>;
}

export default ArticlesList;