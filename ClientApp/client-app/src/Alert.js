import React, { useEffect } from 'react';
import './css/alert.css';

const Alert = ({ message, color, onClose }) => {
    useEffect(() => {
        const timer = setTimeout(() => {
            onClose(); }, 3000);

        return () => clearTimeout(timer);
    }, [onClose]);

    return (
        <div className={`alert alert--${color}`}>{message}</div>
    );
};

export default Alert;
