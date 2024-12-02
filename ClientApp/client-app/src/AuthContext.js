import React, { createContext, useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [userRoles, setUserRoles] = useState([]);
  const [userId, setUserId] = useState(null);
  const [firstName, setFirstName] = useState(null);
  const [lastName, setLastName] = useState(null);


  useEffect(() => {
    const token = Cookies.get('token');
    if (token) {
      setIsAuthenticated(true);
      const decodedToken = jwtDecode(token);
      setUserId(decodedToken.userId);
      setFirstName(decodedToken.firstName);
      setLastName(decodedToken.lastName);
      // setUserRoles(decodedToken.role || []);
      setUserRoles(Array.isArray(decodedToken.role) ? decodedToken.role : [decodedToken.role]);
    }
  }, []);

  const login = (token, refreshToken) => {
    Cookies.set('token', token, { expires: 7 });
    Cookies.set('refreshToken', refreshToken, { expires: 7 });
    setIsAuthenticated(true);
    const decodedToken = jwtDecode(token);
    setUserId(decodedToken.userId);
    setFirstName(decodedToken.firstName);
    setLastName(decodedToken.lastName);
    // setUserRoles(decodedToken.role || []);
    setUserRoles(Array.isArray(decodedToken.role) ? decodedToken.role : [decodedToken.role]);
  };

  const logout = () => {
    Cookies.remove('token');
    Cookies.remove('refreshToken');
    setIsAuthenticated(false);
    setUserRoles([]);
    setFirstName(null);
    setLastName(null);
    setUserId(null);
  };

  return (
    <AuthContext.Provider value={{ userId, isAuthenticated, userRoles, firstName, lastName, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;
