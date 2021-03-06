USE [barDB]
GO
/****** Object:  Table [dbo].[Cocktails]    Script Date: 09/05/2020 11:27:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cocktails](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[price_to_sell] [float] NOT NULL,
	[cost] [float] NOT NULL,
	[cocktailCategory] [varchar](50) NULL,
 CONSTRAINT [PK_Cocktails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CocktailsComposition]    Script Date: 09/05/2020 11:27:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CocktailsComposition](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[drink_id] [bigint] NOT NULL,
	[cocktail_id] [bigint] NOT NULL,
	[quantity] [bigint] NOT NULL,
 CONSTRAINT [PK_CocktailsComposition] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Drinks]    Script Date: 09/05/2020 11:27:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drinks](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[brand] [varchar](50) NULL,
	[category] [varchar](50) NULL,
 CONSTRAINT [PK_Drinks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stocks]    Script Date: 09/05/2020 11:27:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stocks](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[drink_id] [bigint] NOT NULL,
	[quantity] [bigint] NULL,
	[price] [float] NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 09/05/2020 11:27:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[sell_date] [datetime] NOT NULL,
	[value] [float] NOT NULL,
	[cocktail_id] [bigint] NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cocktails] ON 

INSERT [dbo].[Cocktails] ([id], [name], [price_to_sell], [cost], [cocktailCategory]) VALUES (1, N'Pastis Américain', 5, 0.98, N'Cocktail')
INSERT [dbo].[Cocktails] ([id], [name], [price_to_sell], [cost], [cocktailCategory]) VALUES (2, N'Coca', 5, 0.66, N'Soft')
INSERT [dbo].[Cocktails] ([id], [name], [price_to_sell], [cost], [cocktailCategory]) VALUES (3, N'Verre de vin rouge', 8, 2.75, N'Alcool')
SET IDENTITY_INSERT [dbo].[Cocktails] OFF
SET IDENTITY_INSERT [dbo].[CocktailsComposition] ON 

INSERT [dbo].[CocktailsComposition] ([id], [drink_id], [cocktail_id], [quantity]) VALUES (1, 2, 1, 3)
INSERT [dbo].[CocktailsComposition] ([id], [drink_id], [cocktail_id], [quantity]) VALUES (2, 1, 1, 25)
INSERT [dbo].[CocktailsComposition] ([id], [drink_id], [cocktail_id], [quantity]) VALUES (3, 1, 2, 33)
INSERT [dbo].[CocktailsComposition] ([id], [drink_id], [cocktail_id], [quantity]) VALUES (4, 3, 3, 25)
SET IDENTITY_INSERT [dbo].[CocktailsComposition] OFF
SET IDENTITY_INSERT [dbo].[Drinks] ON 

INSERT [dbo].[Drinks] ([id], [name], [brand], [category]) VALUES (1, N'Cola', N'Coca Cola', N'Soft')
INSERT [dbo].[Drinks] ([id], [name], [brand], [category]) VALUES (2, N'Pastis', N'Ricard', N'Alcool')
INSERT [dbo].[Drinks] ([id], [name], [brand], [category]) VALUES (3, N'Vin rouge', N'Bordeaux', N'Alcool')
SET IDENTITY_INSERT [dbo].[Drinks] OFF
SET IDENTITY_INSERT [dbo].[Stocks] ON 

INSERT [dbo].[Stocks] ([id], [drink_id], [quantity], [price]) VALUES (1, 1, 660, 0.02)
INSERT [dbo].[Stocks] ([id], [drink_id], [quantity], [price]) VALUES (2, 2, 600, 0.16)
INSERT [dbo].[Stocks] ([id], [drink_id], [quantity], [price]) VALUES (3, 3, 900, 0.11)
SET IDENTITY_INSERT [dbo].[Stocks] OFF
SET IDENTITY_INSERT [dbo].[Transactions] ON 

INSERT [dbo].[Transactions] ([id], [sell_date], [value], [cocktail_id]) VALUES (1, CAST(N'2020-05-09T11:22:01.713' AS DateTime), -16, NULL)
INSERT [dbo].[Transactions] ([id], [sell_date], [value], [cocktail_id]) VALUES (2, CAST(N'2020-05-09T11:22:10.943' AS DateTime), -96, NULL)
INSERT [dbo].[Transactions] ([id], [sell_date], [value], [cocktail_id]) VALUES (3, CAST(N'2020-05-09T11:22:29.353' AS DateTime), -96, NULL)
SET IDENTITY_INSERT [dbo].[Transactions] OFF
ALTER TABLE [dbo].[CocktailsComposition]  WITH CHECK ADD  CONSTRAINT [FK_CocktailsComposition_Cocktails] FOREIGN KEY([cocktail_id])
REFERENCES [dbo].[Cocktails] ([id])
GO
ALTER TABLE [dbo].[CocktailsComposition] CHECK CONSTRAINT [FK_CocktailsComposition_Cocktails]
GO
ALTER TABLE [dbo].[CocktailsComposition]  WITH CHECK ADD  CONSTRAINT [FK_CocktailsComposition_Drinks] FOREIGN KEY([drink_id])
REFERENCES [dbo].[Drinks] ([id])
GO
ALTER TABLE [dbo].[CocktailsComposition] CHECK CONSTRAINT [FK_CocktailsComposition_Drinks]
GO
ALTER TABLE [dbo].[Stocks]  WITH CHECK ADD  CONSTRAINT [FK_Stocks_Drinks] FOREIGN KEY([drink_id])
REFERENCES [dbo].[Drinks] ([id])
GO
ALTER TABLE [dbo].[Stocks] CHECK CONSTRAINT [FK_Stocks_Drinks]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Cocktails] FOREIGN KEY([cocktail_id])
REFERENCES [dbo].[Cocktails] ([id])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Cocktails]
GO
