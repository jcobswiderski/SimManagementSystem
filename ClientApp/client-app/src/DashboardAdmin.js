import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';

const DashboardAdmin = () => {
    const { logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/start');
    };

    return (
        <div>
            <h1>Dashboard Admin</h1>
            <button onClick={handleLogout}>Wyloguj się</button>
        </div>
    );
}
 
export default DashboardAdmin;