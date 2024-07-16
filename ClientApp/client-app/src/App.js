import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { useContext } from 'react';
import AuthContext, { AuthProvider } from './AuthContext';
import ProtectedRoute from './ProtectedRoute';
import WelcomePage from './WelcomePage';
import Dashboard from './Dashboard';
import NotFound from './NotFound';
import AdminPage from './AdminPage';
import Unauthorized from './Unauthorized';

const App = () => {
  const { isAuthenticated } = useContext(AuthContext);

  return (
    <Router>
      <Routes>
        <Route path="/" element={isAuthenticated ? <Navigate to="/dashboard" /> : <Navigate to="/welcome" />} />
        <Route path="/welcome" element={<WelcomePage />} />
        <Route path="/dashboard" element={<ProtectedRoute roles={['Pilot', 'Admin']}><Dashboard /></ProtectedRoute>} />
        <Route path="/admin" element={<ProtectedRoute roles={['Admin']}><AdminPage /></ProtectedRoute>} />
        <Route path="/unauthorized" element={<Unauthorized />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
};

export default () => (
  <AuthProvider>
    <App />
  </AuthProvider>
);
