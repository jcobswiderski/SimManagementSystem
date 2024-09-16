import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createSimSession.css';

const CreateSimSession = () => {
    const [predefinedSessions, setPredefinedSessions] = useState([]);
    const [predefinedSessionId, setPredefinedSessionId] = useState('');

    const [users, setUsers] = useState([]);
    const [pilotId, setPilotId] = useState('');
    const [copilotId, setCopilotId] = useState('');
    const [observerId, setObserverId] = useState('');
    const [supervisorId, setSupervisorId] = useState('');

    const [beginDate, setBeginDate] = useState('');

    const navigate = useNavigate();
    
    useEffect(() => {
      refreshData();
    }, []);
  
    const refreshData = async () => {
      try {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/PredefinedSessions`);
        const data = await response.json();
        setPredefinedSessions(data);

        const responseUsers = await fetch(`${process.env.REACT_APP_API_URL}/Users`);
        const dataUsers = await responseUsers.json();
        setUsers(dataUsers);
      } catch (error) {
        console.error('Error fetching predefined sessions:', error);
      }
    };

    const handlePredefinedSessionIdChange = (e) => {
      setPredefinedSessionId(e.target.value);
    };

    const handlePilotIdChange = (e) => {
        setPilotId(e.target.value);
    };

    const handleCopilotIdChange = (e) => {
      setCopilotId(e.target.value);
    };

    const handleObserverIdChange = (e) => {
      setObserverId(e.target.value);
    };

    const handleSupervisorIdChange = (e) => {
      setSupervisorId(e.target.value);
    };

    const handleBeginDateChange = (e) => {
      setBeginDate(beginDate);
    };

    return ( 
        <div className="createSimSession">
            <div className="createSimSession__header">
                <h1 className="createSimSession__title">Planowanie sesji symulatorowej</h1>
                <img className="createSimSession__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>

            <span className="createSimSession__label">Typ sesji</span>
            <select className="createSimSession__input" value={predefinedSessionId} onChange={handlePredefinedSessionIdChange}>
                {predefinedSessions.map(s => (
                    <option className="createSimSession__option" key={s.id} value={s.id}>
                        {s.name}
                    </option>
                ))}
            </select>
            
            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Pilot</span>
                  <select className="createSimSession__input" value={pilotId} onChange={handlePilotIdChange}>
                      {users.map(u => (
                          <option className="createSimSession__option" key={u.id} value={u.id}>
                              {u.firstName} {u.lastName}
                          </option>
                      ))}
                  </select>
                </div>

                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Copilot</span>
                  <select className="createSimSession__input" value={copilotId} onChange={handleCopilotIdChange}>
                      {users.map(u => (
                          <option className="createSimSession__option" key={u.id} value={u.id}>
                              {u.firstName} {u.lastName}
                          </option>
                      ))}
                  </select>
                </div>
              </div>
              
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Observer</span>
                  <select className="createSimSession__input" value={observerId} onChange={handleObserverIdChange}>
                      {users.map(u => (
                          <option className="createSimSession__option" key={u.id} value={u.id}>
                              {u.firstName} {u.lastName}
                          </option>
                      ))}
                  </select>
                </div>

                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Supervisor</span>
                  <select className="createSimSession__input" value={supervisorId} onChange={handleSupervisorIdChange}>
                      {users.map(u => (
                          <option className="createSimSession__option" key={u.id} value={u.id}>
                              {u.firstName} {u.lastName}
                          </option>
                      ))}
                  </select>
                </div>
              </div>
            </div>
            
            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Data rozpoczęcia</span> 
                  <input type="datetime-local" className="createSimSession__input" value={beginDate} onChange={handleBeginDateChange}></input>
                </div>

                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Data zakończenia</span> 
                  <input type="datetime-local" className="createSimSession__input" disabled></input>
                </div>
              </div>
            </div>

            <button className="createSimSession__save">Zapisz</button>
        </div>
    );
}
 
export default CreateSimSession;