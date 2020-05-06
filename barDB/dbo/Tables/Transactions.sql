CREATE TABLE [dbo].[Transactions] (
    [id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [sell_date]   DATETIME   NOT NULL,
    [value]       FLOAT (53) NOT NULL,
    [cocktail_id] BIGINT     NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Transactions_Cocktails] FOREIGN KEY ([cocktail_id]) REFERENCES [dbo].[Cocktails] ([id])
);



