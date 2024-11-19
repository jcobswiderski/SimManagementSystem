import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from './AuthContext';
import Cookies from 'js-cookie';
import "./css/register.css";
import "./css/partials/button.css";

const Register = ({showAlert}) => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const { isAuthenticated, userRoles } = useContext(AuthContext);
    const navigate = useNavigate();

    useEffect(() => {
        if (isAuthenticated) {
            navigate('/dashboard');
        }
    }, [isAuthenticated, userRoles, navigate]);

    const checkPassword = (password) => {
        const upperCase = /[A-Z]/.test(password);
        const specialChar = /[!@":|<#&*(),.$%^?>]/.test(password);
        const isValidLength = password.length >= 8;

        return upperCase && specialChar && isValidLength;
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (!login || !password || !firstName || !lastName) {
            showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
            return;
        }

        if(login.length < 5) {
            showAlert('Login powinien się składać z minimum 5 znaków!', 'error');
            return;
        }

        if(!checkPassword(password)) {
            showAlert('Hasło powinno zawierać minimum 8 znaków, jedną dużą literę i jeden znak specjalny!', 'error');
            return;
        }

        const registerData = {
            firstName: firstName,
            lastName: lastName,
            login: login,
            password: password
        };

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Users/register`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(registerData)
            });

            if (response.ok) {
                const data = await response.json();
                showAlert('Pomyślnie zarejestrowano!', 'success');
                navigate('/');
            } else {
                console.log('Błąd:', response.statusText);
                showAlert('Nie udało się utworzyć nowego konta!', 'error');
            }
        } catch (error) {
            console.error('Wystąpił błąd:', error);
        }
    };

    return (
        <div className="register">
            <form className='register__form' onSubmit={handleSubmit}>

                <div className='register__wrap'>
                    <label className='register__label'>First Name</label>
                    <input
                        className='register__input'
                        type="text"
                        value={firstName}
                        onChange={(e) => setFirstName(e.target.value)}
                    />
                </div>

                <div className='register__wrap'>
                    <label className='register__label'>Last Name</label>
                    <input
                        className='register__input'
                        type="text"
                        value={lastName}
                        onChange={(e) => setLastName(e.target.value)}
                    />
                </div>

                <div className='register__wrap'>
                    <label className='register__label'>Login</label>
                    <input
                        className='register__input'
                        type="text"
                        value={login}
                        onChange={(e) => setLogin(e.target.value)}
                    />
                </div>

                <div className='register__wrap'>
                    <label className='register__label'>Password</label>
                    <input
                        className='register__input'
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>

                <div className='register__wrap'>
                    <button className='button register__button' type="submit">Zarejestruj się</button>
                </div>

            </form>
        </div>
    );
}

export default Register;
