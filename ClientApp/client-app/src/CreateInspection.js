import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createInspection.css';

const CreateInspection = () => {
    const [inspectionTypes, setInspectionTypes] = useState([]);
    const [inspectionTypeId, setInspectionTypeId] = useState(null);
    const [inspectionDate, setInspectionDate] = useState(null);
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
        // try {
        //     const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections`, {
        //         method: 'POST',
        //         headers: {
        //             'Content-Type': 'application/json',
        //         },
        //         body: JSON.stringify({ 
        //             inspectionTypeId: inspectionTypeId,
        //             date: inspectionDate,
        //             operator: ???
        //          }),
        //     });

        //     if (response.ok) {
        //         alert('Inspection added successfully!');
        //     } else {
        //         alert('Failed to add inspection.');
        //     }
        // } catch (error) {
        //     console.error('Error adding inspection:', error);
        // }
    };

    return ( 
        <div className="inspection">
            <div className="inspection__header">
                <h1 className="inspection__title">Zaplanuj nową obsługę</h1>
                <img className="inspection__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            <select className="inspection__select" value={inspectionTypeId} onChange={handleInspectionTypeChange}>
                {inspectionTypes.map(i => (
                    <option className="inspection__option" key={i.id} value={i.id}>
                        {i.name}
                    </option>
                ))}
            </select>
            <input className="inspection__date" type="date" onChange={handleInspectionDateChange}/>
            <button className="inspection__save" onClick={addNewInspection}>Zapisz</button>
        </div>
    );
}
 
export default CreateInspection;