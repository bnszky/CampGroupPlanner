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
        return {"isOk": true, "msg": "Succesfully login", "isAccountCreated": true}
      }
    } catch (error) {
      console.error(error.message);
      if (error.response && error.response.status === 403) {
        return {"isOk": false, "msg": "Couldn't login. Your email is not confirmed", "isAccountCreated": true}
      }
      return {"isOk": false, "msg": "Couldn't login. Make sure your password and email are correct", "isAccountCreated": false}
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
        return { isOk: true, msg: "Successfully registered, check your email and confirm your account" };
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

  const confirmEmail = async (email, token) => {
    try {
      const response = await axios.get('/api/auth/confirm-email', {
        params: { email, token }
      });
      if (response.status === 200) {
        return { isOk: true, msg: "Email successfully confirmed" };
      }
    } catch (error) {
      console.error('Email confirmation failed', error);
      return { isOk: false, msg: `Email confirmation failed: resend email confirmation link and try again` };
    }
  };

  const resendConfirmationLink = async (email) => {
    console.log("sth")
    try {
      const response = await axios.get('/api/auth/resend-confirmation-link', {
        params: { email }
      });
      if (response.status === 200) {
        return { isOk: true, msg: "Confirmation link has been sent again" };
      }
    } catch (error) {
      console.error('Confirmation link send failed', error);
      return { isOk: false, msg: `Confirmation link send failed, check email is correct` };
    }
  }

  const recoverPassword = async (email) => {
    try {
      const response = await axios.get('/api/auth/recover-password', {
        params: { email }
      });
      if (response.status === 200) {
        return { isOk: true, msg: "Recovery email sent" };
      }
    } catch (error) {
      console.error('Password recovery failed', error);
      return { isOk: false, msg: `Password recovery failed. This email is not related to any account` };
    }
  };

  const resetPassword = async (email, token, password, confirmedPassword) => {
    if (password !== confirmedPassword) {
      return { isOk: false, msg: "Reset failed: \n > Passwords do not match" };
    }

    try {
      const response = await axios.put('/api/auth/reset-password', { email, token, password, confirmedPassword });
      if (response.status === 200) {
        return { isOk: true, msg: "Password successfully reset" };
      }
    } catch (error) {
      console.error('Password reset failed', error);
      if (error.response && error.response.data) {
        let errMsg = "Password reset failed:\n";
        error.response.data.slice(0, 3).forEach(err => {
          errMsg += `> ${err.description}\n`;
        });
        return { isOk: false, msg: errMsg.trim() };
      }
      return { isOk: false, msg: `Password reset failed` };
    }
  };

  const validateToken = async (email, token, purpose) => {
    try {
      await axios.get('/api/auth/validate-token', { params: { email, token, purpose } });
      return true;
    } catch (error) {
      console.error("Token validation failed", error);
      return false;
    }
  }

  return (
    <AuthContext.Provider value={{ isLoggedIn, ...userDetails, login, logout, register, isLoading, confirmEmail, recoverPassword, resetPassword, resendConfirmationLink, validateToken }}>
      {children}
    </AuthContext.Provider>
  );
};
