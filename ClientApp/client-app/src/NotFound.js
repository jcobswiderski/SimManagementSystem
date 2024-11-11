import { useNavigate } from 'react-router-dom';
import './css/partials/button.css';
import './css/notfound.css';

const NotFound = () => {
    const navigate = useNavigate();

    return (  
        <div className="notfound">
            <h1 className="notfound__title">404</h1>
            <h2 className="notfound__subtitle">Page not found!</h2>
            <button className="button" onClick={() => navigate('start')}>Back to home.</button>
        </div>
    );
}
 
export default NotFound;