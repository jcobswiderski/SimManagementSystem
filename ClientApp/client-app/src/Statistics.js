import { useNavigate } from 'react-router-dom';
import './css/statistics.css'
import { useState } from 'react';

const Statistics = ({showAlert}) => {
    const navigate = useNavigate();
    const [beginDate, setBeginDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [formBeginDate, setFormBeginDate] = useState('');
    const [formEndDate, setFormEndDate] = useState('');
    const [malfunctionsCount, setMalfunctionsCount] = useState(null);

    const handleBeginDateChange = (e) => {
        setBeginDate(e.target.value);
    }

    const handleEndDateChange = (e) => {
        setEndDate(e.target.value);;
    }

    const generateStatistics = async () => {
        if(beginDate == '' || endDate == '') {
            showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
            return;
        }
        if (beginDate > endDate) {
            showAlert('Data początku nie może być późniejsza niż data końca.', 'error');
            return;
        }

        setFormBeginDate(beginDate);
        setFormEndDate(endDate);

        try {
            const response = await fetch(
                `${process.env.REACT_APP_API_URL}/Malfunctions/count?dateBegin=${beginDate}&dateEnd=${endDate}`
            );

            const data = await response.json();
            setMalfunctionsCount(data);
        } catch (error) {
            console.error('Error fetching malfunctions count:', error);
        }
    }

    return (
        <div className="statistics">
            <div className="statistics__header">
                <h1 className="statistics__title">Statistics</h1>
                <img className="statistics__close" src="./../../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/>
            </div>
            <div className="statistics__form">
                <div className="statistics__container">
                    <div className="statistics__group">
                        <span className="statistics__label">Data początkowa</span>
                        <input className="statistics__input" type="date" onChange={handleBeginDateChange}></input>
                    </div>
                    <div className="statistics__group statistics__group--ml10">
                        <span className="statistics__label">Data końcowa</span>
                        <input className="statistics__input" type="date" onChange={handleEndDateChange}></input>
                    </div>
                </div>
                <button className="button" onClick={generateStatistics}>Generate</button>
            </div>

            <div className="statistics__time">
                <h2>Od: {formBeginDate || "---"}</h2>
                <h2>Do: {formEndDate || "---"}</h2>
            </div>

            <div className="statistics__malfunctions">
                <h2>Liczba usterek: {malfunctionsCount || "---"}</h2>
            </div>
        </div>
    );
}
 
export default Statistics;