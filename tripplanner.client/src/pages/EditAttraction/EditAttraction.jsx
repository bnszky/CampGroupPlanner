import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box } from '@mui/material';
import CreateAttraction from '../CreateAttraction/CreateAttraction';

const EditAttraction = () => {
    const { id } = useParams();
    const [attractionData, setAttractionData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate()

    useEffect(() => {
        const fetchAttractionData = async () => {
            try {
                const response = await fetch(`/api/attraction/${id}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch attraction data');
                }
                const data = await response.json();
                setAttractionData(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchAttractionData();
    }, [id]);

    if (loading) {
        return <Box sx={{ display: 'flex' }}>
            <CircularProgress />
        </Box>
    }

    if (error) {
        navigate(
            '/attraction',
            {state: { infoMsg: {type: 'error', msg: error}} }
        );
        return <Alert variant='outlined' severity='error'>Error: {error}</Alert>
    }

    return <CreateAttraction initialAttractionData={attractionData} />;
};

export default EditAttraction;
