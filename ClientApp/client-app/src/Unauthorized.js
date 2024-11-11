import React from 'react';
import { useNavigate } from 'react-router-dom';
import './css/unauthorized.css';

const Unauthorized = () => {
    const navigate = useNavigate();

    return (
        <div className='unauthorized'>
            <h1 className='unauthorized__title'>Unauthorized</h1>
            <p className='unauthorized__subtitle'>You do not have permission to view this page.</p>
            <button className='button' onClick={() => navigate(-1)}>Back to home.</button>
        </div>
    );
};

export default Unauthorized;
