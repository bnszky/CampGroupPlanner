import React from "react";
import ArticlesList from "../../components/ArticlesList/ArticlesList.jsx"
import { Alert, Typography, Box, Button } from "@mui/material";
import useDataFeed from "../../hooks/useDataFeed.jsx";
import { useLocation } from 'react-router-dom';
import { useAuth } from "../../components/AuthProvider/AuthContext.jsx";
import useFetchFromApi from "../../hooks/useFetchFromApi.jsx";
import MinimalPositivitySlider from "../../components/MinimalPositivitySlider/MinimalPositivitySlider.jsx";

export default function ArticlesFeed() {

    const {isAdmin} = useAuth();
    const {fetchData, isFetching} = useFetchFromApi();
    const location = useLocation();
    const [infoMsg, setInfoMsg] = React.useState(location.state?.infoMsg || null);
    const { data: articles, isLoading, handleEdit, handleDelete } = useDataFeed('/api/articles', '/articles/edit', '/api/articles', '/articles');

    return (
        <Box>
        {infoMsg && (
            <Alert severity={infoMsg.type} variant="outlined" onClose={() => setInfoMsg(null)} sx={{ mb: 2 }}>
            {infoMsg.msg}
            </Alert>
        )}
        {isAdmin &&
        <>
            <Button color="primary" variant="contained" sx={{mx: 1, my: 2}} onClick={async () => await fetchData(`/api/articles/fetch`)}>Fetch Articles</Button>
            <Button color="primary" variant="contained" sx={{mx: 1, my: 2}} onClick={async () => await fetchData(`/api/articles/rate-or-assign`)}>Assign Articles</Button>
            <MinimalPositivitySlider/>
        </>
        } 
        {isFetching && <Typography variant="h4">Fetching...</Typography>}
        {isLoading ? (
            <Typography variant="h2">Loading...</Typography>
        ) : (
            <ArticlesList articles={articles} handleDelete={handleDelete} handleEdit={handleEdit} />
        )}
        </Box>
    );
}