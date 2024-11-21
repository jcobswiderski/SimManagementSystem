import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createSimSession.css';
import './css/partials/button.css';

const CreateSimSession = ({showAlert}) => {
    const [predefinedSessions, setPredefinedSessions] = useState([]);
    const [predefinedSessionId, setPredefinedSessionId] = useState(1);

    const [users, setUsers] = useState([]);
    const [pilotId, setPilotId] = useState(null);
    const [copilotId, setCopilotId] = useState(null);
    const [observerId, setObserverId] = useState(null);
    const [supervisorId, setSupervisorId] = useState(null);

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
      setBeginDate(e.target.value);
    };

    const getDurationForSessionId = async (id) => {
        const tempSession = predefinedSessions.find(s => s.id == id);
        return tempSession.duration;
    }

    const checkIfConflictOccurs = async () => {
        const duration = await getDurationForSessionId(predefinedSessionId);
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions/checkConflict?dateBegin=${beginDate}&duration=${duration}`);
            const data = await response.json();
            return data || 0;
        } catch (error) {
            console.error('Error fetching conflict sessions:', error);
        }
    }

    const addSimulatorSession = async () => {
        const isConflict = await checkIfConflictOccurs();

        if(isConflict && isConflict > 0)
        {
            showAlert('Niepowodzenie! W tym czasie zaplanowano inną sesję!', 'error');
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    predefinedSession: predefinedSessionId,
                    beginDate: beginDate,
                    pilotSeat: pilotId,
                    copilotSeat: copilotId,
                    supervisorSeat: supervisorId,
                    observerSeat: observerId,
                    realized: false
                }),
            });

            if (response.ok) {
              showAlert('Pomyślnie dodano nową sesję symulatorową.', 'success');
              navigate(-1);
            } else {
              showAlert('Nie udało się dodać sesji symulatorowej, wypełnij wymagane pola!', 'error');
            }
        } catch (error) {
            console.error('Error adding session:', error);
            showAlert('Wystąpił błąd podczas dodawania sesji symulatorowej!.', 'error');
        }
    };

    return ( 
        <div className="createSimSession">
         {/* {showAlert && <Alert message={alertMessage} onClose={handleCloseAlert} />} */}
            <div className="createSimSession__header">
                <h1 className="createSimSession__title">Planowanie sesji symulatorowej</h1>
                <img className="createSimSession__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>

            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Typ sesji</span>
                  <select className="createSimSession__input" value={predefinedSessionId} onChange={handlePredefinedSessionIdChange}>
                      {predefinedSessions.map(s => (
                          <option className="createSimSession__option" key={s.id} value={s.id}>
                              {s.name}
                          </option>
                      ))}
                  </select>
                </div>
                <div className="createSimSession__group--single">
                  <button className="button createSimSession__button createSimSession__button--scheme" onClick={() => navigate('/createPredefinedSession')}>Stwórz nowy szablon</button>
                </div>  
              </div>
            </div>
            
            
            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Pilot</span>
                  <select className="createSimSession__input" value={pilotId} onChange={handlePilotIdChange}>
                  <option className="createSimSession__option" value={null}>---</option>
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
                  <option className="createSimSession__option" value={null}>---</option>
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
                      <option className="createSimSession__option" value={null}>---</option>
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
                  <option className="createSimSession__option" value={null}>---</option>
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
              </div>
            </div>

            <button className="button createSimSession__button" onClick={addSimulatorSession}>Zapisz</button>
        </div>
    );
}
 
export default CreateSimSession;