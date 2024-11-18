BEGIN TRANSACTION;
BEGIN TRY

INSERT INTO "Device" (Name, Tag) VALUES
('Control loading system', 'CLS'),
('Cyclic stick', 'CYC'),
('Collective lever', 'COLL'),
('Autopilot', 'AP'),
('Flight Management System', 'FMS'),
('Instructor Operating Station', 'IOS'),
('Clock', 'CLCK'),
('Image generator', 'IG'),
('Display', 'DSP'),
('Simulation server', 'SS'),
('Fuse panel', 'FUSPAN'),
('Motion system', 'MS'),
('Projector', 'PROJ');

INSERT INTO "Inspection_Type" (Name) VALUES
('Aktualizacja baz nawigacyjnych'),
('Aktualizacja baz graficznych'),
('Kalibracja siatki projekcyjnej'),
('Kalibracja blend projekcyjnych'),
('Kalibracja urządzenia'),
('Naprawa urządzenia'),
('Wymiana baterii CMOS'),
('Wymiana filtrów chłodniczych'),
('Wykonanie kopii zapasowej'),
('Przegląd klimatyzacji'),
('Przegląd systemów alarmowych'),
('Przegląd systemów przeciwpożarowych');

INSERT INTO "User" (FirstName, LastName, Login, Password, Salt) VALUES
('Maxwell', 'Rodgers', 'mrodgers', 'kf/dwpqPjMk5hNZOlP7/5aomgQqMrsGYzD3X+BcTk5U=', 'EsYJwGkRd6ortKIiH1bjRQ=='),
('Flora', 'Garrison', 'fgarrison', 'fGvdEgYYyzWRyRrdZj4sd0t80iDaorpNQ2KzeW5MXG0=', 'EtDUISy66+W2MP+dCPYA/g=='),
('Erin', 'Black', 'eblack', 'TQ8UPWxC3ZqyF6mZBW16W/eY/AbU6fsA6RveMPkLA+4=', 'SZWOYxO05WOrKdA/C3wRRw=='),
('Gordon', 'Rowe', 'growe', 'O2mxzaqoR0c7M2nM8/k1W2h4w0I0SN2gLVpV0+7HvdI=', 'hLDpJ5JqLazQOUNOn6E8Iw=='),
('Sami', 'Charles', 'scharles', 'XJD659gZlgFFMmIfTVGdge2Tm5aC63IYJ7/jxkubHWA=', '3qHHNtsr4vkWCY0KzvcAYg=='),
('Brooke', 'Poole', 'bpoole', 'mop1RIdj+c1LgXCoJ1grO/Sa8qQLPsjIKi8jyoFc3dw=', 'ibmAWwwv/d9nDIqNmICEUg=='),
('Casper', 'Schneider', 'cschneider', '4Y3DfMc6c48ZCJpU4ev/6jaBj1CpvCuUKXa1zob8gLc=', 'Sa30PhqJDRRGeuEVCTm4iA=='),
('Rohan', 'Nelson', 'rnelson', 'E7NNPtF+DAM111+lsHWxtjCjAz4gF/BYPDhmRqg31M8=', 'Ttw6ceK469JKNDNkyzow3w=='),
('Raihan', 'Moreno', 'rmoreno', 'N5anourwDxcbKAJ8FGNMz9Vg2GfpfmhCp/AXQ3Iz+EU=', 'ovjyN/x0ZScwLnXDyWExFw=='),
('Walther', 'Thicker', 'wthicker', 'g1wL2N5nZhVxo2wRyiaSb3BU6tsBeRo0c/PkTUiW/Jw=', 'ksTDNS6ZT6LRqEHtr+J/8g=='),
('Timon', 'Sanches', 'tsanches', '/Cqmndy9q1atZo90EKGcjKfo2hz9pDF+VmDwcy3JHw8=', 'dWRVIezwaawkrcZBDwNxNA=='),
('Julio', 'Visone', 'jvisone', 'xdoPxq1u8mFKo4zC4jlqIL49+NhoYcHEzwMoTsIteQE=', 'AomZKg8nJuXKMm5jPuHUFQ=='),
('Victoria', 'Peggins', 'vpeggins', 'vIYMQHL3z/2WTVLoOGwUmTneBZzrXdrCuKOsPCXG838=', 'viRbDmr0mnoTyJJB+LCjnQ=='),
('Jakub', 'Kowalski', 'jkowalski', '1GnkthuoEqDdqJgSIaW8anZESltSDC1xZo/Fk3idDBg=', '+A01SHPVjK0ZvkxkLsrrEg==');

INSERT INTO "Role" (Name) VALUES
('Admin'),
('Engineer'),
('Planer'),
('Pilot'),
('Copilot'),
('Instructor'),
('Auditor');

