import { useEffect, useState } from "react"
import AttractionsList from "../../components/AttractionsList/AttractionsList";
import { Alert, Typography, Box } from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";

export default function AttractionsFeed(){

    const location = useLocation();
    const [infoMsg, setInfoMsg] = useState(location.state?.infoMsg);

    const navigate = useNavigate();

    const [attractions, setAttractions] = useState(null);

    const handleEdit = (id) => {
        navigate(`/attraction/edit/${id}`);
    }

    const handleDelete = async (id) => {
        try {
            const response = await fetch(`/api/attraction/${id}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData);
            }

            await getData();
            navigate('/attraction', {state: {infoMsg: {type: 'success', msg: `Attraction ${id} successfully deleted`}}});
            window.location.reload();
        } catch (error) {
            console.log(error.message);
            navigate('/attraction', {state: {infoMsg: {type: 'errror', msg: `Attraction ${id} couldn't be deleted`}}});
            window.location.reload();
        }
    };

    async function getData(){
        try{
            const response = await fetch('api/attraction');
            const data = await response.json();
            setAttractions(data);
        }
        catch (error){
            console.error(error.message);
            setAttractions([]);
        }
    }

    useEffect(() => {getData();}, []);

    return <Box sx={{maxWidth: 800, margin: 'auto'}}>
        {infoMsg && <Alert severity={infoMsg.type} variant="outlined" onClose={() => {setInfoMsg(null); navigate('.');}} sx={{mb: 2}}>{infoMsg.msg}</Alert>}
        {attractions ? <AttractionsList attractions={attractions} handleDelete={handleDelete} handleEdit={handleEdit}/> : <Typography variant="h2">Loading...</Typography>}
        </Box>
}