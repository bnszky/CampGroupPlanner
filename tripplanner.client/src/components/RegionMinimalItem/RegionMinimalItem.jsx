import { Button, Card, CardActions, CardContent, CardHeader, CardMedia, Typography } from "@mui/material";
import { useAuth } from "../AuthProvider/AuthContext";

function RegionMinimalItem({region, handleEdit, handleDelete, handleVisit}) {

    const {isAdmin} = useAuth();

    return <Card sx={{maxWidth: 350, padding: 2, height: 500, border: .5, borderRadius: 10}}>
        <CardHeader
        title={region.name} />

        <CardMedia
        component='img'
        height={180}
        image={region?.images ? region.images[0] : region.image}
        alt={`image ${region.name}`}/>

        <CardContent>
            <Typography variant="body2">
                {region.description.substring(0, 400) + "..."}
            </Typography>
        </CardContent>

        {handleEdit && <CardActions>
            <Button size="small" onClick={handleVisit}>Show</Button>
            {isAdmin && <><Button size="small" variant="contained" color="secondary" onClick={handleEdit}>Edit</Button>
            <Button size="small" variant="contained" color="error" onClick={handleDelete}>Delete</Button></>}
        </CardActions>}
    </Card>;
}

export default RegionMinimalItem;