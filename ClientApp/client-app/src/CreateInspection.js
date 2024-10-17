import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createInspection.css';
import './css/partials/button.css';

const CreateInspection = ({userId, showAlert}) => {
    const [inspectionTypes, setInspectionTypes] = useState([]);
    const [inspectionTypeId, setInspectionTypeId] = useState(1);
    const [inspectionDate, setInspectionDate] = useState('');
    const [inspectionNotice, setInspectionNotice] = useState('');
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
    
    const handleInspectionNoticeChange = (e) => {
        setInspectionNotice(e.target.value);
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
                    operator: userId,
                    notice: inspectionNotice
                 }),
            });

            if (response.ok) {
                showAlert('Pomyślnie dodano nową inspekcję!', 'success');
                navigate(-1);
            } else {
                showAlert('Wypełnij wymagane pola przed dodaniem nowej aktywności!', 'error');
            }
        } catch (error) {
            console.error('Błąd przy dodawaniu inspekcji:', error);
        }
    };

    return ( 
        <div className="createInspection">
            <div className="createInspection__header">
                <h1 className="createInspection__title">Planowanie nowej aktywności</h1>
                <img className="createInspection__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>

            <span className="createInspection__label">Typ</span>
            <select className="createInspection__select" value={inspectionTypeId} onChange={handleInspectionTypeChange}>
                {inspectionTypes.map(i => (
                    <option className="createInspection__option" key={i.id} value={i.id}>
                        {i.name}
                    </option>
                ))}
            </select>

            <span className="createInspection__label">Data</span>
            <input className="createInspection__input" type="datetime-local" onChange={handleInspectionDateChange}/>

            <span className="createInspection__label">Opis</span>
            <input className="createInspection__input createInspection__input--text" type="text" onChange={handleInspectionNoticeChange} />
            <button className="button createInspection__button" onClick={addNewInspection}>Zapisz</button>
        </div>
    );
}
 
export default CreateInspection;