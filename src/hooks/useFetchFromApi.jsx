import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import axios from 'axios';

const useFetchFromApi = () => {
  const navigate = useNavigate();
  const [isFetching, setIsFetching] = useState(false);

  const token = localStorage.getItem('token');
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

  async function fetchData(apiUrl) {
    setIsFetching(true);
    try {
      const response = await axios.get(apiUrl);
      console.log(response);
      navigate("", {
        state: { infoMsg: { type: 'success', msg: `Fetched ${response.data.length} items successfully` } },
      });
      setIsFetching(false);
      window.location.reload()
    } catch (error) {
      console.error(error.message);
      if(error.response){
        if(error.response.status === 401 || error.response.status === 403){
            setMessage("Couldn't allow, no access rights")
        } else {
            setMessage(error.response.data?.title ? error.response.data.title : "Couldn't fetch data");
        }
      }
      navigate("", {
        state: { infoMsg: { type: 'error', msg: "Couldn't fetch data" } },
      });
      setIsFetching(false);
      window.location.reload()
    }
  }

  return {
    fetchData, isFetching
  };
};

export default useFetchFromApi;
