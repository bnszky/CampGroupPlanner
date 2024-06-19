import React, { useEffect, useState } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import CreateRegion from '../CreateRegion/CreateRegion';
import CircularProgress from '@mui/material/CircularProgress';
import { Alert, Box, Divider, Button, Typography } from '@mui/material';
import useDataFeed from '../../hooks/useDataFeed';
import AttractionsList from '../../components/AttractionsList/AttractionsList';
import { useAuth } from '../../components/AuthProvider/AuthContext';
import useFetchFromApi from '../../hooks/useFetchFromApi';

const EditRegion = () => {
    const { regionName } = useParams();
    const { isAdmin } = useAuth()
    const { fetchData, isFetching } = useFetchFromApi();
    const navigate = useNavigate()

    const location = useLocation();
    const [infoMsg, setInfoMsg] = React.useState(location.state?.infoMsg || null);
    const { data: attractions, isLoading: isLoadingAttractions, handleEdit, handleDelete } = useDataFeed(`/api/attraction/region/${regionName}`, '/attraction/edit', '/api/attraction', `/attraction/edit/${regionName}`);

    const {
        isLoading,
        data,
        error
    } = useDataFeed (
        `/api/region/${regionName}`,
        '', '', 'region'
    )

    if (isLoading || isLoadingAttractions) {
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

    return <>
        <CreateRegion regionData={data} />
        <Divider color="black" width="100%" sx={{margin: 5}}/>
        <Box sx={{ maxWidth: 800, margin: 'auto' }}>
            {infoMsg && (
                <Alert severity={infoMsg.type} variant="outlined" onClose={() => setInfoMsg(null)} sx={{ mb: 2 }}>
                {infoMsg.msg}
                </Alert>
            )}
            {isAdmin && 
                <Button color="secondary" variant="contained" sx={{mx: 1, my: 2}} onClick={async () => await fetchData(`/api/attraction/fetch/${regionName}`)}>Fetch Attractions</Button>
            } 
            {isFetching && <Typography variant="h4">Fetching...</Typography>}
            <AttractionsList attractions={attractions} handleDelete={handleDelete} handleEdit={handleEdit} />
        </Box>
    </>;
};

export default EditRegion;
