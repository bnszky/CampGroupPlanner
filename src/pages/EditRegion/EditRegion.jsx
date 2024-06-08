import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CreateRegion from '../CreateRegion/CreateRegion';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box } from '@mui/material';
import useDataFeed from '../../hooks/useDataFeed';

const EditRegion = () => {
    const { regionName } = useParams();
    const navigate = useNavigate()

    const {
        isLoading,
        data,
        error
    } = useDataFeed (
        `/api/region/${regionName}`,
        '', 'region'
    )

    if (isLoading) {
        return <Box sx={{ display: 'flex' }}>
            <CircularProgress />
        </Box>
    }

    if (error) {
        navigate(
            '/region',
            {state: { infoMsg: {type: 'error', msg: error}} }
        );
    }

    return <CreateRegion regionData={data} />;
};

export default EditRegion;
