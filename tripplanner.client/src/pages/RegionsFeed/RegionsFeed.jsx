import { useEffect, useState } from "react"
import { Alert, Typography, Box } from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";
import RegionList from "../../components/RegionList/RegionList.jsx";

export default function RegionsFeed(){

    const location = useLocation();
    const [infoMsg, setInfoMsg] = useState(location.state?.infoMsg);

    const navigate = useNavigate();

    const [regions, setRegions] = useState(null);

    const handleEdit = (regionName) => {
        navigate(`/region/edit/${regionName}`);
    }

    const handleDelete = async (regionName) => {
        try {
            const response = await fetch(`/api/region/${regionName}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData);
            }

            await getData();
            navigate('/region', {state: {infoMsg: {type: 'success', msg: `Region ${regionName} successfully deleted`}}});
            window.location.reload();
        } catch (error) {
            console.log(error.message);
            navigate('/region', {state: {infoMsg: {type: 'errror', msg: `Region ${id} couldn't be deleted`}}});
            window.location.reload();
        }
    };

    const handleVisit = (regionName) => {
        navigate(`/region/${regionName}`);
    }

    async function getData(){
        try{
            const response = await fetch('api/region');
            const data = await response.json();
            setRegions(data);
        }
        catch (error){
            console.error(error.message);
            setRegions([]);
        }
    }

    useEffect(() => {getData();}, []);

    return <Box>
        {infoMsg && <Alert severity={infoMsg.type} variant="outlined" onClose={() => {setInfoMsg(null); navigate('.');}} sx={{mb: 2}}>{infoMsg.msg}</Alert>}
        {regions ? <RegionList regions={regions} handleDelete={handleDelete} handleEdit={handleEdit} handleVisit={handleVisit}/> : <Typography variant="h2">Loading...</Typography>}
        </Box>
}