import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from "./AuthContext";
import './css/partials/loading.css';
import './css/users.css';

const Users = () => {
    const {userRoles} = useContext(AuthContext);
    const [loading, setLoading] = useState(true);
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        refreshUsers();
    }, []);
    
    const navigateToUserManagement = (id) => {
        navigate(`/users/${id}`);
    };

    const navigateToUserProfile = (id) => {
        navigate(`/users/${id}/profile`);
    };

    const refreshUsers = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users`);
            const data = await response.json();
            setUsers(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="users">
            <h1 className="users__title">Users</h1>
            <table className="users__table">
                <thead>
                    <tr>
                        <th className="users__table-th">ID</th>
                        <th className="users__table-th">First Name</th>
                        <th className="users__table-th">Last Name</th>
                        <th className="users__table-th">Roles</th>
                        <th className="users__table-th">Action</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user => (
                        <tr className="users__table-tr" key={user.id}>
                            <td className="users__table-td">{user.id}</td>
                            <td className="users__table-td">{user.firstName}</td>
                            <td className="users__table-td">{user.lastName}</td>
                            <td className="users__table-td">
                            {user.userRoles.length > 0
                                ? user.userRoles.map(role => role.name).join(', ')
                                : 'Brak'}
                            </td>
                            <td className="users__table-td">
                                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                                    <button onClick={() => navigateToUserManagement(user.id)} className="users__button">Zarządzaj</button>
                                )}
                                <button onClick={() => navigateToUserProfile(user.id)} className="users__button">Profil</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
 
export default Users;