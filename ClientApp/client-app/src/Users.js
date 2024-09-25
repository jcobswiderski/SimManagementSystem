import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/users.css';

const Users = () => {
    const [loading, setLoading] = useState(true);
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        refreshUsers();
    }, []);
    
    const navigateToUser = (id) => {
        navigate(`/users/${id}`);
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
                    </tr>
                </thead>
                <tbody>
                    {users.map(user => (
                        <tr onClick={() => navigateToUser(user.id)} className="users__table-tr" key={user.id}>
                            <td className="users__table-td">{user.id}</td>
                            <td className="users__table-td">{user.firstName}</td>
                            <td className="users__table-td">{user.lastName}</td>
                            <td className="users__table-td">
                            {user.userRoles.length > 0
                                ? user.userRoles.map(role => role.name).join(', ')
                                : 'Brak'}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
 
export default Users;