import React, {useState, useEffect, useContext} from 'react';
import { useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/tests.css';
import './css/partials/button.css';
import AuthContext from "./AuthContext";

const Tests = () => {
    const {userRoles} = useContext(AuthContext);
    const [loading, setLoading] = useState(true);
    const [tests, setTests] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        refreshtests();
    }, []);

    const refreshtests = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/TestResults`);
            const data = await response.json();
            setTests(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching test results:', error);
        }
    };

    const filteredTestResults= tests.filter(t =>
        t.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
        (t.date && t.date.includes(searchTerm.toLowerCase()))
    );

    const navigateToTest = (id) => {
        navigate(`/tests/${id}`);
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="tests">
            <h1 className="tests__title">Wyniki testów QTG</h1>
            <div className="tests__group">
                <div className="tests__search">
                    <img className="tests__search-icon" src="./search.png" alt="search-icon"></img>
                    <input className="tests__input tests__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
                </div>
                {userRoles.some(role => role === 'Engineer' || role === 'Admin') && (
                    <button className="button" onClick={() =>  navigate(`/createTest`)}>
                        Wprowadź wynik testu
                    </button>
                )}
            </div>

            {filteredTestResults.map(t => (
                <div className={`tests__card ${t.isPassed ? 'tests__card--green' : 'tests__card--red'}`} key={t.id} onClick={() => navigateToTest(t.id)}>

                    <div className="tests__card-header">
                        <div className="tests__card-title">{t.title}</div>
                        <div className="tests__card-state">Wynik: {t.isPassed === true ? <span class="tests__card--done">pozytywny</span> : <span class="tests__card--waiting">negatywny</span> }</div>
                    </div>

                    <div className="tests__card-content">
                        <div className="tests__card-crew">Wykonał: {t.executor}</div>
                    </div>

                    <div className="tests__card-footer">
                        <div className="tests__card-duration">Data: {t.date}</div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default Tests;
