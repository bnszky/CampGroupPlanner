import humanizeDate from "../HumanDate/HumanDate";
import "./ArticleItem.css" 

import Button from "@mui/material/Button";
import { Card, CardActions, CardContent, CardHeader, CardMedia, Typography, Link, IconButton, Badge } from "@mui/material";
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { useAuth } from "../AuthProvider/AuthContext";

function ArticleItem({article, handleDelete, handleEdit}) {

    const { isAdmin } = useAuth();

    return <Card sx={{maxWidth: 350, padding: 2, height: 550, border: .5, borderRadius: 10}}>
        <CardHeader
        title={article.title} 
        subheader={<>
            {humanizeDate(article.createdAt)}
            {isAdmin && (
                <Typography variant="body1" component="span" sx={{ fontWeight: 'bold', fontSize: '1.2em' }}>
                    - rate: {article.positioningRate}
                </Typography>
            )}
        <br />
            {article.regionName && <Link href={`/region/${article.regionName}`} underline="hover" sx={{ color: 'inherit' }}>
              From {article.regionName}
            </Link>}
        </>}
        action={isAdmin &&
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
                {article.description && article.description.substring(0, 150) + "..."}
            </Typography>
        </CardContent>

        <CardActions>
            <Button size="small" href={article.sourceLink} target="_blank">Read more</Button>
            {isAdmin && <> <Button size="small" variant="contained" color="secondary" onClick={handleEdit}>Edit</Button>
            <Button size="small" variant="contained" color="error" onClick={handleDelete}>Delete</Button> </>}
        </CardActions>
    </Card>;
}

export default ArticleItem;