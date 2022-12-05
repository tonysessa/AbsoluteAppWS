-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
if exists (select * from dbo.sysobjects where id = object_id(N'[GlobalParameter]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [GlobalParameter]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsResources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsResources]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsUsers_Acl]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsUsers_Acl]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsRoles_Acl]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsRoles_Acl]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsSubSections]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsSubSections]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsSections]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsSections]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsFile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsFile]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsRepository]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsRepository]
GO

IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[CmsLabels]') AND OBJECTPROPERTY(ID, N'ISUSERTABLE') = 1)
	DROP TABLE [CmsLabels]
GO

IF EXISTS (SELECT * FROM DBO.SYSOBJECTS WHERE ID = OBJECT_ID(N'[CmsResources]') AND OBJECTPROPERTY(ID, N'ISUSERTABLE') = 1)
	DROP TABLE [CmsResources]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsUsers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsUsers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsRoles]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsRoles]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsRouting]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsRouting]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[CmsNlsContext]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	drop table [CmsNlsContext]
GO

-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Languages x [CmsNlsContext]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------

CREATE TABLE [CmsNlsContext] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[StartingPage]												[NVARCHAR] (255)		NULL,
	[IsoCode]													[NVARCHAR] (50)			NULL,
	[Title]														[NVARCHAR] (150)		NULL,
	[Description]												[NVARCHAR] (255)		NULL,
	[Ord]														[INT] 					DEFAULT 0										-- ordine di visualizzazione		
)
GO

	CREATE INDEX [I_StatusFlag]									ON [CmsNlsContext]([StatusFlag])
	--
	CREATE INDEX [I_StartingPage]								ON [CmsNlsContext]([StartingPage])
	CREATE INDEX [I_IsoCode]									ON [CmsNlsContext]([IsoCode])
	CREATE INDEX [I_Title]										ON [CmsNlsContext]([Title])
	CREATE INDEX [I_Description]								ON [CmsNlsContext]([Description])
	CREATE INDEX [I_Ord]										ON [CmsNlsContext]([Ord])
GO


-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsRoles]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsRoles] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Title]													[NVARCHAR] (150)		NOT NULL,
	[Uriname]													[NVARCHAR] (200)		NOT NULL,	
)
GO
	CREATE INDEX [I_StatusFlag]									ON [CmsRoles]([StatusFlag])
	--	
	CREATE UNIQUE INDEX [I_Title]								ON [CmsRoles]([Title])
	CREATE UNIQUE INDEX [I_Uriname]								ON [CmsRoles]([Uriname])
GO


-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsUsers]
-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
CREATE TABLE [CmsUsers] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Name]														[NVARCHAR] (200)		NULL,
	[Surname]													[NVARCHAR] (200)		NULL,
	[Email]														[NVARCHAR] (200)		NULL,
	[Username]													[NVARCHAR] (20)			NOT NULL,										-- username
	[Password]													[NVARCHAR] (20)			NOT NULL,										-- password
	[DateLastLogin]												[DATETIME]				NULL,											-- data ultima modifica sezione
	[NumLogin]													[INT] 					DEFAULT 0,										-- Numero di accessi
	[Uid_CmsRoles]												[INT]					REFERENCES [CmsRoles] on DELETE SET NULL,		-- uid CmsRoles
	--	
	[Note]														[NTEXT]					NULL											-- valore
)
GO

	CREATE INDEX [I_CreationDate]								ON [CmsUsers]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsUsers]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsUsers]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsUsers]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsUsers]([StatusFlag])
	--
	CREATE INDEX [I_Name]										ON [CmsUsers]([Name])
	CREATE INDEX [I_Surname]									ON [CmsUsers]([Surname])
	CREATE INDEX [I_Email]										ON [CmsUsers]([Email])
	CREATE UNIQUE INDEX [I_Username]							ON [CmsUsers]([Username])
	CREATE INDEX [I_Password]									ON [CmsUsers]([Password])
	CREATE INDEX [I_DateLastLogin]								ON [CmsUsers]([DateLastLogin])
	CREATE INDEX [I_NumLogin]									ON [CmsUsers]([NumLogin])
	CREATE INDEX [I_Uid_CmsRoles]								ON [CmsUsers]([Uid_CmsRoles])
