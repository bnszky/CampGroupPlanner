import humanizeDate from "../HumanDate/HumanDate";
import "./ArticleItem.css" 
import PropTypes from 'prop-types';

import Button from "@mui/material/Button";
import { Grid, Card, CardActions, CardContent, CardHeader, CardMedia, Typography } from "@mui/material";

function ArticleItem({article}) {

    return <Card sx={{maxWidth: 350, padding: 2, height: 500}}>
        <CardHeader
        title={article.title} 
        subheader={humanizeDate(article.createdAt)}/>

        <CardMedia
        component='img'
        height={200}
        image={article.imgUrl}
        alt={`image ${article.title}`}/>

        <CardContent>
            <Typography variant="body2">
                {article.description.substring(0, 150) + "..."}
            </Typography>
        </CardContent>

        <CardActions>
            <Button size="small" href={article.sourceLink} target="_blank">Read more</Button>
            <Button size="small" variant="contained" color="secondary" href="#Edit">Edit</Button>
            <Button size="small" variant="contained" color="error" href="#Delete">Delete</Button>
        </CardActions>
    </Card>;
}

ArticleItem.propTypes = {
    article: PropTypes.shape({
        id: PropTypes.number,
        title: PropTypes.string,
        description: PropTypes.string,
        createdAt: PropTypes.object,
        imgUrl: PropTypes.string,
        sourceLink: PropTypes.string
    })
}

export default ArticleItem;