CREATE TABLE [dbo].[Stocks] (
    [id]       BIGINT     IDENTITY (1, 1) NOT NULL,
    [drink_id] BIGINT     NOT NULL,
    [quantity] BIGINT     NULL,
    [price]    FLOAT (53) NULL,
    CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Stocks_Drinks] FOREIGN KEY ([drink_id]) REFERENCES [dbo].[Drinks] ([id])
);

