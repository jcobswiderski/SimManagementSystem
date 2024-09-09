import React, { useState, useEffect, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import './navbar.css';

const Navbar = () => {
  const {isAuthenticated, logout, userRoles} = useContext(AuthContext);
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth >= 1024)
        setIsMenuOpen(true);
      else
        setIsMenuOpen(false);
    };

    handleResize();
    window.addEventListener('resize', handleResize);

    return () => {
      window.removeEventListener('resize', handleResize);
    };
  }, []);

  const toggleMenu = () => {
    setIsMenuOpen(!isMenuOpen);
  };

  const handleLogout = () => {
    logout();
    navigate('/start');
  };

  return (
    <nav className={`navbar ${isMenuOpen ? 'navbar--open' : ''}`}>
      <img className="navbar__logo" src="./favicon.ico"></img>
      
      <ul className={`${isMenuOpen ? 'navbar__list--open' : 'navbar__list'}`}>
        {isAuthenticated ? (
          <>
            <Link to="/dashboard"><li className="navbar__item">Dashboard</li></Link>
            <Link to="/calendar"><li className="navbar__item">Calendar</li></Link>

            {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
              <Link to="/devices"><li className="navbar__item">Devices</li></Link>
            )}

            {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
              <Link to="/inspections"><li className="navbar__item">Inspections</li></Link>
            )}
            <li className='navbar__item' onClick={handleLogout}>Logout</li>
          </>
        ) : (
          <>
            <Link to="/start"><li className="navbar__item">Login</li></Link>
          </>
        )}
      </ul>
      <button className="navbar__toggle" onClick={toggleMenu}>☰</button>
    </nav>
  );
};

export default Navbar;
