import * as React from 'react';
import RegionInfo from '../../components/RegionInfo/RegionInfo';
import { Divider, Typography, Alert } from '@mui/material';
import AttractionsInfo from '../../components/AttractionsInfo/AttractionsInfo';
import ArticlesList from '../../components/ArticlesList/ArticlesList';
import ReviewList from '../../components/ReviewList/ReviewList';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import useDataFeed from '../../hooks/useDataFeed';
import { useAuth } from '../../components/AuthProvider/AuthContext';

export default function RegionPage() {

  const {isLoggedIn} = useAuth();

  const location = useLocation();
  const [infoMsg, setInfoMsg] = React.useState(location.state?.infoMsg || null);

  const {regionName} = useParams();
  const navigate = useNavigate();
  
  const {
    data: region,
    isLoading: isRegionLoading,
    error: regionError,
  } = useDataFeed(`/api/region/${regionName}`, '/region/edit', '/api/region', '/region');

  const {
    data: articles,
    isLoading: areArticlesLoading,
    handleEdit: handleEditArticle,
    handleDelete: handleDeleteArticle
  } = useDataFeed(`/api/articles/region/${regionName}`, '/articles/edit', '/api/articles', '/articles');

  const {
    data: attractions,
    isLoading: areAttractionsLoading,
    handleEdit: handleEditAttraction,
    handleDelete: handleDeleteAttraction
  } = useDataFeed(`/api/attraction/region/${regionName}`, '/attraction/edit', '/api/attraction', '/attraction');

  const {
    data: reviews,
    isLoading: areReviewsLoading,
    handleDelete: handleDeleteReview
  } = useDataFeed(`/api/review/region/name/${regionName}`, '', '/api/review', `/region/${regionName}`);

  if (regionError) {
    navigate('/region', {
      state: { infoMsg: { type: 'error', msg: "Couldn't find region with this name" } },
    });
  }

  return (
    <>
      {infoMsg && (
        <Alert severity={infoMsg.type} variant="outlined" onClose={() => setInfoMsg(null)} sx={{ mb: 2 }}>
          {infoMsg.msg}
        </Alert>
      )}

      {isRegionLoading || areAttractionsLoading || areArticlesLoading || areReviewsLoading || regionError ? (
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

          <ReviewList reviews={reviews} handleDelete={handleDeleteReview} addReview={isLoggedIn ? `/review/create/${regionName}` : null} />
        </>
      )}
    </>
  );
}