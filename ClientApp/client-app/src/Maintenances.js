import React, {useState, useEffect, useContext} from 'react';
import { useNavigate } from 'react-router-dom';
import './css/partials/loading.css';
import './css/maintenances.css';
import './css/partials/button.css';
import AuthContext from "./AuthContext";

const Maintenances = () => {
    const {userRoles} = useContext(AuthContext);
    const [loading, setLoading] = useState(true);
    const [maintenances, setMaintenances] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        refreshMaintenances();
    }, []);

    const refreshMaintenances = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/Maintenances`);
            const data = await response.json();
            setMaintenances(data);
            setLoading(false);
        } catch (error) {
            console.error('Error fetching maintenances:', error);
        }
    };

    const filteredMaintenances = maintenances.filter(m =>
        m.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    const navigateToMaintenance = (id) => {
        navigate(`/maintenances/${id}`);
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="maintenances">
            <h1 className="maintenances__title">Obsługi symulatora</h1>
            <div className="maintenances__group">
                <div className="maintenances__search">
                    <img className="maintenances__search-icon" src="./search.png"></img>
                    <input className="maintenances__input maintenances__search-input" type="text" value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
                </div>
                <>
                    {userRoles.some(role => role === 'Engineer' || role === 'Admin')  && (
                        <button className="button" onClick={() => {navigate('/createMaintenance');}}>
                            Zaplanuj nową obsługę
                        </button>
                    )}
                </>
            </div>

            {filteredMaintenances.map(m => (
                <div className={`maintenances__card ${m.realized ? 'maintenances__card--green' : 'maintenances__card--red'}`} key={m.id} onClick={() => navigateToMaintenance(m.id)}>

                    <div className="maintenances__card-header">
                        {/* <div className="maintenances__card-title">{m.id}</div> */}
                        <div className="maintenances__card-subgroup">
                            <div className="maintenances__card-title">{m.name}</div>
                            <div className="simulatorSessions__card-crew">Wykonana przez: {m.executor}</div>
                        </div>
                    </div>

                    <div className="maintenances__card-footer">
                        <div className="maintenances__card-status">Status: {m.realized === true ? 'Wykonano' : 'Zaplanowano'}</div>
                        <div className="maintenances__card-date">Data: {m.date}</div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default Maintenances;
