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
    const [workingTime, setWorkingTime] = useState(null);
    const [sessionTime, setSessionTime] = useState(null);
    const [sessionCount, setSessionCount] = useState(null);

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

        try {
            const response = await fetch(
                `${process.env.REACT_APP_API_URL}/SimulatorStates/difference?date1=${beginDate}&date2=${endDate}`
            );

            const data = await response.json();
            setWorkingTime(data);
        } catch (error) {
            console.error('Error fetching working time:', error);
        }

        try {
            const response = await fetch(
                `${process.env.REACT_APP_API_URL}/SimulatorSessions/statistics?dateBegin=${beginDate}&dateEnd=${endDate}`
            );

            const data = await response.json();
            setSessionTime(data.duration);
            setSessionCount(data.count);
        } catch (error) {
            console.error('Error fetching working time:', error);
        }
    }

    return (
        <div className="statistics">
            <div className="statistics__header">
                <h1 className="statistics__title">Statistics</h1>
                <img className="statistics__close" src="./../../close.png" alt="go-back-btn"
                     onClick={() => navigate(-1)}/>
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

            <div className="statistics__item">
                <h2 className="statistics__subtitle">Analizowany okres</h2>
                <span className="statistics__description">Data początku oraz końca branych pod uwagę danych.</span>
                <table className="statistics__table">
                    <thead>
                    <tr>
                        <th className="statistics__table-th">Data początkowa</th>
                        <th className="statistics__table-th">Data końcowa</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td className="statistics__table-td">{formBeginDate || "---"}</td>
                        <td className="statistics__table-td">{formEndDate || "---"}</td>
                    </tr>
                    </tbody>
                </table>
            </div>

            <div className="statistics__item">
                <h2 className="statistics__subtitle">Usterki</h2>
                <span className="statistics__description">Liczba usterek, które pojawiły się w danym okresie.</span>
                <table className="statistics__table">
                    <thead>
                    <tr>
                        <th className="statistics__table-th">Liczba usterek</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td className="statistics__table-td">{malfunctionsCount || "---"}</td>
                    </tr>
                    </tbody>
                </table>
            </div>

            <div className="statistics__item">
                <h2 className="statistics__subtitle">Czas pracy symulatora</h2>
                <span className="statistics__description">Czas pracy urządzenia określony w minutach.</span>
                <table className="statistics__table">
                    <thead>
                    <tr>
                        <th className="statistics__table-th">Czas pracy [min]</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td className="statistics__table-td">{workingTime || "---"}</td>
                    </tr>
                    </tbody>
                </table>
            </div>

            <div className="statistics__item">
                <h2 className="statistics__subtitle">Wykorzystanie w celach szkoleniowych</h2>
                <span className="statistics__description">Czas sesji symulatorowych określony w minutach.</span>
                <table className="statistics__table">
                    <thead>
                    <tr>
                        <th className="statistics__table-th">Czas sesji symulatorowych [min]</th>
                        <th className="statistics__table-th">Liczba sesji symulatorowych</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr>
                        <td className="statistics__table-td">{sessionTime || "---"}</td>
                        <td className="statistics__table-td">{sessionCount || "---"}</td>
                    </tr>
                    </tbody>
                </table>
            </div>

        </div>
    );
}

export default Statistics;