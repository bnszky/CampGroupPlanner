import { Grid, Stack, Chip, Button, Typography } from "@mui/material";
import ReviewContainer from "../ReviewContainer/ReviewContainer";
import { useEffect, useState } from "react";

function ReviewList({reviews, handleDelete, addReview}) {

    const nextItemsNumber = 3;
    const [reviewsToShow, setReviewsToShow] = useState(reviews.slice(0, nextItemsNumber))
    const [variants, setVariants] = useState(['outlined', 'outlined', 'outlined'])

    function setVariant(index){
        let _variants = ['outlined', 'outlined', 'outlined'];

        if(index > 2){
            index = 0;
        }
        _variants[index] = 'contained';

        setVariants(_variants);
    }

    function sortReviews(sortingType){

        switch(sortingType){
            case 1:
                reviews = reviews.sort((a, b) => b.rate - a.rate)
                break;
            case 2:
                reviews = reviews.sort((a, b) => a.rate - b.rate)
                break;
            default:
                reviews = reviews.sort((a, b) => b.createdAt - a.createdAt)
                break;
        }

        setVariant(sortingType);

        setReviewsToShow(reviews.slice(0, reviewsToShow.length))
    }

    useEffect(() => {
        sortReviews(0);
    }, []);

    return <>
    <Grid container alignItems='center' justifyContent='space-between'>
        <Grid item>
            <Typography variant="h2" p={2}>Reviews</Typography>
        </Grid>
        <Grid item>
            {addReview != null &&
            <Button size="large" variant="contained" color="primary" href={addReview}>Add Review</Button>
            }
        </Grid>
    </Grid>
    {reviews.length > 0 && <Stack direction="row" spacing={2} p={2}>
        <Typography variant="subtitle1">Sort by</Typography>
        <Chip label="Latest" color="primary" variant={variants[0]} onClick={() => sortReviews(0)} />
        <Chip label="Highest" color="primary" variant={variants[1]} onClick={() => sortReviews(1)}/>
        <Chip label="Lowest" color="primary" variant={variants[2]} onClick={() => sortReviews(2)} />
    </Stack>}
    <Grid container spacing={5}>
        { reviews.length > 0 ?
            reviewsToShow.map(review => <ReviewContainer key={review.id} review={review} handleDelete={() => handleDelete(review.id)}/>) :
            <Typography variant="h4" sx={{pt: 8, px: 8}}>No reviews for this region, be the first one</Typography>
        }
    </Grid>
    {(reviewsToShow.length < reviews.length) &&
        <Grid container my={5} alignItems='center' justifyContent='center'>    
            <Button sx={{width: '300px'}} variant='outlined' color='primary'  onClick={() => setReviewsToShow(reviews.slice(0, reviewsToShow.length + nextItemsNumber))}>Show more</Button>
        </Grid>
    }
    </>;
}

export default ReviewList;