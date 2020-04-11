CREATE TABLE [dbo].[Drinks] (
    [id]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [name]     VARCHAR (50) NOT NULL,
    [brand]    VARCHAR (50) NULL,
    [category] VARCHAR (50) NULL,
    CONSTRAINT [PK_Drinks] PRIMARY KEY CLUSTERED ([id] ASC)
);

