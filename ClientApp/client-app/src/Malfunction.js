import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/malfunction.css';
import './css/partials/button.css';

const Malfunction = () => {
    const {id} = useParams();
    const [loading, setLoading] = useState(true);
    const [malfunction, setMalfunction] = useState(null);
    const [recoveryActions, setRecoveryActions] = useState([]);

    const [recoveryActionDate, setRecoveryActionDate] = useState(null);
    const [recoveryActionDescription, setRecoveryActionDescription] = useState(null);
    const [recoveryActionState, setRecoveryActionState] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        refreshData();
    }, []);

    const refreshData = async () => {
        try {
            const responseMalfunctions = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/${id}`);
            const dataMalfunctions = await responseMalfunctions.json();
            setMalfunction(dataMalfunctions);

            const responseRecoveryActions = await fetch(`${process.env.REACT_APP_API_URL}/RecoveryActions/${id}`);
            const dataRecoveryActions = await responseRecoveryActions.json();
            setRecoveryActions(dataRecoveryActions);

            setLoading(false);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const handleRecoveryActionDateChange = (e) => {
        setRecoveryActionDate(e.target.value);
    };

    const handleRecoveryActionDescriptionChange = (e) => {
        setRecoveryActionDescription(e.target.value);
    };

    const handleRecoveryActionStateChange = (e) => {
        setRecoveryActionState(e.target.value);
    };

    const deleteMalfunction = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            const responseBody = await response.text();
            console.log(responseBody);

            if (response.ok) {
                navigate(-1);
            } else {
                alert('Failed to remove malfunction.');
            }
        } catch (error) {
            console.error('Error removing malfunction:', error);
        }
    };

    const updateMalfunctionState = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    dateEnd: recoveryActionDate,
                    status: true
                }),
            });

            if (response.ok) {
                navigate(-1);
            } else {
                alert('Failed to update malfunction.');
            }
        } catch (error) {
            console.error('Error updating malfunction:', error);
        }
    };

    const addRecoveryAction = async () => {
        if (!recoveryActionDescription || !recoveryActionDate) {
            alert('Proszę wypełnić wszystkie wymagane pola.');
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/RecoveryActions`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    date: recoveryActionDate,
                    description: recoveryActionDescription,
                    malfunctionId: id
                }),
            });

            if (response.ok) {
                refreshData();

                if(recoveryActionState) {
                    updateMalfunctionState();
                }
            } else {
                alert('Failed to add recovery action.');
            }
        } catch (error) {
            console.error('Error adding recovery action:', error);
        }
    };

    const deleteRecoveryAction = async (id) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/RecoveryActions/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (response.ok) {
                refreshData();
            } else {
                alert('Failed to remove recovery action.');
            }
        } catch (error) {
            console.error('Error removing recovery action:', error);
        }
    }

    if (loading) {
        return <div className="loading">Loading...</div>;
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

            <span className="malfunction__label malfunction__label-mt20">Działania naprawcze:</span>
            {recoveryActions.map(action => (
                <div className="malfunction__action" key={action.id}>
                    <div className="malfunction__action-group">
                        <div className="malfunction__action-date">{action.date}</div>
                        <img className="malfunction__action-delete" src="./../clear.png" alt="" onClick={() => deleteRecoveryAction(action.id)}/>
                    </div>

                    <div className="malfunction__action-description">{action.description}</div>
                </div>
            ))}

            <>
                {malfunction.status === false ? (
                    <div className="malfunction__form">
                        <div className="malfunction__form-label">Opisz podjęte działanie naprawcze</div>
                        <input className="malfunction__form-input" type="text"
                               onChange={handleRecoveryActionDescriptionChange}/>

                        <div className="malfunction__form-label">Wprowadź datę zakończenia działania</div>
                        <input className="malfunction__form-input" type="datetime-local"
                               onChange={handleRecoveryActionDateChange}/>

                        <div className="group">
                            <div className="malfunction__form-label malfunction__form-label-check">Czy usterka została
                                rozwiązana?
                            </div>
                            <input className="malfunction__form-check" type="checkbox"
                                   onChange={handleRecoveryActionStateChange}/>
                        </div>

                        <button className="button malfunction__button" onClick={addRecoveryAction}>Dodaj rozwiąznie
                        </button>
                    </div>
                ) : null}
            </>


            <div className="malfunction__buttons">
                <button className="button">Wygeneruj raport</button>
                <button className="button" onClick={deleteMalfunction}>Usuń usterkę</button>
            </div>
        </div>
    );
}

export default Malfunction;