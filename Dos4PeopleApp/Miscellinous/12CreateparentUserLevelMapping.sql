

CREATE TABLE [dbo].[tblParentUserLevelMapping](
	[UserId] [uniqueidentifier] NOT NULL,
	[ParentUserId] [uniqueidentifier] NOT NULL,
	[ParentUserLevel] [int] NOT NULL,
 CONSTRAINT [IX_tblParentUserLevelMapping] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[ParentUserId] ASC
)) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblParentUserLevelMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblParentUserLevelMapping_tblUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[tblUser] ([UserId])
GO

ALTER TABLE [dbo].[tblParentUserLevelMapping] CHECK CONSTRAINT [FK_tblParentUserLevelMapping_tblUser]
GO

ALTER TABLE [dbo].[tblParentUserLevelMapping]  WITH CHECK ADD  CONSTRAINT [FK_tblParentUserLevelMapping_tblUser1] FOREIGN KEY([ParentUserId])
REFERENCES [dbo].[tblUser] ([UserId])
GO

ALTER TABLE [dbo].[tblParentUserLevelMapping] CHECK CONSTRAINT [FK_tblParentUserLevelMapping_tblUser1]
GO

---------------------Transaction Master--------------------------
alter table [dbo].[tblTransationMaster] add [UserIdAsRef] [uniqueidentifier] NULL
-----------------------------------------------------------------------


