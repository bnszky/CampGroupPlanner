import React, { createContext, useState, useContext, useEffect } from 'react';
import axios from 'axios';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userDetails, setUserDetails] = useState({ username: '', email: '', isAdmin: false });
  const [isLoading, setIsLoading] = useState(true);

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
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    checkAuth();
  }, []);

  const login = async (username, password) => {
    try {
      const response = await axios.post('/api/auth/login', {username, password});
      if (response.status === 200) {
        const { token } = response.data;
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        await checkAuth();
        return {"isOk": true, "msg": "Succesfully login"}
      }
    } catch (error) {
      console.error(error.message);
      return {"isOk": false, "msg": "Couldn't login. Make sure your password and email are correct"}
    }
  };

  const register = async (username, email, password, confirmedPassword) => {
    if (password !== confirmedPassword) {
      return { isOk: false, msg: "Registration failed: \n > Passwords do not match" };
    }

    try {
      const response = await axios.post('/api/auth/register', { username, email, password, confirmedPassword });
      if (response.status === 200) {
        const { token } = response.data;
        localStorage.setItem('token', token);
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
        await checkAuth();
        return { isOk: true, msg: "Successfully registered" };
      }
    } catch (error) {
      console.log(error.response);
      if (error.response && error.response.data) {
        let errMsg = "Registration failed:\n";
        error.response.data.slice(0, 3).forEach(err => {
          errMsg += `> ${err.description}\n`;
        });
        return { isOk: false, msg: errMsg.trim() };
      }
      console.error('Registration failed', error);
      return { isOk: false, msg: `Registration failed` };
    }
  };

  const logout = async () => {
    try {
      await axios.post('/api/auth/logout');
      localStorage.removeItem('token');
      setIsLoggedIn(false);
      setUserDetails({ username: '', email: '', isAdmin: false });
      delete axios.defaults.headers.common['Authorization'];
      return {"isOk": true, "msg": "Succesfully logout"}
    } catch (error) {
      console.error('Logout failed', error);
      return {"isOk": false, "msg": `Logout failed ${error}`}
    }
  };

  return (
    <AuthContext.Provider value={{ isLoggedIn, ...userDetails, login, logout, register, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};
