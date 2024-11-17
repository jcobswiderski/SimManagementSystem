import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './css/createSimSession.css';
import './css/partials/button.css';

const CreatePredefinedSession = ({showAlert}) => {
    const [sessionCategories, setSessionCategories] = useState([]);
    const [sessionCategorieId, setSessionCategorieId] = useState(1);

    const [sessionName, setSessionName] = useState('');
    const [sessionDescription, setSessionDescription] = useState('');
    const [sessionDuration, setSessionDuration] = useState('');
    const [sessionAbbreviation, setSessionAbbreviation] = useState('');

    const navigate = useNavigate();
    
    useEffect(() => {
      refreshData();
    }, []);
  
    const refreshData = async () => {
      try {
        const response = await fetch(`${process.env.REACT_APP_API_URL}/SessionCategories`);
        const data = await response.json();
        setSessionCategories(data);

      } catch (error) {
        console.error('Error fetching session categories:', error);
      }
    };

    const handleSessionCategorieIdChange = (e) => {
        setSessionCategorieId(e.target.value);
    };

    const handleSessionNameChange = (e) => {
        setSessionName(e.target.value);
    };

    const handleSessionDescriptionChange = (e) => {
        setSessionDescription(e.target.value);
    };

    const handleSessionDurationChange = (e) => {
        setSessionDuration(e.target.value);
    };

    const handleSessionAbbreviationChange = (e) => {
        setSessionAbbreviation(e.target.value);
    };

    const addSimulatorSessionScheme = async () => {
        if (!sessionCategorieId || !sessionName || !sessionDescription || !sessionDuration || !sessionAbbreviation) {
            showAlert('Proszę wypełnić wszystkie wymagane pola.', 'info');
            return;
        }

        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/PredefinedSessions`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    category: sessionCategorieId,
                    name: sessionName,
                    description: sessionDescription,
                    duration: sessionDuration,
                    abbreviation: sessionAbbreviation
                }),
            });

            if (response.ok) {
              showAlert('Pomyślnie dodano nowy szablon sesji.', 'success');
              navigate(-1);
            } else {
              showAlert('Nie udało się dodać szablonu, wypełnij wymagane pola!', 'error');
            }
        } catch (error) {
            console.error('Error adding scheme:', error);
            showAlert('Wystąpił błąd podczas dodawania szablonu!.', 'error');
        }
    };

    return ( 
        <div className="createSimSession">
         {/* {showAlert && <Alert message={alertMessage} onClose={handleCloseAlert} />} */}
            <div className="createSimSession__header">
                <h1 className="createSimSession__title">Dodawanie szablonu sesji</h1>
                <img className="createSimSession__close" src="./../close.png" alt="go-back-btn" onClick={() => navigate(-1)}/> 
            </div>

            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Rodzaj sesji</span>
                  <select className="createSimSession__input" value={sessionCategorieId} onChange={handleSessionCategorieIdChange}>
                      {sessionCategories.map(c => (
                          <option className="createSimSession__option" key={c.id} value={c.id}>
                              {c.name}
                          </option>
                      ))}
                  </select>
                </div>
              </div>
            </div>
            
            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Nazwa sesji predefiniowanej</span> 
                  <input type="text" className="createSimSession__input" value={sessionName} onChange={handleSessionNameChange}></input>
                </div>
              </div>
            </div>

            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Skrót</span> 
                  <input type="value" className="createSimSession__input" value={sessionAbbreviation} onChange={handleSessionAbbreviationChange}></input>
                </div>
              </div>
            </div>

            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Opis sesji predefiniowanej</span> 
                  <input type="text" className="createSimSession__input" value={sessionDescription} onChange={handleSessionDescriptionChange}></input>
                </div>
              </div>
            </div>

            <div className="createSimSession__group">
              <div className="createSimSession__group--row">
                <div className="createSimSession__group--single">
                  <span className="createSimSession__label">Czas trwania sesji (min)</span> 
                  <input type="number" className="createSimSession__input" value={sessionDuration} onChange={handleSessionDurationChange}></input>
                </div>
              </div>
            </div>
            
            <button className="button createSimSession__button" onClick={addSimulatorSessionScheme}>Zapisz</button>
        </div>
    );
}
 
export default CreatePredefinedSession;