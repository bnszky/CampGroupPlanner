import humanizeDate from "../HumanDate/HumanDate";
import "./ArticleItem.css" 
import PropTypes from 'prop-types';

import Button from "@mui/material/Button";
import { Card, CardActions, CardContent, CardHeader, CardMedia, Typography, Link, IconButton, Badge } from "@mui/material";
import { Visibility, VisibilityOff } from '@mui/icons-material';

function ArticleItem({article, handleDelete, handleEdit}) {

    return <Card sx={{maxWidth: 350, padding: 2, height: 500}}>
        <CardHeader
        title={article.title} 
        subheader={<>
            {humanizeDate(article.createdAt)} - rate: {article.positioningRate}
        <br />
            {article.regionName && <Link href={`/region/${article.regionName}`} underline="hover" sx={{ color: 'inherit' }}>
              From {article.regionName}
            </Link>}
        </>}
        action={
          <Badge
            badgeContent={
                article.isVisible ? <Visibility sx={{color: 'primary.main'}}/> : <VisibilityOff />
            }
          />
        }/>

        <CardMedia
        component='img'
        height={200}
        image={article.imageURL || "/img/no-image.png"}
        alt={`image ${article.title}`}/>

        <CardContent>
            <Typography variant="body2">
                {article.description.substring(0, 150) + "..."}
            </Typography>
        </CardContent>

        <CardActions>
            <Button size="small" href={article.sourceLink} target="_blank">Read more</Button>
            <Button size="small" variant="contained" color="secondary" onClick={handleEdit}>Edit</Button>
            <Button size="small" variant="contained" color="error" onClick={handleDelete}>Delete</Button>
        </CardActions>
    </Card>;
}

export default ArticleItem;