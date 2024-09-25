CREATE TABLE Device (
    ID int identity(1,1)  NOT NULL,
    Name nvarchar(250)  NOT NULL,
    Tag nvarchar(50)  NULL,
    CONSTRAINT Device_pk PRIMARY KEY  (ID)
);

CREATE TABLE Inspection (
    ID int identity(1,1)  NOT NULL,
    Inspection_Type_ID int  NOT NULL,
    Date datetime  NOT NULL,
    Operator int  NOT NULL,
    Notice nvarchar(800) NULL,
    CONSTRAINT Inspection_pk PRIMARY KEY  (ID)
);

CREATE TABLE Inspection_Type (
    ID int identity(1,1)  NOT NULL,
    Name varchar(80)  NOT NULL,
    CONSTRAINT Inspection_Type_pk PRIMARY KEY  (ID)
);

CREATE TABLE Maintenance (
    ID int identity(1,1)  NOT NULL,
    Type int  NOT NULL,
    Executor int  NOT NULL,
    Date datetime  NOT NULL,
    Realized bit  NOT NULL,
    CONSTRAINT Maintenance_pk PRIMARY KEY  (ID)
);

CREATE TABLE Maintenance_Type (
    ID int identity(1,1)  NOT NULL,
    Name varchar(64)  NOT NULL,
    Tasks varchar(2500)  NOT NULL,
    CONSTRAINT Maintenance_Type_pk PRIMARY KEY  (ID)
);

CREATE TABLE Malfunction (
    ID int identity(1,1)  NOT NULL,
    Name nvarchar(180)  NOT NULL,
    Description nvarchar(2500)  NOT NULL,
    User_Reporter int  NOT NULL,
    User_Handler int  NOT NULL,
    Date_Begin datetime  NOT NULL,
    Date_End datetime  NULL,
    Status bit  NOT NULL,
    CONSTRAINT Malfunction_pk PRIMARY KEY  (ID)
);

CREATE TABLE MalfunctionDevice (
    Device_ID int  NOT NULL,
    Malfunction_ID int  NOT NULL,
    CONSTRAINT MalfunctionDevice_pk PRIMARY KEY  (Device_ID,Malfunction_ID)
);

CREATE TABLE Predefined_Session (
    ID int identity(1,1) NOT NULL,
    Category int  NOT NULL,
    Name nvarchar(240)  NOT NULL,
    Description nvarchar(600)  NOT NULL,
    Duration int  NOT NULL,
    Abbreviation nvarchar(15)  NOT NULL,
    CONSTRAINT Predefined_Session_pk PRIMARY KEY  (ID)
);

CREATE TABLE RecoveryAction (
    ID int identity(1,1) NOT NULL,
    Date datetime  NOT NULL,
    Description nvarchar(2500)  NOT NULL,
    Malfunction_ID int  NOT NULL,
    CONSTRAINT RecoveryAction_pk PRIMARY KEY  (ID)
);

CREATE TABLE Role (
    ID int identity(1,1) NOT NULL,
    Name varchar(80)  NOT NULL,
    CONSTRAINT Role_pk PRIMARY KEY  (ID)
);

CREATE TABLE Session_Category (
    ID int identity(1,1) NOT NULL,
    Name nvarchar(200)  NOT NULL,
    CONSTRAINT Session_Category_pk PRIMARY KEY  (ID)
);

CREATE TABLE Simulator_Session (
    ID int identity(1,1) NOT NULL,
    Predefined_Session int  NOT NULL,
    BeginDate datetime  NOT NULL,
    EndDate datetime NOT NULL,
    Pilot_Seat int  NULL,
    Copilot_Seat int  NULL,
    Supervisor_Seat int  NULL,
    Observer_Seat int  NULL,
    Realized bit  NOT NULL,
    CONSTRAINT Simulator_Session_pk PRIMARY KEY  (ID)
);

CREATE TABLE Simulator_State (
    ID int identity(1,1) NOT NULL,
    Startup_Time datetime  NOT NULL,
    Meter_State int  NOT NULL,
    Operator int  NOT NULL,
    CONSTRAINT Simulator_State_pk PRIMARY KEY  (ID)
);

CREATE TABLE "Statistics" (
    ID int identity(1,1) NOT NULL,
    Date_Begin datetime  NOT NULL,
    Date_End datetime  NOT NULL,
    Malfunctions_Count int  NOT NULL,
    Maintenances_Count int  NOT NULL,
    Sessions_Time int  NOT NULL,
    Operating_Time int  NOT NULL,
    Efficiency_Factor int  NOT NULL,
    CONSTRAINT Statistics_pk PRIMARY KEY  (ID)
);

CREATE TABLE Test_QTG (
    ID int identity(1,1) NOT NULL,
    Stage varchar(20)  NOT NULL,
    Title varchar(80)  NOT NULL,
    Description varchar(2500)  NOT NULL,
    CONSTRAINT Test_QTG_pk PRIMARY KEY  (ID)
);

