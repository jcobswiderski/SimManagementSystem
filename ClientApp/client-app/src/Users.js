import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from "./AuthContext";
import './css/partials/loading.css';
import './css/users.css';

const Users = ({userId}) => {
    const {userRoles} = useContext(AuthContext);
    const [loading, setLoading] = useState(true);
    const [users, setUsers] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        refreshUsers();
    }, []);
    
    const filteredUsers = users.filter(u =>
        u.firstName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        u.lastName.toLowerCase().includes(searchTerm.toLowerCase())
    );

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
            <h1 className="users__title">Użytkownicy</h1>
            <h2 className="users__info">Wyszukaj użytkownika po imieniu lub nazwisku.</h2>
            <div className="users__group">
                <div className="users__search">
                    <img className="users__search-icon" src="./search.png"></img>
                    <input className="users__input users__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
                </div>
            </div>
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
                    {filteredUsers.map(user => (
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
                                {userRoles.some(role => role === 'Planer' || role === 'Admin') && (
                                    <button onClick={() => navigateToUserManagement(user.id)} className="users__button">Zarządzaj</button>
                                )}
                                {(userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Planer' || role === 'Auditor' || role === 'Instructor') || (user.id == userId)) && (
                                    <button onClick={() => navigateToUserProfile(user.id)}
                                            className="users__button">Profil</button>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
 
export default Users;