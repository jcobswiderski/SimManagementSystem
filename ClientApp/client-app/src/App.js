import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import { useContext } from 'react';
import AuthContext, { AuthProvider } from './AuthContext';
import ProtectedRoute from './ProtectedRoute';
import WelcomePage from './WelcomePage';
import NotFound from './NotFound';
import Unauthorized from './Unauthorized';
import DashboardAdmin from './DashboardAdmin';  // nowy komponent
import DashboardPilot from './DashboardPilot';  // nowy komponent

const App = () => {
  const { isAuthenticated, userRoles } = useContext(AuthContext);

  const getDefaultRoute = () => {
    if (userRoles.includes('Admin')) {
      return '/dashboardAdmin';
    } else if (userRoles.includes('Pilot')) {
      return '/dashboardPilot';
    } else {
      return '/welcome';
    }
  };

  return (
    <Router>
        <Routes>
          <Route path="/" element={isAuthenticated ? <Navigate to={getDefaultRoute()} /> : <Navigate to="/welcome" />} />
          <Route path="/welcome" element={<WelcomePage />} />
          <Route path="/dashboardAdmin" element={<ProtectedRoute roles={['Admin']}><DashboardAdmin /></ProtectedRoute>} />
          <Route path="/dashboardPilot" element={<ProtectedRoute roles={['Pilot']}><DashboardPilot /></ProtectedRoute>} />
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
