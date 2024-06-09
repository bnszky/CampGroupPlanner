import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box } from '@mui/material';
import CreateArticle from '../CreateArticle/CreateArticle';
import useDataFeed from '../../hooks/useDataFeed';

const EditArticle = () => {
    const { id } = useParams();
    const navigate = useNavigate();

    const {
        isLoading,
        data,
        error
    } = useDataFeed (
        `/api/articles/${id}`,
        '', 'articles'
    )

    if (isLoading) {
        return <Box sx={{ display: 'flex' }}>
            <CircularProgress />
        </Box>
    }

    if (error) {
        navigate(
            '/articles',
            {state: { infoMsg: {type: 'error', msg: error}} }
        );
    }

    return <CreateArticle initialData={data} />;
};

export default EditArticle;
