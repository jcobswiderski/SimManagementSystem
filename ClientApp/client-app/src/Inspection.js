import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom'; 
import './inspection.css';

const Inspection = () => {
    const {id} = useParams();
    const [inspection, setInspection] = useState(null);

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
            <h1 className="inspection__title">{inspection.inspectionType}</h1>
        </div>
    );
}
 
export default Inspection;