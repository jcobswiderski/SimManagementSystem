import React, { useContext } from 'react';
import {useNavigate} from 'react-router-dom';
import AuthContext from './AuthContext';
import './css/dashboard.css';

const Dashboard = () => {
    const { userRoles } = useContext(AuthContext);
    const navigate = useNavigate();

    return (
        <div className="dashboard">
            <h1 className="dashboard__title">Dashboard</h1>

            <div className="dashboard__container">
                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt="malfunction-icon"/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                <div className="dashboard__card" onClick={() => navigate(`/calendar`)}>
                    <img className="dashboard__card-icon" src="./../calendar.png" alt="calendar-icon"/>
                    <h2 className="dashboard__card-title">Kalendarz</h2>
                </div>

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/simSessions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Sesje</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt=""/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}
            </div>

        </div>
    );
}

export default Dashboard;