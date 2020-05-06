CREATE TABLE [dbo].[CocktailsComposition] (
    [id]          BIGINT IDENTITY (1, 1) NOT NULL,
    [drink_id]    BIGINT NOT NULL,
    [cocktail_id] BIGINT NOT NULL,
    [quantity]    BIGINT NOT NULL,
    CONSTRAINT [PK_CocktailsComposition] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_CocktailsComposition_Cocktails] FOREIGN KEY ([cocktail_id]) REFERENCES [dbo].[Cocktails] ([id]),
    CONSTRAINT [FK_CocktailsComposition_Drinks] FOREIGN KEY ([drink_id]) REFERENCES [dbo].[Drinks] ([id])
);

