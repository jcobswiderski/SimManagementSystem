import React, {useState, useEffect, useContext} from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import AuthContext from "./AuthContext";
import SimulatorSessionReport from './reports/SimulatorSessionReport';
import { PDFDownloadLink } from '@react-pdf/renderer';
import './css/partials/loading.css';
import './css/simSession.css';

const SimSession = ({showAlert}) => {
    const {userRoles} = useContext(AuthContext);
    const {id} = useParams();
    const [loading, setLoading] = useState(true);
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
            setLoading(false);
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
                showAlert('Pomyślnie usunięto sesję symulatorową.', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się usunąć sesji symulatorowej!', 'error');
            }
        } catch (error) {
            console.error('Błąd przy usuwaniu sesji symulatorowej:', error);
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
                    showAlert('Pomyślnie zaaktualizowano sesję symulatorową!', 'success');
                    refreshSession();
                } else {
                    showAlert('Nie udało się zaaktualizować sesji symulatorowej!', 'error');
                }
            } catch (error) {
                console.error('Błąd przy aktualizacji sesji symulatorowej:', error);
            }
        } else {
            showAlert('Nie można aktualizować sesji z przyszłości!', 'info');
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="simSession">
            <div className="simSession__header">
                <h1 className="simSession__title">Sesja symulatorowa</h1>
                <img className="simSession__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/>
            </div>

            <img className="simSession__image" src="./../session.png" alt="session"/>

            <span className="simSession__label">Rodzaj:</span>
            <h2 className="simSession__subtitle">{session.category}</h2>

            <span className="simSession__label">Nazwa:</span>
            <h2 className="simSession__subtitle">{session.name} ({session.abbreviation})</h2>

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

                <>
                    {session.realized === true ?
                        <PDFDownloadLink document={ <SimulatorSessionReport session={session} /> } fileName={`raport-sesja-symulatorowa-nr${session.id}.pdf`}>
                            <button className="simSession__button">Wygeneruj raport</button>
                        </PDFDownloadLink> : null
                    }
                </>

                {userRoles.some(role => role === 'Engineer' || role === 'Admin' || role === 'Planner') && (
                    <button className="simSession__button" onClick={deleteSimSession}>Usuń sesję</button>
                )}
            </div>

        </div>
    );
}

export default SimSession;