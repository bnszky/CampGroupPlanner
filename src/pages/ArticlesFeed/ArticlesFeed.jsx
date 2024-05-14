import "./ArticlesFeed.css"
import { useEffect, useState } from "react"
import ArticlesList from "../../components/ArticlesList/ArticlesList.jsx"
import { Alert, Typography, Box } from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";

export default function ArticlesFeed(){

    const location = useLocation();
    const [infoMsg, setInfoMsg] = useState(location.state?.infoMsg);

    const navigate = useNavigate();

    const [articles, setArticles] = useState(null);

    const handleEdit = (id) => {
        return id;
    }

    const handleDelete = async (id) => {
        try {
            const response = await fetch(`/api/articles/${id}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData);
            }

            await getData();
            navigate('/articles', {state: {infoMsg: {type: 'success', msg: `Article ${id} successfully deleted`}}});
            window.location.reload();
        } catch (error) {
            console.log(error.message);
        }
    };

    async function getData(){
        const response = await fetch('api/articles');
        const data = await response.json();
        setArticles(data);
    }

    useEffect(() => {getData();}, []);

    return <Box>
        {infoMsg && <Alert severity={infoMsg.type} variant="outlined" onClose={() => {setInfoMsg(null); navigate('.');}} sx={{mb: 2}}>{infoMsg.msg}</Alert>}
        {articles ? <ArticlesList articles={articles} handleDelete={handleDelete} handleEdit={handleEdit}/> : <Typography variant="h2">Loading...</Typography>}
        </Box>
}