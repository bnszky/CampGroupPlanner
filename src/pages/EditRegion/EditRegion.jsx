import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CreateRegion from '../CreateRegion/CreateRegion';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box } from '@mui/material';

const EditRegion = () => {
    const { regionName } = useParams();
    const [regionData, setRegionData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigate = useNavigate()

    useEffect(() => {
        const fetchRegionData = async () => {
            try {
                const response = await fetch(`/api/region/${regionName}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch region data');
                }
                const data = await response.json();
                setRegionData(data);
            } catch (error) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchRegionData();
    }, [regionName]);

    if (loading) {
        return <Box sx={{ display: 'flex' }}>
            <CircularProgress />
        </Box>
    }

    if (error) {
        navigate(
            '/region',
            {state: { infoMsg: {type: 'error', msg: error}} }
        );
        return <Alert variant='outlined' severity='error'>Error: {error}</Alert>
    }

    return <CreateRegion regionData={regionData} />;
};

export default EditRegion;