GO

-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsSections]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsSections] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Title]														[NVARCHAR] (150)		NOT NULL,
	[ContentTable]												[NVARCHAR] (150)		NULL,
	[SectionUri]												[NVARCHAR] (150)		NULL,
	--
	[Link]														[NVARCHAR] (200)		NULL,
	[Link_Target]												[NVARCHAR] (20)			NULL,
	[Link_Preview]												[NVARCHAR] (200)		NULL,
	[IconClass]													[NVARCHAR] (200)		NULL,
	--
	[Ord]														[INT] 					DEFAULT 0										-- ordine di visualizzazione
)
GO

	CREATE INDEX [I_CreationDate]								ON [CmsSections]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsSections]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsSections]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsSections]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsSections]([StatusFlag])
	--
	CREATE INDEX [I_Title]										ON [CmsSections]([Title])
	CREATE INDEX [I_ContentTable]								ON [CmsSections]([ContentTable])
	CREATE INDEX [I_SectionUri]									ON [CmsSections]([SectionUri])	
	CREATE INDEX [I_Ord]										ON [CmsSections]([Ord])
GO

-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsSubSections]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsSubSections] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsSections]											[INT]					REFERENCES CmsSections on DELETE SET NULL,		-- uid CmsNlsContext
	[Title]														[NVARCHAR] (150)		NOT NULL,
	[ContentTable]												[NVARCHAR] (150)		NULL,
	[SectionUri]												[NVARCHAR] (150)		NULL,
	--
	[Link]														[NVARCHAR] (200)		NULL,
	[Link_Target]												[NVARCHAR] (20)			NULL,
	[Link_Preview]												[NVARCHAR] (200)		NULL,
	[IconClass]													[NVARCHAR] (200)		NULL,
	--
	[Ord]														[INT] 					DEFAULT 0										-- valore
)
GO
	
	CREATE INDEX [I_CreationDate]								ON [CmsSubSections]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsSubSections]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsSubSections]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsSubSections]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsSubSections]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsSections]							ON [CmsSubSections]([Uid_CmsSections])
	CREATE INDEX [I_ContentTable]								ON [CmsSubSections]([ContentTable])
	CREATE INDEX [I_SectionUri]									ON [CmsSubSections]([SectionUri])	
	CREATE INDEX [I_Ord]										ON [CmsSubSections]([Ord])
GO

-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsUsers_Acl]
-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
CREATE TABLE [CmsUsers_Acl] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsUsers]												[INT]					REFERENCES [CmsUsers] on DELETE SET NULL,		-- uid CmsRoles
	[Uid_CmsNlsContext]											[INT]					REFERENCES [CmsNlsContext] on DELETE SET NULL,	-- uid CmsNlsContext
	[CanRead]													[INT] 					DEFAULT 1,										-- Accesso in Lettura
	[CanWrite]													[INT] 					DEFAULT 1										-- Accesso in scrittura
)
GO

	CREATE INDEX [I_CreationDate]								ON [CmsUsers_Acl]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsUsers_Acl]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsUsers_Acl]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsUsers_Acl]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsUsers_Acl]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsNlsContext]							ON [CmsUsers_Acl]([Uid_CmsNlsContext])
	CREATE INDEX [I_CanRead]									ON [CmsUsers_Acl]([CanRead])
	CREATE INDEX [I_CanWrite]									ON [CmsUsers_Acl]([CanWrite])
GO