INSERT INTO UserRole (User_ID, Role_ID) VALUES
(1, 1), -- Maxwell Rodgers / Admin
(2, 2), -- Flora Garrison / Engineer
(3, 3), -- Erin Black / Planer
(4, 4), -- Gordon Rowe / Pilot
(5, 5), -- Sami Charles / Copilot
(6, 6), -- Brooke Poole / Instructor
(7, 7), -- Casper Schnerider / Auditor
(8, 4), -- Rohan Nelson / Pilot
(8, 6), -- Rohan Nelson / Instructor
(10, 4), -- Pilot
(11, 5), -- Copilot
(12, 4), -- Pilot
(13, 5), -- Copilot
(14, 6); -- Instructor

INSERT INTO Inspection (Inspection_Type_ID, Date, Operator, Notice) VALUES
(1, '2024-09-01 08:00:00', 2, 'Aktualizacja do najnowszej wersji, przesunięto ścieżkę podejścia EPWA, nowy punkt EPPX.'),
(5, '2024-09-13 21:30:00', 2, 'Ponowne zakodowanie urządzenia symulującego działanie filtrów mechanicznych silnika.'),
(7, '2024-09-23 13:10:00', 2, 'Zmieniono rozładowaną baterię w głównym komputerze wizyjnym PCV12.'),
(9, '2024-10-03 06:00:00', 2, 'Backup dysku, który przechowuje konfigurację systemu ruchu'),
(6, '2024-10-11 06:00:00', 2, 'Wymiana spuchniętych kondensatorów na płytce PCB - sterownik automatycznego wyrzutu bezpieczników.'),
(1, '2024-08-13 11:45:00', 2, 'Nowa wersja, dodaje dane o przeszkodach w podejściu na lądowisko wyniesione Banacha.'),
(7, '2024-09-23 13:10:00', 2, 'Prewencyjna wymiana baterii w głównym komputerze sterującym.');

INSERT INTO Maintenance_Type (Name, Tasks) VALUES
('Obsługa dzienna', 'Przegląd sprawności systemów bezpieczeństwa, czynności porządkowe, przegląd logów urządzeń, uruchomienie symulatora, weryfikacja poprawności działania systemów symulacji.'),
('Obsługa tygodniowa', 'Przegląd połączeń kablowych, wykonanie kopii zapasowej dysków, analiza logów systemowych, przegląd stanu urządzeń zapasowych'),
('Obsługa miesięczna', 'Sprawdzenie systemów przeciwpożarowych, test agregatu prądotwórczego, czyszczenie klimatyzacji wewnątrz kopuły, sprawdzenie nawigacyjnej bazy danych.'),
('Obsługa roczna', 'Czyszczenie koryt kablowych, wymiana past termoprzewodzących i termopadów, szkolenie okresowe personelu.');

INSERT INTO Maintenance (Type, Executor, Date, Realized) VALUES
(1, 2, '2024-09-16 08:00:00', 1),
(1, 2, '2024-09-17 08:00:00', 1),
(1, 2, '2024-09-18 08:00:00', 1),
(1, 2, '2024-09-19 08:00:00', 1),
(1, 2, '2024-09-20 08:00:00', 1),
(2, 2, '2024-09-20 08:00:00', 1),
(1, 2, '2024-09-23 08:00:00', 1),
(1, 2, '2024-09-24 08:00:00', 1),
(1, 2, '2024-09-25 08:00:00', 1),
(1, 2, '2024-09-26 08:00:00', 1),
(1, 2, '2024-09-27 08:00:00', 1),
(1, 2, '2024-09-30 08:00:00', 1),
(2, 2, '2024-09-30 08:00:00', 1),
(3, 2, '2024-09-30 08:00:00', 1),
(1, 2, '2024-11-01 08:00:00', 1),
(1, 2, '2024-11-04 08:00:00', 1),
(1, 2, '2024-11-05 08:00:00', 1),
(1, 2, '2024-11-06 08:00:00', 1),
(1, 2, '2024-11-07 08:00:00', 1),
(2, 2, '2024-11-07 08:00:00', 1),
(1, 2, '2024-11-08 08:00:00', 1),
(1, 2, '2024-11-11 08:00:00', 1),
(1, 2, '2024-11-12 08:00:00', 1),
(1, 2, '2024-11-13 08:00:00', 1),
(1, 2, '2024-11-14 08:00:00', 1),
(1, 2, '2024-11-15 08:00:00', 1),
(1, 2, '2024-11-18 08:00:00', 1),
(1, 2, '2024-11-19 08:00:00', 1),
(1, 2, '2024-11-20 08:00:00', 1),
(1, 2, '2024-11-21 08:00:00', 1),
(1, 2, '2024-11-22 08:00:00', 1),
(1, 2, '2024-11-25 08:00:00', 1),
(1, 2, '2024-11-26 08:00:00', 1),
(1, 2, '2024-11-27 08:00:00', 1),
(1, 2, '2024-11-28 08:00:00', 1),
(1, 2, '2024-11-29 08:00:00', 1),
(2, 2, '2024-11-29 08:00:00', 1),
(3, 2, '2024-11-29 08:00:00', 1);


