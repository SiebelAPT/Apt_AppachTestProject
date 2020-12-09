CREATE TABLE [dbo].[Task]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Subject] VARCHAR(255) NOT NULL, 
    [IsComplete] bit NOT NULL DEFAULT ((0)), 
    [AssignedToId] UNIQUEIDENTIFIER NULL
)

--Foreign Key Constraint is Nt Applied as the GUID Type [AssignedToId] coud be Guid.Empty / Null
--ALTER TABLE [FamilyTaskTest001].[dbo].[Task] 
--WITH NOCHECK ADD CONSTRAINT FK_MEMBER 
--FOREIGN KEY ([AssignedToId]) REFERENCES [dbo].[Member] ([Id]);