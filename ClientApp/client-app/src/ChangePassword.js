import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/createDevice.css';
import './css/partials/button.css';

const ChangePassword = ({userId, showAlert}) => {
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const navigate = useNavigate();

    const handleOldPasswordChange = (e) => {
        setOldPassword(e.target.value);
    };

    const handleNewPasswordChange = (e) => {
        setNewPassword(e.target.value);
    };

    const checkPassword = (password) => {
        const upperCase = /[A-Z]/.test(password);
        const specialChar = /[!@":|<#&*(),.$%^?>]/.test(password);
        const isValidLength = password.length >= 8;

        return upperCase && specialChar && isValidLength;
    };

    const changePassword = async () => {
        if (!oldPassword || !newPassword) {
            showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
            return;
        }

        if(!checkPassword(newPassword)) {
            showAlert('Hasło powinno zawierać minimum 8 znaków, jedną dużą literę i jeden znak specjalny!', 'error');
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/${userId}/changePassword`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    oldPassword: oldPassword,
                    newPassword: newPassword,
                })
            });

            if (response.ok) {
                showAlert('Pomyślnie zmieniono hasło!', 'success');
                navigate(-1);
            } else {
                showAlert('Wprowadzono nieprawidłowe hasło!', 'error');
            }
        } catch (error) {
            console.error('Error changing password:', error);
        }
    };

    return (
        <div className="createDevice">
            <div className="createDevice__header">
                <h1 className="createDevice__title">Change password</h1>
                <img className="createDevice__close" src="./../../../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/>
            </div>

            <span className="createDevice__label">Old password</span>
            <input className="createDevice__input" type="password" onChange={handleOldPasswordChange}/>

            <span className="createDevice__label">New password</span>
            <input className="createDevice__input" type="password" onChange={handleNewPasswordChange} />
            <button className="button createDevice__button" onClick={changePassword}>Save</button>
        </div>
    );
}

export default ChangePassword;