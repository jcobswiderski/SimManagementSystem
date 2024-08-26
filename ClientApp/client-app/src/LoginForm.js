import { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import AuthContext from './AuthContext';
import "./loginForm.css";

const LoginForm = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const { login: authLogin } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();

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
        navigate('/');
      } else {
        console.log('Błąd:', response.statusText);
      }
    } catch (error) {
      console.error('Wystąpił błąd:', error);
    }
  };

  return ( 
    <form className='login__form' onSubmit={handleSubmit}>
      <div className='login__wrap'>
        <label className='login__label'>Login:</label>
        <input
          className='login__input' 
          type="text" 
          value={login} 
          onChange={(e) => setLogin(e.target.value)} 
          required 
        />
      </div>
      <div className='login__wrap'>
        <label className='login__label'>Hasło:</label>
        <input 
          className='login__input' 
          type="password" 
          value={password} 
          onChange={(e) => setPassword(e.target.value)} 
          required 
        />
      </div>
      <div className='login__wrap'>
        <button className='login__button' type="submit">Zaloguj się</button>
      </div>
    </form>
  );
}

export default LoginForm;
