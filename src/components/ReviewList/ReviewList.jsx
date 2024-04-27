import { Grid, Button, Typography } from "@mui/material";
import ReviewContainer from "../ReviewContainer/ReviewContainer";

function ReviewList({reviews}) {
    return <>
    <Grid container alignItems='center' justifyContent='space-between'>
        <Grid item>
            <Typography variant="h2" p={2}>Reviews</Typography>
        </Grid>
        <Grid item>
            <Button size="large" variant="contained" color="primary" href="#AddPost">Add Review</Button>
        </Grid>
    </Grid>
    <Grid container spacing={5}>
        {reviews.map(review => <ReviewContainer key={review.id} review={review}/>)}
    </Grid></>;
}

export default ReviewList;