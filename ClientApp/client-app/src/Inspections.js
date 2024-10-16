import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/inspections.css';
import './css/partials/button.css';

const Inspections = () => {
  const [loading, setLoading] = useState(true);
  const [inspections, setInspections] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const navigate = useNavigate();
  
  useEffect(() => {
    refreshInspections();
  }, []);

  const refreshInspections = async () => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections`);
      const data = await response.json();
      setInspections(data);
      setLoading(false);
    } catch (error) {
      console.error('Error fetching inspections:', error);
    }
  };

  const filteredInspections = inspections.filter(i => 
    i.inspectionType.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const navigateToInspection = (id) => {
    navigate(`/inspections/${id}`);
  };

  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  return (
    <div className="inspections">
      <h1 className="inspections__title">Zadania inspekcyjne</h1>
      <div className="inspections__group">
        <div className="inspections__search">
          <img className="inspections__search-icon" src="./search.png"></img>
          <input className="inspections__input inspections__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
        </div>
        <button className="button" onClick={() => {navigate('/createInspection');}}>
          Zaplanuj nową czynność
        </button>
      </div>
    
      {filteredInspections.map(inspection => (
        <div className="inspections__card" key={inspection.id} onClick={() => navigateToInspection(inspection.id)}>
          <div className="inspections__card__title">
            {inspection.inspectionType}
          </div>
          <div className="inspections__card__info">
            <div className="inspections__card__date">{inspection.date}</div>
            <div className="inspections__card__operator">Made by: {inspection.operator}</div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default Inspections;
