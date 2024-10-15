import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/inspection.css';
import './css/partials/button.css';

const Inspection = ({showAlert}) => {
    const {id} = useParams();
    const [loading, setLoading] = useState(true);
    const [inspection, setInspection] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        refreshInspection();
    }, []);
    
    const refreshInspection = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections/${id}`);
            const data = await response.json();
            setInspection(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching inspection:', error);
        }
    };
    
    const deleteInspection = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Inspections/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (response.ok) {
                showAlert('Pomyślnie usunięto inspekcję!', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się usunąć inspekcji!', 'error');
            }
        } catch (error) {
            console.error('Błąd przy usuwaniu inspekcji:', error);
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="inspection">
            <div className="inspection__header">
                <h1 className="inspection__title">Podgląd obsługi</h1>
                <img className="inspection__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            <span className="inspection__label">Nazwa:</span>
            <h2 className="inspection__subtitle">{inspection.inspectionType}</h2>
            <span className="inspection__label">Data:</span>
            <div className="inspection__date">{inspection.date}</div>
            <span className="inspection__label">Operator:</span>
            <div className="inspection__operator">{inspection.operator}</div>
            <span className="inspection__label">Comments:</span>
            <div className="inspection__date">{inspection.notice != null ? inspection.notice : "---"}</div>
            <button className="button" onClick={deleteInspection}>Usuń obsługę</button>
        </div>
    );
}
 
export default Inspection;