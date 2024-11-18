import React, { useState, useEffect, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import './css/navbar.css';

const Navbar = () => {
  const {isAuthenticated, logout, userRoles, firstName, lastName} = useContext(AuthContext);
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
      <img className="navbar__logo" src="./favicon.ico" alt="logo"></img>
      
      <ul className={`${isMenuOpen ? 'navbar__list--open' : 'navbar__list'}`}>
        {isAuthenticated ? (
          <>
            <Link to="/dashboard"><li className="navbar__item">Dashboard</li></Link>
            <Link to="/calendar"><li className="navbar__item">Calendar</li></Link>
            <Link to="/simSessions"><li className="navbar__item">Sessions</li></Link>

            {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Auditor') && (
                <Link to="/maintenances"><li className="navbar__item">Maintenances</li></Link>
            )}

            {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Auditor' || role === 'Instructor') && (
                <Link to="/malfunctions"><li className="navbar__item">Malfunctions</li></Link>
            )}

            {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Auditor') && (
              <Link to="/devices"><li className="navbar__item">Devices</li></Link>
            )}

            {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Auditor') && (
              <Link to="/inspections"><li className="navbar__item">Tasks</li></Link>
            )}

            {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Auditor') && (
              <Link to="/meter"><li className="navbar__item">Meter</li></Link>
            )}

            {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Auditor') && (
                <Link to="/tests"><li className="navbar__item">Tests QTG</li></Link>
            )}

            {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor' || role === 'Planer' || role === 'Instructor') && (
                <Link to="/statistics"><li className="navbar__item">Statistics</li></Link>
            )}

            <Link to="/users"><li className="navbar__item navbar__item--last">Users</li></Link>

            <li className='navbar__profile'>
              <div className='navbar__profile-info'>
                <img className="navbar__profile-img" src="./user2.png"></img>{firstName} {lastName}
              </div>
              <img className="navbar__profile-img navbar__profile-logout" src="./logout.png" onClick={handleLogout}></img> 
            </li>

          </>
        ) : (
          <>
            <Link to="/start"><li className="navbar__item">Login</li></Link>
            <Link to="/register"><li className="navbar__item">Register</li></Link>
          </>
        )}
      </ul>
      <button className={`${isMenuOpen ? 'navbar__toggle--open' : 'navbar__toggle'}`} onClick={toggleMenu}>☰</button>
    </nav>
  );
};

export default Navbar;
