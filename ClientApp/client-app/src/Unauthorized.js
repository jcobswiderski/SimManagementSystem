import React from 'react';
import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';

const Unauthorized = () => {
    const { logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/start');
    };

    return (
        <div>
            <h1>Unauthorized</h1>
            <p>You do not have permission to view this page.</p>
            <button onClick={handleLogout}>Zaloguj się</button>
        </div>
    );
};

export default Unauthorized;
