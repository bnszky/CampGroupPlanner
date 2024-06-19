import * as React from 'react'
import { Card, CardActionArea, CardContent, CardMedia, Typography, CardActions, Button } from '@mui/material';
import { useAuth } from '../AuthProvider/AuthContext';

function AttractionCard({attraction, handleDelete, handleEdit}) {

    const {isAdmin} = useAuth();

    return <Card sx={{margin: 2, border: .5, borderRadius: 10}}>
        <CardActionArea>
            <CardMedia
                component="img"
                height={200}
                image={attraction.imageURL != null ? attraction.imageURL : "/img/no-image.png"}
                alt="image ${attraction.name}"
                sx={{borderRadius: 0}}
            />
            <CardContent>
                <Typography variant='h5'>{attraction.name}</Typography>
                <Typography variant='body2'>{attraction.description}</Typography>
            </CardContent>

            {isAdmin && <CardActions>
                <Button size="small" variant="contained" color="secondary" onClick={handleEdit}>Edit</Button>
                <Button size="small" variant="contained" color="error" onClick={handleDelete}>Delete</Button>
            </CardActions>}
        </CardActionArea>
    </Card>;
}

export default AttractionCard;