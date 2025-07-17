USE [EmployeeManagementDB]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[Gender] [nvarchar](10) NULL,
	[DateOfBirth] [date] NULL,
	[DepartmentId] [int] NULL,
	[IsActive] [bit] NULL,
	[JoinDate] [date] NULL,
	[AadharPath] [nvarchar](500) NULL,
	[TermsAccepted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Department] ON 

SET IDENTITY_INSERT [dbo].[Department] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

GO
INSERT [dbo].[Employee] ([EmployeeId], [FirstName], [LastName], [Email], [Phone], [Gender], [DateOfBirth], [DepartmentId], [IsActive], [JoinDate], [AadharPath], [TermsAccepted]) VALUES (10044, N'Meet', N'virani', N'meet00.gnwebsoft@gmail.com', N'', NULL, CAST(N'2025-07-09' AS Date), 1, 1, CAST(N'2025-07-09' AS Date), NULL, 0)
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Employee__A9D105342A4A218A]    Script Date: 09-07-2025 12:57:40 ******/
ALTER TABLE [dbo].[Employee] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT (getdate()) FOR [JoinDate]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [TermsAccepted]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD CHECK  (([Gender]='Other' OR [Gender]='Female' OR [Gender]='Male'))
GO
/****** Object:  StoredProcedure [dbo].[sp_AddDepartment]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AddDepartment]
	@DepartmentName NVARCHAR(100),
	@Description NVARCHAR(250)
AS
BEGIN
	INSERT INTO
	Department
		(DepartmentName,
		Description)
	VALUES	
		(@DepartmentName,
		@Description);
END


GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteDepartment]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteDepartment]
	@DepartmentId INT
AS
BEGIN
	DELETE
	FROM	Department
	WHERE	DepartmentId = @DepartmentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteEmployee]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteEmployee]
	@EmployeeId		INT
AS
BEGIN
	DELETE FROM 
		Employee 
	WHERE	EmployeeId = @EmployeeId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllDepartments]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC [dbo].[sp_GetAllDepartments]
CREATE PROCEDURE [dbo].[sp_GetAllDepartments]
AS
BEGIN
	SELECT	*
	FROM	Department;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllEmployees]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC [dbo].[sp_GetAllEmployees]
CREATE PROCEDURE [dbo].[sp_GetAllEmployees]
AS
BEGIN
	SELECT 
		Employee.*,
		Department.DepartmentName
	FROM	Employee

	LEFT JOIN	Department
	ON	Employee.DepartmentId = Department.DepartmentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllEmployeesPaged]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC sp_GetAllEmployeesPaged 2,100,NULL,NULL,NULL
CREATE PROCEDURE [dbo].[sp_GetAllEmployeesPaged]
	@PageNumber		INT,
	@PageSize		INT,
	@Department		NVARCHAR(100) = NULL,
	@Gender			NVARCHAR(10) = NULL,
	@Status			BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*)
	FROM	Employee

	LEFT JOIN Department
		ON	Employee.DepartmentId = Department.DepartmentId

	WHERE (@Department IS NULL OR Department.DepartmentName = @Department)
		AND (@Gender IS NULL OR Employee.Gender = @Gender)
		AND (@Status IS NULL OR Employee.IsActive = @Status);
	
	SELECT
		Employee.EmployeeId,
		Employee.FirstName,
		Employee.LastName,
		Employee.Gender,
		Employee.Email,
		Employee.IsActive,
		Employee.DepartmentId,
		Department.DepartmentName
	FROM	Employee

	LEFT JOIN	Department
		ON		Employee.DepartmentId = Department.DepartmentId

	WHERE		(@Department IS NULL OR Department.DepartmentName = @Department)
		AND		(@Gender IS NULL OR Employee.Gender = @Gender)
		AND		(@Status IS NULL OR Employee.IsActive = @Status)

	ORDER BY	Employee.EmployeeId
	OFFSET (@PageNumber - 1) * @PageSize ROWS

	FETCH NEXT	@PageSize ROWS ONLY;
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetDepartmentById]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetDepartmentById]
	@DepartmentId	INT
AS
BEGIN
	SELECT	*
	FROM	Department
	WHERE	DepartmentId = @DepartmentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEmployeeById]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [dbo].[sp_GetEmployeeById] 30
CREATE PROCEDURE [dbo].[sp_GetEmployeeById]
	@EmployeeId		INT
AS
BEGIN
	SELECT
		Employee.*,
		Department.DepartmentName
	FROM	Employee
	LEFT JOIN	Department
		ON	Employee.DepartmentId = Department.DepartmentId
		WHERE	Employee.EmployeeId = @EmployeeId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertEmployee]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertEmployee]
	@FirstName		NVARCHAR(50),
	@LastName		NVARCHAR(50),
	@Email			NVARCHAR(100),
	@Phone			NVARCHAR(20),
	@Gender			NVARCHAR(10),
	@DateOfBirth	DATE,
	@DepartmentId	INT,
	@IsActive		BIT,
	@AadharPath		NVARCHAR(500),
	@TermsAccepted	BIT,
	@EmployeeId		INT OUTPUT
AS
BEGIN
	INSERT INTO	Employee 
		(FirstName,
		LastName,
		Email,
		Phone,
		Gender,
		DateOfBirth,
		DepartmentId,
		IsActive,
		AadharPath,
		TermsAccepted)
	VALUES
		(@FirstName,
		@LastName,
		@Email,
		@Phone,
		@Gender,
		@DateOfBirth,
		@DepartmentId,
		@IsActive,
		@AadharPath,
		@TermsAccepted);
	
	SET	@EmployeeId = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateDepartment]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateDepartment]
	@DepartmentId INT,
	@DepartmentName NVARCHAR(100),
	@Description NVARCHAR(250)
AS
BEGIN
	UPDATE	Department
	SET	DepartmentName = @DepartmentName,
		Description = @Description
	WHERE	DepartmentId = @DepartmentId;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateEmployee]    Script Date: 09-07-2025 12:57:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateEmployee]
	@EmployeeId		INT,
	@FirstName		NVARCHAR(50),
	@LastName		NVARCHAR(50),
	@Email			NVARCHAR(100),
	@Phone			NVARCHAR(20),
	@Gender			NVARCHAR(10),
	@DateOfBirth	DATE,
	@DepartmentId	INT,
	@IsActive		BIT,
	@AadharPath		NVARCHAR(500),
	@TermsAccepted	BIT
AS
BEGIN
	UPDATE	Employee
	SET	FirstName = @FirstName,
		LastName = @LastName,
		Email = @Email,
		Phone = @Phone,
		Gender = @Gender,
		DateOfBirth = @DateOfBirth,
		DepartmentId = @DepartmentId,
		IsActive = @IsActive,
		AadharPath = @AadharPath,
		TermsAccepted = @TermsAccepted
	WHERE	EmployeeId = @EmployeeId;
END
GO
