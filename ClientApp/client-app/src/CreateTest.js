import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './css/createTest.css';
import './css/partials/button.css';

const CreateTest = ({userId, showAlert}) => {
    const [predefinedTests, setPredefinedTests] = useState([]);
    const [predefinedTestId, setPredefinedTestId] = useState(1);
    const [isPassed, setIsPassed] = useState(false);
    const [date, setDate] = useState(null);
    const [observation, setObservation] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
        refreshData();
    }, []);

    const refreshData = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/TestQTGs`);
            const data = await response.json();
            setPredefinedTests(data);
        } catch (error) {
            console.error('Error fetching predefined tests:', error);
        }
    };

    const handlePredefinedTestIdChange = (e) => {
        setPredefinedTestId(e.target.value);
    };

    const handleIsPassedChange = (e) => {
        setIsPassed(e.target.value);
    };

    const handleDateChange = (e) => {
        setDate(e.target.value);
    };

    const handleObservationChange = (e) => {
        setObservation(e.target.value);
    };

    const addNewTest = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/TestResults`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    test: predefinedTestId,
                    isPassed: isPassed === 'on' ? true : false,
                    date: date,
                    observation: observation,
                    executor: userId
                }),
            });

            if (response.ok) {
                showAlert('Dodano nowy test!', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się dodać nowego testu!', 'error');
            }
        } catch (error) {
            console.error('Error adding test result:', error);
        }
    };

    return (
        <div className="createTest">
            <div className="createTest__header">
                <h1 className="createTest__title">Dodawanie nowego testu</h1>
                <img className="createTest__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/>
            </div>

            <span className="createTest__label">Rodzaj</span>
            <select className="createTest__select" value={predefinedTestId} onChange={handlePredefinedTestIdChange}>
                {predefinedTests.map(t => (
                    <option className="createTest__option" key={t.id} value={t.id}>
                        {t.title}
                    </option>
                ))}
            </select>

            <span className="createTest__label">Data</span>
            <input className="createTest__input" type="datetime-local" onChange={handleDateChange}/>

            <span className="createTest__label">Obserwacje</span>
            <input className="createTest__input createTest__input--text" type="text" onChange={handleObservationChange}/>

            <div className="createTest__group">
                <div className="createTest__form-label createTest__form-label-check">Czy test został zakończony pozytywnie?</div>
                <input className="createTest__form-check" type="checkbox" onChange={handleIsPassedChange}/>
            </div>

            <button className="button createTest__button" onClick={addNewTest}>Zapisz</button>
        </div>
    );
}

export default CreateTest;