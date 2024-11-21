import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/user.css';
import './css/partials/button.css';

const User = ({userId, showAlert}) => {
    const {id} = useParams();
    const [user, setUser] = useState(null);
    const [userFirstName, setUserFirstName] = useState(null);
    const [userLastName, setUserLastName] = useState(null);
    const [rolesList, setRolesList] = useState([]);
    const [roleToAssignId, setRoleToAssignId] = useState(1);
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

    const deleteUser = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            const responseBody = await response.text();
            console.log(responseBody);

            if (response.ok) {
                showAlert('Usunięto użytkownika!', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się usunąć użytkownika!', 'error');
            }
        } catch (error) {
            console.error('Error removing user:', error);
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
                showAlert('Zaaktualizowano użytkownika!', 'success');
                refreshUser();
            } else {
                showAlert('Nie udało się zaaktualizować użytkownika!', 'error');
            }
        } catch (error) {
            console.error('Error updating user:', error);
        }
    };

    const assignNewRole = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Roles/${id}/AssignRole`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ id: roleToAssignId }),
            });

            if (response.ok) {
                showAlert('Nadano nową rolę!', 'success');
                refreshUser();
            } else {
                showAlert('Nie udało się nadać nowej roli!', 'error');
            }
        } catch (error) {
            console.error('Error assigning role:', error);
        }
    };

    const deleteRole = async (roleId) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Roles/${id}/RemoveRole`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ id: roleId }),
            });

            if (response.ok) {
                showAlert('Usunięto rolę użytkownika!', 'success');
                refreshUser();
            } else {
                showAlert('Nie udało się usunąć roli użytkownika!', 'error');
            }
        } catch (error) {
            console.error('Error removing role:', error);
        }
    };

    const resetPassword = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${id}/resetPassword`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            const data = await response.json();

            if (response.ok) {
                alert("Nowe tymczasowe hasło dla użytkownika: " + data.tempPassword);
                showAlert('Pomyślnie zresetowano hasło!', 'success');
            } else {
                showAlert('Nie udało się zresetować hasła użytkownika!', 'error');
            }
        } catch (error) {
            console.error('Error reseting password:', error);
        }
    };

    if (!user) {
        return <div>Loading...</div>;
    }

    return (
        <div className="user">
            <div className="user__header">
                <h1 className="user__title">User management</h1>
                <img className="user__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/>
            </div>
            <div className="user__group">
                <img className="user__image" src="./../user.png" alt=""/>
                <div className="user__group-inputs">
                    <input className="user__input" type="text" value={userFirstName} onChange={handleFirstNameChange}/>
                    <input className="user__input" value={userLastName} onChange={handleLastNameChange}/>
                    <button onClick={updateUserName} className="button user__update-button">Save</button>
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
                                <img className="user__table-delete" src="./../clear.png" alt=""
                                     onClick={() => deleteRole(role.id)}/>
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
                <button className="button user__button" onClick={assignNewRole}>Assign role</button>
            </div>
            <button className="button user__button-secondary" onClick={resetPassword}>Reset password</button>
            <>
                {id !== userId ?
                    <button onClick={deleteUser} className="button user__button-secondary">Delete user</button> : null
                }
            </>
        </div>
    );
}

export default User;