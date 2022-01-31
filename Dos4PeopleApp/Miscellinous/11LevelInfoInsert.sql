USE [DOSDB]
GO
SET IDENTITY_INSERT [dbo].[tblLevelInfo] ON 
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (2, 1, CAST(0.0500 AS Decimal(10, 4)), CAST(0.0100 AS Decimal(10, 4)), 10, 10)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (3, 2, CAST(0.0500 AS Decimal(10, 4)), CAST(0.0009 AS Decimal(10, 4)), 9, 9)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (4, 3, CAST(0.0400 AS Decimal(10, 4)), CAST(0.0008 AS Decimal(10, 4)), 8, 8)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (7, 4, CAST(0.0400 AS Decimal(10, 4)), CAST(0.0007 AS Decimal(10, 4)), 7, 7)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (8, 5, CAST(0.0300 AS Decimal(10, 4)), CAST(0.0006 AS Decimal(10, 4)), 6, 6)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (9, 6, CAST(0.0300 AS Decimal(10, 4)), CAST(0.0005 AS Decimal(10, 4)), 5, 5)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (10, 7, CAST(0.0200 AS Decimal(10, 4)), CAST(0.0004 AS Decimal(10, 4)), 4, 4)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (11, 8, CAST(0.0200 AS Decimal(10, 4)), CAST(0.0003 AS Decimal(10, 4)), 3, 3)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (12, 9, CAST(0.0600 AS Decimal(10, 4)), CAST(0.0002 AS Decimal(10, 4)), 2, 2)
GO
INSERT [dbo].[tblLevelInfo] ([LevelId], [Level], [ReferrelCommission], [TaskCommission], [LimitQtyReferrel], [LimitQtyTask]) VALUES (13, 10, CAST(0.0700 AS Decimal(10, 4)), CAST(0.0001 AS Decimal(10, 4)), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tblLevelInfo] OFF
GO
