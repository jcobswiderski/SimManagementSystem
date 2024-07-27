import React, { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from './Navbar';
import LoginForm from './LoginForm';
import Footer from './Footer';
import AuthContext from './AuthContext';

const WelcomePage = () => {
  const { isAuthenticated, userRoles } = useContext(AuthContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      if (userRoles.includes('Admin')) {
        navigate('/dashboardAdmin');
      } else if (userRoles.includes('Pilot')) {
        navigate('/dashboardPilot');
      }
    }
  }, [isAuthenticated, userRoles, navigate]);

  return (
    <div className="welcome-page">
      <Navbar />
      <main className="main-content">
        <LoginForm />
      </main>
      <Footer />
    </div>
  );
}

export default WelcomePage;
