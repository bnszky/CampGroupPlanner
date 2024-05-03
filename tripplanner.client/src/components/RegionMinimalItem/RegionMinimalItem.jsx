import { Card, CardActions, CardContent, CardHeader, CardMedia, Typography } from "@mui/material";

function RegionMinimalItem({region}) {

    return <Card sx={{maxWidth: 350, padding: 2, height: 500}}>
        <CardHeader
        title={region.name} />

        <CardMedia
        component='img'
        height={200}
        image={region.images[0]}
        alt={`image ${region.name}`}/>

        <CardContent>
            <Typography variant="body2">
                {region.description.substring(0, 400) + "..."}
            </Typography>
        </CardContent>
    </Card>;
}

export default RegionMinimalItem;