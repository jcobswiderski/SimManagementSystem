import React, {useContext, useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import AuthContext from './AuthContext';
import './css/partials/loading.css';
import './css/dashboard.css';

const Dashboard = () => {
    const { userRoles } = useContext(AuthContext);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    const [malfunctionsCount, setMalfunctionsCount] = useState(null);
    const [sessionsCount, setSessionsCount] = useState(null);
    const [maintenancesCount, setMaintenancesCount] = useState(null);

    useEffect(() => {
        refreshData();
    }, []);

    const refreshData = async () => {
        try {
            const responseMalfunctions = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/count/unsolved`);
            const dataMalfunctions = await responseMalfunctions.json();
            setMalfunctionsCount(dataMalfunctions);

            const responseSessions = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/count/planned`);
            const dataSessions = await responseSessions.json();
            setSessionsCount(dataSessions);

            const responseMaintenances = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances/count/incomplete`);
            const dataMaintenances = await responseMaintenances.json();
            setMaintenancesCount(dataMaintenances);

            setLoading(false);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="dashboard">
            <h1 className="dashboard__title">Dashboard</h1>

            <div className="dashboard__container">

                <div className="dashboard__card" onClick={() => navigate(`/calendar`)}>
                    <img className="dashboard__card-icon" src="./../calendar.png" alt="calendar-icon"/>
                    <h2 className="dashboard__card-title">Kalendarz</h2>
                </div>

                <div className="dashboard__card" onClick={() => navigate(`/simSessions`)}>
                    <img className="dashboard__card-icon" src="./../session.png" alt=""/>
                    <h2 className="dashboard__card-title">Sesje</h2>
                </div>

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/maintenances`)}>
                        <img className="dashboard__card-icon" src="./../maintenance.png" alt=""/>
                        <h2 className="dashboard__card-title">Obsługi</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor' || role === 'Instructor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/malfunctions`)}>
                        <img className="dashboard__card-icon" src="./../malfunction.png" alt="malfunction-icon"/>
                        <h2 className="dashboard__card-title">Usterki</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/devices`)}>
                        <img className="dashboard__card-icon" src="./../device.png" alt=""/>
                        <h2 className="dashboard__card-title">Urządzenia</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/inspections`)}>
                        <img className="dashboard__card-icon" src="./../inspection.png" alt=""/>
                        <h2 className="dashboard__card-title">Przeglądy</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/meter`)}>
                        <img className="dashboard__card-icon" src="./../meter.png" alt=""/>
                        <h2 className="dashboard__card-title">Licznik</h2>
                    </div>
                )}

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/tests`)}>
                        <img className="dashboard__card-icon" src="./../test.png" alt=""/>
                        <h2 className="dashboard__card-title">Testy</h2>
                    </div>
                )}

                <div className="dashboard__card" onClick={() => navigate(`/users`)}>
                    <img className="dashboard__card-icon" src="./../users.png" alt=""/>
                    <h2 className="dashboard__card-title">Użytkownicy</h2>
                </div>

                {userRoles.some(role => role === 'Admin' || role === 'Engineer' || role === 'Auditor' || role === 'Planer' || role === 'Instructor') && (
                    <div className="dashboard__card" onClick={() => navigate(`/statistics`)}>
                        <img className="dashboard__card-icon" src="./../statistic.png" alt=""/>
                        <h2 className="dashboard__card-title">Statystyki</h2>
                    </div>
                )}

            </div>


            <div className="dashboard__statistics">
                <div className="dashboard__statistics-card">
                    <span
                        className={`dashboard__statistics-malfunctions ${malfunctionsCount > 0 ? 'color-red' : 'color-green'}`}>
                        {malfunctionsCount}
                    </span>
                    <h2 className="dashboard__statistics-title">Liczba nierozwiązanych usterek</h2>
                </div>

                <div className="dashboard__statistics-card">
                    <span
                        className={`dashboard__statistics-malfunctions ${sessionsCount == 0 ? 'color-red' : 'color-green'}`}>
                        {sessionsCount}
                    </span>
                    <h2 className="dashboard__statistics-title">Liczba zaplanowanych sesji</h2>
                </div>

                <div className="dashboard__statistics-card">
                    <span
                        className={`dashboard__statistics-malfunctions ${maintenancesCount > 0 ? 'color-red' : 'color-green'}`}>
                        {maintenancesCount}
                    </span>
                    <h2 className="dashboard__statistics-title">Liczba zaległych obsług</h2>
                </div>
            </div>


        </div>
    );
}

export default Dashboard;