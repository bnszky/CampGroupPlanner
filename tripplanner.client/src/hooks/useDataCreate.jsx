import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const useDataCreate = (initialData, apiEndpoint, redirectPath, entityName, extraProcess = null) => {
  const [data, setData] = useState(initialData);
  const [errors, setErrors] = useState({});
  const [errorMsg, setErrorMsg] = useState(null);
  const [createdMsg, setCreatedMsg] = useState(null);
  const navigate = useNavigate();

  const handleSubmit = async () => {
    const formData = new FormData();
    console.log(data);
    for (const key in data) {
      if (data[key] !== null && data[key] !== undefined) {
        if (Array.isArray(data[key])) {
          data[key].forEach((item, index) => formData.append(`${key}[${index}]`, item));
        } else {
          // Check if the data is a number (float or double)
          if (typeof data[key] === 'number' && !Number.isInteger(data[key])) {
            const value = data[key].toString().replace('.', ',');
            formData.append(key, value);
          } else {
              formData.append(key, data[key]);
          }
        }
      }
    }

    console.log(formData);

    if (extraProcess) {
      await extraProcess(formData);
    }

    try {
      const token = localStorage.getItem('token');
      const response = await axios({
        method: initialData.id ? 'PUT' : 'POST',
        url: `${apiEndpoint}${initialData.id ? `/${initialData.id}` : ''}`,
        data: formData,
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'multipart/form-data'
        }
      });

      setErrors({});
      setErrorMsg('');
      setCreatedMsg(`${entityName} ${initialData.id ? 'edited' : 'created'} successfully`);
      navigate(redirectPath, {
        state: { infoMsg: { type: 'success', msg: `${entityName} ${data.name || data.title} ${initialData.id ? 'edited' : 'created'} successfully` } },
      });
    } catch (error) {
      console.error(error);

      if(error.response && error.response.data){
        const result = error.response.data;
        if (result.errors) {
          const _errors = {};
          for (const errorKey in result.errors) {
            if (result.errors.hasOwnProperty(errorKey)) {
              _errors[errorKey] = result.errors[errorKey][0];
            }
          }
          console.error(_errors)
          setErrors(_errors);
        }
        if (result.title) {
          setErrorMsg(result.title);
        }
      }
      else{
        setErrorMsg('An unexpected error occurred');
      }
    }
  };

  return {
    data,
    setData,
    errors,
    errorMsg,
    createdMsg,
    handleSubmit,
  };
};

export default useDataCreate;
