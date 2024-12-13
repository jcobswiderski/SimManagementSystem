import React, {useState, useEffect, useContext} from 'react';
import './css/partials/loading.css';
import './css/meter.css';
import './css/partials/button.css';
import AuthContext from "./AuthContext";

const Meter = ({userId, showAlert}) => {
    const {userRoles} = useContext(AuthContext);
    const [loading, setLoading] = useState(true);
    const [simulatorStates, setSimulatorStates] = useState([]);
    const [startup, setStartup] = useState('');
    const [meter, setMeter] = useState('');

    useEffect(() => {
        refreshSimulatorStates();
    }, []);

    const refreshSimulatorStates = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorStates`);
            const data = await response.json();
            setSimulatorStates(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    };
    
    const handleStartupChange = (e) => {
        setStartup(e.target.value);
    };

    const handleMeterChange = (e) => {
        setMeter(e.target.value);
    };

    const getLastSimulatorState = () => {
        if(simulatorStates.length === 0) {
            return null;
        }
        const lastState = simulatorStates[simulatorStates.length - 1];
        return lastState.meterState;
    }

    const addSimulatorState = async () => {
        const lastMeterValue = getLastSimulatorState();

        if (lastMeterValue !== null && meter <= lastMeterValue) {
            showAlert('Próba wprowadzenia licznika mniejszego niż ostatni wprowadzony!', 'error');
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorStates`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ 
                    startupTime: startup,
                    meterState: meter,
                    operator: userId
                 }),
            });

            if (response.ok) {
                refreshSimulatorStates();
                showAlert('Dodano nowy stan licznika!', 'success');
            } else {
                showAlert('Nie udało się dodać nowego stanu!', 'error');
            }
        } catch (error) {
            console.error('Error adding state:', error);
        }
    };

    const deleteSimulatorState = async (id) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorStates/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if (response.ok) {
                refreshSimulatorStates();
                showAlert('Pomyślnie usunięto stan licznika!', 'success');
            } else {
                showAlert('Nie udało się usunąć stanu!', 'error');
            }
        } catch (error) {
            console.error('Error removing state:', error);
        }
    }

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="meter">
            <h1 className="meter__title">Licznik</h1>
            <>
                {userRoles.some(role => role === 'Engineer' || role === 'Admin')  && (
                    <div className="meter__form">
                        <div className="meter__container">
                            <div className="meter__group">
                                <span className="meter__label">Czas uruchomienia</span>
                                <input className="meter__input" type="datetime-local"
                                       onChange={handleStartupChange}></input>
                            </div>
                            <div className="meter__group meter__group--ml10">
                                <span className="meter__label">Stan licznika</span>
                                <input className="meter__input" type="number" onChange={handleMeterChange}></input>
                            </div>
                        </div>
                        <button className="button" onClick={addSimulatorState}>Zapisz nowy stan</button>
                    </div>
                )}
            </>

            <table className="meter__table">
                <thead>
                <tr>
                    {/* <th className="meter__table-th">ID</th> */}
                    <th className="meter__table-th">Czas uruchomienia</th>
                    <th className="meter__table-th">Stan licznika</th>
                    <th className="meter__table-th">Akcja</th>
                </tr>
                </thead>
                <tbody>
                {simulatorStates.map(state => (
                    <tr className="meter__table-tr" key={state.id}>
                        {/* <td className="meter__table-td">{state.id}</td> */}
                        <td className="meter__table-td">{state.startupTime}</td>
                        <td className="meter__table-td">{state.meterState}</td>
                        <td className="meter__table-td">
                            <>
                                {userRoles.some(role => role === 'Engineer' || role === 'Admin')  && (
                                    <img className="meter__table-delete" src="./../clear.png" alt="" onClick={() => deleteSimulatorState(state.id)}/>
                                )}
                            </>
                        </td>
                    </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}
 
export default Meter;