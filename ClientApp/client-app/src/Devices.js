import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/devices.css';

const Devices = () => {
  const [devices, setDevices] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();
  
  useEffect(() => {
    refreshDevices();
  }, []);

  const refreshDevices = async () => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Devices`);
      const data = await response.json();
      setDevices(data);
    } catch (error) {
      console.error('Error fetching devices:', error);
    }
  };

  const filteredDevices = devices.filter(device => 
    device.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const createNewDevice = async () => {
    const newDeviceName = document.querySelector('#newDeviceName').value;
    const newDeviceTag = document.querySelector('#newDeviceTag').value;
    
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Devices`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          name: newDeviceName,
          tag: newDeviceTag,
        }),
      });
      
      const result = await response.json();
      alert(result.message || 'Device created successfully!');
      refreshDevices();
    } catch (error) {
      console.error('Error creating device:', error);
    }
  };

  const deleteDevice = async (id) => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Devices/${id}`, {
        method: 'DELETE',
      });
      
      if (response.ok) {
        alert('Device deleted successfully!');
        refreshDevices();
      } else {
        alert('Failed to delete device.');
      }
    } catch (error) {
      console.error('Error deleting device:', error);
    }
  };

  const navigateToDevice = (id) => {
    navigate(`/devices/${id}`);
  };

  return (
    <div className="devices">
      <h1 className="devices__title">Devices</h1>
      <h2 className="devices__info">Wyszukaj urządzenie po nazwie lub kliknij na nazwę urządzenia aby uzyskać więcej informacji.</h2>
      <div className="devices__search">
        <img className="devices__search-icon" src="./search.png"></img>
        <input className="devices__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
      </div>
      
      <div className="devices__group">
        <div className="devices__group-items">
          {filteredDevices.map(device => (
            <div className="devices__card" key={device.id}>
              <h2 className="devices__card__title" onClick={() => navigateToDevice(device.id)}>{device.name}</h2>
              <img className='devices__button devices__button--delete' src="./clear.png" onClick={() => deleteDevice(device.id)}></img>
            </div>
          ))}
        </div>
        
        <div className="devices__group-creating">
          <h2 className="devices__subtitle">Add new device</h2>
          <input className="devices__input" type="text" placeholder="Name" id="newDeviceName" />
          <input className="devices__input" type="text" placeholder="Tag" id="newDeviceTag" />
          <img className="devices__button devices__button--add" src="./add.png" onClick={createNewDevice}></img>
        </div>
      </div>
    </div>
  );
};

export default Devices;
