CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Clinics` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `Type` int NOT NULL,
    `Address` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ImageUrl` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Clinics` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Departments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `TotalAmountOfPeople` int NOT NULL,
    `Status` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Departments` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Diseases` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Diseases` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `MedicineCategories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ImageUrl` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Status` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NULL,
    `CreateBy` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_MedicineCategories` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Payments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PaymentDate` datetime(6) NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `PaymentMethod` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Payer` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Notes` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Payments` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Roles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Roles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `suppliers` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Phone` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Address` TEXT CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_suppliers` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Units` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    CONSTRAINT `PK_Units` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `RefreshToken` varchar(100) CHARACTER SET utf8mb4 NULL,
    `RefreshTokenExpiryTime` datetime(6) NULL,
    `ResetPasswordToken` varchar(100) CHARACTER SET utf8mb4 NULL,
    `ResetPasswordTokenExpiryTime` datetime(6) NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Services` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` TEXT CHARACTER SET utf8mb4 NULL,
    `ImageUrl` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Price` decimal(18,2) NOT NULL,
    `Status` int NOT NULL,
    `DepartmentId` int NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Services` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Services_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`Id`) ON DELETE SET NULL
) CHARACTER SET=utf8mb4;

CREATE TABLE `DiseaseDetail` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `DiseaseId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_DiseaseDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_DiseaseDetail_Diseases_DiseaseId` FOREIGN KEY (`DiseaseId`) REFERENCES `Diseases` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `medicine_imports` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Notes` TEXT CHARACTER SET utf8mb4 NOT NULL,
    `SupplierId` int NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_medicine_imports` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_medicine_imports_suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `Medicines` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ImageUrl` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Status` int NOT NULL,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UnitId` int NOT NULL,
    `UnitPrice` decimal(65,30) NOT NULL,
    `Prescribed` int NOT NULL,
    `Dosage` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `MedicineCategoryId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Medicines` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Medicines_MedicineCategories_MedicineCategoryId` FOREIGN KEY (`MedicineCategoryId`) REFERENCES `MedicineCategories` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Medicines_Units_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `Units` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `Doctors` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Gender` int NOT NULL,
    `Dob` datetime(6) NOT NULL,
    `CCCD` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Phone` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ImageURL` longtext CHARACTER SET utf8mb4 NOT NULL,
    `LicenseNumber` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `YearOfExperience` float NOT NULL,
    `WorkingHours` float NOT NULL,
    `Status` int NOT NULL,
    `UserId` int NOT NULL,
    `DepartmentId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Doctors` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Doctors_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Doctors_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Notifications` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Title` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Content` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `SendTime` datetime(6) NOT NULL,
    `UserId` int NOT NULL,
    CONSTRAINT `PK_Notifications` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Notifications_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Nurses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Gender` int NOT NULL,
    `Dob` datetime(6) NOT NULL,
    `CCCD` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Phone` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ImageURL` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `UserId` int NOT NULL,
    `DepartmentId` int NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Nurses` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Nurses_Departments_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `Departments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Nurses_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Patients` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Gender` int NOT NULL,
    `Dob` datetime(6) NOT NULL,
    `CCCD` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Phone` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `EmergencyContact` longtext CHARACTER SET utf8mb4 NULL,
    `Address` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `InsuranceNumber` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Allergies` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `BloodType` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ImageURL` longtext CHARACTER SET utf8mb4 NOT NULL,
    `UserId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Patients` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Patients_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Receptions` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Gender` int NOT NULL,
    `Dob` datetime(6) NOT NULL,
    `CCCD` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Phone` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `ImageURL` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `UserId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Receptions` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Receptions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `User_Roles` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `RoleId` int NOT NULL,
    `UserId` int NOT NULL,
    CONSTRAINT `PK_User_Roles` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_User_Roles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_User_Roles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `medicine_detail` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `MedicineId` int NOT NULL,
    `Ingredients` longtext CHARACTER SET utf8mb4 NULL,
    `ExpiryDate` datetime(6) NULL,
    `Manufacturer` datetime(6) NULL,
    `Warning` longtext CHARACTER SET utf8mb4 NULL,
    `StorageInstructions` longtext CHARACTER SET utf8mb4 NULL,
    `Status` int NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Description` TEXT CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_medicine_detail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_medicine_detail_Medicines_MedicineId` FOREIGN KEY (`MedicineId`) REFERENCES `Medicines` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `medicine_import_details` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ImportId` int NOT NULL,
    `MedicineId` int NOT NULL,
    `BatchNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Quantity` int NOT NULL,
    `UnitPrice` decimal(65,30) NOT NULL,
    `ManufactureDate` datetime(6) NOT NULL,
    `ExpiryDate` datetime(6) NOT NULL,
    `SupplierId` int NOT NULL,
    `UnitId` int NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Code` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_medicine_import_details` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_medicine_import_details_Medicines_MedicineId` FOREIGN KEY (`MedicineId`) REFERENCES `Medicines` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_medicine_import_details_Units_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `Units` (`Id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_medicine_import_details_medicine_imports_ImportId` FOREIGN KEY (`ImportId`) REFERENCES `medicine_imports` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_medicine_import_details_suppliers_SupplierId` FOREIGN KEY (`SupplierId`) REFERENCES `suppliers` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `doctor_shifts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `DoctorId` int NOT NULL,
    `ShiftDate` datetime(6) NOT NULL,
    `ShiftType` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `StartTime` time(6) NOT NULL,
    `EndTime` time(6) NOT NULL,
    `Notes` varchar(255) CHARACTER SET utf8mb4 NULL,
    `CreateDate` datetime(6) NULL,
    `UpdateDate` datetime(6) NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_doctor_shifts` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_doctor_shifts_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Insurances` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `CoveragePercent` int NOT NULL,
    `StartDate` datetime(6) NOT NULL,
    `EndDate` datetime(6) NOT NULL,
    `PatientId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Insurances` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Insurances_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Prescriptions` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Note` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `PatientId` int NOT NULL,
    `DoctorId` int NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Prescriptions` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Prescriptions_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Prescriptions_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Medicine_Inventories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Quantity` int NOT NULL,
    `BatchNumber` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UnitPrice` decimal(65,30) NOT NULL,
    `ImportDate` datetime(6) NOT NULL,
    `ExpiryDate` datetime(6) NOT NULL,
    `Status` int NOT NULL,
    `MedicineId` int NOT NULL,
    `ImportDetailId` int NOT NULL,
    CONSTRAINT `PK_Medicine_Inventories` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Medicine_Inventories_Medicines_MedicineId` FOREIGN KEY (`MedicineId`) REFERENCES `Medicines` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Medicine_Inventories_medicine_import_details_ImportDetailId` FOREIGN KEY (`ImportDetailId`) REFERENCES `medicine_import_details` (`Id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `shift_request` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `DoctorId` int NOT NULL,
    `ShiftId` int NOT NULL,
    `RequestType` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Reason` TEXT CHARACTER SET utf8mb4 NOT NULL,
    `Status` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `CreatedDate` datetime(6) NOT NULL,
    `ApprovedDate` datetime(6) NULL,
    CONSTRAINT `PK_shift_request` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_shift_request_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_shift_request_doctor_shifts_ShiftId` FOREIGN KEY (`ShiftId`) REFERENCES `doctor_shifts` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Invoices` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `InitialAmount` decimal(65,30) NOT NULL,
    `DiscountAmount` decimal(65,30) NOT NULL,
    `TotalAmount` decimal(65,30) NOT NULL,
    `Notes` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `AppointmentId` int NOT NULL,
    `InsuranceId` int NULL,
    `PatientId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Invoices` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Invoices_Insurances_InsuranceId` FOREIGN KEY (`InsuranceId`) REFERENCES `Insurances` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Invoices_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Medical_Records` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `Diagnosis` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `TestResults` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Notes` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `AppointmentId` int NOT NULL,
    `PatientId` int NOT NULL,
    `DoctorId` int NOT NULL,
    `PrescriptionId` int NULL,
    `DiseaseId` int NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Medical_Records` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Medical_Records_Diseases_DiseaseId` FOREIGN KEY (`DiseaseId`) REFERENCES `Diseases` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Medical_Records_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Medical_Records_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Medical_Records_Prescriptions_PrescriptionId` FOREIGN KEY (`PrescriptionId`) REFERENCES `Prescriptions` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PrescriptionDetails` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Quantity` int NOT NULL,
    `Usage` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Status` int NOT NULL,
    `PrescriptionId` int NOT NULL,
    `MedicineId` int NOT NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_PrescriptionDetails` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PrescriptionDetails_Medicines_MedicineId` FOREIGN KEY (`MedicineId`) REFERENCES `Medicines` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PrescriptionDetails_Prescriptions_PrescriptionId` FOREIGN KEY (`PrescriptionId`) REFERENCES `Prescriptions` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `InvoiceDetail` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `Discount` decimal(65,30) NOT NULL,
    `TotalAmount` decimal(65,30) NOT NULL,
    `Notes` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `InvoiceId` int NOT NULL,
    `PrescriptionsId` int NULL,
    `ServiceId` int NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_InvoiceDetail` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_InvoiceDetail_Invoices_InvoiceId` FOREIGN KEY (`InvoiceId`) REFERENCES `Invoices` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_InvoiceDetail_Prescriptions_PrescriptionsId` FOREIGN KEY (`PrescriptionsId`) REFERENCES `Prescriptions` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_InvoiceDetail_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `Services` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Payment_Invoices` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AmountPaid` decimal(65,30) NOT NULL,
    `PaymentId` int NOT NULL,
    `InvoiceId` int NOT NULL,
    CONSTRAINT `PK_Payment_Invoices` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Payment_Invoices_Invoices_InvoiceId` FOREIGN KEY (`InvoiceId`) REFERENCES `Invoices` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Payment_Invoices_Payments_PaymentId` FOREIGN KEY (`PaymentId`) REFERENCES `Payments` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Appointments` (
    `Id` int NOT NULL,
    `AppointmentDate` datetime(6) NOT NULL,
    `StartTime` time(6) NOT NULL,
    `EndTime` time(6) NULL,
    `Status` int NOT NULL,
    `Note` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `isSend` tinyint(1) NOT NULL,
    `PatientId` int NOT NULL,
    `ClinicId` int NOT NULL,
    `ReceptionId` int NULL,
    `ServiceId` int NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Appointments` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Appointments_Clinics_ClinicId` FOREIGN KEY (`ClinicId`) REFERENCES `Clinics` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Appointments_Invoices_Id` FOREIGN KEY (`Id`) REFERENCES `Invoices` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Appointments_Medical_Records_Id` FOREIGN KEY (`Id`) REFERENCES `Medical_Records` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Appointments_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Appointments_Receptions_ReceptionId` FOREIGN KEY (`ReceptionId`) REFERENCES `Receptions` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Appointments_Services_ServiceId` FOREIGN KEY (`ServiceId`) REFERENCES `Services` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Doctor_Appointment` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `DoctorId` int NOT NULL,
    `AppointmentId` int NOT NULL,
    CONSTRAINT `PK_Doctor_Appointment` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Doctor_Appointment_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `Appointments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Doctor_Appointment_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Feedbacks` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Content` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `PatientId` int NOT NULL,
    `DoctorId` int NULL,
    `AppointmentId` int NULL,
    CONSTRAINT `PK_Feedbacks` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Feedbacks_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `Appointments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Feedbacks_Doctors_DoctorId` FOREIGN KEY (`DoctorId`) REFERENCES `Doctors` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Feedbacks_Patients_PatientId` FOREIGN KEY (`PatientId`) REFERENCES `Patients` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Nurse_Appointments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `NurseId` int NOT NULL,
    `AppointmentId` int NOT NULL,
    CONSTRAINT `PK_Nurse_Appointments` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Nurse_Appointments_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `Appointments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Nurse_Appointments_Nurses_NurseId` FOREIGN KEY (`NurseId`) REFERENCES `Nurses` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Supplies` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Status` int NOT NULL,
    `Description` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `AppointmentId` int NOT NULL,
    `UnitId` int NULL,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `CreateDate` datetime(6) NOT NULL,
    `UpdateDate` datetime(6) NOT NULL,
    `CreateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `UpdateBy` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Supplies` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Supplies_Appointments_AppointmentId` FOREIGN KEY (`AppointmentId`) REFERENCES `Appointments` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Supplies_Units_UnitId` FOREIGN KEY (`UnitId`) REFERENCES `Units` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Supply_Inventories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Quantity` int NOT NULL,
    `ImportDate` datetime(6) NOT NULL,
    `ExpiryDate` datetime(6) NOT NULL,
    `SupplierName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `SupplyId` int NOT NULL,
    CONSTRAINT `PK_Supply_Inventories` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Supply_Inventories_Supplies_SupplyId` FOREIGN KEY (`SupplyId`) REFERENCES `Supplies` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Appointments_ClinicId` ON `Appointments` (`ClinicId`);

CREATE INDEX `IX_Appointments_PatientId` ON `Appointments` (`PatientId`);

CREATE INDEX `IX_Appointments_ReceptionId` ON `Appointments` (`ReceptionId`);

CREATE INDEX `IX_Appointments_ServiceId` ON `Appointments` (`ServiceId`);

CREATE INDEX `IX_DiseaseDetail_DiseaseId` ON `DiseaseDetail` (`DiseaseId`);

CREATE INDEX `IX_Doctor_Appointment_AppointmentId` ON `Doctor_Appointment` (`AppointmentId`);

CREATE INDEX `IX_Doctor_Appointment_DoctorId` ON `Doctor_Appointment` (`DoctorId`);

CREATE INDEX `IX_doctor_shifts_DoctorId` ON `doctor_shifts` (`DoctorId`);

CREATE INDEX `IX_Doctors_DepartmentId` ON `Doctors` (`DepartmentId`);

CREATE INDEX `IX_Doctors_UserId` ON `Doctors` (`UserId`);

CREATE INDEX `IX_Feedbacks_AppointmentId` ON `Feedbacks` (`AppointmentId`);

CREATE INDEX `IX_Feedbacks_DoctorId` ON `Feedbacks` (`DoctorId`);

CREATE INDEX `IX_Feedbacks_PatientId` ON `Feedbacks` (`PatientId`);

CREATE INDEX `IX_Insurances_PatientId` ON `Insurances` (`PatientId`);

CREATE INDEX `IX_InvoiceDetail_InvoiceId` ON `InvoiceDetail` (`InvoiceId`);

CREATE INDEX `IX_InvoiceDetail_PrescriptionsId` ON `InvoiceDetail` (`PrescriptionsId`);

CREATE INDEX `IX_InvoiceDetail_ServiceId` ON `InvoiceDetail` (`ServiceId`);

CREATE INDEX `IX_Invoices_InsuranceId` ON `Invoices` (`InsuranceId`);

CREATE INDEX `IX_Invoices_PatientId` ON `Invoices` (`PatientId`);

CREATE INDEX `IX_Medical_Records_DiseaseId` ON `Medical_Records` (`DiseaseId`);

CREATE INDEX `IX_Medical_Records_DoctorId` ON `Medical_Records` (`DoctorId`);

CREATE INDEX `IX_Medical_Records_PatientId` ON `Medical_Records` (`PatientId`);

CREATE INDEX `IX_Medical_Records_PrescriptionId` ON `Medical_Records` (`PrescriptionId`);

CREATE UNIQUE INDEX `IX_medicine_detail_MedicineId` ON `medicine_detail` (`MedicineId`);

CREATE INDEX `IX_medicine_import_details_ImportId` ON `medicine_import_details` (`ImportId`);

CREATE UNIQUE INDEX `IX_medicine_import_details_MedicineId_BatchNumber_SupplierId` ON `medicine_import_details` (`MedicineId`, `BatchNumber`, `SupplierId`);

CREATE INDEX `IX_medicine_import_details_SupplierId` ON `medicine_import_details` (`SupplierId`);

CREATE INDEX `IX_medicine_import_details_UnitId` ON `medicine_import_details` (`UnitId`);

CREATE INDEX `IX_medicine_imports_SupplierId` ON `medicine_imports` (`SupplierId`);

CREATE INDEX `IX_Medicine_Inventories_ImportDetailId` ON `Medicine_Inventories` (`ImportDetailId`);

CREATE INDEX `IX_Medicine_Inventories_MedicineId` ON `Medicine_Inventories` (`MedicineId`);

CREATE INDEX `IX_Medicines_MedicineCategoryId` ON `Medicines` (`MedicineCategoryId`);

CREATE INDEX `IX_Medicines_UnitId` ON `Medicines` (`UnitId`);

CREATE INDEX `IX_Notifications_UserId` ON `Notifications` (`UserId`);

CREATE INDEX `IX_Nurse_Appointments_AppointmentId` ON `Nurse_Appointments` (`AppointmentId`);

CREATE INDEX `IX_Nurse_Appointments_NurseId` ON `Nurse_Appointments` (`NurseId`);

CREATE INDEX `IX_Nurses_DepartmentId` ON `Nurses` (`DepartmentId`);

CREATE INDEX `IX_Nurses_UserId` ON `Nurses` (`UserId`);

CREATE INDEX `IX_Patients_UserId` ON `Patients` (`UserId`);

CREATE INDEX `IX_Payment_Invoices_InvoiceId` ON `Payment_Invoices` (`InvoiceId`);

CREATE INDEX `IX_Payment_Invoices_PaymentId` ON `Payment_Invoices` (`PaymentId`);

CREATE INDEX `IX_PrescriptionDetails_MedicineId` ON `PrescriptionDetails` (`MedicineId`);

CREATE INDEX `IX_PrescriptionDetails_PrescriptionId` ON `PrescriptionDetails` (`PrescriptionId`);

CREATE INDEX `IX_Prescriptions_DoctorId` ON `Prescriptions` (`DoctorId`);

CREATE INDEX `IX_Prescriptions_PatientId` ON `Prescriptions` (`PatientId`);

CREATE INDEX `IX_Receptions_UserId` ON `Receptions` (`UserId`);

CREATE INDEX `IX_Services_DepartmentId` ON `Services` (`DepartmentId`);

CREATE INDEX `IX_shift_request_DoctorId` ON `shift_request` (`DoctorId`);

CREATE INDEX `IX_shift_request_ShiftId` ON `shift_request` (`ShiftId`);

CREATE INDEX `IX_Supplies_AppointmentId` ON `Supplies` (`AppointmentId`);

CREATE INDEX `IX_Supplies_UnitId` ON `Supplies` (`UnitId`);

CREATE INDEX `IX_Supply_Inventories_SupplyId` ON `Supply_Inventories` (`SupplyId`);

CREATE INDEX `IX_User_Roles_RoleId` ON `User_Roles` (`RoleId`);

CREATE INDEX `IX_User_Roles_UserId` ON `User_Roles` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250729144023_Initial', '8.0.2');

COMMIT;

