import React, { useState, useEffect } from 'react';
import './css/createMaintenance.css';
import { useNavigate } from 'react-router-dom';
import './css/partials/button.css';

const CreateMaintenance = ({userId, showAlert}) => {
    const [maintenanceTypes, setMaintenanceTypes] = useState([]);
    const [maintenanceTypeId, setMaintenanceTypeId] = useState(1);
    const [maintenanceDate, setMaintenanceDate] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
        refreshData();
    }, []);

    const refreshData = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/MaintenanceTypes`);
            const data = await response.json();
            setMaintenanceTypes(data);
        } catch (error) {
            console.error('Error fetching maintenance types:', error);
        }
    };

    const handleMaintenanceTypeIdChange = (e) => {
        setMaintenanceTypeId(e.target.value);
    };

    const handleMaintenanceDateChange = (e) => {
        setMaintenanceDate(e.target.value);
    };

    const addMaintenance = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    type: maintenanceTypeId,
                    executor: userId,
                    date: maintenanceDate
                }),
            });

            if (response.ok) {
                showAlert('Dodano nową obsługę!', 'success');
                navigate(-1);
            } else {
                showAlert('Wypełnij wymagane pola przed dodaniem nowej obsługi!', 'error');
            }
        } catch (error) {
            console.error('Błąd przy dodawaniu obsługi:', error);
        }
    };

    return (
        <div className="createMaintenance">
            <div className="createMalfunction__header">
                <h1 className="createMalfunction__title">Tworzenie nowej obsługi</h1>
                <img className="createMalfunction__close" src="./../close.png" alt="go-back-btn"
                     onClick={() => navigate(-1)}/>
            </div>
            <div className="createMaintenance__form">
                <div className="createMaintenance__group">
                    <span className="createMaintenance__label">Rodzaj obsługi</span>
                    <select className="createMaintenance__input" value={maintenanceTypeId}
                            onChange={handleMaintenanceTypeIdChange}>
                        {maintenanceTypes.map(m => (
                            <option className="createSimSession__option" key={m.id} value={m.id}>
                                {m.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="createMaintenance__group">
                    <span className="createMaintenance__label">Data</span>
                    <input className="createMaintenance__input" type="date"
                           onChange={handleMaintenanceDateChange}></input>
                </div>
                <button className="button" onClick={addMaintenance}>Zapisz</button>
            </div>
        </div>
    );
}

export default CreateMaintenance;