-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsRoles_Acl]
-- -----------------------------------------------------------------------------------------------
-- -----------------------------------------------------------------------------------------------
CREATE TABLE [CmsRoles_Acl] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsRoles]												[INT]					REFERENCES [CmsRoles] on DELETE SET NULL,		-- uid [CmsRoles]
	[Uid_CmsSubSections]										[INT]					REFERENCES [CmsSubSections] on DELETE SET NULL,	-- uid [CmsSubSections]
	[CanRead]													[INT] 					DEFAULT 1,										-- Accesso in Lettura
	[CanWrite]													[INT] 					DEFAULT 1										-- Accesso in scrittura
)
GO

	CREATE INDEX [I_CreationDate]								ON [CmsRoles_Acl]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsRoles_Acl]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsRoles_Acl]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsRoles_Acl]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsRoles_Acl]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsRoles]								ON [CmsRoles_Acl]([Uid_CmsRoles])
	CREATE INDEX [I_Uid_CmsSubSections]							ON [CmsRoles_Acl]([Uid_CmsSubSections])
	CREATE INDEX [I_CanRead]									ON [CmsRoles_Acl]([CanRead])
	CREATE INDEX [I_CanWrite]									ON [CmsRoles_Acl]([CanWrite])
GO


-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsRepository]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsRepository] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsNlsContext]											[INT]					REFERENCES CmsNlsContext on DELETE SET NULL,	-- uid CmsNlsContext
	--
	[Folder]													[VARCHAR] (255)			NOT NULL,
	[SharedFlag]												[INT] 					NOT NULL DEFAULT 0,								-- 0 Privato, 1 Pubblico
	[Note]														[NTEXT]					NULL)
GO

 CREATE INDEX [I_StatusFlag]									ON [CmsRepository]([StatusFlag]) 
 CREATE UNIQUE INDEX [I_Folder]									ON [CmsRepository]([Folder]) 
 CREATE INDEX [I_SharedFlag]									ON [CmsRepository]([SharedFlag])
GO

-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsFile]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsFile] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsNlsContext]											[INT]					REFERENCES CmsNlsContext on DELETE SET NULL,	-- uid CmsNlsContext
	--
	[Uid_CmsRepository]											[INT]					NOT NULL REFERENCES CmsRepository ON DELETE CASCADE,
	[Uid_Parent]												[INT]					NULL,											-- REFERENCES CmsRepository ON DELETE CASCADE,
	[Name]														[NVARCHAR] (255)		NOT NULL,
	[FileTypeFlag]												[INT] 					NOT NULL,										-- 0 Stream, 1 Directory
	[FileSize]													[INT] 					NULL DEFAULT(0),
	[FileExtension]												[NVARCHAR] (255)		NULL,	
	[FileContentType]											[NVARCHAR] (255)		NULL,
	[FileMetaData]												[NVARCHAR] (500)		NULL,											-- json set di valore-proprieta
	[ImageIco]													[NVARCHAR] (255)		NULL,
	[ImagePrev]													[NVARCHAR] (255)		NULL,
	[Note]														[NTEXT]					NULL)
GO

 CREATE INDEX [I_StatusFlag]									ON [CmsFile]([StatusFlag])
 CREATE INDEX [Uid_CmsRepository]								ON [CmsFile]([Uid_CmsRepository])
 CREATE INDEX [Uid_Parent]										ON [CmsFile]([Uid_Parent])
 CREATE INDEX [Name]											ON [CmsFile]([Name])
 CREATE INDEX [FileTypeFlag]									ON [CmsFile]([FileTypeFlag])
 CREATE INDEX [FileSize]										ON [CmsFile]([FileSize])
 CREATE INDEX [FileExtension]									ON [CmsFile]([FileExtension])
 CREATE INDEX [FileContentType]									ON [CmsFile]([FileContentType])
 CREATE UNIQUE INDEX [SubPath]									ON [CmsFile](Uid_CmsRepository,Uid_Parent,Name)
GO


-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsLabels]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsLabels] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsNlsContext]											[INT]					REFERENCES CmsNlsContext on DELETE SET NULL,	-- CmsNlsContext
	--
	[Key]														[NVARCHAR] (150)		NOT NULL,
	[Description]												[NTEXT] 				NULL,
	[Note]														[NTEXT]					NULL
	)
