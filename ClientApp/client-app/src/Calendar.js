import React, { useState } from "react";
import "./css/calendar.css";
import './css/partials/button.css';

const Calendar = () => {
    const [currentDate, setCurrentDate] = useState(new Date());

    const events = [
    {
        id: 1,
        title: "zk",
        start: "2024-09-09 08:20:00",
        end: "2024-09-09 10:00:00",
    },
    {
        id: 2,
        title: "Abb",
        start: "2024-09-09 12:00:00",
        end: "2024-09-09 16:00:00",
    },
    {
        id: 3,
        title: "GDD",
        start: "2024-09-11 15:00:00",
        end: "2024-09-11 16:00:00",
    },
    {
        id: 4,
        title: "Zxda",
        start: "2024-09-12 10:30:00",
        end: "2024-09-12 11:00:00",
    },
    {
        id: 5,
        title: "Fsar",
        start: "2024-09-13 14:00:00",
        end: "2024-09-13 15:30:00",
    }
    ];

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
            const eventDate = new Date(event.start);
            return (
                eventDate.getDate() === day.getDate() &&
                eventDate.getMonth() === day.getMonth() &&
                eventDate.getFullYear() === day.getFullYear()
            );
        });
    };

    const calculateEventStyle = (event) => {
        const start = new Date(event.start);
        const end = new Date(event.end);
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
                            if (new Date(event.start).getHours() === hour) {
                                return (
                                    <div key={event.id} className="calendar__event" style={calculateEventStyle(event)}>
                                        {event.title}
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
