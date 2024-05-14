import "./ArticlesFeed.css"
import { useEffect, useState } from "react"
import ArticlesList from "../../components/ArticlesList/ArticlesList.jsx"
import { Alert, Typography, Box } from "@mui/material";
import { useLocation } from "react-router-dom";

export default function ArticlesFeed(){

    const location = useLocation();
    const [infoMsg, setInfoMsg] = useState(location.state?.infoMsg);

    const [articles, setArticles] = useState(null);

    async function getData(){
        const response = await fetch('api/articles');
        const data = await response.json();
        setArticles(data);
    }

    useEffect(() => {getData();}, []);

    return <Box>
        {infoMsg && <Alert severity={infoMsg.type} variant="outlined" onClose={() => setInfoMsg(null)} sx={{mb: 2}}>{infoMsg.msg}</Alert>}
        {articles ? <ArticlesList articles={articles}/> : <Typography variant="h2">Loading...</Typography>}
        </Box>
}