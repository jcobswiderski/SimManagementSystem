import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/maintenances.css';
import './css/partials/button.css';

const Maintenances = () => {
    const [maintenances, setMaintenances] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        refreshMaintenances();
    }, []);

    const refreshMaintenances = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances`);
            const data = await response.json();
            setMaintenances(data);
        } catch (error) {
            console.error('Error fetching maintenances:', error);
        }
    };

    const filteredMaintenances = maintenances.filter(m =>
        m.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const deleteMaintenance = async (id) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (response.ok) {
                refreshMaintenances();
            } else {
                alert('Failed to remove maintenance.');
            }
        } catch (error) {
            console.error('Error removing maintenance:', error);
        }
    }

    if (!maintenances) {
        return <div>Loading...</div>;
    }

    return (
        <div className="maintenances">
            <h1 className="maintenances__title">Konserwacja symulatora</h1>
            <div className="maintenances__group">
                <div className="maintenances__search">
                    <img className="maintenances__search-icon" src="./search.png"></img>
                    <input className="maintenances__input maintenances__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
                </div>
                <button className="button" onClick={() => {navigate('/createMaintenance');}}>Wprowadź nową obsługę</button>
            </div>

            {filteredMaintenances.map(m => (
                <div className={`maintenances__card maintenances__card--orange`} key={m.id}>

                    <div className="maintenances__card-header">
                        <div className="maintenances__card-title">{m.id}</div>
                        <div className="maintenances__card-subgroup">
                            <div className="maintenances__card-title">{m.name}</div>
                            <div className="simulatorSessions__card-crew">Wykonana przez: {m.executor}</div>
                        </div>
                    </div>

                    <div className="maintenances__card-footer">
                        <div className="maintenances__card-date">Data: {m.date}</div>
                        <button className="button maintenances__button" onClick={() => deleteMaintenance(m.id)}>Usuń</button>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default Maintenances;
