CREATE TABLE Children (Id TEXT NOT NULL PRIMARY KEY, Nickname TEXT NOT NULL, AvatarId INTEGER NOT NULL,
  CreatedAt TEXT NOT NULL, LastSeenAt TEXT NOT NULL);
CREATE TABLE Sessions (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ClientSessionId TEXT NOT NULL, ChildId TEXT NOT NULL,
  Game TEXT NOT NULL, StartedAt TEXT NOT NULL, EndedAt TEXT NOT NULL, DifficultyLevel INTEGER NOT NULL,
  QuestionCount INTEGER NOT NULL, PerfectCount INTEGER NOT NULL,
  CONSTRAINT CK_Sessions_Game CHECK (Game IN ('sound', 'shadow', 'food')),
  CONSTRAINT CK_Sessions_Level CHECK (DifficultyLevel BETWEEN 1 AND 3),
  CONSTRAINT CK_Sessions_Time CHECK (EndedAt >= StartedAt),
  CONSTRAINT FK_Sessions_Children_ChildId FOREIGN KEY (ChildId) REFERENCES Children (Id) ON DELETE CASCADE);
CREATE TABLE Answers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, PlaySessionId INTEGER NOT NULL, AnimalId TEXT NOT NULL,
  ChosenId TEXT NOT NULL, Correct INTEGER NOT NULL, FirstTry INTEGER NOT NULL, Seconds REAL NOT NULL,
  CONSTRAINT FK_Answers_Sessions_PlaySessionId FOREIGN KEY (PlaySessionId) REFERENCES Sessions (Id) ON DELETE CASCADE);
CREATE TABLE Masteries (ChildId TEXT NOT NULL, AnimalId TEXT NOT NULL, Score REAL NOT NULL, UpdatedAt TEXT NOT NULL,
  CONSTRAINT PK_Masteries PRIMARY KEY (ChildId, AnimalId),
  CONSTRAINT CK_Masteries_Score CHECK (Score >= 0 AND Score <= 1),
  CONSTRAINT FK_Masteries_Children_ChildId FOREIGN KEY (ChildId) REFERENCES Children (Id) ON DELETE CASCADE);
CREATE UNIQUE INDEX IX_Sessions_ClientSessionId ON Sessions (ClientSessionId);
CREATE INDEX IX_Sessions_ChildId_StartedAt ON Sessions (ChildId, StartedAt);
CREATE INDEX IX_Answers_PlaySessionId ON Answers (PlaySessionId);
