import React, {useState, useEffect, useContext} from 'react';
import { useNavigate } from 'react-router-dom';
import './css/devices.css';
import './css/partials/loading.css';
import AuthContext from "./AuthContext";

const Devices = () => {
  const {userRoles} = useContext(AuthContext);
  const [devices, setDevices] = useState([]);
  const [loading, setLoading] = useState(true);
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
      setLoading(false);
    } catch (error) {
      console.error('Error fetching devices:', error);
    }
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

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

  const navigateToDevice = (id) => {
    navigate(`/devices/${id}`);
  };

  return (
    <div className="devices">
      <h1 className="devices__title">Urządzenia</h1>
      <h2 className="devices__info">Wyszukaj urządzenie po nazwie lub kliknij na nazwę urządzenia aby uzyskać więcej informacji.</h2>
      <div className="malfunctions__group">
        <div className="devices__search">
          <img className="devices__search-icon" src="./search.png"></img>
          <input className="devices__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
        </div>

        <>
          {userRoles.some(role => role === 'Engineer' || role === 'Admin')  && (
              <button className="button" onClick={() => {navigate('/createDevice');}}>
                Dodaj nowe urządzenie
              </button>
          )}
        </>
      </div>

      <div className="devices__group">
        <div className="devices__group-items">
        {filteredDevices.map(device => (
            <div className="devices__card" key={device.id} onClick={() => navigateToDevice(device.id)}>
              <h2 className="devices__card__title">{device.name}</h2>
              <div className="devices__card__tag">{device.tag}</div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Devices;
