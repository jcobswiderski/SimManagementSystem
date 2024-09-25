import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/device.css';

const Device = () => {
    const {id} = useParams();
    const [device, setDevice] = useState(null);
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
        } catch (error) {
            console.error('Error fetching malfunctions:', error);
        }
    };

    const navigateToMalfunction = (id) => {
        navigate(`/malfunctions/${id}`);
    };

    if (!device) {
        return <div>Loading...</div>;
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
                        <div className="malfunctions__card-id">{m.id}</div>
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
        </div>
    );
}
 
export default Device;