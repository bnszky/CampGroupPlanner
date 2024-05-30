import * as React from 'react';
import RegionInfo from '../../components/RegionInfo/RegionInfo';
import { Divider } from '@mui/material';
import AttractionsInfo from '../../components/AttractionsInfo/AttractionsInfo';
import ArticlesList from '../../components/ArticlesList/ArticlesList';
import ReviewList from '../../components/ReviewList/ReviewList';
import { useNavigate, useParams } from 'react-router-dom';

export default function RegionPage({attractions, articles, reviews}) {

  const {regionName} = useParams();
  const [region, setRegion] = React.useState({
    name: '',
    description: '',
    country: '',
    cities: [],
    images: []
  });

  async function getData(){
    const response = await fetch(`/api/region/${regionName}`);
    const data = await response.json();
    setRegion(data);
  }

  React.useEffect(() => {getData();}, []);

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