GO
	CREATE INDEX [I_CreationDate]								ON [CmsLabels]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsLabels]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsLabels]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsLabels]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsLabels]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsNlsContext]							ON [CmsLabels]([Uid_CmsNlsContext])
	CREATE INDEX [I_Key]										ON [CmsLabels]([Key])
GO



-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsResources]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsResources] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsNlsContext]											[INT]					REFERENCES CmsNlsContext on DELETE SET NULL,	-- CmsNlsContext
	--
	[Key]														[NVARCHAR] (150)		NOT NULL,
	[Description]												[NVARCHAR] (4000)		NULL,
	[Note]														[NTEXT]					NULL
	)
GO
 
 	CREATE INDEX [I_CreationDate]								ON [CmsResources]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsResources]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsResources]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsResources]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsResources]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsNlsContext]							ON [CmsResources]([Uid_CmsNlsContext])
	CREATE INDEX [I_Key]										ON [CmsResources]([Key])
GO



-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [CmsRouting]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [CmsRouting] (
    [Uid]										[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]								[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]							[INT] 					NULL,											-- Uid Utente
    [UpdateDate]								[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]							[INT] 					NULL,											-- Uid Utente			
	[StatusFlag]								[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsNlsContext]							[INT]					REFERENCES CmsNlsContext on DELETE SET NULL,	-- CmsNlsContext
	--
	[NameRoute]									[NVARCHAR] (150)		NOT NULL,
	[UrlMapping]								[NVARCHAR] (1000)		NOT NULL,
	[UrlPhysicalPage]							[NVARCHAR] (255)		NULL,
	[MetaTagTitle]								[NVARCHAR] (255)		NULL,
    [MetaTagDescription]						[NVARCHAR] (1000)		NULL,
    [MetaTagKeywords]							[NVARCHAR] (1000)		NULL
	)
GO
 
  	CREATE INDEX [I_CreationDate]								ON [CmsRouting]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [CmsRouting]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [CmsRouting]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [CmsRouting]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [CmsRouting]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsNlsContext]							ON [CmsRouting]([Uid_CmsNlsContext])
	CREATE INDEX [I_NameRoute]									ON [CmsRouting]([NameRoute])
	CREATE INDEX [I_UrlMapping]									ON [CmsRouting]([UrlMapping])
	CREATE INDEX [I_UrlPhysicalPage]							ON [CmsRouting]([UrlPhysicalPage])
	CREATE INDEX [I_MetaTagTitle]								ON [CmsRouting]([MetaTagTitle])
GO


-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Struttura della tabella [GlobalParameter]
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
CREATE TABLE [GlobalParameter] (
    [Uid]														[INT] IDENTITY (1, 1)	NOT NULL PRIMARY KEY,
	[CreationDate]												[DATETIME]				NOT NULL DEFAULT(getDate()),					-- data di inserimento cms
    [Uid_CreationUser]											[INT] 					NULL,											-- Uid Utente
    [UpdateDate]												[DATETIME]				NULL,											-- data di inserimento cms
    [Uid_UpdateUser]											[INT] 					NULL,											-- Uid Utente
	[StatusFlag]												[INT]					NOT NULL DEFAULT 0,								-- [1] Attivo, [0] disattivo, [2] Cancellato
	--
	[Uid_CmsNlsContext]											[INT]					REFERENCES CmsNlsContext on DELETE SET NULL,	-- CmsNlsContext
	[Title]														[NVARCHAR] (150)		NULL,
	--
	[ImageUrl]													[NVARCHAR] (255)		NULL,
	[ImageUrl_Prev]												[NVARCHAR] (255)		NULL,
	)
GO
 
  	CREATE INDEX [I_CreationDate]								ON [GlobalParameter]([CreationDate])
	CREATE INDEX [I_Uid_CreationUser]							ON [GlobalParameter]([Uid_CreationUser])
	CREATE INDEX [I_UpdateDate]									ON [GlobalParameter]([UpdateDate])
	CREATE INDEX [I_Uid_UpdateUser]								ON [GlobalParameter]([Uid_UpdateUser])
	CREATE INDEX [I_StatusFlag]									ON [GlobalParameter]([StatusFlag])
	--
	CREATE INDEX [I_Uid_CmsNlsContext]							ON [GlobalParameter]([Uid_CmsNlsContext])
