import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import Navbar from './Navbar';

const DashboardPilot = () => {
    const { logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/welcome');
    };

    return (
        <div>
        <Navbar />
        <h1>Dashboard Pilot</h1>
        <button onClick={handleLogout}>Wyloguj się</button>
        </div>
    );
}
 
export default DashboardPilot;