import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box } from '@mui/material';
import CreateAttraction from '../CreateAttraction/CreateAttraction';
import useDataFeed from '../../hooks/useDataFeed';

const EditAttraction = () => {
    const { id } = useParams();
    const navigate = useNavigate()

    const {
        isLoading,
        data,
        error
    } = useDataFeed (
        `/api/attraction/${id}`,
        '', 'attraction'
    )

    if (isLoading) {
        return <Box sx={{ display: 'flex' }}>
            <CircularProgress />
        </Box>
    }

    if (error) {
        navigate(
            '/attraction',
            {state: { infoMsg: {type: 'error', msg: error}} }
        );
    }

    return <CreateAttraction initialAttractionData={data} />;
};

export default EditAttraction;
