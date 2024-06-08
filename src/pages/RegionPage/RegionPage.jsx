import * as React from 'react';
import RegionInfo from '../../components/RegionInfo/RegionInfo';
import { Divider, Typography } from '@mui/material';
import AttractionsInfo from '../../components/AttractionsInfo/AttractionsInfo';
import ArticlesList from '../../components/ArticlesList/ArticlesList';
import ReviewList from '../../components/ReviewList/ReviewList';
import { useNavigate, useParams } from 'react-router-dom';
import useDataFeed from '../../hooks/useDataFeed';

export default function RegionPage({reviews}) {

  const {regionName} = useParams();
  const navigate = useNavigate();
  
  const {
    data: region,
    isLoading: isRegionLoading,
    error: regionError,
  } = useDataFeed(`/api/region/${regionName}`, '/region/edit', '/region');

  const {
    data: articles,
    isLoading: areArticlesLoading,
    handleEdit: handleEditArticle,
    handleDelete: handleDeleteArticle
  } = useDataFeed(`/api/articles/region/${regionName}`, '/articles/edit', '/articles');

  const {
    data: attractions,
    isLoading: areAttractionsLoading,
    handleEdit: handleEditAttraction,
    handleDelete: handleDeleteAttraction
  } = useDataFeed(`/api/attraction/region/${regionName}`, '/attraction/edit', '/attraction');

  if (regionError) {
    navigate('/region', {
      state: { infoMsg: { type: 'error', msg: "Couldn't find region with this name" } },
    });
  }

  return (
    <>
      {isRegionLoading || areAttractionsLoading || areArticlesLoading || regionError ? (
        <Typography variant="h2">Loading...</Typography>
      ) : (
        <>
          <RegionInfo region={region} />
          <Divider sx={{ margin: 10, backgroundColor: 'black' }} />

          {attractions.length != 0 && <>
          <AttractionsInfo attractions={attractions} handleDelete={handleDeleteAttraction} handleEdit={handleEditAttraction}/>
          <Divider sx={{ margin: 10, backgroundColor: 'black' }} /> </>}

          {articles.length != 0 && <>
          <ArticlesList articles={articles} handleDelete={handleDeleteArticle} handleEdit={handleEditArticle} regionName={region.name} />
          <Divider sx={{ margin: 10, backgroundColor: 'black' }} /> </>}

          <ReviewList reviews={reviews} />
        </>
      )}
    </>
  );
}