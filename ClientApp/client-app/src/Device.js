import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/device.css';

const Device = ({showAlert}) => {
    const {id} = useParams();
    const [loading, setLoading] = useState(true);
    const [device, setDevice] = useState('');
    const [malfunctions, setMalfunctions] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        refreshDevice();
        refreshMalfunctions();
    }, []);
    
    const refreshDevice = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Devices/${id}`);
            const data = await response.json();
            setDevice(data);
        } catch (error) {
            console.error('Error fetching device:', error);
        }
    };

    const refreshMalfunctions = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions/device/${id}`);
            const data = await response.json();
            setMalfunctions(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching malfunctions:', error);
        }
    };

    const navigateToMalfunction = (id) => {
        navigate(`/malfunctions/${id}`);
    };

    const deleteDevice = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Devices/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (response.ok) {
                showAlert('Usunięto urządzenie!', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się usunąć urządzenia!', 'error');
            }
        } catch (error) {
            console.error('Error removing device:', error);
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="device">
            <div className="device__header">
                <h1 className="device__title">{device.name}</h1>
                <img className="device__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            <h2 className="device__tag">Tag: {device.tag}</h2>

            {malfunctions.map(m => (
                <div className={`malfunctions__card ${m.status ? 'malfunctions__card--green' : 'malfunctions__card--red'}`} key={m.id} onClick={() => navigateToMalfunction(m.id)}>


                    <div className="malfunctions__card-header">
                        <div className="malfunctions__card-subgroup">
                            <div className="malfunctions__card-title">{m.name}</div>
                            <div className="malfunctions__card-status">Status: {m.status == false ? "oczekuje na rozwiązanie" : "rozwiązana"}</div>
                        </div>
                    </div>

                    <div className="malfunctions__card-content">
                        <div className="simulatorSessions__card-crew">Zgłosił: {m.userReporter}</div>
                        <div className="simulatorSessions__card-crew">Przyjął: {m.userHandler}</div>
                    </div>

                    <div className="malfunctions__card-footer">
                        <div className="malfunctions__card-date">Początek: {m.dateBegin}</div>
                        <div className="malfunctions__card-date">Koniec: {m.dateEnd == null ? "---" : m.dateEnd}</div>
                    </div>
                </div>
            ))}
            <>
                {malfunctions.length === 0 ? 
                    <button className="button" onClick={deleteDevice}>Usuń urządzenie</button> : null
                } 
            </>
            
        </div>
    );
}
 
export default Device;