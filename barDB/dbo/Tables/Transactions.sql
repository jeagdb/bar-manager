CREATE TABLE [dbo].[Transactions] (
    [id]        BIGINT     IDENTITY (1, 1) NOT NULL,
    [sell_date] DATETIME   NOT NULL,
    [value]     FLOAT (53) NOT NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED ([id] ASC)
);

