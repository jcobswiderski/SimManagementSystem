import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';

const DashboardPilot = () => {
    const { logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/start');
    };

    return (
        <div>
            <h1>Dashboard</h1>
            <button onClick={handleLogout}>Wyloguj się</button>
        </div>
    );
}
 
export default DashboardPilot;