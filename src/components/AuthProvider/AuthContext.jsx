import React, { createContext, useState, useContext, useEffect } from 'react';
import axios from 'axios';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userDetails, setUserDetails] = useState({ username: '', email: '', isAdmin: false });

  const checkAuth = async () => {
    try {
      const token = localStorage.getItem('token');
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      const response = await axios.get('/api/auth/info');
      if (response.status === 200) {
        const { username, isAdmin } = response.data;
        setIsLoggedIn(true);
        setUserDetails({ username, email: '', isAdmin });
      }
    } catch (error) {
      setIsLoggedIn(false);
      setUserDetails({ username: '', email: '', isAdmin: false });
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  const login = async (credentials) => {
    try {
      const response = await axios.post('/api/auth/login', credentials);
      if (response.status === 200) {
        const { token, username, email, isAdmin } = response.data;
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        checkAuth();
      }
    } catch (error) {
      console.error('Login failed', error);
    }
  };

  const logout = async () => {
    try {
      await axios.post('/api/auth/logout');
      localStorage.removeItem('token');
      setIsLoggedIn(false);
      setUserDetails({ username: '', email: '', isAdmin: false });
      delete axios.defaults.headers.common['Authorization'];
    } catch (error) {
      console.error('Logout failed', error);
    }
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, ...userDetails, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
