# Sudoku Solver
Nathan Ortbals, Nathaniel Banderas, Michael Ehnes

### About
Sudoku Solver is an asp.net model-view-controller application that persists Sudoku Puzzles to a Microsoft SQL Server. Sudoku Solver allows you to create an account and store multiple sudoku puzzles. Sudoku Solver makes use of Microsoft’s Orm (Entity Framework) to simplify the database – object interaction in the program. It also uses asp.net’s authentication middleware OWIN which provides a modern and secure cookie-based authentication method. The goal of the project is to provide an asp.net based web application for working on sudoku puzzles, as well as solve them if need be.

Sudoku Solver uses a constraint-satisfaction based backtracking search algorithm to solve the sudoku puzzles. This algorithm almost always runs in under a second, giving an efficient way to solve any puzzle.

### Database Schema
``` 
CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL
);

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([UserName] ASC);
    
ALTER TABLE [dbo].[AspNetUsers]
    ADD CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC);
    
CREATE TABLE [dbo].[Puzzles] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [UserID]     NVARCHAR (128) NULL,
    [LastEdited] DATETIME       NOT NULL
);

CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [dbo].[Puzzles]([UserID] ASC);

ALTER TABLE [dbo].[Puzzles]
    ADD CONSTRAINT [PK_dbo.Puzzles] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[Puzzles]
    ADD CONSTRAINT [FK_dbo.Puzzles_dbo.AspNetUsers_UserID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[AspNetUsers] ([Id]);
    
CREATE TABLE [dbo].[Positions] (
    [ID]       INT IDENTITY (1, 1) NOT NULL,
    [PuzzleId] INT NOT NULL,
    [X]        INT NOT NULL,
    [Y]        INT NOT NULL,
    [Value]    INT NULL
);

CREATE NONCLUSTERED INDEX [IX_PuzzleId]
    ON [dbo].[Positions]([PuzzleId] ASC);
    
ALTER TABLE [dbo].[Positions]
    ADD CONSTRAINT [PK_dbo.Positions] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[Positions]
    ADD CONSTRAINT [FK_dbo.Positions_dbo.Puzzles_PuzzleId] FOREIGN KEY ([PuzzleId]) REFERENCES [dbo].[Puzzles] ([ID]) ON DELETE CASCADE;
 ```
 
 ### Entity Relation Diagram
 ![Alt ERD](https://github.com/nathanortbals/SudokuSolver/blob/master/ERD.jpg)
 
