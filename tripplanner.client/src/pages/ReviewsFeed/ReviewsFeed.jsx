import React from "react";
import { Typography, Box } from "@mui/material";
import useDataFeed from "../../hooks/useDataFeed";
import ReviewList from "../../components/ReviewList/ReviewList";

export default function ReviewsFeed(){

    const { data: reviews, isLoading, handleDelete } = useDataFeed('/api/review/user', '', '/api/review', '/review');

    return (
        <Box sx={{ maxWidth: 800, margin: 'auto' }}>
        {isLoading ? (
            <Typography variant="h2">Loading...</Typography>
        ) : (
            <ReviewList reviews={reviews} handleDelete={handleDelete} isRegion={true}/>
        )}
        </Box>
    );
}