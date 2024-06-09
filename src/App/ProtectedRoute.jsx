import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../components/AuthProvider/AuthContext";
import { CircularProgress } from "@mui/material";

const ProtectedRoute = ({ isAdminRequired, isLoggedInRequired }) => {
  const { isAdmin, isLoading, isLoggedIn } = useAuth();
  console.log("Admin: " + isAdmin)

  if(isLoading){
    return <CircularProgress/>
  }

  if (isAdminRequired && !isAdmin) {
    return <Navigate to="/" />;
  }

  if(isLoggedInRequired && !isLoggedIn){
    return <Navigate to="/" />;
  }

  return <Outlet />;
};

export default ProtectedRoute;
