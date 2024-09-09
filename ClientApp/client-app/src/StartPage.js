import React, { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from './LoginForm';
import AuthContext from './AuthContext';
import "./css/start.css";

const StartPage = () => {
  const { isAuthenticated, userRoles } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate('/dashboard');
    }
  }, [isAuthenticated, userRoles, navigate]);

  return (
    <div className="start__page">
      <main className="start__content">
        <LoginForm />
      </main>
    </div>
  );
}

export default StartPage;
