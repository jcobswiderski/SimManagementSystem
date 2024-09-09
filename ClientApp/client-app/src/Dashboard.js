import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import './css/dashboard.css';

const DashboardPilot = () => {
    const { logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/start');
    };

    return (
        <div className='dashboard'>
            <h1>Dashboard</h1>
        </div>
    );
}
 
export default DashboardPilot;