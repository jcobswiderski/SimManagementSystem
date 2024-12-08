import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import AuthContext from './AuthContext';

const ProtectedRoute = ({ children, roles }) => {
  const { isAuthenticated, userRoles, loading } = useContext(AuthContext);

  if (loading) {
    return <div className='loading'>Loading...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/start" />;
  }

  if (roles && roles.length > 0 && !roles.some(role => userRoles.includes(role))) {
    return <Navigate to="/unauthorized" />;
  }

  return children;
};

export default ProtectedRoute;
