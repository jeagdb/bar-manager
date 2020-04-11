USE [barDB]
GO
/****** Object:  Table [dbo].[Cocktails]    Script Date: 11/04/2020 18:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cocktails](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[price_to_sell] [float] NOT NULL,
	[cost] [float] NOT NULL,
 CONSTRAINT [PK_Cocktails] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cocktails_Composition]    Script Date: 11/04/2020 18:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cocktails_Composition](
	[cocktail_id] [bigint] NOT NULL,
	[drink_id] [bigint] NOT NULL,
	[quantity] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Drinks]    Script Date: 11/04/2020 18:46:30 ******/
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
/****** Object:  Table [dbo].[Stocks]    Script Date: 11/04/2020 18:46:30 ******/
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
/****** Object:  Table [dbo].[Transactions]    Script Date: 11/04/2020 18:46:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[sell_date] [datetime] NOT NULL,
	[value] [float] NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cocktails_Composition]  WITH CHECK ADD  CONSTRAINT [FK_Cocktails_Composition_Cocktails] FOREIGN KEY([cocktail_id])
REFERENCES [dbo].[Cocktails] ([id])
GO
ALTER TABLE [dbo].[Cocktails_Composition] CHECK CONSTRAINT [FK_Cocktails_Composition_Cocktails]
GO
ALTER TABLE [dbo].[Cocktails_Composition]  WITH CHECK ADD  CONSTRAINT [FK_Cocktails_Composition_Drinks] FOREIGN KEY([drink_id])
REFERENCES [dbo].[Drinks] ([id])
GO
ALTER TABLE [dbo].[Cocktails_Composition] CHECK CONSTRAINT [FK_Cocktails_Composition_Drinks]
GO
ALTER TABLE [dbo].[Stocks]  WITH CHECK ADD  CONSTRAINT [FK_Stocks_Drinks] FOREIGN KEY([drink_id])
REFERENCES [dbo].[Drinks] ([id])
GO
ALTER TABLE [dbo].[Stocks] CHECK CONSTRAINT [FK_Stocks_Drinks]
GO
