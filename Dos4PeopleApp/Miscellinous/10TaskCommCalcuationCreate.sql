CREATE TABLE [dbo].[tblLevelInfo](
	[LevelId] [int] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[ReferrelCommission] [decimal](10, 4) NOT NULL,
	[TaskCommission] [decimal](10, 4) NOT NULL,
	[LimitQtyReferrel] [int] NOT NULL,
	[LimitQtyTask] [int] NOT NULL,
 CONSTRAINT [PK_tblPackageLevelInfo] PRIMARY KEY CLUSTERED 
(
	[LevelId] ASC
)) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserLevelCount]    Script Date: 1/28/2022 12:36:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserLevelCount](
	[UserId] [uniqueidentifier] NOT NULL,
	[Level] [int] NOT NULL,
	[ReferrelCount] [int] NOT NULL,
	[TaskCount] [int] NOT NULL,
 CONSTRAINT [PK_tblUserLevelCount] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[Level] ASC
)) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblLevelInfo] ADD  CONSTRAINT [DF_tblPackageLevelInfo_ReferrelCommission]  DEFAULT ((0)) FOR [ReferrelCommission]
GO
ALTER TABLE [dbo].[tblLevelInfo] ADD  CONSTRAINT [DF_Table_1_TaskCommissino]  DEFAULT ((0)) FOR [TaskCommission]
GO
ALTER TABLE [dbo].[tblUserLevelCount] ADD  CONSTRAINT [DF_tblUserLevelCount_ReferrelCount]  DEFAULT ((0)) FOR [ReferrelCount]
GO
ALTER TABLE [dbo].[tblUserLevelCount] ADD  CONSTRAINT [DF_tblUserLevelCount_TaskCount]  DEFAULT ((0)) FOR [TaskCount]
GO