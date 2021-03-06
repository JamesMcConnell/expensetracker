USE [ExpenseTracker]
GO
/****** Object:  Table [dbo].[PaycheckBudget]    Script Date: 10/31/2012 13:58:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PaycheckBudget]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PaycheckBudget](
	[PaycheckBudgetId] [int] IDENTITY(1,1) NOT NULL,
	[PaycheckBudgetDate] [datetime] NOT NULL,
	[PaycheckBudgetAmount] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_PaycheckBudget] PRIMARY KEY CLUSTERED 
(
	[PaycheckBudgetId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[PaycheckBudget] ON
INSERT [dbo].[PaycheckBudget] ([PaycheckBudgetId], [PaycheckBudgetDate], [PaycheckBudgetAmount]) VALUES (1, CAST(0x0000A10400000000 AS DateTime), CAST(2315 AS Decimal(18, 0)))
INSERT [dbo].[PaycheckBudget] ([PaycheckBudgetId], [PaycheckBudgetDate], [PaycheckBudgetAmount]) VALUES (2, CAST(0x0000A11200000000 AS DateTime), CAST(2315 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[PaycheckBudget] OFF
/****** Object:  Table [dbo].[PaycheckBudgetItem]    Script Date: 10/31/2012 13:58:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PaycheckBudgetItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PaycheckBudgetItem](
	[PaycheckBudgetItemId] [int] IDENTITY(1,1) NOT NULL,
	[PaycheckBudgetId] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[PaidDate] [datetime] NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[IsPaid] [bit] NOT NULL,
 CONSTRAINT [PK_PaycheckBudgetItem] PRIMARY KEY CLUSTERED 
(
	[PaycheckBudgetItemId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[PaycheckBudgetItem] ON
INSERT [dbo].[PaycheckBudgetItem] ([PaycheckBudgetItemId], [PaycheckBudgetId], [Description], [PaidDate], [Amount], [IsPaid]) VALUES (1, 1, N'Rent', CAST(0x0000A10400000000 AS DateTime), CAST(400 AS Decimal(18, 0)), 0)
INSERT [dbo].[PaycheckBudgetItem] ([PaycheckBudgetItemId], [PaycheckBudgetId], [Description], [PaidDate], [Amount], [IsPaid]) VALUES (2, 1, N'Cell Phone', CAST(0x0000A10400000000 AS DateTime), CAST(156 AS Decimal(18, 0)), 0)
INSERT [dbo].[PaycheckBudgetItem] ([PaycheckBudgetItemId], [PaycheckBudgetId], [Description], [PaidDate], [Amount], [IsPaid]) VALUES (3, 2, N'Rent', CAST(0x0000A11200000000 AS DateTime), CAST(400 AS Decimal(18, 0)), 0)
INSERT [dbo].[PaycheckBudgetItem] ([PaycheckBudgetItemId], [PaycheckBudgetId], [Description], [PaidDate], [Amount], [IsPaid]) VALUES (4, 2, N'Bug''s Tuition', CAST(0x0000A11200000000 AS DateTime), CAST(250 AS Decimal(18, 0)), 0)
SET IDENTITY_INSERT [dbo].[PaycheckBudgetItem] OFF
/****** Object:  ForeignKey [FK_PaycheckBudgetItem_PaycheckBudgetItem]    Script Date: 10/31/2012 13:58:24 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PaycheckBudgetItem_PaycheckBudgetItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[PaycheckBudgetItem]'))
ALTER TABLE [dbo].[PaycheckBudgetItem]  WITH CHECK ADD  CONSTRAINT [FK_PaycheckBudgetItem_PaycheckBudgetItem] FOREIGN KEY([PaycheckBudgetId])
REFERENCES [dbo].[PaycheckBudget] ([PaycheckBudgetId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PaycheckBudgetItem_PaycheckBudgetItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[PaycheckBudgetItem]'))
ALTER TABLE [dbo].[PaycheckBudgetItem] CHECK CONSTRAINT [FK_PaycheckBudgetItem_PaycheckBudgetItem]
GO
