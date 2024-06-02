import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box } from '@mui/material';
import CreateArticle from '../CreateArticle/CreateArticle';

const EditArticle = () => {
    const { id } = useParams();
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch(`/api/articles/${id}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch articles data');
                }
                const data = await response.json();
                setData(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [id]);

    if (loading) {
        return <Box sx={{ display: 'flex' }}>
            <CircularProgress />
        </Box>
    }

    if (error) {
        navigate(
            '/articles',
            {state: { infoMsg: {type: 'error', msg: error}} }
        );
        return <Alert variant='outlined' severity='error'>Error: {error}</Alert>
    }

    return <CreateArticle initialData={data} />;
};

export default EditArticle;