INSERT INTO Malfunction (Name, Description, User_Reporter, User_Handler, Date_Begin, Date_End, Status) VALUES
('Brak podświetlenia zegarka', 'W trakcie misji tarcza zegarka przestała się świecić, w trakcie misji nocnej nie widać wskazania czasu.', 6, 2, '2024-04-01 08:00:00', '2024-04-06 09:30:00', 1),
('Nie działa    AP', 'Nie da się uruchomić procedury testowania AP', 6, 2, '2024-04-02 09:30:00', '2024-04-07 10:45:00', 1),
('Brak obrazu', 'Nie działa wyświetlanie obrazu na trzecim od góry projektorze', 4, 2, '2024-04-03 10:45:00', NULL, 0),
('Nie działa wyświetlacz ND', 'W trakcie ćwiczenia zgasł wyświetlacz ND zawiesił się ze statycznym obrazem.', 6, 2, '2024-09-03 10:45:00', '2024-10-03 13:30:00', 1),
('Brak kolizji na pasie EPWR.', 'W trakcie oblotu testowego pojawił się problem z przyziemieniem statku powietrzenego na lotnisku we Wrocławiu. Śmigłowiec wleciał pod ziemię, nie zatrzymując się na przeszkodzie.', 14, 2, '2024-10-04 07:30:00', '2024-10-04 21:13:00', 1);

INSERT INTO MalfunctionDevice (Device_ID, Malfunction_ID) VALUES
(7, 1),
(4, 2),
(8, 3),
(13, 3),
(9, 4),
(10, 5);

INSERT INTO Session_Category (Name) VALUES
('Sesja treningowa'),
('Sesja egzaminacyjna'),
('Sesja obsługowa'),
('Sesja pokazowa');

INSERT INTO Predefined_Session (Category, Name, Description, Duration, Abbreviation) VALUES
(1, 'Zarządzanie zasobami załogi', 'Sesja związana z zarządzaniem zasobami załogi.', 120, 'CRM'),
(1, 'Trening na bazie zewnętrznych punktów odniesienia', 'Sesja treningowa w locie ze wzparciem analogowych przyrządów.', 90, 'VFR'),
(1, 'Lot według przyrządów', 'Trening lotu według przyrządów. Procedury podejścia do lądowania.', 120, 'IR'),
(2, 'Ocena orientacji przestrzennej', 'Sesja sprawdzająca orientację przestrzenną załogi w trudnych warunkach pogodowych.', 90, 'SO'),
(2, 'Ocena sprawności operatora', 'Sesja oceny sprawności operatora.', 90, 'OPC'),
(4, 'Sesja pokazowa', 'Sesja pokazowa, oblot centrum Warszawy.', 30, 'SIG'),
(3, 'Sesja techniczna', 'Sesja techniczna.', 120, 'TECH'),
(1, 'Trening H135', 'Sesja przygotowująca do uzyskania uprawnienia do pilotażu statku powietrznego.', 60, 'TR'),
(1, 'Trening sytuacji awaryjnych', 'Sesja umożliwiająca analizę zachowania załogi statku powietrznego w sytuacjach krytycznych.', 60, 'ES'),
(2, 'Type rating', 'Egzamin sprawdzający predyspozcyje egzaminowanego do pilotażu statku powietrznego.', 60, 'ETR'),
(1, 'Świadomość lokalizacji przyrządów', 'Ćwiczenie pozwalające na trening świadomości położenia przyrządów w kokpicie.', 60, 'IL');

INSERT INTO RecoveryAction (Date, Description, Malfunction_ID) VALUES
('2024-04-05 08:00:00', 'Próba przestawienia mechanizmu zegarka.', 1),
('2024-04-06 09:30:00', 'Wymiana modułu podświetlenia zegarka', 1),
('2024-04-07 10:45:00', 'Restart modułu Autopilota', 2),
('2024-10-03 11:20:00', 'Restart wyświetlacza, który nie rozwiązał problemu. Obraz przestał się całkowicie wyświetlać.', 4),
('2024-10-03 13:30:00', 'Wymiana urządzenia na zapasowe.', 4),
('2024-10-04 07:55:00', 'Ponowne uruchomienie misji. Drugie podejście do lądowania - ponownie zakończone błędem.', 5),
('2024-10-04 09:10:00', 'Weryfikacja stanów i wartości w bazie graficznej. Zmiana wartości coll na true. Oczekiwanie na sprawdzenie poprawności działania.', 5),
('2024-10-04 21:13:00', 'Po modyfikacjach i próbnym przelocie wszystko działa jak należy. Problem rozwiązany.', 5);

