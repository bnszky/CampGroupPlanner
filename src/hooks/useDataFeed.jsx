import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';

const useDataFeed = (apiUrl, editPath, listPath) => {
  const location = useLocation();
  const [infoMsg, setInfoMsg] = useState(location.state?.infoMsg);
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const token = localStorage.getItem('token');
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

  const handleEdit = (identifier) => {
    navigate(`${editPath}/${identifier}`);
  };

  const handleVisit = async (identifier) => {
    navigate(`${listPath}/${identifier}`);
  }

  const handleDelete = async (identifier) => {
    try {
      const response = await axios.delete(`${apiUrl}/${identifier}`);

      if (!response.status === 200) {
        const errorData = response.data;
        setError(errorData);
        throw new Error(errorData);
      }
      setError(null);

      await getData();
      navigate(listPath, {
        state: { infoMsg: { type: 'success', msg: `Item with ${identifier} successfully deleted` } },
      });
      window.location.reload();
    } catch (error) {
      console.error(error.message);
      navigate(listPath, {
        state: { infoMsg: { type: 'error', msg: `Item with ${identifier} couldn't be deleted` } },
      });
      window.location.reload();
    }
  };

  async function getData() {
    setIsLoading(true);
    try {
      const response = await axios.get(apiUrl);
      setError(null);
      setData(response.data);
    } catch (error) {
      console.error(error.message);
      setError(error.message);
      setData([]);
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    getData();
  }, []);

  return {
    data,
    isLoading,
    infoMsg,
    error,
    setInfoMsg,
    handleEdit,
    handleDelete,
    handleVisit
  };
};

export default useDataFeed;
