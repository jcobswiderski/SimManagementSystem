ALTER TABLE Inspection DROP CONSTRAINT Inspection_Inspection_Type;
ALTER TABLE Inspection DROP CONSTRAINT Inspection_User;
ALTER TABLE Maintenance DROP CONSTRAINT Maintenance_Maintenance_Type;
ALTER TABLE Maintenance DROP CONSTRAINT Maintenance_User;
ALTER TABLE MalfunctionDevice DROP CONSTRAINT MalfunctionDevice_Device;
ALTER TABLE MalfunctionDevice DROP CONSTRAINT MalfunctionDevice_Malfunction;
ALTER TABLE Malfunction DROP CONSTRAINT Malfunction_Handler;
ALTER TABLE Malfunction DROP CONSTRAINT Malfunction_Reporter;
ALTER TABLE Predefined_Session DROP CONSTRAINT Predefined_Session_SessionCategory;
ALTER TABLE RecoveryAction DROP CONSTRAINT RecoveryAction_Malfunction;
ALTER TABLE Simulator_Session DROP CONSTRAINT SimulatorSession_Copilot;
ALTER TABLE Simulator_Session DROP CONSTRAINT SimulatorSession_Observer;
ALTER TABLE Simulator_Session DROP CONSTRAINT SimulatorSession_Pilot;
ALTER TABLE Simulator_Session DROP CONSTRAINT SimulatorSession_Supervisor;
ALTER TABLE Simulator_Session DROP CONSTRAINT Simulator_Session_Predefined_Session;
ALTER TABLE Simulator_State DROP CONSTRAINT Simulator_State_User;
ALTER TABLE Test_Result DROP CONSTRAINT Test_Result_Test_QTG;
ALTER TABLE Test_Result DROP CONSTRAINT Test_Result_User;
ALTER TABLE UserRole DROP CONSTRAINT UserRole_Role;
ALTER TABLE UserRole DROP CONSTRAINT UserRole_User;

DROP TABLE Device;
DROP TABLE Inspection;
DROP TABLE Inspection_Type;
DROP TABLE Maintenance;
DROP TABLE Maintenance_Type;
DROP TABLE Malfunction;
DROP TABLE MalfunctionDevice;
DROP TABLE Predefined_Session;
DROP TABLE RecoveryAction;
DROP TABLE Role;
DROP TABLE Session_Category;
DROP TABLE Simulator_Session;
DROP TABLE Simulator_State;
DROP TABLE "Statistics";
DROP TABLE Test_QTG;
DROP TABLE Test_Result;
DROP TABLE "User";
DROP TABLE UserRole;