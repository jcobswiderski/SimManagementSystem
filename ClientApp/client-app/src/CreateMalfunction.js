import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createMalfunction.css';
import './css/partials/button.css';

const CreateMalfunction = ({userId, showAlert}) => {
  const [devices, setDevices] = useState([]);
  const [users, setUsers] = useState([]);

  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [reporterId, setReporterId] = useState(1);
  const [dateBegin, setDateBegin] = useState('');
  const [deviceIds, setDeviceIds] = useState([]);

  const navigate = useNavigate();
  
  useEffect(() => {
    refreshData();
  }, []);

  const refreshData = async () => {
    try {
      const responseDevices = await fetch(`${process.env.REACT_APP_API_URL}/Devices`);
      const dataDevices = await responseDevices.json();
      setDevices(dataDevices);

      const responseUsers = await fetch(`${process.env.REACT_APP_API_URL}/Users`);
      const dataUsers = await responseUsers.json();
      setUsers(dataUsers);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  const handleNameChange = (e) => {
    setName(e.target.value);
  };

  const handleDescriptionChange = (e) => {
    setDescription(e.target.value);
  };

  const handleReporterChange = (e) => {
    setReporterId(e.target.value);
  };
  
  const handleDateBeginChange = (e) => {
    setDateBegin(e.target.value);
  };

  const handleDeviceIdsChange = (e) => {
    const selectedOptions = Array.from(e.target.selectedOptions, option => option.value);
    setDeviceIds(selectedOptions);
};


const addMalfunction = async () => {
  if (!name || !description || !dateBegin || deviceIds.length === 0) {
    showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
    return;
  }

  try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions`, {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify({ 
              name: name,
              description: description,
              userReporter: reporterId,
              userHandler: userId,
              dateBegin: dateBegin,
              status: false,
              devices: deviceIds
            }),
      });

      if (response.ok) {
        showAlert('Pomyślnie dodano nową usterkę!', 'success');
        navigate(-1);
      } else {
        showAlert('Nie udało się dodać nowej usterki!', 'error');
      }
  } catch (error) {
      console.error('Error adding malfunction:', error);
  }
};

  return ( 
      <div className="createMalfunction">
          <div className="createMalfunction__header">
              <h1 className="createMalfunction__title">Dodawanie nowej usterki</h1>
              <img className="createMalfunction__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
          </div>

          <span className="createMalfunction__label">Nazwa</span> 
          <input type="text" className="createMalfunction__input" value={name} onChange={handleNameChange}/>

          <span className="createMalfunction__label">Opis</span> 
          <input type="text" className="createMalfunction__input createMalfunction__input--large" value={description} onChange={handleDescriptionChange}/>

          <span className="createMalfunction__label">Dotyczy (można zaznaczyć kilka)</span>
          <select className="createMalfunction__input createMalfunction__input--large" value={deviceIds} onChange={handleDeviceIdsChange} multiple>
              {devices.map(d => (
                  <option className="createMalfunction__option" key={d.id} value={d.id}>
                      {d.name}
                  </option>
              ))}
          </select>

          <span className="createMalfunction__label">Osoba zgłaszająca</span>
          <select className="createMalfunction__input" value={reporterId} onChange={handleReporterChange}>
              {users.map(u => (
                  <option className="createMalfunction__option" key={u.id} value={u.id}>
                      {u.firstName} {u.lastName}
                  </option>
              ))}
          </select>
          
          <span className="createMalfunction__label">Data zgłoszenia</span> 
          <input type="datetime-local" className="createMalfunction__input" value={dateBegin} onChange={handleDateBeginChange}/>

          <button className="button createMalfunction__button" onClick={addMalfunction}>Zapisz</button>
      </div>
  );
}

export default CreateMalfunction;