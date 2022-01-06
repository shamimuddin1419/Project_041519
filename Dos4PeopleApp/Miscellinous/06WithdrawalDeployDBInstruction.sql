

--alter table tblCustomerBalance add TotalTaskCommission numeric(18,4) NULL
--alter table tblCustomerBalance add TotalReferrelCommission numeric(18,4) NULL
--alter table tblCustomerBalance add TotalTaskEarn numeric(18,4) NULL
--alter table tblCustomerBalance add TotalEarnWithdraw numeric(18,4) NULL DEFAULT 0
--alter table tblCustomerBalance add TotalCommissionWithdraw numeric(18,4) NULL DEFAULT 0


--update tblCustomerBalance
--set TotalTaskEarn = TotalBalance


--procedure DailyTaskAutoProcess --need to modify, should not give the latest sp. there is one backup from live which is updated, should give that one
--drop table [dbo].[tblWithdraw] 

--GO

--CREATE TABLE  [dbo].[tblWithdraw](
--	[WithdrawId]  int IDENTITY(1,1)  NOT NULL PRIMARY KEY,
--	[WithdrawBalanceType] char(1) NOT NULL,
--	[UserId] [uniqueidentifier] NOT NULL,
--	[PaymentMethodTypeId] int NOT NULL,
--	[PaymentDetails] nvarchar(300) NULL,
--	[WithdrawRequestBalance] [numeric](18, 4) NOT NULL,
--	[WithdrawCharge] [numeric](18, 4)  NULL,
--	[WithdrawStatus] char(1) NOT NULL,
--	[Remarks] nvarchar(300) NULL,
--	[CreatedBy] [uniqueidentifier] NOT NULL,
--	[CreatedDate] [datetime] NOT NULL,
--	[EditedBy] [uniqueidentifier] NULL,
--	[EditedDate] [datetime] NULL,
--) 
--GO


--ALTER TABLE [dbo].[tblWithdraw]  WITH CHECK ADD  CONSTRAINT [FK_tblWithdraw_tblUser] FOREIGN KEY([CreatedBy])
--REFERENCES [dbo].[tblUser] ([UserId])
--GO

--ALTER TABLE [dbo].[tblWithdraw] CHECK CONSTRAINT [FK_tblWithdraw_tblUser]
--GO

--ALTER TABLE [dbo].[tblWithdraw]  WITH CHECK ADD  CONSTRAINT [FK_tblWithdraw_tblUser1] FOREIGN KEY([EditedBy])
--REFERENCES [dbo].[tblUser] ([UserId])
--GO

--ALTER TABLE [dbo].[tblWithdraw] CHECK CONSTRAINT [FK_tblWithdraw_tblUser1]
--GO

--ALTER TABLE dbo.[tblWithdraw] WITH CHECK ADD CONSTRAINT [FK_tblWithdraw_tblPaymentMethodType] FOREIGN KEY([PaymentMethodTypeId])
--REFERENCES [dbo].[tblPaymentMethodType] ([PaymentMethodTypeId])

--GO

--ALTER TABLE [dbo].[tblWithdraw] CHECK CONSTRAINT [FK_tblWithdraw_tblPaymentMethodType]
--GO




--drop table tblWithdrawalConfig

--Create Table dbo.tblWithdrawalConfig
--(
--	WithdrawalDateConfigId int  IDENTITY(1,1) NOT NULL PRIMARY KEY,
--	[WithdrawBalanceType] char(1) NOT NULL,
--	[Day] int NOT NULL,
--	[ChargeMulti] numeric(18,4) NOT NULL DEFAULT 0,
--	[FixedCharge] numeric(18,4) NOT NULL DEFAULT 0,
--  [MinimumAmount] numeric(18,4)  NULL,
--  [MaximumAmount] numeric(18,4)  NULL
--)

--ALTER TABLE tblTransationMaster
--add WithDrawId int NULL

--GO
--ALTER TABLE dbo.tblTransationMaster WITH CHECK ADD CONSTRAINT [FK_tblTransationMaster_tblWithdraw] FOREIGN KEY([WithDrawId])
--REFERENCES [dbo].[tblWithdraw] ([WithDrawId])

--GO

--ALTER TABLE [dbo].[tblTransationMaster] CHECK CONSTRAINT [FK_tblTransationMaster_tblWithdraw]
--GO


--[dbo].[WithdrawRequest_Add]   --this is for adding request

--[dbo].[WithdrawRequest_Update] --this sp is for approve/reject

--Withdraw_Get --this sp is for getting Request Data

--[dbo].[DashboardFirstCardData_Get] --This SP is modified










