import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/userProfile.css';
import './css/partials/button.css';

const UserProfile = () => {
    const {id} = useParams();
    const [user, setUserProfile] = useState(null);
    const [userSessions, setUserSessions] = useState([]);
    const [daysFromSession, setDaysFromSession] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        refreshUser();
        refreshSessionsList();
        refreshDaysFromSession();
    }, []);
    
    const refreshUser = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}`);
            const data = await response.json();
            setUserProfile(data);
        } catch (error) {
            console.error('Error fetching user:', error);
        }
    };
 
    const refreshSessionsList = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/byuser/${id}`);
            const data = await response.json();
            setUserSessions(data);
        } catch (error) {
            console.error('Error fetching user sessions:', error);
        }
    };

    const refreshDaysFromSession = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/byuser/${id}/last`);
            const data = await response.json();
            setDaysFromSession(data);
        } catch (error) {
            console.error('Error fetching days from session:', error);
        }
    };

    const navigateToSession = (id) => {
        navigate(`/simSessions/${id}`);
    };

    const getUserRoleInSession = (session) => {
        if (session.pilot === user.id) 
            return 'Pilot';

        if (session.copilot === user.id) 
            return 'Copilot';

        if (session.instructor === user.id) 
            return 'Instructor';

        if (session.observer === user.id) 
            return 'Observer';
    };

    if (!user) {
        return <div>Loading...</div>;
    }

    return (
        <div className="user">
            <div className="user__header">
                <h1 className="user__title">Profile</h1>
                <img className="user__close" src="./../../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            <div className="user__group">
                <img className="user__image" src="./../../user.png" alt="" />
                <h2 className="user__name">{user.firstName} {user.lastName} </h2>
            </div>   

            <div className="user__group-roles">
                <h2 className="user__table-title">User roles</h2>
                <table className="user__table">
                    <thead>
                        <tr>
                            <th className="user__table-th">Role</th>
                        </tr>
                    </thead>
                    <tbody>
                        {user.userRoles.map(role => (
                            <tr className="user__table-tr--role" key={role.id}>
                                <td className="user__table-td">{role.name}</td>
                            </tr>   
                        ))}
                    </tbody>
                </table>
            </div>

            <div className="user__group-roles">
                <h2 className="user__table-title">User sessions</h2>
                <table className="user__table">
                    <thead>
                        <tr>
                            <th className="user__table-th">ID</th>
                            <th className="user__table-th">Title</th>
                            <th className="user__table-th">Role</th>
                            <th className="user__table-th">Date</th>
                            <th className="user__table-th">Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {userSessions.map(s => (
                            <tr className={`user__table-tr ${s.realized ? 'user__table-tr--green' : 'user__table-tr--red'}`} key={s.id} onClick={() => navigateToSession(s.id)}>
                                <td className="user__table-td">{s.id}</td>
                                <td className="user__table-td">[{s.abbreviation}] {s.name}</td>
                                <td className="user__table-td">{getUserRoleInSession(s)}</td>
                                <td className="user__table-td">{s.beginDate}</td>
                                <td className="user__table-td">{s.realized === true ? 'Zrealizowano' : 'Zaplanowana'}</td>
                            </tr>   
                        ))}
                    </tbody>
                </table>
            </div>

            <div className="user__group-roles">
                <h2 className="user__table-title">Days from last session</h2>
                <span className="user__table-description">Liczba dni, które minęły od ostatniej zrealizowanej sesji. Powyżej 90 dni generuje żółte ostrzeżenie. Przekroczenie 180 dni czerwonym kolorem informuje o braku aktualności szkolenia!</span>
                <table className="user__table">
                    <thead>
                        <tr>
                            <th className="user__table-th">Title</th>
                            <th className="user__table-th">Interval</th>
                        </tr>
                    </thead>
                    <tbody>
                        {daysFromSession.map(s => (
                            <tr className={`${s.daysSinceLastSession >= 180 ? 'user__table-tr--red' : s.daysSinceLastSession > 90 ? 'user__table-tr--yellow' : 'user__table-tr--green'}`} key={s.id}>
                                <td className="user__table-td">{s.session}</td>
                                <td className="user__table-td">ostatnia sesja {s.daysSinceLastSession} dni temu</td>
                            </tr>   
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
 
export default UserProfile;