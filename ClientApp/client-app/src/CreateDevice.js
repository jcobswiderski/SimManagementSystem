import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createDevice.css';
import './css/partials/button.css';

const CreateDevice = ({showAlert}) => {
    const [deviceName, setDeviceName] = useState('');
    const [deviceTag, setDeviceTag] = useState('');
    const navigate = useNavigate();
    
    const handleDeviceNameChange = (e) => {
        setDeviceName(e.target.value);
    };

    const handleDeviceTagChange = (e) => {
        setDeviceTag(e.target.value);
    };

    const addNewDevice = async () => {
        if (!deviceName || !deviceTag) {
            showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Devices`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ 
                    name: deviceName,
                    tag: deviceTag,
                })
            });

            if (response.ok) {
                showAlert('Pomyślnie dodano nowe urządzenie!', 'success');
                navigate(-1);
            } else {
                showAlert('Wypełnij wymagane pola przed dodaniem nowego urządzenia!', 'error');
            }
        } catch (error) {
            console.error('Error addning device:', error);
        }
    };

    return ( 
        <div className="createDevice">
            <div className="createDevice__header">
                <h1 className="createDevice__title">Dodaj nowe urządzenie</h1>
                <img className="createDevice__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>

            <span className="createDevice__label">Nazwa</span>
            <input className="createDevice__input" type="text" onChange={handleDeviceNameChange}/>

            <span className="createDevice__label">Tag</span>
            <input className="createDevice__input" type="text" onChange={handleDeviceTagChange} />
            <button className="button createDevice__button" onClick={addNewDevice}>Zapisz</button>
        </div>
    );
}
 
export default CreateDevice;