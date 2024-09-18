import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/malfunctions.css';
import './css/partials/button.css';

const Malfunctions = () => {
    const [malfunctions, setMalfunctions] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        refreshMalfunctions();
    }, []);

    const refreshMalfunctions = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Malfunctions`);
            const data = await response.json();
            setMalfunctions(data);
        } catch (error) {
            console.error('Error fetching malfunctions:', error);
        }
    };

    const filteredMalfunctions = malfunctions.filter(m =>
        m.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const navigateToMalfunction = (id) => {
        navigate(`/malfunctions/${id}`);
    };

    if (!malfunctions) {
        return <div>Loading...</div>;
    }

    return (
        <div className="malfunctions">
            <h1 className="malfunctions__title">Usterki</h1>
            <div className="malfunctions__group">
                <div className="malfunctions__search">
                    <img className="malfunctions__search-icon" src="./search.png"></img>
                    <input className="malfunctions__input malfunctions__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
                </div>
                <button className="button">
                    Dodaj nową usterkę
                </button>
            </div>

            {filteredMalfunctions.map(m => (
                <div className={`malfunctions__card ${m.status ? 'malfunctions__card--green' : 'malfunctions__card--red'}`} key={m.id} onClick={() => navigateToMalfunction(m.id)}>


                    <div className="malfunctions__card-header">
                        <div className="malfunctions__card-id">{m.id}</div>
                        <div className="malfunctions__card-subgroup">
                            <div className="malfunctions__card-title">{m.name}</div>
                            <div className="malfunctions__card-status">Status: {m.status == false ? "oczekuje na rozwiązanie" : "rozwiązana"}</div>
                        </div>
                    </div>

                    <div className="malfunctions__card-content">
                        <div className="simulatorSessions__card-crew">Zgłosił: {m.userReporter}</div>
                        <div className="simulatorSessions__card-crew">Przyjął: {m.userHandler}</div>
                    </div>

                    <div className="malfunctions__card-footer">
                        <div className="malfunctions__card-date">Początek: {m.dateBegin}</div>
                        <div className="malfunctions__card-date">Koniec: {m.dateEnd == null ? "---" : m.dateEnd}</div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default Malfunctions;