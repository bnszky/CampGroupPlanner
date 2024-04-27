import * as React from 'react';
import RegionInfo from '../../components/RegionInfo/RegionInfo';
import { Divider } from '@mui/material';
import AttractionsInfo from '../../components/AttractionsInfo/AttractionsInfo';
import ArticlesList from '../../components/ArticlesList/ArticlesList';

export default function RegionPage({region, attractions}) {
  return <>
    <RegionInfo region={region}/>
    <Divider sx={{margin: 10, backgroundColor: 'black'}}/>
    <AttractionsInfo attractions={attractions}/>
    <Divider sx={{margin: 10, backgroundColor: 'black'}}/>
    <ArticlesList/>
  </>
}