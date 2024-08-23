import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import Navbar from './Navbar';

const DashboardAdmin = () => {
    const { logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/start');
    };

    return (
        <div>
        <Navbar />
        <h1>Dashboard Admin</h1>
        <button onClick={handleLogout}>Wyloguj się</button>
        </div>
    );
}
 
export default DashboardAdmin;