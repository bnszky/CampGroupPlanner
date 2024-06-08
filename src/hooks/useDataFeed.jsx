import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

const useDataFeed = (apiUrl, editPath, listPath) => {
  const location = useLocation();
  const [infoMsg, setInfoMsg] = useState(location.state?.infoMsg);
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  const handleEdit = (identifier) => {
    navigate(`${editPath}/${identifier}`);
  };

  const handleVisit = async (identifier) => {
    navigate(`${listPath}/${identifier}`);
  }

  const handleDelete = async (identifier) => {
    try {
      const response = await fetch(`${apiUrl}/${identifier}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData);
      }

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
      const response = await fetch(apiUrl);
      const result = await response.json();
      setError(null);
      setData(result);
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
    setInfoMsg,
    handleEdit,
    handleDelete,
    handleVisit
  };
};

export default useDataFeed;
