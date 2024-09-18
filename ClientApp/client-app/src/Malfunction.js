import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/malfunction.css';
import './css/partials/button.css';

const Malfunction = () => {
    const {id} = useParams();
    const [malfunction, setMalfunction] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        refreshMalfunction();
    }, []);

    const refreshMalfunction = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/${id}`);
            const data = await response.json();
            setMalfunction(data);
        } catch (error) {
            console.error('Error fetching malfunction:', error);
        }
    };

    const deleteMalfunction = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });
    
            if (response.ok) {
                navigate(-1);
            } else {
                alert('Failed to remove malfunction.');
            }
        } catch (error) {
            console.error('Error removing malfunction:', error);
        }
    };

    if (!malfunction) {
        return <div>Loading...</div>;
    }

    return (
        <div className="malfunction">
            <div className="malfunction__header">
                <h1 className="malfunction__title">Usterka</h1>
                <img className="malfunction__close" src="./../close.png" alt="go-back-btn"
                     onClick={() => navigate(-1)}/>
            </div>

            <img className="malfunction__image" src="./../malfunction.png" alt="session"/>

            <span className="malfunction__label">Nazwa:</span>
            <h2 className="malfunction__subtitle">{malfunction.name}</h2>

            <span className="malfunction__label">Opis:</span>
            <h2 className="malfunction__subtitle">{malfunction.description}</h2>

            <span className="malfunction__label">Status:</span>
            <h2 className="malfunction__subtitle">{malfunction.status ? "Rozwiązana" : "Oczekuje na rozwiązanie"}</h2>

            <span className="malfunction__label">Dotyczy:</span>
            <h2 className="malfunction__subtitle">{malfunction.devices}</h2>

            <span className="malfunction__label">Osoba zgłaszająca:</span>
            <h2 className="malfunction__subtitle">{malfunction.userReporter}</h2>

            <span className="malfunction__label">Osoba przyjmująca zgłoszenie:</span>
            <h2 className="malfunction__subtitle">{malfunction.userHandler}</h2>

            <span className="malfunction__label">Data zgłoszenia:</span>
            <h2 className="malfunction__subtitle">{malfunction.dateBegin}</h2>

            <span className="malfunction__label">Data zamknięcia:</span>
            <h2 className="malfunction__subtitle">{malfunction.dateEnd ? malfunction.dateEnd : "---"}</h2>

            <div className="malfunction__buttons">
                <button className="button">Dodaj rozwiąznie</button>
                <button className="button">Zamknij usterkę</button>
                <button className="button">Wygeneruj raport</button>
                <button className="button" onClick={deleteMalfunction}>Usuń usterkę</button>
            </div>
        </div>
    );
}

export default Malfunction;