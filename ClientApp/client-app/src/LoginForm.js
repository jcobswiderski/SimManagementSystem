import config from './config';
import { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import Cookies from 'js-cookie';
import AuthContext from './AuthContext';

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
      const response = await fetch(`${config.API_URL}/Users/login`, {
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
    <div className="loginForm">
      <form onSubmit={handleSubmit}>
        <div>
          <label>Login:</label>
          <input 
            type="text" 
            value={login} 
            onChange={(e) => setLogin(e.target.value)} 
            required 
          />
        </div>
        <div>
          <label>Hasło:</label>
          <input 
            type="password" 
            value={password} 
            onChange={(e) => setPassword(e.target.value)} 
            required 
          />
        </div>
        <button type="submit">Zaloguj się</button>
      </form>
    </div>
  );
}

export default LoginForm;
