import React from "react";
import { Alert, Typography, Box } from "@mui/material";
import RegionList from "../../components/RegionList/RegionList.jsx";
import useDataFeed from "../../hooks/useDataFeed.jsx";
import { useLocation } from 'react-router-dom';

export default function RegionsFeed() {

  const location = useLocation();
  const [infoMsg, setInfoMsg] = React.useState(location.state?.infoMsg || null);
    const {
      data: regions,
      isLoading,
      handleEdit,
      handleDelete,
      handleVisit,
    } = useDataFeed('/api/region', '/region/edit', '/region');
  
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
          <RegionList regions={regions} handleDelete={handleDelete} handleEdit={handleEdit} handleVisit={handleVisit} />
        )}
      </Box>
    );
  }