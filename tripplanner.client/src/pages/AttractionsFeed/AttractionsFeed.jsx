import React from "react";
import AttractionsList from "../../components/AttractionsList/AttractionsList";
import { Alert, Typography, Box } from "@mui/material";
import useDataFeed from "../../hooks/useDataFeed";
import { useLocation } from 'react-router-dom';

export default function AttractionsFeed(){

    const location = useLocation();
    const [infoMsg, setInfoMsg] = React.useState(location.state?.infoMsg || null);
    const { data: attractions, isLoading, handleEdit, handleDelete } = useDataFeed('/api/attraction', '/attraction/edit', '/api/attraction', '/attraction');

    return (
        <Box sx={{ maxWidth: 800, margin: 'auto' }}>
        {infoMsg && (
            <Alert severity={infoMsg.type} variant="outlined" onClose={() => setInfoMsg(null)} sx={{ mb: 2 }}>
            {infoMsg.msg}
            </Alert>
        )}
        {isLoading ? (
            <Typography variant="h2">Loading...</Typography>
        ) : (
            <AttractionsList attractions={attractions} handleDelete={handleDelete} handleEdit={handleEdit} />
        )}
        </Box>
    );
}