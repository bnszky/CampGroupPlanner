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

function App() {

  const reviews = [
    {
      id: 1,
      author: {
        nick: "michali",
        profileImage: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGDohX4qAelLzi3t8vCfqccDFxifY-huxkmRrgnSRoig&s"
      },
      createdAt: new Date("04-14-2024 15:41:00"), 
      rate: 4.5,
      title: "Opinion 1",
      text: "Beautiful place! I would like to be there again"
    },
    {
      id: 2,
      author: {
        nick: "gombalo",
        profileImage: "https://static.vecteezy.com/system/resources/thumbnails/002/002/403/small/man-with-beard-avatar-character-isolated-icon-free-vector.jpg"
      },
      createdAt: new Date("07-18-2023 15:41:00"), 
      rate: 2,
      title: "Opinion 2",
      text: "I don't like spanish people, Ughh..."
    },
    {
      id: 3,
      author: {
        nick: "wealthyGuy",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("07-18-2023 15:41:00"), 
      rate: 5,
      title: "Opinion 3",
      text: "Very cheap region, I really recommend you to eat out"
    },
    {
      id: 4,
      author: {
        nick: "somebody",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("01-25-2024 15:41:00"), 
      rate: 1,
      title: "Opinion 4",
      text: "Hello! lorelorleroleolroelrlelrelo"
    },
    {
      id: 5,
      author: {
        nick: "Amanda",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("07-18-2020 15:41:00"), 
      rate: 3.5,
      title: "Opinion 5",
      text: "I'd say that's average"
    },
    {
      id: 6,
      author: {
        nick: "impolite Man",
        profileImage: "https://cdn1.vectorstock.com/i/1000x1000/73/15/female-avatar-profile-icon-round-woman-face-vector-18307315.jpg"
      },
      createdAt: new Date("07-18-2022 15:41:00"), 
      rate: 4.5,
      title: "Opinion 6",
      text: "Everything's good but you have too many 5 stars so that's why I give you 4 and half"
    }
  ]

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
        <Route path="/region/:regionName" element={<RegionPage reviews={reviews} />} />
        <Route path="/attraction" element={<AttractionsFeed />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/review/create/:regionName" element={<CreateReview />} />

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
