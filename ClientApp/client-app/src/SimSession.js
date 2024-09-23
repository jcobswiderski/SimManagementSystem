import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/simSession.css';

const Inspection = () => {
    const {id} = useParams();
    const [session, setSession] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        refreshSession();
    }, []);
    
    const refreshSession = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/${id}`);
            const data = await response.json();
            setSession(data);
        } catch (error) {
            console.error('Error fetching session:', error);
        }
    };

    const deleteSimSession = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (response.ok) {
                navigate(-1);
            } else {
                alert('Failed to remove session.');
            }
        } catch (error) {
            console.error('Error removing session:', error);
        }
    };

    const updateSimSession = async () => {
        const sessionDate = new Date(session.beginDate);
        const currentDate = new Date();

        if(currentDate >= sessionDate) {
            try {
                const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/${id}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                });

                if (response.ok) {
                    refreshSession();
                } else {
                    alert('Failed to update simulator session.');
                }
            } catch (error) {
                console.error('Error updating simulator session:', error);
            }
        } else {
            alert('Cannot update session from the future!');
        }


    };

    if (!session) {
        return <div>Loading...</div>;
    }

    return (
        <div className="simSession">
            <div className="simSession__header">
                <h1 className="simSession__title">Sesja symulatorowa</h1>
                <img className="simSession__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/>
            </div>

            <img className="simSession__image" src="./../session.png" alt="session"/>

            <span className="simSession__label">Skrót:</span>
            <h2 className="simSession__subtitle">{session.abbreviation}</h2>

            <span className="simSession__label">Nazwa:</span>
            <h2 className="simSession__subtitle">{session.name}</h2>

            <span className="simSession__label">Opis:</span>
            <h2 className="simSession__subtitle">{session.description}</h2>

            <span className="simSession__label">Czas trwania:</span>
            <h2 className="simSession__subtitle">{session.duration}</h2>

            <span className="simSession__label">Data rozpoczęcia:</span>
            <h2 className="simSession__subtitle">{session.beginDate}</h2>

            <span className="simSession__label">Data zakończenia:</span>
            <h2 className="simSession__subtitle">{session.endDate}</h2>

            <span className="simSession__label">Pilot:</span>
            <h2 className="simSession__subtitle">{session.pilotName !== ' ' ? session.pilotName : "---"}</h2>

            <span className="simSession__label">Copilot:</span>
            <h2 className="simSession__subtitle">{session.copilotName !== ' ' ? session.copilotName : "---"}</h2>

            <span className="simSession__label">Obserwator:</span>
            <h2 className="simSession__subtitle">{session.observerName !== ' ' ? session.observerName : "---"}</h2>

            <span className="simSession__label">Instruktor:</span>
            <h2 className="simSession__subtitle">{session.supervisorName !== ' ' ? session.supervisorName : "---"}</h2>

            <span className="simSession__label">Status:</span>
            <h2 className="simSession__subtitle">{session.realized ? "Zrealizowano" : "Zaplanowano"}</h2>

            <div className="simSession__buttons">
                <>
                    {session.realized === false ?
                        <button className="simSession__button" onClick={updateSimSession}>Oznacz jako
                            ukończona</button> : null
                    }
                </>
                <button className="simSession__button">Wygeneruj raport</button>
                <button className="simSession__button" onClick={deleteSimSession}>Usuń sesję</button>
            </div>

        </div>
    );
}

export default Inspection;