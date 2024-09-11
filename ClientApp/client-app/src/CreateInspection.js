import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createInspection.css';

const CreateInspection = ({userId}) => {
    const [inspectionTypes, setInspectionTypes] = useState([]);
    const [inspectionTypeId, setInspectionTypeId] = useState('');
    const [inspectionDate, setInspectionDate] = useState('');
    const navigate = useNavigate();
    
    useEffect(() => {
      refreshData();
    }, []);
  
    const refreshData = async () => {
      try {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections/types`);
        const data = await response.json();
        setInspectionTypes(data);
      } catch (error) {
        console.error('Error fetching inspection types:', error);
      }
    };
    
    const handleInspectionTypeChange = (e) => {
        setInspectionTypeId(e.target.value);
    };

    const handleInspectionDateChange = (e) => {
        setInspectionDate(e.target.value);
    };

    const addNewInspection = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ 
                    inspectionTypeId: inspectionTypeId,
                    date: inspectionDate,
                    operator: userId
                 }),
            });

            if (response.ok) {
                alert('Inspection added successfully!');
                navigate(-1);
            } else {
                alert('Failed to add inspection.');
            }
        } catch (error) {
            console.error('Error adding inspection:', error);
        }
    };

    return ( 
        <div className="createInspection">
            <div className="createInspection__header">
                <h1 className="createInspection__title">Zaplanuj nową obsługę</h1>
                <img className="createInspection__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            <select className="createInspection__select" value={inspectionTypeId} onChange={handleInspectionTypeChange}>
                {inspectionTypes.map(i => (
                    <option className="createInspection__option" key={i.id} value={i.id}>
                        {i.name}
                    </option>
                ))}
            </select>
            <input className="createInspection__date" type="datetime-local" onChange={handleInspectionDateChange}/>
            <button className="createInspection__save" onClick={addNewInspection}>Zapisz</button>
        </div>
    );
}
 
export default CreateInspection;