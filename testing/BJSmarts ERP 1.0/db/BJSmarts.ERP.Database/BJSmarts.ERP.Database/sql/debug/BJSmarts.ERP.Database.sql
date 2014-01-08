/*
Deployment script for BJSmarts.ERP.Database
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "BJSmarts.ERP.Database"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"

GO
USE [master]

GO
:on error exit
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO

IF NOT EXISTS (SELECT 1 FROM [master].[dbo].[sysdatabases] WHERE [name] = N'$(DatabaseName)')
BEGIN
    RAISERROR(N'You cannot deploy this update script to target DONALD. The database for which this script was built, BJSmarts.ERP.Database, does not exist on this server.', 16, 127) WITH NOWAIT
    RETURN
END

GO

IF (@@servername != 'DONALD')
BEGIN
    RAISERROR(N'The server name in the build script %s does not match the name of the target server %s. Verify whether your database project settings are correct and whether your build script is up to date.', 16, 127,N'DONALD',@@servername) WITH NOWAIT
    RETURN
END

GO

IF CAST(DATABASEPROPERTY(N'$(DatabaseName)','IsReadOnly') as bit) = 1
BEGIN
    RAISERROR(N'You cannot deploy this update script because the database for which it was built, %s , is set to READ_ONLY.', 16, 127, N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)]

GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Creating [dbo].[AccountCategories]...';


GO
CREATE TABLE [dbo].[AccountCategories] (
    [AccountCategoryId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50)  NULL,
    [Description]       NVARCHAR (250) NULL,
    [Sort_Order]        INT            NULL,
    [Language]          INT            NULL,
    [Deleted]           INT            NULL
);


GO
PRINT N'Creating [dbo].[Accounts]...';


GO
CREATE TABLE [dbo].[Accounts] (
    [AccountsId]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50)  NULL,
    [Description]       NVARCHAR (500) NULL,
    [Account Number]    NVARCHAR (50)  NULL,
    [Main Phone]        NVARCHAR (50)  NULL,
    [Primary Contact]   INT            NULL,
    [Fax]               NVARCHAR (50)  NULL,
    [WebSite]           NVARCHAR (50)  NULL,
    [Email]             NVARCHAR (50)  NULL,
    [StreeAddress1]     NVARCHAR (250) NULL,
    [StreeAddress2]     NVARCHAR (250) NULL,
    [City]              NVARCHAR (50)  NULL,
    [State/Province]    NVARCHAR (50)  NULL,
    [Zip/Postal Code]   NVARCHAR (50)  NULL,
    [CountryId]         INT            NULL,
    [Country]           NVARCHAR (50)  NULL,
    [IndustryId]        INT            NULL,
    [Industry]          NVARCHAR (50)  NULL,
    [CurrencyId]        INT            NULL,
    [Currency]          NVARCHAR (50)  NULL,
    [AccountCategoryId] INT            NULL,
    [AccountCategory]   NVARCHAR (50)  NULL,
    [Notes]             NVARCHAR (500) NULL,
    [OrganizationId]    INT            NULL,
    [Organization]      NVARCHAR (50)  NULL,
    [LeadId]            INT            NULL
);


GO
PRINT N'Creating [dbo].[Appointment]...';


GO
CREATE TABLE [dbo].[Appointment] (
    [AppointmentId] INT            IDENTITY (1, 1) NOT NULL,
    [Subject]       NVARCHAR (30)  NULL,
    [Location]      NVARCHAR (30)  NULL,
    [RegardingId]   INT            NULL,
    [Start Time]    DATETIME       NULL,
    [End Time]      DATETIME       NULL,
    [Notes]         NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[BankAccounts]...';


GO
CREATE TABLE [dbo].[BankAccounts] (
    [BankAccountId]     INT           IDENTITY (1, 1) NOT NULL,
    [BankName]          NVARCHAR (50) NULL,
    [BankRouter]        NVARCHAR (50) NULL,
    [BankAccountNumber] NVARCHAR (50) NULL,
    [BankAccountType]   INT           NULL,
    [EmployeeId]        INT           NULL,
    [Deleted]           INT           NULL
);


GO
PRINT N'Creating [dbo].[BankAccountType]...';


GO
CREATE TABLE [dbo].[BankAccountType] (
    [BankAccountTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (25)  NULL,
    [Description]       NVARCHAR (100) NULL,
    [Sort_Order]        INT            NULL,
    [Language]          INT            NULL,
    [Deleted]           INT            NULL
);


GO
PRINT N'Creating [dbo].[Benefits]...';


GO
CREATE TABLE [dbo].[Benefits] (
    [BenefitsId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (25)  NULL,
    [Description]  NVARCHAR (100) NULL,
    [AccruedHours] DECIMAL (18)   NULL,
    [Sort_Order]   INT            NULL,
    [EmployeeId]   INT            NULL,
    [Language]     INT            NULL,
    [Deleted]      INT            NULL
);


GO
PRINT N'Creating [dbo].[Campaign]...';


GO
CREATE TABLE [dbo].[Campaign] (
    [CampaignId]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (50)  NULL,
    [Description]            NVARCHAR (250) NULL,
    [Total Cost of Campaign] DECIMAL (18)   NULL,
    [Budget Allocated]       DECIMAL (18)   NULL,
    [Estimated Revenue]      DECIMAL (18)   NULL,
    [Proposed Begin Date]    DATETIME       NULL,
    [Proposed End Date]      DATETIME       NULL,
    [Actual Begin Date]      DATETIME       NULL,
    [Actual End Date]        DATETIME       NULL,
    [CampaignStatusId]       INT            NULL,
    [CampaignStatus]         NVARCHAR (50)  NULL,
    [CampaignTypeId]         INT            NULL,
    [CampaignType]           NVARCHAR (50)  NULL,
    [OrganizationId]         INT            NULL,
    [Organization]           NVARCHAR (50)  NULL
);


GO
PRINT N'Creating [dbo].[CampaignStatus]...';


GO
CREATE TABLE [dbo].[CampaignStatus] (
    [CampaignStatusId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (25)  NULL,
    [Description]      NVARCHAR (100) NULL,
    [Sort_Order]       INT            NULL,
    [Language]         INT            NULL,
    [Deleted]          INT            NULL
);


GO
PRINT N'Creating [dbo].[CampaignTypes]...';


GO
CREATE TABLE [dbo].[CampaignTypes] (
    [CampaignTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (25)  NULL,
    [Description]    NVARCHAR (100) NULL,
    [Sort_Order]     INT            NULL,
    [Language]       INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[CaseOrigin]...';


GO
CREATE TABLE [dbo].[CaseOrigin] (
    [CaseOriginId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Description]  NVARCHAR (250) NULL,
    [Sort_Order]   INT            NULL,
    [Language]     INT            NULL,
    [Deleted]      INT            NULL
);


GO
PRINT N'Creating [dbo].[Cases]...';


GO
CREATE TABLE [dbo].[Cases] (
    [CasesId]        INT            IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (50)  NULL,
    [Description]    NVARCHAR (500) NULL,
    [CustomerId]     INT            NULL,
    [SubjectId]      INT            NULL,
    [CaseTypeId]     INT            NULL,
    [CaseType]       NVARCHAR (50)  NULL,
    [CaseOriginId]   INT            NULL,
    [SatisfactionId] INT            NULL,
    [OwnerId]        INT            NULL,
    [CaseStatusId]   INT            NULL,
    [CaseStatus]     NVARCHAR (50)  NULL,
    [OrganizationId] INT            NULL,
    [Organization]   NVARCHAR (50)  NULL,
    [PriorityId]     INT            NULL
);


GO
PRINT N'Creating [dbo].[CaseStatus]...';


GO
CREATE TABLE [dbo].[CaseStatus] (
    [CaseStatusId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Description]  NVARCHAR (250) NULL,
    [Sort_Order]   INT            NULL,
    [Language]     INT            NULL,
    [Deleted]      INT            NULL
);


GO
PRINT N'Creating [dbo].[CaseType]...';


GO
CREATE TABLE [dbo].[CaseType] (
    [CaseTypeId]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [Description] NVARCHAR (250) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[Companies]...';


GO
CREATE TABLE [dbo].[Companies] (
    [CompanyId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (25)  NULL,
    [Description] NVARCHAR (100) NULL,
    [LocationId]  INT            NULL,
    [Sort_Order]  INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[Competitors]...';


GO
CREATE TABLE [dbo].[Competitors] (
    [CompetitorId]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50)  NULL,
    [Description]     NVARCHAR (250) NULL,
    [WebSite]         NVARCHAR (50)  NULL,
    [StreeAddress1]   NVARCHAR (250) NULL,
    [StreeAddress2]   NVARCHAR (250) NULL,
    [City]            NVARCHAR (50)  NULL,
    [State/Province]  NVARCHAR (50)  NULL,
    [Zip/Postal Code] NVARCHAR (50)  NULL,
    [CountryId]       INT            NULL,
    [Country]         NVARCHAR (50)  NULL,
    [CurrencyId]      INT            NULL,
    [Currency]        NVARCHAR (50)  NULL,
    [Notes]           NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Contacts]...';


GO
CREATE TABLE [dbo].[Contacts] (
    [ContactsId]      INT            IDENTITY (1, 1) NOT NULL,
    [First Name]      NVARCHAR (50)  NULL,
    [Last Name]       NVARCHAR (50)  NULL,
    [Job Title]       NVARCHAR (50)  NULL,
    [AccountsId]      INT            NULL,
    [Business Phone]  NVARCHAR (50)  NULL,
    [Home Phone]      NVARCHAR (50)  NULL,
    [Mobile Phone]    NVARCHAR (50)  NULL,
    [Fax]             NVARCHAR (50)  NULL,
    [Email]           NVARCHAR (50)  NULL,
    [StreeAddress1]   NVARCHAR (250) NULL,
    [StreeAddress2]   NVARCHAR (250) NULL,
    [City]            NVARCHAR (50)  NULL,
    [State/Province]  NVARCHAR (50)  NULL,
    [Zip/Postal Code] NVARCHAR (50)  NULL,
    [CountryId]       INT            NULL,
    [Country]         NVARCHAR (50)  NULL,
    [Description]     NVARCHAR (250) NULL,
    [Department]      NVARCHAR (50)  NULL,
    [RoleId]          INT            NULL,
    [Manager]         NVARCHAR (50)  NULL,
    [Manager Phone]   NVARCHAR (50)  NULL,
    [GenderId]        INT            NULL,
    [CurrencyId]      INT            NULL,
    [Currency]        NVARCHAR (50)  NULL,
    [Notes]           NVARCHAR (500) NULL,
    [OrganizationId]  INT            NULL,
    [Organization]    NVARCHAR (50)  NULL,
    [LeadId]          INT            NULL
);


GO
PRINT N'Creating [dbo].[Countries]...';


GO
CREATE TABLE [dbo].[Countries] (
    [CountryId]   INT           IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50) NULL,
    [Description] NVARCHAR (50) NULL,
    [Sort_Order]  INT           NULL,
    [Language]    INT           NULL,
    [Deleted]     INT           NULL
);


GO
PRINT N'Creating [dbo].[CurrencyType]...';


GO
CREATE TABLE [dbo].[CurrencyType] (
    [CurrencyTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (25)  NULL,
    [Description]    NVARCHAR (100) NULL,
    [Sort_Order]     INT            NULL,
    [Language]       INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[Departments]...';


GO
CREATE TABLE [dbo].[Departments] (
    [DepartmentId]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50)  NULL,
    [Description]     NVARCHAR (50)  NULL,
    [ManagerId]       INT            NULL,
    [StreeAddress1]   NVARCHAR (250) NULL,
    [StreeAddress2]   NVARCHAR (250) NULL,
    [City]            NVARCHAR (50)  NULL,
    [State/Province]  NVARCHAR (50)  NULL,
    [Zip/Postal Code] NVARCHAR (50)  NULL,
    [Sort_Order]      INT            NULL,
    [OrganizationId]  INT            NULL,
    [Organization]    NVARCHAR (50)  NULL,
    [Deleted]         INT            NULL
);


GO
PRINT N'Creating [dbo].[Dependants]...';


GO
CREATE TABLE [dbo].[Dependants] (
    [DependantId] INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (25) NULL,
    [LastName]    NVARCHAR (25) NULL,
    [Email]       NVARCHAR (25) NULL,
    [PhoneNumber] NVARCHAR (25) NULL,
    [DateOfBirth] DATE          NULL,
    [GenderId]    INT           NULL,
    [EmployeeId]  INT           NULL,
    [LocationId]  INT           NULL,
    [Deleted]     INT           NULL
);


GO
PRINT N'Creating [dbo].[EmployeeAccounts]...';


GO
CREATE TABLE [dbo].[EmployeeAccounts] (
    [EmployeeAccountsId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (25)  NULL,
    [Description]        NVARCHAR (100) NULL,
    [Sort_Order]         INT            NULL,
    [Language]           INT            NULL,
    [Organization]       INT            NULL,
    [Deleted]            INT            NULL
);


GO
PRINT N'Creating [dbo].[EmployeePayStub]...';


GO
CREATE TABLE [dbo].[EmployeePayStub] (
    [EmployeePayStubId] INT             IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (25)   NULL,
    [Description]       NVARCHAR (100)  NULL,
    [Deduction Type]    NVARCHAR (25)   NULL,
    [EmployeeId]        INT             NULL,
    [Percentage]        DECIMAL (18, 2) NULL,
    [Deleted]           INT             NULL
);


GO
PRINT N'Creating [dbo].[Employees]...';


GO
CREATE TABLE [dbo].[Employees] (
    [EmployeeId]                INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]                 NVARCHAR (25)  NULL,
    [LastName]                  NVARCHAR (25)  NULL,
    [Email]                     NVARCHAR (25)  NULL,
    [EmployeeGovernmentId]      NVARCHAR (25)  NULL,
    [HomePhoneNumber]           NVARCHAR (25)  NULL,
    [WorkPhoneNumber]           NVARCHAR (25)  NULL,
    [MovilPhoneNumber]          NVARCHAR (25)  NULL,
    [HireDate]                  DATETIME       NULL,
    [DateOfBirth]               DATETIME       NULL,
    [BiometricEmployeeId]       NVARCHAR (25)  NULL,
    [Salary]                    DECIMAL (18)   NULL,
    [EmployeTypeId]             INT            NULL,
    [EmployeeType]              NVARCHAR (25)  NULL,
    [ResourceTypeId]            INT            NULL,
    [ResourceType]              NVARCHAR (25)  NULL,
    [GenderId]                  INT            NULL,
    [Gender]                    NVARCHAR (25)  NULL,
    [CurrencyTypeId]            INT            NULL,
    [CurrencyType]              NVARCHAR (25)  NULL,
    [MaritalStatusId]           INT            NULL,
    [MaritalStatus]             NVARCHAR (25)  NULL,
    [ManagerId]                 INT            NULL,
    [EmployeeTerminationTypeId] INT            NULL,
    [EmployeeTerminationType]   NVARCHAR (25)  NULL,
    [HomeStreetAddress1]        NVARCHAR (100) NULL,
    [HomeStreetAddress2]        NVARCHAR (100) NULL,
    [HomePostalCode]            NVARCHAR (25)  NULL,
    [HomeCity]                  NVARCHAR (25)  NULL,
    [HomeStateProvince]         NVARCHAR (25)  NULL,
    [HomeCountryId]             INT            NULL,
    [WorkStreetAddress1]        NVARCHAR (100) NULL,
    [WorkStreetAddress2]        NVARCHAR (100) NULL,
    [WorkPostalCode]            NVARCHAR (25)  NULL,
    [WorkCity]                  NVARCHAR (25)  NULL,
    [WorkStateProvince]         NVARCHAR (25)  NULL,
    [WorkCountryId]             INT            NULL,
    [DepartmentId]              INT            NULL,
    [Department]                NVARCHAR (25)  NULL,
    [CompanyId]                 INT            NULL,
    [OrganizationId]            INT            NULL,
    [Organization]              NVARCHAR (25)  NULL,
    [Deleted]                   INT            NULL
);


GO
PRINT N'Creating [dbo].[EmployeeTerminationType]...';


GO
CREATE TABLE [dbo].[EmployeeTerminationType] (
    [EmployeeTerminationTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                      NVARCHAR (25)  NULL,
    [Description]               NVARCHAR (100) NULL,
    [Sort_Order]                INT            NULL,
    [Language]                  INT            NULL,
    [Deleted]                   INT            NULL
);


GO
PRINT N'Creating [dbo].[EmployeeTypes]...';


GO
CREATE TABLE [dbo].[EmployeeTypes] (
    [EmployeeTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (25)  NULL,
    [Description]    NVARCHAR (100) NULL,
    [Sort_Order]     INT            NULL,
    [Language]       INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[Fax]...';


GO
CREATE TABLE [dbo].[Fax] (
    [FaxId]       INT            IDENTITY (1, 1) NOT NULL,
    [SenderId]    INT            NULL,
    [Recipient]   INT            NULL,
    [Fax Number]  NVARCHAR (30)  NULL,
    [Subject]     NVARCHAR (30)  NULL,
    [Description] NVARCHAR (500) NULL,
    [RegardingId] INT            NULL,
    [OwnerId]     INT            NULL,
    [PriorityId]  INT            NULL,
    [Due]         DATETIME       NULL,
    [Notes]       NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Genders]...';


GO
CREATE TABLE [dbo].[Genders] (
    [GenderId]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (25)  NULL,
    [Description] NVARCHAR (100) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL
);


GO
PRINT N'Creating [dbo].[Industry]...';


GO
CREATE TABLE [dbo].[Industry] (
    [IndustryId]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (25)  NULL,
    [Description] NVARCHAR (100) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[Invoices]...';


GO
CREATE TABLE [dbo].[Invoices] (
    [InvoiceId]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (50)  NULL,
    [Description]          NVARCHAR (50)  NULL,
    [CustomerId]           INT            NULL,
    [Price List]           INT            NULL,
    [CurrencyId]           INT            NULL,
    [Currency]             NVARCHAR (50)  NULL,
    [Detail Amount]        DECIMAL (18)   NULL,
    [Invoice Discount]     DECIMAL (18)   NULL,
    [Total Tax]            DECIMAL (18)   NULL,
    [Total Amount]         DECIMAL (18)   NULL,
    [Date Delivered]       DATETIME       NULL,
    [Due Date]             DATETIME       NULL,
    [Bill StreeAddress1]   NVARCHAR (250) NULL,
    [Bill StreeAddress2]   NVARCHAR (250) NULL,
    [Bill City]            NVARCHAR (50)  NULL,
    [Bill State/Province]  NVARCHAR (50)  NULL,
    [Bill Zip/Postal Code] NVARCHAR (50)  NULL,
    [Bill CountryId]       INT            NULL,
    [Bill PHone]           NVARCHAR (50)  NULL,
    [Bill Address Contact] NVARCHAR (50)  NULL,
    [Ship StreeAddress1]   NVARCHAR (250) NULL,
    [Ship StreeAddress2]   NVARCHAR (250) NULL,
    [Ship City]            NVARCHAR (50)  NULL,
    [ShipState/Province]   NVARCHAR (50)  NULL,
    [Ship Zip/Postal Code] NVARCHAR (50)  NULL,
    [Ship CountryId]       INT            NULL,
    [Ship Phone]           NVARCHAR (50)  NULL,
    [Ship Address Contact] NVARCHAR (50)  NULL,
    [OwnerId]              INT            NULL,
    [OpportunityId]        INT            NULL,
    [OrderId]              INT            NULL,
    [OrganizationId]       INT            NULL,
    [Organization]         NVARCHAR (50)  NULL
);


GO
PRINT N'Creating [dbo].[Language]...';


GO
CREATE TABLE [dbo].[Language] (
    [LanguageId]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (25)  NULL,
    [Description] NVARCHAR (100) NULL,
    [Sort_Order]  INT            NULL
);


GO
PRINT N'Creating [dbo].[Leads]...';


GO
CREATE TABLE [dbo].[Leads] (
    [LeadsId]         INT            IDENTITY (1, 1) NOT NULL,
    [Subject]         NVARCHAR (250) NULL,
    [Description]     NVARCHAR (500) NULL,
    [Salutation]      NVARCHAR (50)  NULL,
    [First Name]      NVARCHAR (50)  NULL,
    [Last Name]       NVARCHAR (50)  NULL,
    [Job Title]       NVARCHAR (50)  NULL,
    [Company Name]    NVARCHAR (50)  NULL,
    [Home Phone]      NVARCHAR (50)  NULL,
    [Mobile Number]   NVARCHAR (50)  NULL,
    [Fax Number]      NVARCHAR (50)  NULL,
    [Other Phone]     NVARCHAR (50)  NULL,
    [Web Site]        NVARCHAR (50)  NULL,
    [Email]           NVARCHAR (50)  NULL,
    [StreeAddress1]   NVARCHAR (250) NULL,
    [StreeAddress2]   NVARCHAR (250) NULL,
    [City]            NVARCHAR (50)  NULL,
    [State/Province]  NVARCHAR (50)  NULL,
    [Zip/Postal Code] NVARCHAR (50)  NULL,
    [CountryId]       INT            NULL,
    [LeadSourceId]    INT            NULL,
    [RatingId]        INT            NULL,
    [IndustryId]      INT            NULL,
    [Industry]        NVARCHAR (25)  NULL,
    [CurrencyId]      INT            NULL,
    [Currency]        NVARCHAR (25)  NULL,
    [Notes]           NVARCHAR (500) NULL,
    [CampaingId]      INT            NULL,
    [OrganizationId]  INT            NULL,
    [Organization]    NVARCHAR (50)  NULL,
    [Deleted]         INT            NULL
);


GO
PRINT N'Creating [dbo].[LeadSource]...';


GO
CREATE TABLE [dbo].[LeadSource] (
    [LeadSourceId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Description]  NVARCHAR (250) NULL,
    [Sort_Order]   INT            NULL,
    [Language]     INT            NULL,
    [Deleted]      INT            NULL
);


GO
PRINT N'Creating [dbo].[Locations]...';


GO
CREATE TABLE [dbo].[Locations] (
    [LocationId]     INT            IDENTITY (1, 1) NOT NULL,
    [StreetAddress1] NVARCHAR (100) NULL,
    [StreetAddress2] NVARCHAR (100) NULL,
    [PostalCode]     NVARCHAR (25)  NULL,
    [City]           NVARCHAR (25)  NULL,
    [StateProvince]  NVARCHAR (25)  NULL,
    [CountryId]      INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[MaritalStatus]...';


GO
CREATE TABLE [dbo].[MaritalStatus] (
    [MaritalStatusId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (25)  NULL,
    [Description]     NVARCHAR (100) NULL,
    [Language]        INT            NULL,
    [Sort_Order]      INT            NULL
);


GO
PRINT N'Creating [dbo].[Opportunities]...';


GO
CREATE TABLE [dbo].[Opportunities] (
    [OpportunitiesId]      INT            IDENTITY (1, 1) NOT NULL,
    [Subject]              NVARCHAR (50)  NULL,
    [Description]          NVARCHAR (500) NULL,
    [Potential CustomerId] INT            NULL,
    [ExpectedRevenue]      DECIMAL (18)   NULL,
    [ExpectedCloseDate]    DATETIME       NULL,
    [Percent]              DECIMAL (18)   NULL,
    [RatingId]             INT            NULL,
    [OwnerId]              INT            NULL,
    [LeadId]               INT            NULL,
    [CampaignId]           INT            NULL,
    [OrganizationId]       INT            NULL,
    [Organization]         NVARCHAR (50)  NULL,
    [Deleted]              INT            NULL
);


GO
PRINT N'Creating [dbo].[Orders]...';


GO
CREATE TABLE [dbo].[Orders] (
    [OrderId]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]                    NVARCHAR (50)  NULL,
    [Description]             NVARCHAR (50)  NULL,
    [Potential Customer]      INT            NULL,
    [CurrencyId]              INT            NULL,
    [Currency]                NVARCHAR (50)  NULL,
    [Price List]              INT            NULL,
    [Detail Amount]           DECIMAL (18)   NULL,
    [Order Discount]          DECIMAL (18)   NULL,
    [Total Tax]               DECIMAL (18)   NULL,
    [Total Amount]            DECIMAL (18)   NULL,
    [Requested Delivery Date] DATETIME       NULL,
    [Bill StreeAddress1]      NVARCHAR (250) NULL,
    [Bill StreeAddress2]      NVARCHAR (250) NULL,
    [Bill City]               NVARCHAR (50)  NULL,
    [Bill State/Province]     NVARCHAR (50)  NULL,
    [Bill Zip/Postal Code]    NVARCHAR (50)  NULL,
    [Bill CountryId]          INT            NULL,
    [Bill PHone]              NVARCHAR (50)  NULL,
    [Bill Address Contact]    NVARCHAR (50)  NULL,
    [Ship StreeAddress1]      NVARCHAR (250) NULL,
    [Ship StreeAddress2]      NVARCHAR (250) NULL,
    [Ship City]               NVARCHAR (50)  NULL,
    [ShipState/Province]      NVARCHAR (50)  NULL,
    [Ship Zip/Postal Code]    NVARCHAR (50)  NULL,
    [Ship CountryId]          INT            NULL,
    [Ship Phone]              NVARCHAR (50)  NULL,
    [Ship Address Contact]    NVARCHAR (50)  NULL,
    [OwnerId]                 INT            NULL,
    [OpportunityId]           INT            NULL,
    [QuotaId]                 INT            NULL,
    [OrganizationId]          INT            NULL,
    [Organization]            NVARCHAR (50)  NULL,
    [Notes]                   NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[OrderStatus]...';


GO
CREATE TABLE [dbo].[OrderStatus] (
    [OrderStatusId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50)  NULL,
    [Description]   NVARCHAR (250) NULL,
    [Sort_Order]    INT            NULL,
    [Language]      INT            NULL,
    [Deleted]       INT            NULL
);


GO
PRINT N'Creating [dbo].[Organizations]...';


GO
CREATE TABLE [dbo].[Organizations] (
    [OrganizationId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (25)  NULL,
    [Description]    NVARCHAR (100) NULL,
    [SiteId]         NVARCHAR (100) NULL,
    [SiteUrl]        NVARCHAR (100) NULL,
    [CurrencyId]     INT            NULL,
    [IndustryId]     INT            NULL,
    [Industry]       NVARCHAR (100) NULL,
    [Language]       INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[PhoneCall]...';


GO
CREATE TABLE [dbo].[PhoneCall] (
    [PhoneCallId]  INT            IDENTITY (1, 1) NOT NULL,
    [SenderId]     INT            NULL,
    [RecipientId]  INT            NULL,
    [PHone Number] NVARCHAR (30)  NULL,
    [Subject]      NVARCHAR (30)  NULL,
    [Description]  NVARCHAR (500) NULL,
    [OwnerId]      INT            NULL,
    [Due]          DATETIME       NULL,
    [PriorityId]   INT            NULL,
    [Notes]        NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Priority]...';


GO
CREATE TABLE [dbo].[Priority] (
    [PriorityId]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [Description] NVARCHAR (250) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[ProductCategories]...';


GO
CREATE TABLE [dbo].[ProductCategories] (
    [ProductCategoriesId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (50)  NULL,
    [Description]         NVARCHAR (250) NULL,
    [Sort_Order]          INT            NULL,
    [Language]            INT            NULL,
    [Deleted]             INT            NULL
);


GO
PRINT N'Creating [dbo].[Products]...';


GO
CREATE TABLE [dbo].[Products] (
    [ProductId]           INT            IDENTITY (1, 1) NOT NULL,
    [Product Name]        NVARCHAR (50)  NULL,
    [Product Description] NVARCHAR (250) NULL,
    [Product Code]        NVARCHAR (50)  NULL,
    [SubjectId]           INT            NULL,
    [Subject]             NVARCHAR (50)  NULL,
    [CategoryId]          INT            NULL,
    [Category]            NVARCHAR (50)  NULL,
    [CanBeSold]           INT            NULL,
    [CanBePurchased]      INT            NULL,
    [ProductTypeId]       INT            NULL,
    [CurrencyId]          INT            NULL,
    [Currency]            NVARCHAR (50)  NULL,
    [SalePrice]           DECIMAL (18)   NULL,
    [CostPrice]           DECIMAL (18)   NULL,
    [Vendor Name]         NVARCHAR (50)  NULL,
    [Vendor Description]  NVARCHAR (250) NULL,
    [Vendor Part Number]  NVARCHAR (50)  NULL,
    [IndustryId]          INT            NULL,
    [Industry]            NVARCHAR (50)  NULL,
    [Sort_Order]          INT            NULL,
    [Language]            INT            NULL,
    [Deleted]             INT            NULL
);


GO
PRINT N'Creating [dbo].[ProductType]...';


GO
CREATE TABLE [dbo].[ProductType] (
    [ProductTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50)  NULL,
    [Description]   NVARCHAR (250) NULL,
    [Sort_Order]    INT            NULL,
    [Language]      INT            NULL,
    [Deleted]       INT            NULL
);


GO
PRINT N'Creating [dbo].[Quotes]...';


GO
CREATE TABLE [dbo].[Quotes] (
    [QuoteId]                 INT            IDENTITY (1, 1) NOT NULL,
    [Name]                    NVARCHAR (50)  NULL,
    [Description]             NVARCHAR (50)  NULL,
    [Potential Customer]      INT            NULL,
    [CurrencyId]              INT            NULL,
    [Currency]                NVARCHAR (50)  NULL,
    [Price List]              DECIMAL (18)   NULL,
    [Detail Amount]           DECIMAL (18)   NULL,
    [Quota Discount]          DECIMAL (18)   NULL,
    [Total Tax]               DECIMAL (18)   NULL,
    [Total Amount]            DECIMAL (18)   NULL,
    [Effective From]          DATETIME       NULL,
    [Effective To]            DATETIME       NULL,
    [Requested Delivery Date] DATETIME       NULL,
    [Due By]                  DATETIME       NULL,
    [Bill StreeAddress1]      NVARCHAR (250) NULL,
    [Bill StreeAddress2]      NVARCHAR (250) NULL,
    [Bill City]               NVARCHAR (50)  NULL,
    [Bill State/Province]     NVARCHAR (50)  NULL,
    [Bill Zip/Postal Code]    NVARCHAR (50)  NULL,
    [Bill CountryId]          INT            NULL,
    [Bill PHone]              NVARCHAR (50)  NULL,
    [Bill Address Contact]    NVARCHAR (50)  NULL,
    [Ship StreeAddress1]      NVARCHAR (250) NULL,
    [Ship StreeAddress2]      NVARCHAR (250) NULL,
    [Ship City]               NVARCHAR (50)  NULL,
    [ShipState/Province]      NVARCHAR (50)  NULL,
    [Ship Zip/Postal Code]    NVARCHAR (50)  NULL,
    [Ship CountryId]          INT            NULL,
    [Ship Phone]              NVARCHAR (50)  NULL,
    [Ship Address Contact]    NVARCHAR (50)  NULL,
    [OwnerId]                 INT            NULL,
    [OpportunityId]           INT            NULL,
    [CampaignId]              INT            NULL,
    [OrganizationId]          INT            NULL,
    [Organization]            NVARCHAR (50)  NULL,
    [Notes]                   NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Rating]...';


GO
CREATE TABLE [dbo].[Rating] (
    [RatingId]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [Description] NVARCHAR (250) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[ResourceTypes]...';


GO
CREATE TABLE [dbo].[ResourceTypes] (
    [ResourceTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (25)  NULL,
    [Description]    NVARCHAR (100) NULL,
    [Sort_Order]     INT            NULL,
    [Language]       INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[Role]...';


GO
CREATE TABLE [dbo].[Role] (
    [RoleId]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [Description] NVARCHAR (250) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[Satisfaction]...';


GO
CREATE TABLE [dbo].[Satisfaction] (
    [SatisfactionId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (50)  NULL,
    [Description]    NVARCHAR (250) NULL,
    [Sort_Order]     INT            NULL,
    [Language]       INT            NULL,
    [Deleted]        INT            NULL
);


GO
PRINT N'Creating [dbo].[Services]...';


GO
CREATE TABLE [dbo].[Services] (
    [ServicesId]          INT            IDENTITY (1, 1) NOT NULL,
    [Service Name]        NVARCHAR (50)  NULL,
    [Service Description] NVARCHAR (50)  NULL,
    [OrganizationId]      INT            NULL,
    [Organization]        NVARCHAR (50)  NULL,
    [Notes]               NVARCHAR (250) NULL
);


GO
PRINT N'Creating [dbo].[Subject]...';


GO
CREATE TABLE [dbo].[Subject] (
    [SubjectId]   INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NULL,
    [Description] NVARCHAR (250) NULL,
    [Sort_Order]  INT            NULL,
    [Language]    INT            NULL,
    [Deleted]     INT            NULL
);


GO
PRINT N'Creating [dbo].[Task]...';


GO
CREATE TABLE [dbo].[Task] (
    [TaskId]      INT            IDENTITY (1, 1) NOT NULL,
    [Subject]     NVARCHAR (500) NULL,
    [RegardingId] INT            NULL,
    [OwnerId]     INT            NULL,
    [Due]         DATETIME       NULL,
    [PriorityId]  INT            NULL,
    [Notes]       NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Timesheet]...';


GO
CREATE TABLE [dbo].[Timesheet] (
    [TimeSheetId]         INT           IDENTITY (1, 1) NOT NULL,
    [EmployeeAccount]     NVARCHAR (50) NULL,
    [EmployeeTimeAccount] NVARCHAR (50) NULL,
    [WeekNumber]          INT           NULL,
    [day1]                INT           NULL,
    [day1Time]            DATETIME      NULL,
    [day2]                INT           NULL,
    [day2Time]            DATETIME      NULL,
    [day3]                INT           NULL,
    [day3Time]            DATETIME      NULL,
    [day4]                INT           NULL,
    [day4Time]            DATETIME      NULL,
    [day5]                INT           NULL,
    [day5Time]            DATETIME      NULL,
    [day6]                INT           NULL,
    [day6Time]            DATETIME      NULL,
    [day7]                INT           NULL,
    [day7Time]            DATETIME      NULL
);


GO
PRINT N'Creating [dbo].[WeekNumber]...';


GO
CREATE TABLE [dbo].[WeekNumber] (
    [TimeSheetId] INT           IDENTITY (1, 1) NOT NULL,
    [WeekNumber]  INT           NULL,
    [StartDate]   DATETIME      NULL,
    [EndDate]     DATETIME      NULL,
    [Year]        NVARCHAR (10) NULL,
    [Language]    INT           NULL
);


GO
PRINT N'Creating [dbo].[GetEmployeePayStubData]...';


GO
CREATE PROCEDURE [dbo].[GetEmployeePayStubData]
	
AS
	DECLARE @SQL NVARCHAR(4000)
	DECLARE @cols NVARCHAR(2000) 

	select @cols = STUFF(( SELECT DISTINCT TOP 100 PERCENT   
	'],[' + name   
	FROM employeepaystub    
	ORDER BY '],[' + name 
	FOR XML PATH('')   
	), 1, 2, '') + ']'  from employeepaystub
	
	SET @SQL = 'select * from
	(
	select 
		Employees.EmployeeId,
			Employees.Email,
			Employees.FirstName, 
			Employees.LastName,
			Employees.ResourceType,
			Employees.Department,   
			Employees.Salary,  		    
			EmployeePayStub.Name,
			(select dbo.CalculateDeduction(Employees.EmployeeId, EmployeePayStub.Percentage) ) as [Amount], 
			(select dbo.CalculateActualSalary(Employees.EmployeeId) ) as [Net Pay]
	    	
	from [EmployeePayStub], [Employees] 
	where [EmployeePayStub].EmployeeId = [Employees].EmployeeId 
	)t 
	PIVOT (SUM(Amount) FOR name 
	IN (' + @cols + ' ) ) as pvt'

	EXECUTE(@SQL);
RETURN 0
GO
PRINT N'Creating [dbo].[CalculateActualSalary]...';


GO
CREATE FUNCTION [dbo].[CalculateActualSalary]
(
	@EmployeeId int	
)
RETURNS MONEY
AS
BEGIN
	
	DECLARE @Salary as MONEY;
	DECLARE @Deductions as MONEY;

	select 	    
		@Salary = SUM(Salary) 
		from Employees 
		where Employees.EmployeeId = @EmployeeId

	select 
		@Deductions = SUM(Percentage) 
		from EmployeePayStub 
		where EmployeePayStub.EmployeeId = @EmployeeId	
	
	RETURN @Salary - ( ( @Salary * @Deductions ) / 100 )

END
GO
PRINT N'Creating [dbo].[CalculateDeduction]...';


GO
CREATE FUNCTION [dbo].[CalculateDeduction]
(
	@EmployeeId int, 
	@Percentage decimal (18, 4) 
)
RETURNS INT
AS
BEGIN
	DECLARE @Salary as MONEY;
	
	select 
	    @Salary = Salary 
		from Employees 
		where EmployeeId = @EmployeeId 		

	RETURN @Salary * @Percentage / 100.00
END
GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

BEGIN
-- =============================================
-- Script Template
-- =============================================

-- ERP Global Tables ---

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Organizations] ([Name],[Description],[CurrencyId],[IndustryId],[Industry],[Language],[Deleted], [SiteId]) VALUES ('Axis Wireless', 'Axis Wireless', 1, 8, 'Retail', 0, 0, 'a97a17d8-584e-4ed9-ad81-e9ffc3871970');
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Organizations] ([Name],[Description],[CurrencyId],[IndustryId],[Language],[Deleted]) VALUES ('BJSmarts LLC', 'BJSmarts LLC', 5, 10, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Language] ([Name] , [Description] ,[Sort_Order]) VALUES ('English', 'English', 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Language] ([Name] , [Description] ,[Sort_Order]) VALUES ('Spanish', 'Spanish', 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Genders] ([Name] , [Description] ,[Sort_Order], [Language]) VALUES ('Male', 'Male', 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Genders] ([Name] , [Description] ,[Sort_Order], [Language]) VALUES ('Female', 'Female', 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Genders] ([Name] , [Description] ,[Sort_Order], [Language]) VALUES ('Varon', 'Varon', 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Genders] ([Name] , [Description] ,[Sort_Order], [Language]) VALUES ('Mujer', 'Mujer', 1, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[CurrencyType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('USD', 'USD', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CurrencyType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('EUR', 'EUR', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CurrencyType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Dolares', 'Dolares', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CurrencyType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Euros', 'Euros', 1, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CurrencyType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Bolivianos', 'Bolivianos', 2, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Countries] ( [Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES ('United States', 'United States', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Countries] ( [Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES ('Canada', 'Canada', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Countries] ( [Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES ('Mexico', 'Mexico', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Countries] ( [Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES ('Estados Unidos', 'Estados Unidos', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Countries] ( [Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES ('Canada', 'Canada', 1, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Countries] ( [Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES ('Mexico', 'Mexico', 2, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Accounting', 'Accounting', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Brokers', 'Brokers', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('WholeSale', 'WholeSale', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Consulting', 'Consulting', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Consumer Services', 'Consumer Services', 5, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Financial', 'Financial', 6, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Marketing', 'Marketing', 7, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Retail', 'Retail', 7, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Contabilidad', 'Contabiliday', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Consultoria', 'Consultoria', 4, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Servicios', 'Servicios', 5, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Bancos', 'Bancos', 6, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Marketing', 'Marketing', 7, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Industry] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Ventas', 'Ventas', 7, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Married', 'Married', 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Single', 'Single', 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Divorced', 'Divorced', 0, 2);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Widow', 'Widow', 0, 3);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Casado', 'Casado', 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Soltero', 'Soltero', 1, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Divorciado', 'Divorciado', 1, 2);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[MaritalStatus] ([Name] ,[Description] ,[Language] ,[Sort_Order]) VALUES ('Viudo', 'Viudo', 1, 3);

-- ERP Human Resource Tables --
INSERT INTO [BJSmarts.ERP.Database].[dbo].[BankAccountType] ([Name], [Description], [Sort_Order] ,[Deleted], [Language]) VALUES ('Checking', 'Checking', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[BankAccountType] ([Name], [Description], [Sort_Order] ,[Deleted], [Language]) VALUES ('Savings', 'Savings', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[BankAccountType] ([Name], [Description], [Sort_Order] ,[Deleted], [Language]) VALUES ('Corriente', 'Corriente', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[BankAccountType] ([Name], [Description], [Sort_Order] ,[Deleted], [Language]) VALUES ('Ahorros', 'Ahorros', 1, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Full Time', 'Full Time', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Part Time', 'Part Time Time', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Contractor', 'Contractor', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Tiempo Completo', 'Contractor', 2, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Medio Tiempo', 'Contractor', 2, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Independiente', 'Contractor', 2, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Project Manager', 'Project Manager', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Office Manager', 'Office Manager', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Software Engineer', 'Software Engineer', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Sales Person', 'Sales Person', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Project Manager', 'Project Manager', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Office Manager', 'Office Manager', 1, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Software Engineer', 'Software Engineer', 2, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ResourceTypes] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Sales Person', 'Sales Person', 3, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Voluntary', 'Voluntary', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Involuntary', 'Involuntary', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Deceased', 'Deceased', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Disability', 'Disability', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Retired', 'Retired', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Voluntaria', 'Voluntaria', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Involuntaria', 'Involuntaria', 1, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Fallecido', 'Fallecido', 2, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Discapacidad', 'Discapacidad', 3, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeTerminationType] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Retirado', 'Retirado', 4, 0, 1);


INSERT INTO [BJSmarts.ERP.Database].[dbo].[BankAccounts] ([BankName],[BankRouter] ,[BankAccountNumber] ,[BankAccountType],[EmployeeId],[Deleted]) VALUES ('Bank of America', '0090000017', '890202000220', 1, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Companies] ([Name] ,[Description] ,[LocationId] ,[Sort_Order] ,[Deleted]) VALUES ('Double Apex', 'Double Apex', 5, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Departments] ([Name] ,[Description] ,[Sort_Order] ,[OrganizationId], [Organization], [Deleted]) VALUES ('Human Resources', 'Human Resources', 1, 1, 'Axis Wireless', 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Departments] ([Name] ,[Description] ,[Sort_Order] ,[OrganizationId], [Organization], [Deleted]) VALUES ('Management', 'Management', 2, 1, 'Axis Wireless', 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Departments] ([Name] ,[Description] ,[Sort_Order] ,[OrganizationId], [Organization], [Deleted]) VALUES ('Finance', 'Finance', 3, 1, 'Axis Wireless', 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Departments] ([Name] ,[Description] ,[Sort_Order] ,[OrganizationId], [Organization], [Deleted]) VALUES ('Sales', 'Sales', 4, 1, 'Axis Wireless', 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Departments] ([Name] ,[Description] ,[Sort_Order] ,[OrganizationId], [Organization], [Deleted]) VALUES ('Ventas', 'Ventas', 1, 2, 'BJSmarts LLC', 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Departments] ([Name] ,[Description] ,[Sort_Order] ,[OrganizationId], [Organization], [Deleted]) VALUES ('Recursos Humanos', 'Recursos Humanos', 2, 2, 'BJSmarts LLC', 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Dependants] ([FirstName] ,[LastName] ,[Email] ,[PhoneNumber] ,[DateOfBirth] ,[GenderId],[EmployeeId],[LocationId] ,[Deleted]) VALUES ('Andrea Janai', 'Smith', 'asmith@yahoo.com', '703-990-0888', '12/01/1976', 1, 1, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeAccounts] ([Name],[Description],[Sort_Order], [Language], [Organization], [Deleted]) VALUES ('Regular Time', 'Regular Time', 0, 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeAccounts] ([Name],[Description],[Sort_Order], [Language], [Organization], [Deleted]) VALUES ('Vacation Time', 'Vacation Time', 1, 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeAccounts] ([Name],[Description],[Sort_Order], [Language], [Organization], [Deleted]) VALUES ('Sick Time', 'Sick Time', 2, 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeAccounts] ([Name],[Description],[Sort_Order], [Language], [Organization], [Deleted]) VALUES ('Tiempo Regular', 'Tiempo Regular', 0, 1, 2, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeAccounts] ([Name],[Description],[Sort_Order], [Language], [Organization], [Deleted]) VALUES ('Vacaciones', 'Vacaciones', 1, 1, 2, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeeAccounts] ([Name],[Description],[Sort_Order], [Language], [Organization], [Deleted]) VALUES ('Enfermedad', 'Enfermedad', 2, 1, 2, 0);
 	 	
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Federal Income Tax', 'Federal Income Tax', 1, 'Tax Deduction', 13.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('VA Income Tax', 'VA Income Tax', 1, 'Tax Deduction', 8.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Social Security', 'Social Security', 1, 'Tax Deduction', 6.2, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Medicare', 'Medicare', 1, 'Tax Deduction', 1.45, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('PPO Medicare System', 'PPO Medicare System', 1, 'Voluntary Deduction', 9.2, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Federal Income Tax', 'Federal Income Tax', 2, 'Tax Deduction', 13.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('VA Income Tax', 'VA Income Tax', 2, 'Tax Deduction', 8.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Social Security', 'Social Security', 2, 'Tax Deduction', 6.2, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Medicare', 'Medicare', 2, 'Tax Deduction', 1.45, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('PPO Medicare System', 'PPO Medicare System', 2, 'Voluntary Deduction', 8.6, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Federal Income Tax', 'Federal Income Tax', 3, 'Tax Deduction', 13.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('VA Income Tax', 'VA Income Tax', 3, 'Tax Deduction', 8.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Social Security', 'Social Security', 3, 'Tax Deduction', 6.2, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Medicare', 'Medicare', 3, 'Tax Deduction', 1.45, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('PPO Medicare System', 'PPO Medicare System', 3, 'Voluntary Deduction', 8.4, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Federal Income Tax', 'Federal Income Tax', 4, 'Tax Deduction', 13.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('VA Income Tax', 'VA Income Tax', 4, 'Tax Deduction', 8.0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Social Security', 'Social Security', 4, 'Tax Deduction', 6.2, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Medicare', 'Medicare', 4, 'Tax Deduction', 1.45, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('PPO Medicare System', 'PPO Medicare System', 4, 'Voluntary Deduction', 6.6, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[EmployeePayStub]([Name],[Description],[EmployeeId],[Deduction Type],[Percentage],[Deleted]) VALUES ('Dental Vision', 'Dental Vision', 4, 'Voluntary Deduction', 2.4, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Benefits]([Name],[Description],[AccruedHours],[Sort_Order],[EmployeeId],[Language],[Deleted]) VALUES ('Vacation', 'Vacation', 0.0, 1, 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Benefits]([Name],[Description],[AccruedHours],[Sort_Order],[EmployeeId],[Language],[Deleted]) VALUES ('Sick', 'Sick', 0.0, 2, 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Benefits]([Name],[Description],[AccruedHours],[Sort_Order],[EmployeeId],[Language],[Deleted]) VALUES ('Vacacion', 'Vacacion',0.0, 1, 2, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] 
       ([FirstName],[LastName],[Email],[HomePhoneNumber],[WorkPhoneNumber],[MovilPhoneNumber],[HireDate],[DateOfBirth],[Salary],[EmployeTypeId],[EmployeeType],[ResourceTypeId],[ResourceType],[GenderId],[Gender],[CurrencyTypeId],[CurrencyType],[MaritalStatusId],[MaritalStatus],[ManagerId],[DepartmentId],[Department],[CompanyId],[Deleted], [HomeStreetAddress1] ,[HomeStreetAddress2] ,[HomePostalCode] ,[HomeCity] ,[HomeStateProvince] ,[HomeCountryId], [WorkStreetAddress1] ,[WorkStreetAddress2] ,[WorkPostalCode] ,[WorkCity] ,[WorkStateProvince] ,[WorkCountryId]) 
VALUES ('John', 'Smith', 'jsmith@bjsmarts.com', '703-309-0099', '289-299-0000', '277-299-2000', '2/1/2010', '2/22/1960', 4000.00, 1, 'Full Time', 4, 'Sales Person', 1, 'Male', 1, 'USD', 1, 'Married', 2, 1, 'Human Resources',1, 0, '280 Cedar Avenue', '', '22180', 'Fairfax', 'VA', 1, '6065 Leesburg Pike', '', '22041', 'Falls Church', 'VA', 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] 
       ([FirstName],[LastName],[Email],[HomePhoneNumber],[WorkPhoneNumber],[MovilPhoneNumber],[HireDate],[DateOfBirth],[Salary],[EmployeTypeId],[EmployeeType],[ResourceTypeId],[ResourceType],[GenderId],[Gender],[CurrencyTypeId],[CurrencyType],[MaritalStatusId],[MaritalStatus],[ManagerId],[DepartmentId],[Department],[CompanyId],[Deleted], [HomeStreetAddress1] ,[HomeStreetAddress2] ,[HomePostalCode] ,[HomeCity] ,[HomeStateProvince] ,[HomeCountryId], [WorkStreetAddress1] ,[WorkStreetAddress2] ,[WorkPostalCode] ,[WorkCity] ,[WorkStateProvince] ,[WorkCountryId]) 
VALUES ('Charles', 'Singleton', 'csingleton@bjsmarts.com', '703-309-0099', '301-299-0000', '301-299-2000', '2/1/2005', '2/22/1972', 5000.00, 1, 'Full Time', 7, 'Software Engineer',1, 'Male',1, 'USD',2, 'Single', 2, 2, 'Finance',1, 0, '280 Cedar Avenue', '', '22180', 'Fairfax', 'VA', 1, '6065 Leesburg Pike', '', '22041', 'Falls Church', 'VA', 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] 
       ([FirstName],[LastName],[Email],[HomePhoneNumber],[WorkPhoneNumber],[MovilPhoneNumber],[HireDate],[DateOfBirth],[Salary],[EmployeTypeId],[EmployeeType],[ResourceTypeId],[ResourceType],[GenderId],[Gender],[CurrencyTypeId],[CurrencyType],[MaritalStatusId],[MaritalStatus],[ManagerId],[DepartmentId],[Department],[CompanyId],[Deleted], [HomeStreetAddress1] ,[HomeStreetAddress2] ,[HomePostalCode] ,[HomeCity] ,[HomeStateProvince] ,[HomeCountryId], [WorkStreetAddress1] ,[WorkStreetAddress2] ,[WorkPostalCode] ,[WorkCity] ,[WorkStateProvince] ,[WorkCountryId]) 
VALUES ('Jessica', 'Thompson', 'jthompson@bjsmarts.com', '703-309-0099', '202-299-0000', '202-299-2000', '2/1/2002', '2/22/1970', 10000.00, 1, 'Full Time', 2, 'Project Manager',2, 'Female', 1, 'USD',2, 'Single', 3, 1, 'Human Resources',1, 0, '280 Cedar Avenue', '', '22180', 'Fairfax', 'VA', 1, '6065 Leesburg Pike', '', '22041', 'Falls Church', 'VA', 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Employees] 
       ([FirstName],[LastName],[Email],[HomePhoneNumber],[WorkPhoneNumber],[MovilPhoneNumber],[HireDate],[DateOfBirth],[Salary],[EmployeTypeId],[EmployeeType],[ResourceTypeId],[ResourceType],[GenderId],[Gender],[CurrencyTypeId],[CurrencyType],[MaritalStatusId],[MaritalStatus],[ManagerId],[DepartmentId],[Department],[CompanyId],[Deleted], [HomeStreetAddress1] ,[HomeStreetAddress2] ,[HomePostalCode] ,[HomeCity] ,[HomeStateProvince] ,[HomeCountryId], [WorkStreetAddress1] ,[WorkStreetAddress2] ,[WorkPostalCode] ,[WorkCity] ,[WorkStateProvince] ,[WorkCountryId])  
VALUES ('Ken', 'Jamison', 'jamison@bjsmarts.com', '703-309-0099', '703-299-0000', '703-299-2000', '2/1/2002', '2/22/1960', 2800.00, 1, 'Full Time', 8, 'Sales Person',1, 'Male', 1, 'USD',1, 'Married', 2, 1, 'Human Resources',1, 0, '280 Cedar Avenue', '', '22180', 'Fairfax', 'VA', 1, '6065 Leesburg Pike', '', '22041', 'Falls Church', 'VA', 1);



-- ERP CRM Tables --

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Lowest', 'Lowest', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Low', 'Low', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Normal', 'Normal', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('High', 'High', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Highest', 'Highest', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Baja', 'Baja', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Normal', 'Normal', 1, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Priority] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Alta', 'Alta', 2, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Product', 'Product', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Software', 'Software', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Service', 'Service', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Information', 'Information', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Training', 'Training', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Consulting', 'Consulting', 5, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Producto', 'Producto', 0, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Software', 'Software', 1, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Servicio', 'Servicio', 2, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Informacion', 'Informacion', 3, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Entrenamiento', 'Entrenamiento', 4, 0, 1);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductCategories] ([Name], [Description], [Sort_Order], [Deleted], [Language]) VALUES('Consultoria', 'Consultoria', 5, 0, 1);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Materials', 'Materials', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Services', 'Services', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Warranty', 'Warranty', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Materiales', 'Materiales', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Servicios', 'Servicios', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[ProductType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Garantias', 'Garantias', 2, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Advertising', 'Advertising', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Employee Referral', 'Employee Referral', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('External Referral', 'External Referral', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Partner', 'Partner', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Public Relations', 'Public Relations', 5, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Seminar', 'Seminar', 6, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Trade Show', 'Trade Show', 7, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Web', 'Web', 8, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[LeadSource] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Other', 'Other', 9, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Proposed', 'Proposed', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Ready to launch', 'Ready to launch', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Launched', 'Launched', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Completed', 'Completed', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Canceled', 'Canceled', 5, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Suspended', 'Suspended', 6, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignTypes] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Advertisement', 'Advertisement', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignTypes] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Direct Marketing', 'Direct Marketing', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignTypes] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Event', 'Event', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CampaignTypes] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Other', 'Other', 4, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[AccountCategories] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Preferred Customer', 'Preferred Customer', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[AccountCategories] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Standard', 'Standard', 2, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Rating] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Hot', 'Hot', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Rating] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Warm', 'Warm', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Rating] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Cold', 'Cold', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Rating] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Caliente', 'Caliente', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Rating] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Tivio', 'Tivio', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Rating] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Frio', 'Frio', 2, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Role] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Decision Maker', 'Decision Maker', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Role] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Employee', 'Employee', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Role] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Jefe', 'Jefe', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Role] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Empleado', 'Empleado', 1, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Question', 'Question', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Problem', 'Problem', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Request', 'Request', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Pregunta', 'Pregunta', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Problema', 'Problema', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseType] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Requerimiento', 'Requerimiento', 2, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseOrigin] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Phone', 'Phone', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseOrigin] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Email', 'Email', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseOrigin] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Web', 'Web', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseOrigin] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Telefono', 'Telefono', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseOrigin] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Correo Electronico', 'Correo Electronico', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseOrigin] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Internet', 'Internet', 2, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Very Satisfied', 'Very Satisfied', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Satisfied', 'Satisfied', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Neutral', 'Neutral', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Disatisfied', 'Disatisfied', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Muy Satisfecho', 'Muy Satisfecho', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Satisfecho', 'Satisfecho', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Neutral', 'Neutral', 2, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Satisfaction] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Desastifecho', 'Desastifecho', 3, 1, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('In Progress', 'In Progress', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('On Hold', 'On Hold', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Waiting for Details', 'Waiting for Details', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Researching', 'Researching', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('En Progreso', 'En Progreso', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('En Esperando', 'En Esperando', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Esperando for Details', 'Esperando for Details', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[CaseStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('En buscando', 'En buscado', 3, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('New', 'New', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Update', 'Update', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Awaiting Shipping', 'Awaiting Shipping', 2, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Shipped', 'Shipped', 3, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Cancel', 'Cancel', 4, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Close', 'Close', 5, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Nueva', 'Nueva', 0, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Modification', 'Modification', 1, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Esperando por Mercancia', 'Esperando por Mercancia', 2, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Enviado', 'Enviado', 3, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Cancelado', 'Cancelado', 4, 1, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[OrderStatus] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('Cerrado', 'Cerrado', 5, 1, 0);


INSERT INTO [BJSmarts.ERP.Database].[dbo].[Campaign]
           ([Name],[Description],[Total Cost of Campaign],[Proposed Begin Date],[Proposed End Date],[Actual Begin Date],[Actual End Date],[CampaignStatusId],[CampaignStatus],[CampaignTypeId],[CampaignType], [OrganizationId], [Organization])
     VALUES
           ('TV Show', 'TV Show', 2000, '4/16/2013', '5/16/2013', '5/1/2013', '6/1/2013', 1, 'Proposed', 1, 'Advertising', 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Campaign]
           ([Name],[Description],[Total Cost of Campaign],[Proposed Begin Date],[Proposed End Date],[Actual Begin Date],[Actual End Date],[CampaignStatusId],[CampaignStatus],[CampaignTypeId],[CampaignType], [OrganizationId], [Organization])
     VALUES
           ('Internet Advertising', 'Internet Advertising', 100, '1/16/2013', '2/16/2013', '4/1/2013', '5/1/2013', 1, 'Proposed', 1, 'Advertising', 2, 'BJSmarts LLC');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Leads]
           ([Subject],[Description],[First Name],[Last Name],[Job Title],[Company Name],[Home Phone],[Mobile Number],[Web Site],[Email],[StreeAddress1],[City],[State/Province],[Zip/Postal Code],[CountryId],[LeadSourceId],[RatingId],[IndustryId],[Industry],[CurrencyId],[Currency],[Notes],[CampaingId],[OrganizationId], [Organization], [Deleted])
     VALUES
           ('Test Lead', 'Test Lead', 'John', 'Smith', 'Promoter', 'Excel LLC', '703-390-0000', '512-980-0000', 'www.excel.com', 'jsmith@excel.com', '8909 Debi St', 'Alexandria', 'VA', '22079', 1, 1, 1, 4, 'Consulting', 1, 'USD', 'Test Notes', 1, 1, 'Axis Wireless', 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Leads]
           ([Subject],[Description],[First Name],[Last Name],[Job Title],[Company Name],[Home Phone],[Mobile Number],[Web Site],[Email],[StreeAddress1],[City],[State/Province],[Zip/Postal Code],[CountryId],[LeadSourceId],[RatingId],[IndustryId],[Industry],[CurrencyId],[Currency],[Notes],[CampaingId],[OrganizationId], [Organization], [Deleted])
     VALUES
           ('Installation & Configuration', 'Installation & Configuration', 'Debbie', 'Johnson', 'Manager', 'Techtam LLC', '703-390-0000', '512-980-0000', 'www.techtam.com', 'djohnson@techtam.com', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, 1, 1, 4, 'Consulting', 1, 'USD', 'Test Notesz', 1, 2, 'BJSmarts LLC', 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Opportunities]
           ([Subject],[Description],[Potential CustomerId],[RatingId],[OwnerId],[LeadId],[CampaignId],[OrganizationId], [Organization],[Deleted])
     VALUES
           ('Test Lead', 'Test Lead', 1, 2, 1, 1, 1, 1, 'Axis Wireless', 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Accounts]
           ([Name],[Description],[Account Number],[Main Phone],[Primary Contact],[WebSite],[Email],[StreeAddress1],[City],[State/Province],[Zip/Postal Code],[CountryId],[Country],[IndustryId],[Industry],[CurrencyId],[Currency],[AccountCategoryId],[AccountCategory],[LeadId], [OrganizationId], [Organization])
     VALUES
           ('Excel LLC', 'Excel LLC promotion company', '010001', '703-220-1099', 1, 'www.excel.com', 'jsmith@excel.com', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, 'USA', 7, 'Marketing', 1, 'USD', 2, 'Standard', 1, 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Contacts]
           ([First Name],[Last Name],[Job Title],[AccountsId],[Business Phone],[Home Phone],[Mobile Phone],[Email],[StreeAddress1],[City],[State/Province],[Zip/Postal Code],[CountryId],[Country],[Description],[Department],[RoleId],[Manager],[Manager Phone],[GenderId],[CurrencyId],[Currency],[LeadId],[OrganizationId],[Organization])
     VALUES
           ('John', 'Smith', 'Promoter', 1, '703-220-1099', '703-220-1099', '703-220-1099', 'jsmith@excel.com','8909 Debi St', 'Fairfax', 'VA', '22081', 1, 'USA', '', '', 2, '', '', 1, 1, 'USD', 1, 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Cases]
           ([Title],[Description],[CustomerId],[SubjectId],[CaseTypeId],[CaseType],[CaseOriginId],[SatisfactionId],[OwnerId],[CaseStatusId],[CaseStatus],[PriorityId],[OrganizationId],[Organization])
     VALUES
           ('Help Desk Support', 'Help Desk Support', 1, 1, 3, 'Request', 1, 1, 1, 1, 'In Progress', 1, 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Quotes]
           ([Name],[Description],[Potential Customer],[CurrencyId],[Currency],[Price List],[Detail Amount],[Quota Discount],[Total Tax],[Total Amount],[Effective From],[Effective To],[Bill StreeAddress1],[Bill City],[Bill State/Province],[Bill Zip/Postal Code],[Bill CountryId],[Bill PHone],[Bill Address Contact],[Ship StreeAddress1],[Ship City],[ShipState/Province],[Ship Zip/Postal Code],[Ship CountryId],[Ship Phone],[Ship Address Contact],[OwnerId],[OpportunityId],[CampaignId],[OrganizationId],[Organization])
     VALUES
           ('CRM Module', 'CRM Module', 1, 1, 'USD', 5000, 5000, 10, 550, 5500, '5/1/2013', '6/1/2013', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, '703-390-9999', 'John Smith', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, '703-309-9999', 'John Smith', 1, 1, 1, 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Orders]
           ([Name],[Description],[Potential Customer],[CurrencyId],[Currency],[Price List],[Detail Amount],[Order Discount],[Total Tax],[Total Amount],[Requested Delivery Date],[Bill StreeAddress1],[Bill City],[Bill State/Province],[Bill Zip/Postal Code],[Bill CountryId],[Bill PHone],[Bill Address Contact],[Ship StreeAddress1],[Ship City],[ShipState/Province],[Ship Zip/Postal Code],[Ship CountryId],[Ship Phone],[Ship Address Contact],[OwnerId],[OpportunityId],[QuotaId],[OrganizationId],[Organization])
     VALUES
           ('CRM Module', 'CRM Module', 1, 1, 'USD', 5000, 5000, 10, 550, 5500, '6/1/2013', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, '703-309-9999', 'John Smith', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, '703-309-9999', 'John Smith', 1, 1, 1, 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Invoices]
           ([Name],[Description],[CustomerId],[Price List],[CurrencyId],[Currency],[Detail Amount],[Invoice Discount],[Total Tax],[Total Amount],[Date Delivered],[Bill StreeAddress1],[Bill City],[Bill State/Province],[Bill Zip/Postal Code],[Bill CountryId],[Bill PHone],[Bill Address Contact],[Ship StreeAddress1],[Ship City],[ShipState/Province],[Ship Zip/Postal Code],[Ship CountryId],[Ship Phone],[Ship Address Contact],[OwnerId],[OpportunityId],[OrderId],[OrganizationId],[Organization])
     VALUES
           ('CRM Module', 'CRM Module', 1, 5000, 1, 'USD', 5000, 10, 550, 5500, '6/1/2013', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, '703-309-9999', 'John Smith', '8909 Debi St', 'Fairfax', 'VA', '22081', 1, '703-309-9999', 'John Smith', 1, 1, 1, 1, 'Axis Wireless');

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Subject] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('ERP', 'ERP', 0, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Subject] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('ERP Module - HR', 'ERP Module - Human Resources', 1, 0, 0);
INSERT INTO [BJSmarts.ERP.Database].[dbo].[Subject] ([Name],[Description],[Sort_Order],[Language],[Deleted]) VALUES ('ERP Module - CRM', 'ERP Module - Customer Relationship Management', 2, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Products]
           ([Product Name],[Product Description],[Product Code],[SubjectId],[Subject],[CategoryId],[Category],[ProductTypeId],[CurrencyId],[Currency],[SalePrice],[CostPrice],[Vendor Name],[Vendor Description],[Vendor Part Number],[Industry], [IndustryId],[Sort_Order],[Language],[Deleted])
     VALUES
           ('ERP', 'ERP Description', '10001', 1, 'ERP', 2, 'Software', 2, 1, 'USD', 10000, 9000, '', '', '', 'Retail', 7, 0, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Products]
           ([Product Name],[Product Description],[Product Code],[SubjectId],[Subject],[CategoryId],[Category],[ProductTypeId],[CurrencyId],[Currency],[SalePrice],[CostPrice],[Vendor Name],[Vendor Description],[Vendor Part Number],[Industry], [IndustryId],[Sort_Order],[Language],[Deleted])
     VALUES
           ('HR', 'HR Description', '10002', 1, 'HR', 2, 'Software', 2, 1, 'USD', 1000, 900, '', '', '',  'Retail', 7, 1, 0, 0);

INSERT INTO [BJSmarts.ERP.Database].[dbo].[Products]
           ([Product Name],[Product Description],[Product Code],[SubjectId],[Subject],[CategoryId],[Category],[ProductTypeId],[CurrencyId],[Currency],[SalePrice],[CostPrice],[Vendor Name],[Vendor Description],[Vendor Part Number],[Industry], [IndustryId],[Sort_Order],[Language],[Deleted])
     VALUES
           ('CRM', 'CRM Description', '10003', 1, 'CRM', 2, 'Software', 2, 1, 'USD', 1000, 900, '', '', '',  'Retail', 7, 2, 0, 0);

END
GO

GO