GO



SET IDENTITY_INSERT [CmsRoles] ON
GO
INSERT INTO [CmsRoles] ([Uid],[StatusFlag],[Title],[Uriname]) VALUES (1,1,'Super Admin','ADMIN'); 
INSERT INTO [CmsRoles] ([Uid],[StatusFlag],[Title],[Uriname]) VALUES (2,1,'Admin User','ADMIN_USER'); 
INSERT INTO [CmsRoles] ([Uid],[StatusFlag],[Title],[Uriname]) VALUES (3,1,'User','USER'); 
GO
SET IDENTITY_INSERT [CmsRoles] OFF
GO


-- [CmsUser]
SET IDENTITY_INSERT [CmsUsers] ON
GO
INSERT INTO [CmsUsers] ([Uid],[StatusFlag],[Name],[Surname],[Email],[Username],[Password],[Uid_CmsRoles]) VALUES (1,1,'Admin',null,null,'root','bratislava',1); 
SET IDENTITY_INSERT [CmsUsers] OFF
GO


-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------
-- Master Context
-- ----------------------------------------------------------------------------------------------------
-- ----------------------------------------------------------------------------------------------------

-- [CmsNlsContext]
SET IDENTITY_INSERT [CmsNlsContext] ON
INSERT INTO [CmsNlsContext] ([Uid], [Title], [Description],[IsoCode],[StatusFlag]) VALUES (1, 'fonesa', 'fonesa it','it-IT',1);
INSERT INTO [CmsNlsContext] ([Uid], [Title], [Description],[IsoCode],[StatusFlag]) VALUES (1, 'fonesa', 'fonesa en','en-UK',1);
SET IDENTITY_INSERT [CmsNlsContext] OFF
GO

-- [CmsRepository]
SET IDENTITY_INSERT [CmsRepository] ON
INSERT INTO [CmsRepository] ([Uid], [StatusFlag],[Folder],[Uid_CmsNlsContext]) VALUES (1, 1, 'Default Repository', 1); 
INSERT INTO [CmsRepository] ([Uid], [StatusFlag],[Folder],[Uid_CmsNlsContext]) VALUES (2, 1, 'fonesa-it', 1);
INSERT INTO [CmsRepository] ([Uid], [StatusFlag],[Folder],[Uid_CmsNlsContext]) VALUES (3, 1, 'fonesa-en', 1);
SET IDENTITY_INSERT [CmsRepository] OFF
GO

-- [CmsFile]
SET IDENTITY_INSERT [CmsFile] ON
INSERT INTO [CmsFile] ([Uid], [StatusFlag],[Uid_CmsNlsContext],[Uid_CmsRepository],[Name],[FileTypeFlag]) VALUES (1, 1, 1, 1,'default_upload',1); 
SET IDENTITY_INSERT [CmsFile] OFF
GO

-- [GlobalParameter]
INSERT INTO [GlobalParameter] ( [StatusFlag],[Uid_CmsNlsContext],[Title]) VALUES (1, 1,'fonesa'); 
GO

delete [CmsSubSections]
delete [CmsSections]

