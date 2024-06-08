import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

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
          formData.append(key, data[key]);
        }
      }
    }

    console.log(formData);

    if (extraProcess) {
      await extraProcess(formData);
    }

    try {
      const response = await fetch(`${apiEndpoint}${initialData.id ? `/${initialData.id}` : ''}`, {
        method: initialData.id ? 'PUT' : 'POST',
        body: formData,
      });

      if (!response.ok) {
        const result = await response.json();
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
        return;
      }

      setErrors({});
      setErrorMsg('');
      setCreatedMsg(`${entityName} ${initialData.id ? 'edited' : 'created'} successfully`);
      navigate(redirectPath, {
        state: { infoMsg: { type: 'success', msg: `${entityName} ${data.name || data.title} ${initialData.id ? 'edited' : 'created'} successfully` } },
      });
    } catch (error) {
      console.error(error);
      setErrorMsg('An unexpected error occurred');
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
