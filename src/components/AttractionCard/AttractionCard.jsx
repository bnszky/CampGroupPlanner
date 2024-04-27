import * as React from 'react'
import { Card, CardActionArea, CardContent, CardMedia, Divider, Typography } from '@mui/material';

function AttractionCard({attraction}) {
    return <Card sx={{border: 2, margin: 2}}>
        <CardActionArea>
            <CardMedia
                component="img"
                height={200}
                image={attraction.image}
                alt="image ${attraction.name}"
                sx={{borderRadius: 0}}
            />
            <CardContent>
                <Typography variant='h5'>{attraction.name}</Typography>
                <Typography variant='body2'>{attraction.description}</Typography>
            </CardContent>
        </CardActionArea>
    </Card>;
}

export default AttractionCard;