INSERT INTO "Device" (Name, Tag) VALUES
('Control loading system', 'CLS'),
('Collective', 'COLLECTIVE'),
('Autopilot', 'AP'),
('Flight Management System', 'FMS'),
('Instructor Operating Station', 'IOS'),
('Clock', 'CLOCK'),
('Projector', 'PROJECTOR');

INSERT INTO "Inspection_Type" (Name) VALUES
('Przegląd klimatyzacji'),
('Wymiana filtra klimatyzacji'),
('Sprawdzenie czujek przeciwpożarowych');

INSERT INTO "User" (FirstName, LastName, Login, Password, Salt) VALUES
('Andreas', 'Duo', 'andreas_duo', 'Andreas@123', 'aaa'),
('Jude', 'Smiths', 'jude_smiths', 'Smiths@111Jude', 'aaa'),
('Robert', 'Johanson', 'robert_johanson', 'pass!woRd331', 'aaa'),
('Jakub', 'Świderski', 'jswiderski', 'XOmEjo/m5PYk+zn01HmIjS06riQ2rNb7D4TM9YKRJBg=', 'qB7Jz8tZZUzG14vpPgKsiw=='),
('Sam', 'Kramer', 's.kramer', '9qWz&2mE$jP', 'aaa'),
('Mikey', 'Walter', 'm.walter', '5@Kc!pF9z#Jx', 'aaa'),
('Brendon', 'Rodrigueza', 'b.rodriguez11', 'H4@Ry&tN8qXu', 'aaa');

INSERT INTO "Role" (Name) VALUES
('Admin'),
('Engineer'),
('Planer'),
('Pilot'),
('Copilot'),
('Instructor'),
('Guest');

INSERT INTO UserRole (User_ID, Role_ID) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7);

INSERT INTO Inspection (Inspection_Type_ID, Date, Operator) VALUES
(1, '2024-04-01 08:00:00', 2),
(2, '2024-04-02 09:30:00', 2),
(3, '2024-04-03 10:45:00', 2);

INSERT INTO Maintenance_Type (Name, Tasks) VALUES
('Obsługa dzienna', 'Task1, Task2, Task3'),
('Obsługa tygodniowa', 'Task4, Task5, Task6'),
('Obsługa miesięczna', 'Task7, Task8, Task9');

INSERT INTO Maintenance (Type, Executor, Date, Realized) VALUES
(1, 2, '2024-04-05 08:00:00', 1),
(2, 2, '2024-04-06 09:30:00', 1),
(3, 2, '2024-04-07 10:45:00', 0);

INSERT INTO Malfunction (Name, Description, User_Reporter, User_Handler, Date_Begin, Date_End, Status) VALUES
('Brak podświetlenia zegarka', 'W trakcie misji tarcza zegarka przestała się świecić, w trakcie misji nocnej nie widać wskazania czasu.', 6, 2, '2024-04-01 08:00:00', NULL, 0),
('Nie działa AP', 'Nie da się uruchomić procedury testowania AP', 6, 2, '2024-04-02 09:30:00', '2024-04-03 10:45:00', 0),
('Brak obrazu', 'Nie działa wyświetlanie obrazu na trzecim od góry projektorze', 4, 2, '2024-04-03 10:45:00', NULL, 1);

INSERT INTO MalfunctionDevice (Device_ID, Malfunction_ID) VALUES
(6, 1),
(3, 2),
(7, 3);

INSERT INTO Session_Category (Name) VALUES
('Sesja treningowa'),
('Sesja egzaminacyjna'),
('Sesja obsługowa'),
('Sesja pokazowa');

INSERT INTO Predefined_Session (Category, Name, Description, Duration, Abbreviation) VALUES
(1, 'Crew resource management', 'Sesja związana z zarządzaniem zasobami załogi.', 120, 'CRM'),
(2, 'Operator Proficiency Check', 'Sesja oceny sprawności operatora.', 90, 'OPC');

INSERT INTO RecoveryAction (Date, Description, Malfunction_ID) VALUES
('2024-04-05 08:00:00', 'Próba przestawienia mechanizmu zegarka.', 1),
('2024-04-06 09:30:00', 'Wymiana modułu podświetlenia zegarka', 1),
('2024-04-07 10:45:00', 'Restart modułu Autopilota', 2);

INSERT INTO Simulator_Session (Predefined_Session, Date, Pilot_Seat, Copilot_Seat, Supervisor_Seat, Observer_Seat, Realized) VALUES
(1, '2024-04-01 08:00:00', 4, 5, 6, NULL, 1),
(2, '2024-04-02 09:30:00', 4, NULL, 6, NULL, 1),
(1, '2024-04-07 10:45:00', 4, 5, 6, NULL, 0);

INSERT INTO Simulator_State (Startup_Time, Meter_State, Operator) VALUES
('2024-04-01 08:00:00', 3221123, 2),
('2024-04-02 09:30:00', 3222123, 2),
('2024-04-03 10:45:00', 3223123, 2);

INSERT INTO "Statistics" (Date_Begin, Date_End, Malfunctions_Count, Maintenances_Count, Sessions_Time, Operating_Time, Efficiency_Factor) VALUES
('2024-04-01 00:00:00', '2024-04-01 23:59:59', 1, 2, 360, 480, 85);

INSERT INTO Test_QTG (Stage, Title, Description) VALUES
('Test1', 'Title1', 'Description1'),
('Test2', 'Title2', 'Description2'),
('Test3', 'Title3', 'Description3');

INSERT INTO Test_Result (Test, IsPassed, Observation, Excutor) VALUES
(1, 1, 'Passed', 2),
(2, 0, 'Failed, too long opacity', 2),
(3, 1, 'Passed', 2);