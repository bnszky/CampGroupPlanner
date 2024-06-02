import * as React from 'react'
import { Card, CardActionArea, CardContent, CardMedia, Typography, CardActions, Button } from '@mui/material';

function AttractionCard({attraction, handleDelete, handleEdit}) {
    return <Card sx={{border: 2, margin: 2}}>
        <CardActionArea>
            <CardMedia
                component="img"
                height={200}
                image={attraction.imageURL}
                alt="image ${attraction.name}"
                sx={{borderRadius: 0}}
            />
            <CardContent>
                <Typography variant='h5'>{attraction.name}</Typography>
                <Typography variant='body2'>{attraction.description}</Typography>
            </CardContent>

            <CardActions>
                <Button size="small" variant="contained" color="secondary" onClick={handleEdit}>Edit</Button>
                <Button size="small" variant="contained" color="error" onClick={handleDelete}>Delete</Button>
            </CardActions>
        </CardActionArea>
    </Card>;
}

export default AttractionCard;