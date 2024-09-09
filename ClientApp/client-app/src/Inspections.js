import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './inspections.css';

const Inspections = () => {
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
    } catch (error) {
      console.error('Error fetching inspections:', error);
    }
  };

  const filteredInspections = inspections.filter(i => 
    i.inspectionType.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const createNewInspection = async () => {
    const newInspectionTypeId = document.querySelector('#newInspectionTypeId').value;
    const newInspectionDate = document.querySelector('#newInspectionDate').value;
    const newInspectionOperator = document.querySelector('#newInspectionOperator').value;
    
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            inspectionTypeId: newInspectionTypeId,
            date: newInspectionDate,
            operator: newInspectionOperator
        }),
        });
      
        const result = await response.json();
        alert(result.message || 'Inspection created successfully!');
        refreshInspections();
    } catch (error) {
        console.error('Error creating inspection:', error);
    }
  };

  const deleteInspection = async (id) => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections/${id}`, {
        method: 'DELETE',
      });
      
      if (response.ok) {
        alert('Inspection deleted successfully!');
        refreshInspections();
      } else {
        alert('Failed to delete inspection.');
      }
    } catch (error) {
      console.error('Error deleting inspection:', error);
    }
  };

  const navigateToInspection = (id) => {
    navigate(`/inspections/${id}`);
  };

  return (
    <div className="inspections">
      <h1 className="inspections__title">Obsługi</h1>
      <div className="inspections__search">
        <img className="inspections__search-icon" src="./search.png"></img>
        <input className="inspections__input inspections__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
      </div>
      
      {filteredInspections.map(inspection => (
        <div className="inspections__card" key={inspection.id}>
          <h2 className="inspections__card__title" onClick={() => navigateToInspection(inspection.id)}>{inspection.inspectionType}</h2>
          <img className="inspections__button inspections__button--delete" src="./clear.png" onClick={() => deleteInspection(inspection.id)}></img>
        </div>
      ))}
    </div>
  );
};

export default Inspections;
