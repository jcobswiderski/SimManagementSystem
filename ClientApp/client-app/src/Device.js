import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/device.css';

const Device = () => {
    const {id} = useParams();
    const [device, setDevice] = useState(null);
    const navigate = useNavigate();


    useEffect(() => {
        refreshDevice();
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
        </div>
    );
}
 
export default Device;