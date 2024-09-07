import React, { useState } from 'react';
import './calendar.css';

const Calendar = () => {
    const [currentDate, setCurrentDate] = useState(new Date());

    const currentYear = currentDate.getFullYear();
    const currentMonth = currentDate.getMonth(); 
    const firstDayOfMonth = new Date(currentYear, currentMonth, 1).getDay();
    const daysInMonth = new Date(currentYear, currentMonth + 1, 0).getDate();

    const adjustedFirstDay = (firstDayOfMonth + 6) % 7; 

    const daysArray = [];
    for (let i = 1; i <= daysInMonth; i++) {
        daysArray.push(i);
    }

    const emptyCells = [];
    for (let i = 0; i < adjustedFirstDay; i++) {
        emptyCells.push(<div key={`empty-${i}`} className="calendar__day calendar__day--empty"></div>);
    }

    const handlePreviousMonth = () => {
        setCurrentDate(new Date(currentYear, currentMonth - 1, 1));
    };

    const handleNextMonth = () => {
        setCurrentDate(new Date(currentYear, currentMonth + 1, 1));
    };

    return (
        <div className="calendar">
            <h1 className="calendar__title">
                Kalendarz
            </h1>
            <h2 className="calendar__currentMonth">{currentDate.toLocaleString('default', { month: 'long' })} {currentYear}</h2>
            <div className="calendar__controls">
                <button onClick={handlePreviousMonth} className="calendar__button">Poprzedni</button>
                <button onClick={handleNextMonth} className="calendar__button">Następny</button>
            </div>
            <div className="calendar__grid">
                {['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'].map((day, index) => (
                    <div key={index} className="calendar__day calendar__day--header">{day}</div>
                ))}
                {emptyCells}
                {daysArray.map(day => (
                    <div key={day} className="calendar__day">{day}</div>
                ))}
            </div>
        </div>
    );
}

export default Calendar;
