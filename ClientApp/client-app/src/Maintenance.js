import React, {useState, useEffect, useContext} from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/maintenance.css';
import './css/partials/button.css';
import { PDFDownloadLink } from '@react-pdf/renderer';
import MaintenanceReport from './reports/MaintenanceReport';
import AuthContext from "./AuthContext";

const Maintenance = ({showAlert}) => {
    const {userRoles} = useContext(AuthContext);
    const {id} = useParams();
    const [loading, setLoading] = useState(true);
    const [maintenance, setMaintenance] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        refreshData();
    }, []);

    const refreshData = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances/${id}`);
            const data = await response.json();
            setMaintenance(data);

            setLoading(false);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const deleteMaintenance = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            const responseBody = await response.text();
            console.log(responseBody);

            if (response.ok) {
                showAlert('Usunięto obsługę!', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się usunąć obsługi!', 'error');
            }
        } catch (error) {
            console.error('Error removing maintenance:', error);
        }
    };

    const updateMaintenanceState = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                showAlert('Zaaktualizowano obsługę!', 'success')
                navigate(-1);
            } else {
                showAlert('Nie udało się zaaktualizować obsługi!', 'error');
            }
        } catch (error) {
            console.error('Error updating maintenance:', error);
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="maintenance">
            <div className="maintenance__header">
                <h1 className="maintenance__title">Obsługa</h1>
                <img className="maintenance__close" src="./../close.png" alt="go-back-btn"
                     onClick={() => navigate(-1)}/>
            </div>

            <img className="maintenance__image" src="./../maintenance.png" alt="session"/>

            <span className="maintenance__label">Nazwa:</span>
            <h2 className="maintenance__subtitle">{maintenance.name}</h2>

            <span className="maintenance__label">Przebieg:</span>
            <h2 className="maintenance__subtitle">{maintenance.tasks}</h2>

            <span className="maintenance__label">Status:</span>
            <h2 className="maintenance__subtitle">{maintenance.realized ? "Wykonana" : "Oczekuje na wykonanie"}</h2>

            <span className="maintenance__label">Osoba wykonująca:</span>
            <h2 className="maintenance__subtitle">{maintenance.executor}</h2>

            <span className="maintenance__label">Data:</span>
            <h2 className="maintenance__subtitle">{maintenance.date}</h2>

            <div className="maintenance__buttons">
                <>
                    {maintenance.realized === true ?
                        <PDFDownloadLink document={<MaintenanceReport maintenance={maintenance}/>} fileName={`raport-obsługi-nr${maintenance.id}.pdf`}>
                            <button className="button">Wygeneruj raport</button>
                        </PDFDownloadLink> : null
                    }

                    {maintenance.realized === false && userRoles.some(role => role === 'Engineer' || role === 'Admin') ?
                        <button className="button" onClick={() => updateMaintenanceState()}>
                            Potwierdź wykonanie obsługi
                        </button> : null
                    }

                    {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                        <button className="button" onClick={deleteMaintenance}>Usuń obsługę</button>
                    )}
                </>
            </div>
        </div>
    );
}

export default Maintenance;