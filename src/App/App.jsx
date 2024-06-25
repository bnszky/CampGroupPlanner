import Footer from '../components/Footer/Footer'
import Navbar from '../components/Navbar/Navbar'
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import ProtectedRoute from './ProtectedRoute';
import ArticlesFeed from '../pages/ArticlesFeed/ArticlesFeed'
import CreateArticle from '../pages/CreateArticle/CreateArticle'
import CreateAttraction from '../pages/CreateAttraction/CreateAttraction'
import RegionsFeed from '../pages/RegionsFeed/RegionsFeed';
import CreateRegion from '../pages/CreateRegion/CreateRegion'
import CreateReview from '../pages/CreateReview/CreateReview'
import RegionPage from '../pages/RegionPage/RegionPage'
import './App.css'
import { Box } from '@mui/material'
import EditArticle from '../pages/EditArticle/EditArticle';
import EditRegion from '../pages/EditRegion/EditRegion';
import EditAttraction from '../pages/EditAttraction/EditAttraction';
import AttractionsFeed from '../pages/AttractionsFeed/AttractionsFeed';
import { AuthProvider, useAuth } from '../components/AuthProvider/AuthContext';
import LoginPage from '../pages/LoginPage/LoginPage';
import RegisterPage from '../pages/RegisterPage/RegisterPage';
import ReviewsFeed from '../pages/ReviewsFeed/ReviewsFeed';
import ConfirmEmail from '../pages/ConfirmEmail/ConfirmEmail';
import RecoverPassword from '../pages/RecoverPassword/RecoverPassword';
import ResetPassword from '../pages/ResetPassword/ResetPassword';

function App() {

  return (
    <AuthProvider>
      
      <Box sx={{
        display: 'flex',
        flexDirection: 'column',
        minHeight: '100vh',
      }}>

      <Box sx={{flex: 1,
        padding: '2rem',
        paddingBottom: '5rem'}}>
      <BrowserRouter>
      <Navbar/>
      <Routes>
        <Route index element={<ArticlesFeed/>} />
        <Route path="*" element={<Navigate to="/" />} />
        <Route path="/articles" element={<ArticlesFeed />} />
        <Route path="/region" element={<RegionsFeed />} />
        <Route path="/region/:regionName" element={<RegionPage />} />
        <Route path="/attraction" element={<AttractionsFeed />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/confirm-email" element={<ConfirmEmail />} />
        <Route path="/recover-password" element={<RecoverPassword />} />
        <Route path="/reset-password" element={<ResetPassword />} />

        <Route element={<ProtectedRoute isLoggedInRequired={true}/>}>
          <Route path="/review/user" element={<ReviewsFeed />} />
          <Route path="/review/create/:regionName" element={<CreateReview />} />
        </Route>

        <Route element={<ProtectedRoute isAdminRequired={true}/>}>
          <Route path="/articles/create" element={<CreateArticle />} />
          <Route path="/articles/edit/:id" element={<EditArticle />} />
          <Route path="/region/create" element={<CreateRegion />} />
          <Route path="/region/edit/:regionName" element={<EditRegion />} />
          <Route path="/attraction/create" element={<CreateAttraction />} />
          <Route path="/attraction/edit/:id" element={<EditAttraction />} />
        </Route>
      </Routes>
    </BrowserRouter>
    </Box>
      <Footer/>
      </Box>
    </AuthProvider>
  )
}

export default App
