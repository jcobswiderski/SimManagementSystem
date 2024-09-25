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

                <div className="dashboard__card" onClick={() => navigate(`/calendar`)}>
                    <img className="dashboard__card-icon" src="./../calendar.png" alt="calendar-icon"/>
                    <h2 className="dashboard__card-title">Kalendarz</h2>
                </div>

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/simSessions`)}>
                        <img className="dashboard__card-icon" src="./../session.png" alt=""/>
                        <h2 className="dashboard__card-title">Sesje</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/maintenances`)}>
                        <img className="dashboard__card-icon" src="./../maintenance.png" alt=""/>
                        <h2 className="dashboard__card-title">Obsługi</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt="malfunction-icon"/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/devices`)}>
                        <img className="dashboard__card-icon" src="./../device.png" alt=""/>
                        <h2 className="dashboard__card-title">Urządzenia</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/inspections`)}>
                        <img className="dashboard__card-icon" src="./../inspection.png" alt=""/>
                        <h2 className="dashboard__card-title">Przeglądy</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/meter`)}>
                        <img className="dashboard__card-icon" src="./../meter.png" alt=""/>
                        <h2 className="dashboard__card-title">Licznik</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/tests`)}>
                        <img className="dashboard__card-icon" src="./../test.png" alt=""/>
                        <h2 className="dashboard__card-title">Testy</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/users`)}>
                        <img className="dashboard__card-icon" src="./../users.png" alt=""/>
                        <h2 className="dashboard__card-title">Użytkownicy</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../statistic.png" alt=""/>
                        <h2 className="dashboard__card-title">Statystyki</h2>
                    </div>
                )}

            </div>
        </div>
    );
}

export default Dashboard;