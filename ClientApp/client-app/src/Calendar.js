import React, {useEffect, useState} from "react";
import {useNavigate} from "react-router-dom";
import "./css/calendar.css";
import './css/partials/button.css';


const Calendar = () => {
    const [currentDate, setCurrentDate] = useState(new Date());
    const [events, setEvents] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        refreshSimulatorSessions();
    }, []);

    const refreshSimulatorSessions = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_URL}/SimulatorSessions`);
            const data = await response.json();
            console.log(data);
            setEvents(data);
        } catch (error) {
            console.error('Error fetching inspections:', error);
        }
    };

    const previousWeek = () => {
        const tempDate = new Date(currentDate);
        tempDate.setDate(tempDate.getDate() - 7);
        setCurrentDate(tempDate);
    };

    const nextWeek = () => {
        const tempDate = new Date(currentDate);
        tempDate.setDate(tempDate.getDate() + 7);
        setCurrentDate(tempDate);
    };

    const getWeekDays = (date) => {
        const currentWeek = [];
        
        const firstDay = new Date(date);
        firstDay.setDate(firstDay.getDate() - firstDay.getDay() + 1); // ustawiamy poniedzialek
        
        for (let i = 0; i < 7; i++) {
            const day = new Date(firstDay);
            day.setDate(firstDay.getDate() + i);
            currentWeek.push(day);
        }
        
        return currentWeek;
    };

    const getEventsForDay = (day) => {
        return events.filter((event) => {
            const eventDate = new Date(event.beginDate);
            return (
                eventDate.getDate() === day.getDate() &&
                eventDate.getMonth() === day.getMonth() &&
                eventDate.getFullYear() === day.getFullYear()
            );
        });
    };

    const calculateEventStyle = (event) => {
        const start = new Date(event.beginDate);
        const end = new Date(event.endDate);
        const startHour = start.getHours() + start.getMinutes() / 60;
        const endHour = end.getHours() + end.getMinutes() / 60;
        const duration = endHour - startHour;

        return {
            top: `${start.getMinutes() * 50/60}px`,
            height: `${duration * 50}px`,
        };
    };

    const weekDays = getWeekDays(currentDate);

    return (
        <div className="calendar">
            <div className="calendar__header">
                <button className="button" onClick={previousWeek}>Poprzedni</button>
                <h2>Tydzień od {weekDays[0].toLocaleDateString()} do {weekDays[6].toLocaleDateString()}</h2>
                <button className="button" onClick={nextWeek}>Następny</button>
            </div>
            <div className="calendar__grid">
                <div className="calendar__grid-header">
                    {weekDays.map((day) => (
                        <div key={day} className="calendar__grid-header-cell">
                            {day.toLocaleDateString("pl-PL", { weekday: "long", day: "numeric" })}
                        </div>
                    ))}
                    <div className="calendar__grid-header-cell calendar__time"></div>
                </div>
                <div className="calendar__grid-body">
                    {Array.from({ length: 24 }, (_, hour) => (
                    <div key={hour} className="calendar__grid-row">
                        {weekDays.map((day) => (
                        <div key={day} className="calendar__grid-cell">
                            {getEventsForDay(day).map((event) => {
                            if (new Date(event.beginDate).getHours() === hour) {
                                return (
                                    <div key={event.id} className="calendar__event" style={calculateEventStyle(event)} onClick={() => navigate(`/simSessions/${event.id}`)}>
                                        {event.abbreviation}
                                    </div>
                                );
                            }
                            return null;
                            })}
                        </div>
                        ))}
                        <div className="calendar__time">{hour}:00</div>
                    </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default Calendar;
