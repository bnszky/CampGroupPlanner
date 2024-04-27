import { Grid, List, ListItem, Stack, Typography } from '@mui/material';
import * as React from 'react'
import ArticleItem from '../ArticleItem/ArticleItem';

function ArticlesList({articles, regionName}) {
    return <>
    <Typography variant='h3' mb={5}>Latest news from {regionName}</Typography>
    <Grid container alignItems='center' justifyContent='center' spacing={3}>
        {articles.map(article => <ArticleItem key={article.id} article={article}/>)}
    </Grid>
    </>;
}

export default ArticlesList;