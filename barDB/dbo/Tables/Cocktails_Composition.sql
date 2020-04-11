CREATE TABLE [dbo].[Cocktails_Composition] (
    [cocktail_id] BIGINT NOT NULL,
    [drink_id]    BIGINT NOT NULL,
    [quantity]    BIGINT NOT NULL,
    CONSTRAINT [FK_Cocktails_Composition_Cocktails] FOREIGN KEY ([cocktail_id]) REFERENCES [dbo].[Cocktails] ([id]),
    CONSTRAINT [FK_Cocktails_Composition_Drinks] FOREIGN KEY ([drink_id]) REFERENCES [dbo].[Drinks] ([id])
);