CREATE TABLE Test_Result (
    ID int identity(1,1) NOT NULL,
    Test int  NOT NULL,
    IsPassed bit  NOT NULL,
    Date datetime  NOT NULL,
    Observation varchar(1200)  NOT NULL,
    Excutor int  NOT NULL,
    CONSTRAINT Test_Result_pk PRIMARY KEY  (ID)
);

CREATE TABLE "User" (
    ID int identity(1,1)  NOT NULL,
    FirstName varchar(40)  NOT NULL,
    LastName varchar(60)  NOT NULL,
    Login nvarchar(64)  NOT NULL,
    Password nvarchar(512)  NOT NULL,
    Salt nvarchar(512)  NOT NULL,
    RefreshToken nvarchar(512)  NULL,
    RefreshTokenExp datetime  NULL,
    CONSTRAINT User_pk PRIMARY KEY  (ID)
);

CREATE TABLE UserRole (
    User_ID int  NOT NULL,
    Role_ID int  NOT NULL,
    CONSTRAINT UserRole_pk PRIMARY KEY  (User_ID,Role_ID)
);

ALTER TABLE Inspection ADD CONSTRAINT Inspection_Inspection_Type
    FOREIGN KEY (Inspection_Type_ID)
    REFERENCES Inspection_Type (ID);

ALTER TABLE Inspection ADD CONSTRAINT Inspection_User
    FOREIGN KEY (Operator)
    REFERENCES "User" (ID);

ALTER TABLE Maintenance ADD CONSTRAINT Maintenance_Maintenance_Type
    FOREIGN KEY (Type)
    REFERENCES Maintenance_Type (ID);

ALTER TABLE Maintenance ADD CONSTRAINT Maintenance_User
    FOREIGN KEY (Executor)
    REFERENCES "User" (ID);

ALTER TABLE MalfunctionDevice ADD CONSTRAINT MalfunctionDevice_Device
    FOREIGN KEY (Device_ID)
    REFERENCES Device (ID);

ALTER TABLE MalfunctionDevice ADD CONSTRAINT MalfunctionDevice_Malfunction
    FOREIGN KEY (Malfunction_ID)
    REFERENCES Malfunction (ID);

ALTER TABLE Malfunction ADD CONSTRAINT Malfunction_Handler
    FOREIGN KEY (User_Handler)
    REFERENCES "User" (ID);

ALTER TABLE Malfunction ADD CONSTRAINT Malfunction_Reporter
    FOREIGN KEY (User_Reporter)
    REFERENCES "User" (ID);

ALTER TABLE Predefined_Session ADD CONSTRAINT Predefined_Session_SessionCategory
    FOREIGN KEY (Category)
    REFERENCES Session_Category (ID);

ALTER TABLE RecoveryAction ADD CONSTRAINT RecoveryAction_Malfunction
    FOREIGN KEY (Malfunction_ID)
    REFERENCES Malfunction (ID);

ALTER TABLE Simulator_Session ADD CONSTRAINT SimulatorSession_Copilot
    FOREIGN KEY (Observer_Seat)
    REFERENCES "User" (ID);

ALTER TABLE Simulator_Session ADD CONSTRAINT SimulatorSession_Observer
    FOREIGN KEY (Supervisor_Seat)
    REFERENCES "User" (ID);

ALTER TABLE Simulator_Session ADD CONSTRAINT SimulatorSession_Pilot
    FOREIGN KEY (Copilot_Seat)
    REFERENCES "User" (ID);

ALTER TABLE Simulator_Session ADD CONSTRAINT SimulatorSession_Supervisor
    FOREIGN KEY (Pilot_Seat)
    REFERENCES "User" (ID);

ALTER TABLE Simulator_Session ADD CONSTRAINT Simulator_Session_Predefined_Session
    FOREIGN KEY (Predefined_Session)
    REFERENCES Predefined_Session (ID);

ALTER TABLE Simulator_State ADD CONSTRAINT Simulator_State_User
    FOREIGN KEY (Operator)
    REFERENCES "User" (ID);

ALTER TABLE Test_Result ADD CONSTRAINT Test_Result_Test_QTG
    FOREIGN KEY (Test)
    REFERENCES Test_QTG (ID);

ALTER TABLE Test_Result ADD CONSTRAINT Test_Result_User
    FOREIGN KEY (Excutor)
    REFERENCES "User" (ID);

ALTER TABLE UserRole ADD CONSTRAINT UserRole_Role
    FOREIGN KEY (Role_ID)
    REFERENCES Role (ID);

ALTER TABLE UserRole ADD CONSTRAINT UserRole_User
    FOREIGN KEY (User_ID)
    REFERENCES "User" (ID);
