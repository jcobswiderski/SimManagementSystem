import React, {useState, useEffect, useContext} from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './css/test.css';
import './css/partials/button.css';
import TestQtgReport from './reports/TestQtgReport';
import { PDFDownloadLink } from '@react-pdf/renderer';
import AuthContext from "./AuthContext";

const Test = ({showAlert}) => {
    const {userRoles} = useContext(AuthContext);
    const {id} = useParams();
    const [test, setTest] = useState(null);

    const navigate = useNavigate();

    useEffect(() => {
        refreshData();
    }, []);

    const refreshData = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/TestResults/${id}`);
            const data = await response.json();
            setTest(data);
        } catch (error) {
            console.error('Error fetching test:', error);
        }
    };

    const deleteTest = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/TestResults/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            const responseBody = await response.text();
            console.log(responseBody);

            if (response.ok) {
                showAlert('Usunięto test!', 'success');
                navigate(-1);
            } else {
                showAlert('Nie udało się usunąć testu!', 'error');
            }
        } catch (error) {
            console.error('Error removing test:', error);
        }
    };

    if (!test) {
        return <div>Loading...</div>;
    }

    return (
        <div className="test">
            <div className="test__header">
                <h1 className="test__title">Test QTG</h1>
                <img className="test__close" src="./../close.png" alt="go-back-btn"
                     onClick={() => navigate(-1)}/>
            </div>

            <img className="test__image" src="./../test.png" alt="session"/>

            <span className="test__label">Nazwa:</span>
            <h2 className="test__subtitle">{test.title}</h2>

            <span className="test__label">Etap:</span>
            <h2 className="test__subtitle">{test.stage}</h2>

            <span className="test__label">Opis:</span>
            <h2 className="test__subtitle">{test.description}</h2>

            <span className="test__label">Wynik:</span>
            <h2 className="test__subtitle">{test.isPassed ? "pozytywny" : "negatywny"}</h2>

            <span className="test__label">Data wykonania:</span>
            <h2 className="test__subtitle">{test.date}</h2>

            <span className="test__label">Osoba wykonująca test:</span>
            <h2 className="test__subtitle">{test.executor}</h2>

            <div className="test__buttons">
                <PDFDownloadLink document={ <TestQtgReport test={test} /> } fileName={`raport-testQtg-nr${test.id}.pdf`}>
                    <button className="button">Wygeneruj raport</button>
                </PDFDownloadLink>
                <>
                    {userRoles.some(role => role === 'Engineer' || role === 'Admin')  && (
                        <button className="button" onClick={deleteTest}>Usuń test</button>
                    )}
                </>
            </div>
        </div>
    );
}

export default Test;