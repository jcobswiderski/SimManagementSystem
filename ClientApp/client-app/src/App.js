import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { useContext } from 'react';
import AuthContext, { AuthProvider } from './AuthContext';
import ProtectedRoute from './ProtectedRoute';
import Navbar from './Navbar';
import StartPage from './StartPage';
import NotFound from './NotFound';
import Unauthorized from './Unauthorized';
import Dashboard from './Dashboard'; 
import Calendar from './Calendar';
import Devices from './Devices';
import Device from './Device';
import Inspections from './Inspections';
import Inspection from './Inspection';
import CreateInspection from './CreateInspection';
import Users from './Users';
import User from './User';
import Meter from './Meter';
import SimSessions from './SimSessions';
import SimSession from './SimSession';
import CreateSimSession from './CreateSimSession';
import Malfunctions from "./Malfunctions";
import Malfunction from "./Malfunction";
import CreateMalfunction from "./CreateMalfunction";
import Maintenances from "./Maintenances";
import CreateMaintenance from "./CreateMaintenance";
import Tests from "./Tests";
import Test from "./Test";
import CreateTest from "./CreateTest";
import "./css/app.css";


const App = () => {
  const { userId } = useContext(AuthContext);

  return (
    <Router>
      <Navbar />
        <Routes>
          <Route path="/" element={<StartPage />} />
          <Route path="/start" element={<StartPage />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/calendar" element={<Calendar />} />
          <Route path="/devices" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Devices /></ProtectedRoute>} />
          <Route path="/devices/:id" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Device /></ProtectedRoute>} />
          <Route path="/inspections" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Inspections /></ProtectedRoute>} />
          <Route path="/inspections/:id" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Inspection /></ProtectedRoute>} />
          <Route path="/createInspection" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateInspection userId={userId} /></ProtectedRoute>} />
          <Route path="/users" element={<ProtectedRoute roles={['Admin']}><Users /></ProtectedRoute>} />
          <Route path="/users/:id" element={<ProtectedRoute roles={['Admin']}><User /></ProtectedRoute>} />
          <Route path="/meter" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Meter userId={userId} /></ProtectedRoute>} />
          <Route path="/simSessions" element={<SimSessions />} />
          <Route path="/simSessions/:id" element={<SimSession />} />
          <Route path="/createSimSession" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Planer']}><CreateSimSession /></ProtectedRoute>} />
          <Route path="/malfunctions" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Malfunctions /></ProtectedRoute>} />
          <Route path="/malfunctions/:id" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Malfunction /></ProtectedRoute>} />
          <Route path="/createMalfunction" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateMalfunction userId={userId}/></ProtectedRoute>} />
          <Route path="/maintenances" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Maintenances /></ProtectedRoute>} />
          <Route path="/createMaintenance" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateMaintenance userId={userId}/></ProtectedRoute>} />
          <Route path="/tests" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Tests /></ProtectedRoute>} />
          <Route path="/tests/:id" element={<ProtectedRoute roles={['Admin', 'Engineer']}><Test /></ProtectedRoute>} />
          <Route path="/createTest" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateTest userId={userId}/></ProtectedRoute>} />
          <Route path="/unauthorized" element={<Unauthorized />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
    </Router>
  );
};

const AppWrapper = () => (
    <AuthProvider>
      <App />
    </AuthProvider>
);

export default AppWrapper;