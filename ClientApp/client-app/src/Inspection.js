import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; 
import './css/inspection.css';

const Inspection = () => {
    const {id} = useParams();
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
        } catch (error) {
            console.error('Error fetching inspection:', error);
        }
    };
    
    if (!inspection) {
        return <div>Loading...</div>;
    }

    return (
        <div className="inspection">
            <div className="inspection__header">
                <h1 className="inspection__title">{inspection.inspectionType}</h1>
                <img className="inspection__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>
            
        </div>
    );
}
 
export default Inspection;