-- [CmsSections]
SET IDENTITY_INSERT [CmsSections] ON
INSERT INTO [CmsSections] ([Uid],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (1,1,'Backoffice Admin','ADMIN', '', 10000); 
INSERT INTO [CmsSections] ([Uid],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (2,1,'Etichette e Risorse','', '', 9000); 
SET IDENTITY_INSERT [CmsSections] OFF
GO

SET IDENTITY_INSERT [CmsSubSections] ON
INSERT INTO [CmsSubSections] ([Uid],[Uid_CmsSections],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (1,1,1,'Users','ADMIN', 'CmsUsers', 1); 
INSERT INTO [CmsSubSections] ([Uid],[Uid_CmsSections],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (2,1,1,'Section','ADMIN', 'CmsSections', 2); 
INSERT INTO [CmsSubSections] ([Uid],[Uid_CmsSections],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (3,1,1,'Labels','ADMIN', 'CmsLabels', 3); 
--
INSERT INTO [CmsSubSections] ([Uid],[Uid_CmsSections],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (4,2,1,'Etichette','', 'CmsLabels', 1); 
INSERT INTO [CmsSubSections] ([Uid],[Uid_CmsSections],[StatusFlag],[Title],[SectionUri], [ContentTable], [Ord]) VALUES (5,2,1,'Risorse','', 'CmsResources', 2); 
SET IDENTITY_INSERT [CmsSubSections] OFF
GO



INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_SelectCountry', 'Selezionare lingua/contesto');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.loginError', 'Errore generico');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.loginNoContext', 'Nessuna lingua/contesto');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.loginNoUser', 'Utente non trovato');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_Welcome', 'Benvenuto');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_Cms_RecordListFor.Text', 'Elenco ');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.TextBox_Search.Text', 'Cerca');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Action_Add.Text', 'Nuovo');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_Cms_RecordForPage.Text', 'Elementi per pagina');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Section_Preview.Text', 'Anteprima');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Download_Excel.Text', '<i class="dropdown-icon fa fa-file-excel-o"></i>Scarica Excel');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_ReorderRecord.Text', '<i class="fa fa-sort"></i> Riordina elementi');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_ApplyReorder.Text', '<i class="fa fa-check"></i> Applica ordinamento');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_ShowDeleted.Text', 'Mostra elementi cancellati');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Action_Enable.Text', '<i class="dropdown-icon fa fa-power-off success"></i>Abilita');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Action_Disable.Text', '<i class="dropdown-icon fa fa-power-off danger"></i>Disabilita');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Action_Delete.Text', '<i class="dropdown-icon fa fa-trash-o"></i>Elimina');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Action.Text', 'Opzioni<i class="fa fa-sort-desc"></i>');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.LinkButton_GoToPage.Text', 'Vai alla pagina');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.RangeValidator_TextBox_Pager_Find.Text', 'Pagina non valida');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_Paging_All.Text', 'Tutte');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Edit.ToolTip', 'Modifica');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Delete.ToolTip', 'Elimina');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_EnableDisable.ToolTip', 'Cambia Stato');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_ManageFiles.Text', 'Gestione Files');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_GoToSite.Text', 'Vai al sito');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_NoticeBoard.Text', 'Bacheca');

INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_Cms_RecordDetail.Text', 'Dettagli');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_Cms_LastCorrection.Text', 'Ultimo aggiornamento');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_SaveAsDraft.Text', 'Salva come bozza');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_SaveOnStage.Text', 'Salva e pubblica');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_Reset.Text', 'Cancella');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_Cancel.Text', 'Annulla');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_Related_Salva.Text', 'Salva');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_Related_SalvaContinua.Text', 'Salva e continua');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Button_Related_Annulla.Text', 'Annulla');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Related_Action_Add.Text', '<i class="dropdown-icon fa fa-plus"></i>Nuovo');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.HyperLink_Related_Action_Delete.Text', '<i class="dropdown-icon fa fa-trash-o"></i>Elimina');

INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Notify_Draft.Text', 'Record salvato come bozza');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Notify_Publish.Text', 'Record salvato e pubblicato');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Notify_Deleted.Text', 'Record eliminato');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Notify_Copied.Text', 'Record copiato');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Notify_Copied_All.Text', 'Records copiati');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Notify_Error.Text', 'Impossibile salvare il record');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.buttonUploadFast.Text', 'Carica / Seleziona da repository');
INSERT INTO [CmsLabels]  ([StatusFlag], [Uid_CmsNlsContext], [Key], [Description]) VALUES (1, null, 'Cms.Literal_AddFile.Text', 'Aggiungi file');







