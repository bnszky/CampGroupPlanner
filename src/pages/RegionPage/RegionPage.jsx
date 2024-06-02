import * as React from 'react';
import RegionInfo from '../../components/RegionInfo/RegionInfo';
import { Divider } from '@mui/material';
import AttractionsInfo from '../../components/AttractionsInfo/AttractionsInfo';
import ArticlesList from '../../components/ArticlesList/ArticlesList';
import ReviewList from '../../components/ReviewList/ReviewList';
import { useNavigate, useParams } from 'react-router-dom';

export default function RegionPage({reviews}) {

  const {regionName} = useParams();
  const navigate = useNavigate();
  const [region, setRegion] = React.useState({
    name: '',
    description: '',
    country: '',
    cities: [],
    images: []
  });
  const [attractions, setAttractions] = React.useState([]);
  const [articles, setArticles] = React.useState([]);

  async function getData(){
    try{
      const response = await fetch(`/api/region/${regionName}`);
      const data = await response.json();
      setRegion(data);
    }
    catch{
      navigate(
        '/region',
        {state: { infoMsg: {type: 'error', msg: "Couldn't find description for this region"}} }
      );
    }
  }

  async function getAttractions(){
    try{
      const response = await fetch(`/api/attraction/region/${regionName}`);
      const data = await response.json();
      setAttractions(data);
    }
    catch(error){
      console.error(error.message);
    }
  }

  async function getArticles(){
    try{
      const response = await fetch(`/api/articles/region/${regionName}`);
      const data = await response.json();
      setArticles(data);
    }
    catch(error){
      console.error(error.message);
    }
  }

  React.useEffect(() => {getData(); getAttractions(); getArticles();}, []);

  return <>
    <RegionInfo region={region}/>
    <Divider sx={{margin: 10, backgroundColor: 'black'}}/>
    <AttractionsInfo attractions={attractions}/>
    <Divider sx={{margin: 10, backgroundColor: 'black'}}/>
    <ArticlesList articles={articles} regionName={region.name}/>
    <Divider sx={{margin: 10, backgroundColor: 'black'}}/>
    <ReviewList reviews={reviews}/>
  </>
}