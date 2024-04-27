import { Button, Grid, Avatar, Card, CardContent, CardHeader, Rating, Typography, CardActions } from "@mui/material";
import humanizeDate from "../HumanDate/HumanDate";

function ReviewContainer({review}) {
    return <Grid item xs={12}>
            <Card width="100%" sx={{padding: 3}}>
            <CardHeader
            avatar={
                <Avatar src={review.author.profileImage} alt={review.author.nick}/>
            }
            title={review.title}
            subheader={<Typography variant="subtitle" sx={{fontStyle: 'italic'}}>
                {review.author.nick}
            </Typography>}
            action={
                <Grid container direction='column' sx={{display: 'flex'}}>
                <Rating name="read-only" value={review.rate} readOnly/>
                <Typography variant="body2">{humanizeDate(review.createdAt)}</Typography>
                </Grid>    
            }
            />

            <CardContent>
                <Typography variant="body1">
                    {review.text}
                </Typography>
            </CardContent>

            <CardActions>
                <Button size="small" variant="contained" color="secondary" href="#EditPost">Edit</Button>
                <Button size="small" variant="contained" color="error" href="#DeletePost">Delete</Button>
            </CardActions>
        </Card>
    </Grid>;
}

export default ReviewContainer;