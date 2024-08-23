import React, { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from './Navbar';
import LoginForm from './LoginForm';
import Footer from './Footer';
import AuthContext from './AuthContext';
import "./start.css";

const WelcomePage = () => {
  const { isAuthenticated, userRoles } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      if (userRoles.includes('Admin')) {
        navigate('/dashboard-admin');
      } else {
        navigate('/dashboard');
      }
    }
  }, [isAuthenticated, userRoles, navigate]);

  return (
    <div className="start__page">
      <Navbar />
      <main className="start__content">
        <LoginForm />
      </main>
    </div>
  );
}

export default WelcomePage;
