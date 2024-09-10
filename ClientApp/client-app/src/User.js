import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/user.css';

const User = () => {
    const {id} = useParams();
    const [user, setUser] = useState(null);
    const [userFirstName, setUserFirstName] = useState(null);
    const [userLastName, setUserLastName] = useState(null);
    const [rolesList, setRolesList] = useState([]);
    const [roleToAssignId, setRoleToAssignId] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        refreshUser();
        refreshRolesList();
    }, []);
    
    const refreshUser = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}`);
            const data = await response.json();
            setUser(data);
            setUserFirstName(data.firstName);
            setUserLastName(data.lastName);
        } catch (error) {
            console.error('Error fetching user:', error);
        }
    };

    const refreshRolesList = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Roles`);
            const data = await response.json();
            setRolesList(data);
        } catch (error) {
            console.error('Error fetching roles:', error);
        }
    }
 
    const handleFirstNameChange = (e) => {
        setUserFirstName(e.target.value);
    };

    const handleLastNameChange = (e) => {
        setUserLastName(e.target.value);
    };

    const handleRoleChange = (e) => {
        setRoleToAssignId(e.target.value);
    };

    const updateUserName = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    firstName: userFirstName,
                    lastName: userLastName
                }),
            });

            if (response.ok) {
                alert('User updated successfully!');
                refreshUser();
            } else {
                alert('Failed to update user.');
            }
        } catch (error) {
            console.error('Error updating user:', error);
        }
    };

    const assignNewRole = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}/AssignRole`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ id: roleToAssignId }),
            });

            if (response.ok) {
                alert('Role assigned successfully!');
                refreshUser();
            } else {
                alert('Failed to assign role.');
            }
        } catch (error) {
            console.error('Error assigning role:', error);
        }
    };

    const deleteRole = async (roleId) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}/RemoveRole`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ id: roleId }),
            });

            if (response.ok) {
                alert('Role removed successfully!');
                refreshUser();
            } else {
                alert('Failed to remove role.');
            }
        } catch (error) {
            console.error('Error removing role:', error);
        }
    };

    if (!user) {
        return <div>Loading...</div>;
    }

    return (
        <div className="user">
            <div className="user__header">
                <h1 className="user__title">Account</h1>
                <img className="user__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            <div className="user__group">
                <img className="user__image" src="./../user.png" alt="" />
                <div className="user__group-inputs">
                    <input className="user__input" type="text" value={userFirstName} onChange={handleFirstNameChange}  />
                    <input className="user__input" value={userLastName} onChange={handleLastNameChange}  />
                    <button onClick={updateUserName} className="user__save">Zapisz</button>
                </div>
            </div>   
            <div className="user__group-roles">
                <h2 className="user__table-title">User roles</h2>
                <table className="user__table">
                    <thead>
                        <tr>
                            <th className="user__table-th">ID</th>
                            <th className="user__table-th">Role</th>
                            <th className="user__table-th">Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        {user.userRoles.map(role => (
                            <tr className="user__table-tr" key={role.id}>
                                <td className="user__table-td">{role.id}</td>
                                <td className="user__table-td">{role.name}</td>
                                <td className="user__table-td">
                                    <img className="user__table-delete" src="./../clear.png" alt="" onClick={() => deleteRole(role.id)}/>
                                </td>
                            </tr>   
                        ))}
                    </tbody>
                </table>
                <select className="user__select" value={roleToAssignId} onChange={handleRoleChange}>
                        {rolesList.map(r => (
                            <option className="user__option" key={r.id} value={r.id}>
                                {r.name}
                            </option>
                        ))}
                </select>
                <button className="user__save user__save--role" onClick={assignNewRole}>Nadaj nową rolę</button>
            </div>
        </div>
    );
}
 
export default User;