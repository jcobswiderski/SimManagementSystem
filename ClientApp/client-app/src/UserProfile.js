import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/userProfile.css';
import './css/partials/button.css';

const UserProfile = () => {
    const {id} = useParams();
    const [user, setUserProfile] = useState(null);
    const [userSessions, setUserSessions] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        refreshUser();
        // refreshRolesList();
        refreshSessionsList();
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
                            <tr className="user__table-tr" key={role.id}>
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
                        </tr>
                    </thead>
                    <tbody>
                        {userSessions.map(s => (
                            <tr className="user__table-tr" key={s.id} onClick={() => navigateToSession(s.id)}>
                                <td className="user__table-td">{s.id}</td>
                                <td className="user__table-td">[{s.abbreviation}] {s.name}</td>
                                <td className="user__table-td">{getUserRoleInSession(s)}</td>
                                <td className="user__table-td">{s.beginDate}</td>
                            </tr>   
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
 
export default UserProfile;