INSERT INTO Simulator_Session (Predefined_Session, BeginDate, EndDate, Pilot_Seat, Copilot_Seat, Supervisor_Seat, Observer_Seat, Realized) VALUES
(1, '2024-04-01 08:00:00', '2024-04-01 10:00:00', 4, 5, 6, NULL, 1),
(2, '2024-04-02 09:30:00', '2024-04-02 11:00:00', 4, NULL, 6, NULL, 1),
(1, '2024-04-07 10:45:00', '2024-04-07 12:45:00', 4, 5, 6, NULL, 0),
(3, '2024-10-03 10:45:00', '2024-10-03 12:45:00', 12, 13, 6, NULL, 1);

INSERT INTO Simulator_State (Startup_Time, Meter_State, Operator) VALUES
('2024-04-01 08:00:00', 13200, 2),
('2024-04-02 09:30:00', 13900, 2),
('2024-04-03 10:45:00', 14100, 2);

INSERT INTO "Statistics" (Date_Begin, Date_End, Malfunctions_Count, Maintenances_Count, Sessions_Time, Operating_Time, Efficiency_Factor) VALUES
('2024-04-01 00:00:00', '2024-04-01 23:59:59', 1, 2, 360, 480, 85);

INSERT INTO Test_QTG (Stage, Title, Description) VALUES
('Performance', 'Take-off', 'Acceleration time and distance should be recorded for a minimum of 80% of the total time from brake release to VR.May be combined with normal take-off or rejected take-off. For FTDs test limited to time only.'),
('Performance', 'Crosswind take-off', 'Record take-off profile from brake release to at least 61 m (200 ft) AGL. Requires test data, including wind profile, for a crosswind component of at least 60% of the AFM value measured at 10m (33 ft) above the runway.'),
('Performance', 'Level flight acceleration', 'Minimum of 50 kts increase using maximum continuous thrust rating or equivalent. For very small aeroplanes, speed change may be reduced to 80% of operational speed range.'),
('Handling qualities', 'Static Control checks', 'Pitch, roll and yaw controller position vs. force or time should be measured at the control. An alternative method is to instrument the FSTD in an equivalent manner to the flight test aeroplane. The force and position data from this instrumentation should be directly recorded and matched to the aeroplane data. Such a permanent installation could be used without any time for installation of external devices.'),
('Handling qualities', 'Power change dynamics', 'Power change from thrust for approach or level flight to maximum continuous or goaround power. Time history of uncontrolled free response for a time increment equal to at least 5 s before initiation of the power change to completion of the power change + 15 s.'),
('Handling qualities', 'Longitudinal static stability', 'Data for at least two speeds above and two speeds below trim speed. May be a series of snapshot tests. Force tolerance not applicable if forces are generated solely by the use of aeroplane hardware in the FSTD.'),
('Handling qualities', 'Normal landing', 'Test from a minimum of 61 m (200 ft) AGL to nosewheel touch- down. Two tests should be shown, including two normal landing flaps (if applicable) one of which should be near maximum certificated landing weight, the other at light or medium weight.'),
('Motion system', 'Frequency response', 'Appropriate test to demonstrate frequency response required. See also AMC1 FSTD(A).300 (b)(4)(iii)(B).'),
('Motion system', 'In-flight vibrations', 'Test should be conducted to be representative of in-flight vibrations for propeller driven aeroplanes.'),
('Display system', 'System geometry', 'System geometry should be measured using a visual test pattern filling the entire visual scene (all channels) consisting of a matrix of black and white 5° squares with light points at the intersections. The operator should demonstrate that the angular spacing of any chosen 5° square and the relative spacing of adjacent squares are within the stated tolerances.'),
('Display system', 'Surface contrast ratio', 'Surface contrast ratio should be measured using a raster drawn test pattern filling the entire visual scene (all channels). The test pattern should consist of black and white squares, five per square with a white square in the centre of each channel.');

INSERT INTO Test_Result (Test, IsPassed, Date, Observation, Excutor) VALUES
(1, 1, '2024-08-23 00:00:00', 'Passed', 2),
(2, 0, '2024-08-23 00:00:00', 'Failed, too long opacity', 2),
(3, 1, '2024-08-23 00:00:00', 'Passed', 2);

COMMIT TRANSACTION;
END TRY

BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Bład skryptu!';
END CATCH;