IF DB_ID(N'NongTrai') IS NULL
    CREATE DATABASE NongTrai COLLATE Vietnamese_CI_AS;
GO

USE NongTrai;
GO

IF OBJECT_ID(N'dbo.Children', N'U') IS NULL
CREATE TABLE dbo.Children (
    Id          nvarchar(64)  NOT NULL,
    Nickname    nvarchar(32)  NOT NULL,
    AvatarId    int           NOT NULL,
    CreatedAt   datetime2     NOT NULL,
    LastSeenAt  datetime2     NOT NULL,
    CONSTRAINT PK_Children PRIMARY KEY (Id)
);
GO

IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
CREATE TABLE dbo.Sessions (
    Id               int IDENTITY(1, 1) NOT NULL,
    ClientSessionId  nvarchar(64)  NOT NULL,
    ChildId          nvarchar(64)  NOT NULL,
    Game             nvarchar(16)  NOT NULL,
    StartedAt        datetime2     NOT NULL,
    EndedAt          datetime2     NOT NULL,
    DifficultyLevel  int           NOT NULL,
    QuestionCount    int           NOT NULL,
    PerfectCount     int           NOT NULL,
    CONSTRAINT PK_Sessions PRIMARY KEY (Id),
    CONSTRAINT FK_Sessions_Children_ChildId FOREIGN KEY (ChildId)
        REFERENCES dbo.Children (Id) ON DELETE CASCADE,
    CONSTRAINT CK_Sessions_Game  CHECK (Game IN ('sound', 'shadow', 'food')),
    CONSTRAINT CK_Sessions_Level CHECK (DifficultyLevel BETWEEN 1 AND 3),
    CONSTRAINT CK_Sessions_Time  CHECK (EndedAt >= StartedAt)
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Sessions_ClientSessionId')
    CREATE UNIQUE INDEX IX_Sessions_ClientSessionId ON dbo.Sessions (ClientSessionId);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Sessions_ChildId_StartedAt')
    CREATE INDEX IX_Sessions_ChildId_StartedAt ON dbo.Sessions (ChildId, StartedAt);
GO

IF OBJECT_ID(N'dbo.Answers', N'U') IS NULL
CREATE TABLE dbo.Answers (
    Id             int IDENTITY(1, 1) NOT NULL,
    PlaySessionId  int           NOT NULL,
    AnimalId       nvarchar(64)  NOT NULL,
    ChosenId       nvarchar(64)  NOT NULL,
    Correct        bit           NOT NULL,
    FirstTry       bit           NOT NULL,
    Seconds        real          NOT NULL,
    CONSTRAINT PK_Answers PRIMARY KEY (Id),
    CONSTRAINT FK_Answers_Sessions_PlaySessionId FOREIGN KEY (PlaySessionId)
        REFERENCES dbo.Sessions (Id) ON DELETE CASCADE
);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Answers_PlaySessionId')
    CREATE INDEX IX_Answers_PlaySessionId ON dbo.Answers (PlaySessionId);
GO

IF OBJECT_ID(N'dbo.Masteries', N'U') IS NULL
CREATE TABLE dbo.Masteries (
    ChildId    nvarchar(64)  NOT NULL,
    AnimalId   nvarchar(64)  NOT NULL,
    Score      real          NOT NULL,
    UpdatedAt  datetime2     NOT NULL,
    CONSTRAINT PK_Masteries PRIMARY KEY (ChildId, AnimalId),
    CONSTRAINT FK_Masteries_Children_ChildId FOREIGN KEY (ChildId)
        REFERENCES dbo.Children (Id) ON DELETE CASCADE,
    CONSTRAINT CK_Masteries_Score CHECK (Score >= 0 AND Score <= 1)
);
GO

