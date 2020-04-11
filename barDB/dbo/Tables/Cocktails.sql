CREATE TABLE [dbo].[Cocktails] (
    [id]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [name]          VARCHAR (50) NOT NULL,
    [price_to_sell] FLOAT (53)   NOT NULL,
    [cost]          FLOAT (53)   NOT NULL,
    CONSTRAINT [PK_Cocktails] PRIMARY KEY CLUSTERED ([id] ASC)
);

