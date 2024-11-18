import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import Cookies from 'js-cookie';
import "./css/start.css";

const StartPage = ({showAlert}) => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const { login: authLogin } = useContext(AuthContext);
  const { isAuthenticated, userRoles } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/dashboard');
    }
  }, [isAuthenticated, userRoles, navigate]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (!login || !password) {
      showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
      return;
    }

    const loginData = {
      login: login,
      password: password
    };

    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
      });

      if (response.ok) {
        const data = await response.json();
        Cookies.set('token', data.token, { expires: 7 });
        Cookies.set('refreshToken', data.refreshToken, { expires: 7 });
        authLogin(data.token, data.refreshToken);
        showAlert('Pomyślnie zalogowano!', 'success');
        navigate('/');
      } else {
        console.log('Błąd:', response.statusText);
        showAlert('Wprowadzono błędne dane logowania!', 'error');
      }
    } catch (error) {
      console.error('Wystąpił błąd:', error);
    }
  };

  return (
    <div className="start__page">
      <form className='login__form' onSubmit={handleSubmit}>

        <div className='login__wrap'>
          <label className='login__label'>Login</label>
          <input
            className='login__input' 
            type="text" 
            value={login} 
            onChange={(e) => setLogin(e.target.value)} 
          />
        </div>

        <div className='login__wrap'>
          <label className='login__label'>Password</label>
          <input 
            className='login__input' 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
          />
        </div>

        <div className='login__wrap'>
          <button className='button login__button' type="submit">Zaloguj się</button>
        </div>
        
      </form>
    </div>
  );
}

export default StartPage;
