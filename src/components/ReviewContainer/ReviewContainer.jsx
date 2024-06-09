import { Button, Grid, Avatar, Card, CardContent, CardHeader, Rating, Typography, CardActions } from "@mui/material";
import humanizeDate from "../HumanDate/HumanDate";
import { useAuth } from "../AuthProvider/AuthContext";

function ReviewContainer({review, handleDelete}) {

    console.log(review);
    const {username, isAdmin} = useAuth()

    return <Grid item xs={12}>
            <Card width="100%" sx={{padding: 3}}>
            <CardHeader
            avatar={
                <Avatar src="img/no-profile-image.png" alt={review.authorUsername.toUpperCase()} sx={{ bgcolor: "green" }}/>
            }
            title={review.title}
            subheader={<Typography variant="subtitle" sx={{fontStyle: 'italic'}} color={review.authorUsername === username ? "primary" : "inherit"}>
                {review.authorUsername === username ? "Me" : review.authorUsername}
            </Typography>}
            action={
                <Grid container direction='column' sx={{display: 'flex'}}>
                <Rating name="read-only" value={review.rate} precision={0.5} readOnly/>
                <Typography variant="body2">{humanizeDate(review.createdAt)}</Typography>
                </Grid>    
            }
            />

            <CardContent>
                <Typography variant="body1">
                    {review.text}
                </Typography>
            </CardContent>

            {isAdmin &&
            <CardActions>
                <Button size="small" variant="contained" color="error" onClick={handleDelete}>Delete</Button>
            </CardActions>}
        </Card>
    </Grid>;
}

export default ReviewContainer;