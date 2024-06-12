import React from "react";
import ArticlesList from "../../components/ArticlesList/ArticlesList.jsx"
import { Alert, Typography, Box } from "@mui/material";
import useDataFeed from "../../hooks/useDataFeed.jsx";
import { useLocation } from 'react-router-dom';

export default function ArticlesFeed() {

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
        {isLoading ? (
            <Typography variant="h2">Loading...</Typography>
        ) : (
            <ArticlesList articles={articles} handleDelete={handleDelete} handleEdit={handleEdit} />
        )}
        </Box>
    );
}