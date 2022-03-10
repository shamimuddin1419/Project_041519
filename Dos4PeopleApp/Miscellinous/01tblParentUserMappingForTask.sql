CREATE TABLE [dbo].[tblParentUserMappingForTask](
	[UserId] [uniqueidentifier] NOT NULL,
	[ParentUserId] [uniqueidentifier] NOT NULL,
	[Level] [int] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblChildUserLevelMapping] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ParentUserId] ASC,
	[Level] ASC
)
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblParentUserMappingForTask]  WITH CHECK ADD  CONSTRAINT [FK_tblChildUserLevelMapping_tblChildUserLevelMapping] FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUser] ([UserId])
GO

ALTER TABLE [dbo].[tblParentUserMappingForTask] CHECK CONSTRAINT [FK_tblChildUserLevelMapping_tblChildUserLevelMapping]
GO

ALTER TABLE [dbo].[tblParentUserMappingForTask]  WITH CHECK ADD  CONSTRAINT [FK_tblChildUserLevelMapping_tblUser] FOREIGN KEY([ParentUserId])
REFERENCES [dbo].[tblUser] ([UserId])
GO

ALTER TABLE [dbo].[tblParentUserMappingForTask] CHECK CONSTRAINT [FK_tblChildUserLevelMapping_tblUser]
GO

ALTER TABLE [dbo].[tblParentUserMappingForTask]  WITH CHECK ADD  CONSTRAINT [FK_tblChildUserLevelMapping_tblUser1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[tblUser] ([UserId])
GO

ALTER TABLE [dbo].[tblParentUserMappingForTask] CHECK CONSTRAINT [FK_tblChildUserLevelMapping_tblUser1]
GO


