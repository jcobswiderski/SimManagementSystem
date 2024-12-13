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
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = Cookies.get('token');
    if (token) {
      try {
        setIsAuthenticated(true);
        const decodedToken = jwtDecode(token);
        setUserId(decodedToken.userId);
        setFirstName(decodedToken.firstName);
        setLastName(decodedToken.lastName);
        setUserRoles(Array.isArray(decodedToken.role) ? decodedToken.role : [decodedToken.role]);
      } catch (error) {
        console.error("Error decoding token:", error);
      }
    }
    setLoading(false);
  }, []);

  const login = (token, refreshToken) => {
    Cookies.set('token', token, { expires: 7 });
    Cookies.set('refreshToken', refreshToken, { expires: 7 });
    setIsAuthenticated(true);
    const decodedToken = jwtDecode(token);
    setUserId(decodedToken.userId);
    setFirstName(decodedToken.firstName);
    setLastName(decodedToken.lastName);
    setUserRoles(Array.isArray(decodedToken.role) ? decodedToken.role : [decodedToken.role]);
    setLoading(false);
  };

  const logout = () => {
    Cookies.remove('token');
    Cookies.remove('refreshToken');
    setIsAuthenticated(false);
    setUserRoles([]);
    setUserId(null);
    setFirstName(null);
    setLastName(null);
  };

  return (
    <AuthContext.Provider value={{ userId, isAuthenticated, userRoles, loading, login, logout, firstName, lastName }}>
      {children}
    </AuthContext.Provider>
  );
};

export default AuthContext;

