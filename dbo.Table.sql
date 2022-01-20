CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(50) NULL, 
    [Count] INT NULL, 
    CONSTRAINT [PK_Table] PRIMARY KEY ([Id]) 
)
