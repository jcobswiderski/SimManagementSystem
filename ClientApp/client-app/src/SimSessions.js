import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/simSessions.css';

const SimSessions = () => {
  const [simulatorSessions, setSimulatorSessions] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();
  
  useEffect(() => {
    refreshSimulatorSessions();
  }, []);

  const refreshSimulatorSessions = async () => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions`);
      const data = await response.json();
      setSimulatorSessions(data);
    } catch (error) {
      console.error('Error fetching inspections:', error);
    }
  };

  const filteredSimulatorSessions = simulatorSessions.filter(s => 
    s.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // const navigateToSession = (id) => {
  //   navigate(`/inspections/${id}`);
  // };

  if (!simulatorSessions) {
    return <div>Loading...</div>;
}

  return (
    <div className="simulatorSessions">
      <h1 className="simulatorSessions__title">Simulator sessions</h1>
      <div className="simulatorSessions__group">
        <div className="simulatorSessions__search">
          <img className="simulatorSessions__search-icon" src="./search.png"></img>
          <input className="simulatorSessions__input simulatorSessions__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
        </div>
        <button className="simulatorSessions__create" onClick={() => {navigate('/createSimSession');}}>
          Zaplanuj nową sesję
        </button>
      </div>
    
      {filteredSimulatorSessions.map(s => (
        <div className="simulatorSessions__card" key={s.id}>

          <div className="simulatorSessions__card-header">
            <div className="simulatorSessions__card-abbreviation">[{s.abbreviation}]</div>
            <div className="simulatorSessions__card-title">{s.name}</div>
            <div className="simulatorSessions__card-state">Stan: {s.realized == true ? "zrealizowana" : "zaplanowana"}</div>
          </div>
          
          <div className="simulatorSessions__card-content">
            <div className="simulatorSessions__card-crew">Pilot: {s.pilotName !== " " ? s.pilotName : "---"}</div>
            <div className="simulatorSessions__card-crew">Co-pilot: {s.copilotName !== " " ? s.copilotName : "---"}</div>
            <div className="simulatorSessions__card-crew">Obserwator: {s.observerName !== " " ? s.observerName : "---"}</div>
            <div className="simulatorSessions__card-crew">Instruktor: {s.supervisorName !== " " ? s.supervisorName : "---"}</div>
          </div>

          <div className="simulatorSessions__card-footer">
            <div className="simulatorSessions__card-duration">Czas trwania: {s.duration}</div>
            <div className="simulatorSessions__card-date">Początek: {s.beginDate}</div>
            <div className="simulatorSessions__card-date">Koniec: {s.endDate}</div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default SimSessions;
