import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { useContext, useState } from 'react';
import AuthContext, { AuthProvider } from './AuthContext';
import ProtectedRoute from './ProtectedRoute';
import Alert from './Alert';
import Navbar from './Navbar';
import Register from "./Register";
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
import CreateDevice from './CreateDevice';
import UserProfile from './UserProfile';
import "./css/app.css";
import Maintenance from './Maintenance';
import CreatePredefinedSession from './CreatePredefinedSession';
import Statistics from './Statistics';



const App = () => {
  const {userId} = useContext(AuthContext);
  const [alertMessage, setAlertMessage] = useState('');
  const [alertColor, setAlertColor] = useState('red');
  const [showAlert, setShowAlert] = useState(false);

  const showAlertWithMessage = (message, color = 'error') => {
    setAlertMessage(message);
    setAlertColor(color);
    setShowAlert(true);
    setTimeout(() => {
      setShowAlert(false);
    }, 5000);
  };

  return (
    <Router>
      <Navbar />
      {showAlert && <Alert message={alertMessage} color={alertColor} onClose={() => setShowAlert(false)} />}
        <Routes>
          <Route path="/" element={<StartPage />} />
          <Route path="/start" element={<StartPage showAlert={showAlertWithMessage} />} />
          <Route path="/register" element={<Register showAlert={showAlertWithMessage} />} />
          <Route path="/dashboard" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor', 'Pilot', 'Copilot', 'Planer']}><Dashboard /></ProtectedRoute>} />
          <Route path="/calendar" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor', 'Pilot', 'Copilot', 'Planer']}><Calendar /></ProtectedRoute>} />
          <Route path="/devices" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Devices /></ProtectedRoute>} />
          <Route path="/devices/:id" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Device /></ProtectedRoute>} />
          <Route path="/createDevice" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateDevice showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/inspections" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Inspections /></ProtectedRoute>} />
          <Route path="/inspections/:id" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Inspection showAlert={showAlertWithMessage}  /></ProtectedRoute>} />
          <Route path="/createInspection" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateInspection userId={userId} showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/users" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor', 'Pilot', 'Copilot', 'Planer']}><Users userId={userId} /></ProtectedRoute>} />
          <Route path="/users/:id" element={<ProtectedRoute roles={['Admin', 'Planer']}><User showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/users/:id/profile" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Planer', 'Instructor', 'Auditor', 'Pilot', 'Copilot']}><UserProfile /></ProtectedRoute>} />
          <Route path="/meter" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Meter userId={userId} showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/simSessions" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor', 'Pilot', 'Copilot', 'Planer']}><SimSessions/></ProtectedRoute>} />
          <Route path="/simSessions/:id" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor', 'Pilot', 'Copilot', 'Planer']}><SimSession showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/createSimSession" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Planer']}><CreateSimSession showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/createPredefinedSession" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Planer']}><CreatePredefinedSession showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/malfunctions" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor']}><Malfunctions /></ProtectedRoute>} />
          <Route path="/malfunctions/:id" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Instructor']}><Malfunction showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/createMalfunction" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateMalfunction userId={userId} showAlert={showAlertWithMessage}/></ProtectedRoute>} />
          <Route path="/maintenances" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Maintenances /></ProtectedRoute>} />
          <Route path="/maintenances/:id" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Maintenance userId={userId} showAlert={showAlertWithMessage} /></ProtectedRoute>} />
          <Route path="/createMaintenance" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateMaintenance userId={userId} showAlert={showAlertWithMessage}/></ProtectedRoute>} />
          <Route path="/tests" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Tests /></ProtectedRoute>} />
          <Route path="/tests/:id" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor']}><Test showAlert={showAlertWithMessage}/></ProtectedRoute>} />
          <Route path="/createTest" element={<ProtectedRoute roles={['Admin', 'Engineer']}><CreateTest userId={userId} showAlert={showAlertWithMessage}/></ProtectedRoute>} />
          <Route path="/statistics" element={<ProtectedRoute roles={['Admin', 'Engineer', 'Auditor', 'Planer', 'Instructor']}><Statistics showAlert={showAlertWithMessage}/></ProtectedRoute>} />
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