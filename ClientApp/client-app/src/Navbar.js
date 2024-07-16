import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';

const Navbar = () => {
  const { isAuthenticated, logout } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/welcome');
  };

  return (
    <nav className="navbar">
      <h1>My App</h1>
      <ul>
        {isAuthenticated ? (
          <>
            <li><Link to="/dashboard">Dashboard</Link></li>
            <li><button onClick={handleLogout}>Logout</button></li>
          </>
        ) : (
          <>
            <li><Link to="/welcome">Login</Link></li>
          </>
        )}
      </ul>
    </nav>
  );
};

export default Navbar;
