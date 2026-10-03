USE NongTrai;
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @now datetime2 = SYSUTCDATETIME();
DECLARE @vnMidnight datetime2 = DATEADD(HOUR, -7, CAST(CAST(DATEADD(HOUR, 7, @now) AS date) AS datetime2));
DECLARE @sid int;

DELETE a FROM Answers a JOIN Sessions s ON s.Id = a.PlaySessionId WHERE s.ChildId LIKE 'demo-%';
DELETE FROM Sessions  WHERE ChildId LIKE 'demo-%';
DELETE FROM Masteries WHERE ChildId LIKE 'demo-%';
DELETE FROM Children  WHERE Id LIKE 'demo-%';

INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)
VALUES (N'demo-na', N'Na', 2, DATEADD(SECOND, -1073523, @vnMidnight), DATEADD(SECOND, -3228, @now));
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-001', N'demo-na', N'food', DATEADD(SECOND, -1073463, @vnMidnight), DATEADD(SECOND, -1073381, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'frog', N'cow', 0, 1, 2.0),
    (N'frog', N'cow', 0, 0, 4.3),
    (N'frog', N'frog', 1, 0, 5.2),
    (N'cow', N'cow', 1, 1, 6.4),
    (N'duck', N'duck', 1, 1, 3.1),
    (N'sheep', N'sheep', 1, 1, 7.0),
    (N'duck', N'duck', 1, 1, 3.8),
    (N'frog', N'chicken', 0, 1, 6.0),
    (N'frog', N'frog', 1, 0, 2.9),
    (N'duck', N'duck', 1, 1, 3.5),
    (N'cow', N'cow', 1, 1, 6.1)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-002', N'demo-na', N'sound', DATEADD(SECOND, -980330, @vnMidnight), DATEADD(SECOND, -980208, @vnMidnight), 1, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'chicken', N'chicken', 1, 1, 3.1),
    (N'pig', N'cow', 0, 1, 3.3),
    (N'pig', N'frog', 0, 0, 4.0),
    (N'pig', N'pig', 1, 0, 6.5),
    (N'cow', N'frog', 0, 1, 5.8),
    (N'cow', N'cow', 1, 0, 4.6),
    (N'cat', N'dog', 0, 1, 3.6),
    (N'cat', N'chicken', 0, 0, 5.9),
    (N'cat', N'cat', 1, 0, 5.8),
    (N'duck', N'duck', 1, 1, 2.5),
    (N'frog', N'frog', 1, 1, 6.7),
    (N'cat', N'chicken', 0, 1, 4.2),
    (N'cat', N'chicken', 0, 0, 5.7),
    (N'cat', N'chicken', 0, 0, 2.7),
    (N'cat', N'cat', 1, 0, 6.3),
    (N'pig', N'pig', 1, 1, 7.0)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-003', N'demo-na', N'shadow', DATEADD(SECOND, -977784, @vnMidnight), DATEADD(SECOND, -977679, @vnMidnight), 1, 8, 3);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'dog', 1, 1, 5.9),
    (N'sheep', N'sheep', 1, 1, 7.1),
    (N'cat', N'pig', 0, 1, 3.7),
    (N'cat', N'cat', 1, 0, 7.0),
    (N'pig', N'dog', 0, 1, 3.4),
    (N'pig', N'dog', 0, 0, 1.9),
    (N'pig', N'pig', 1, 0, 7.2),
    (N'cat', N'cat', 1, 1, 3.1),
    (N'dog', N'cow', 0, 1, 4.5),
    (N'dog', N'dog', 1, 0, 4.1),
    (N'cow', N'frog', 0, 1, 5.4),
    (N'cow', N'cow', 1, 0, 4.9),
    (N'cat', N'cow', 0, 1, 3.8),
    (N'cat', N'cat', 1, 0, 3.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-004', N'demo-na', N'shadow', DATEADD(SECOND, -883194, @vnMidnight), DATEADD(SECOND, -883114, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'pig', N'duck', 0, 1, 3.8),
    (N'pig', N'pig', 1, 0, 7.3),
    (N'chicken', N'chicken', 1, 1, 4.2),
    (N'duck', N'duck', 1, 1, 1.5),
    (N'pig', N'pig', 1, 1, 4.1),
    (N'cat', N'cat', 1, 1, 6.5),
    (N'dog', N'dog', 1, 1, 5.6),
    (N'pig', N'dog', 0, 1, 4.0),
    (N'pig', N'pig', 1, 0, 7.1),
    (N'cat', N'cat', 1, 1, 6.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-005', N'demo-na', N'food', DATEADD(SECOND, -803492, @vnMidnight), DATEADD(SECOND, -803398, @vnMidnight), 3, 8, 5);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cow', N'cat', 0, 1, 2.5),
    (N'cow', N'cow', 1, 0, 6.7),
    (N'chicken', N'chicken', 1, 1, 2.9),
    (N'duck', N'duck', 1, 1, 2.6),
    (N'sheep', N'sheep', 1, 1, 4.1),
    (N'pig', N'cat', 0, 1, 5.1),
    (N'pig', N'cow', 0, 0, 2.5),
    (N'pig', N'cow', 0, 0, 4.4),
    (N'pig', N'pig', 1, 0, 6.7),
    (N'frog', N'frog', 1, 1, 3.3),
    (N'pig', N'pig', 1, 1, 5.6),
    (N'dog', N'cat', 0, 1, 4.0),
    (N'dog', N'dog', 1, 0, 6.4)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-006', N'demo-na', N'sound', DATEADD(SECOND, -720223, @vnMidnight), DATEADD(SECOND, -720140, @vnMidnight), 2, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cat', N'dog', 0, 1, 2.3),
    (N'cat', N'cat', 1, 0, 3.9),
    (N'sheep', N'sheep', 1, 1, 3.8),
    (N'cat', N'pig', 0, 1, 1.8),
    (N'cat', N'cat', 1, 0, 3.1),
    (N'cow', N'cow', 1, 1, 4.8),
    (N'pig', N'sheep', 0, 1, 2.9),
    (N'pig', N'pig', 1, 0, 4.6),
    (N'cow', N'cow', 1, 1, 5.0),
    (N'frog', N'frog', 1, 1, 6.6),
    (N'cat', N'sheep', 0, 1, 2.8),
    (N'cat', N'cat', 1, 0, 7.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-007', N'demo-na', N'shadow', DATEADD(SECOND, -642940, @vnMidnight), DATEADD(SECOND, -642867, @vnMidnight), 3, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'chicken', N'cat', 0, 1, 2.4),
    (N'chicken', N'chicken', 1, 0, 3.1),
    (N'pig', N'pig', 1, 1, 4.3),
    (N'cow', N'cow', 1, 1, 5.4),
    (N'dog', N'dog', 1, 1, 4.6),
    (N'cat', N'cat', 1, 1, 5.8),
    (N'frog', N'frog', 1, 1, 2.0),
    (N'cat', N'cat', 1, 1, 4.9),
    (N'chicken', N'dog', 0, 1, 4.2),
    (N'chicken', N'chicken', 1, 0, 6.8)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-008', N'demo-na', N'food', DATEADD(SECOND, -558619, @vnMidnight), DATEADD(SECOND, -558516, @vnMidnight), 2, 8, 3);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cow', N'chicken', 0, 1, 1.6),
    (N'cow', N'cow', 1, 0, 3.6),
    (N'chicken', N'cow', 0, 1, 3.1),
    (N'chicken', N'duck', 0, 0, 3.0),
    (N'chicken', N'chicken', 1, 0, 5.7),
    (N'pig', N'pig', 1, 1, 3.3),
    (N'chicken', N'cat', 0, 1, 5.2),
    (N'chicken', N'chicken', 1, 0, 5.7),
    (N'cat', N'cat', 1, 1, 3.2),
    (N'cow', N'cow', 1, 1, 6.7),
    (N'chicken', N'cow', 0, 1, 3.2),
    (N'chicken', N'cow', 0, 0, 2.0),
    (N'chicken', N'chicken', 1, 0, 4.2),
    (N'dog', N'cat', 0, 1, 5.0),
    (N'dog', N'dog', 1, 0, 5.2)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-009', N'demo-na', N'shadow', DATEADD(SECOND, -553672, @vnMidnight), DATEADD(SECOND, -553587, @vnMidnight), 1, 8, 5);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'chicken', N'dog', 0, 1, 4.3),
    (N'chicken', N'pig', 0, 0, 3.1),
    (N'chicken', N'chicken', 1, 0, 2.6),
    (N'dog', N'dog', 1, 1, 3.1),
    (N'cow', N'cow', 1, 1, 2.5),
    (N'chicken', N'chicken', 1, 1, 5.2),
    (N'cow', N'cow', 1, 1, 3.8),
    (N'chicken', N'frog', 0, 1, 1.8),
    (N'chicken', N'chicken', 1, 0, 5.7),
    (N'dog', N'dog', 1, 1, 5.1),
    (N'sheep', N'cow', 0, 1, 5.6),
    (N'sheep', N'sheep', 1, 0, 7.4)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-010', N'demo-na', N'shadow', DATEADD(SECOND, -534516, @vnMidnight), DATEADD(SECOND, -534453, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'pig', N'pig', 1, 1, 1.9),
    (N'chicken', N'chicken', 1, 1, 4.1),
    (N'frog', N'frog', 1, 1, 2.4),
    (N'dog', N'dog', 1, 1, 3.5),
    (N'frog', N'frog', 1, 1, 3.3),
    (N'chicken', N'dog', 0, 1, 4.5),
    (N'chicken', N'chicken', 1, 0, 4.1),
    (N'cat', N'cat', 1, 1, 2.7),
    (N'chicken', N'duck', 0, 1, 2.1),
    (N'chicken', N'chicken', 1, 0, 4.5)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-011', N'demo-na', N'food', DATEADD(SECOND, -453573, @vnMidnight), DATEADD(SECOND, -453506, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'duck', N'duck', 1, 1, 3.2),
    (N'chicken', N'sheep', 0, 1, 4.5),
    (N'chicken', N'chicken', 1, 0, 5.7),
    (N'cat', N'cat', 1, 1, 1.4),
    (N'pig', N'pig', 1, 1, 2.8),
    (N'cow', N'cow', 1, 1, 2.5),
    (N'sheep', N'sheep', 1, 1, 6.3),
    (N'chicken', N'sheep', 0, 1, 2.7),
    (N'chicken', N'chicken', 1, 0, 6.5),
    (N'cow', N'cow', 1, 1, 1.6)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-012', N'demo-na', N'sound', DATEADD(SECOND, -402454, @vnMidnight), DATEADD(SECOND, -402404, @vnMidnight), 3, 8, 7);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'dog', 1, 1, 3.0),
    (N'cow', N'cow', 1, 1, 2.7),
    (N'duck', N'duck', 1, 1, 3.7),
    (N'cat', N'cat', 1, 1, 2.0),
    (N'frog', N'frog', 1, 1, 2.2),
    (N'chicken', N'duck', 0, 1, 3.0),
    (N'chicken', N'chicken', 1, 0, 3.2),
    (N'pig', N'pig', 1, 1, 2.1),
    (N'sheep', N'sheep', 1, 1, 1.5)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-013', N'demo-na', N'shadow', DATEADD(SECOND, -390902, @vnMidnight), DATEADD(SECOND, -390850, @vnMidnight), 3, 8, 8);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cow', N'cow', 1, 1, 3.2),
    (N'chicken', N'chicken', 1, 1, 5.0),
    (N'pig', N'pig', 1, 1, 2.3),
    (N'cat', N'cat', 1, 1, 3.7),
    (N'frog', N'frog', 1, 1, 3.6),
    (N'sheep', N'sheep', 1, 1, 2.4),
    (N'cow', N'cow', 1, 1, 3.5),
    (N'pig', N'pig', 1, 1, 3.4)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-014', N'demo-na', N'shadow', DATEADD(SECOND, -367879, @vnMidnight), DATEADD(SECOND, -367794, @vnMidnight), 2, 8, 5);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'chicken', N'duck', 0, 1, 4.8),
    (N'chicken', N'chicken', 1, 0, 6.0),
    (N'dog', N'cat', 0, 1, 5.1),
    (N'dog', N'frog', 0, 0, 5.4),
    (N'dog', N'dog', 1, 0, 7.2),
    (N'chicken', N'pig', 0, 1, 2.1),
    (N'chicken', N'chicken', 1, 0, 2.8),
    (N'dog', N'dog', 1, 1, 4.9),
    (N'chicken', N'chicken', 1, 1, 4.3),
    (N'pig', N'pig', 1, 1, 1.8),
    (N'chicken', N'chicken', 1, 1, 2.5),
    (N'cat', N'cat', 1, 1, 3.4)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-015', N'demo-na', N'food', DATEADD(SECOND, -314960, @vnMidnight), DATEADD(SECOND, -314884, @vnMidnight), 2, 8, 5);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'dog', 1, 1, 2.4),
    (N'sheep', N'sheep', 1, 1, 3.7),
    (N'dog', N'cat', 0, 1, 3.4),
    (N'dog', N'sheep', 0, 0, 4.7),
    (N'dog', N'dog', 1, 0, 4.7),
    (N'frog', N'frog', 1, 1, 1.9),
    (N'dog', N'chicken', 0, 1, 4.3),
    (N'dog', N'dog', 1, 0, 3.5),
    (N'chicken', N'chicken', 1, 1, 1.7),
    (N'sheep', N'sheep', 1, 1, 2.3),
    (N'cat', N'dog', 0, 1, 5.3),
    (N'cat', N'cat', 1, 0, 3.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-016', N'demo-na', N'food', DATEADD(SECOND, -306065, @vnMidnight), DATEADD(SECOND, -306011, @vnMidnight), 3, 8, 8);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'dog', 1, 1, 5.7),
    (N'cat', N'cat', 1, 1, 3.2),
    (N'frog', N'frog', 1, 1, 2.7),
    (N'pig', N'pig', 1, 1, 3.4),
    (N'dog', N'dog', 1, 1, 5.1),
    (N'chicken', N'chicken', 1, 1, 3.3),
    (N'frog', N'frog', 1, 1, 3.6),
    (N'cat', N'cat', 1, 1, 2.7)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-017', N'demo-na', N'food', DATEADD(SECOND, -229205, @vnMidnight), DATEADD(SECOND, -229160, @vnMidnight), 3, 8, 8);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'frog', N'frog', 1, 1, 3.7),
    (N'sheep', N'sheep', 1, 1, 2.5),
    (N'dog', N'dog', 1, 1, 2.6),
    (N'sheep', N'sheep', 1, 1, 2.8),
    (N'dog', N'dog', 1, 1, 2.3),
    (N'duck', N'duck', 1, 1, 1.5),
    (N'chicken', N'chicken', 1, 1, 2.7),
    (N'dog', N'dog', 1, 1, 2.8)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-018', N'demo-na', N'food', DATEADD(SECOND, -227373, @vnMidnight), DATEADD(SECOND, -227316, @vnMidnight), 3, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cat', N'cat', 1, 1, 3.3),
    (N'pig', N'pig', 1, 1, 1.7),
    (N'cat', N'dog', 0, 1, 2.6),
    (N'cat', N'cat', 1, 0, 1.8),
    (N'chicken', N'chicken', 1, 1, 3.6),
    (N'cat', N'cat', 1, 1, 2.5),
    (N'cow', N'sheep', 0, 1, 3.8),
    (N'cow', N'cow', 1, 0, 3.7),
    (N'cat', N'cat', 1, 1, 2.1),
    (N'dog', N'dog', 1, 1, 2.0)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-019', N'demo-na', N'food', DATEADD(SECOND, -133017, @vnMidnight), DATEADD(SECOND, -132957, @vnMidnight), 3, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cow', N'cow', 1, 1, 1.5),
    (N'dog', N'dog', 1, 1, 2.0),
    (N'cat', N'cat', 1, 1, 3.0),
    (N'chicken', N'chicken', 1, 1, 2.6),
    (N'duck', N'duck', 1, 1, 1.4),
    (N'cat', N'duck', 0, 1, 5.0),
    (N'cat', N'dog', 0, 0, 2.2),
    (N'cat', N'cat', 1, 0, 1.7),
    (N'dog', N'cat', 0, 1, 3.7),
    (N'dog', N'dog', 1, 0, 2.6),
    (N'cat', N'cat', 1, 1, 2.0)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-020', N'demo-na', N'sound', DATEADD(SECOND, -108183, @vnMidnight), DATEADD(SECOND, -108122, @vnMidnight), 3, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'chicken', N'chicken', 1, 1, 2.8),
    (N'frog', N'frog', 1, 1, 1.5),
    (N'cow', N'cow', 1, 1, 2.0),
    (N'dog', N'cat', 0, 1, 5.7),
    (N'dog', N'dog', 1, 0, 2.5),
    (N'pig', N'pig', 1, 1, 2.9),
    (N'frog', N'frog', 1, 1, 1.7),
    (N'dog', N'cow', 0, 1, 5.3),
    (N'dog', N'dog', 1, 0, 4.7),
    (N'frog', N'frog', 1, 1, 2.4)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-021', N'demo-na', N'shadow', DATEADD(SECOND, -108123, @vnMidnight), DATEADD(SECOND, -108019, @vnMidnight), 2, 8, 5);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cow', N'cow', 1, 1, 1.5),
    (N'cat', N'dog', 0, 1, 4.9),
    (N'cat', N'frog', 0, 0, 4.5),
    (N'cat', N'pig', 0, 0, 2.5),
    (N'cat', N'cat', 1, 0, 5.7),
    (N'dog', N'dog', 1, 1, 5.2),
    (N'frog', N'frog', 1, 1, 2.5),
    (N'cat', N'chicken', 0, 1, 5.2),
    (N'cat', N'cat', 1, 0, 4.7),
    (N'duck', N'duck', 1, 1, 3.0),
    (N'dog', N'cat', 0, 1, 3.8),
    (N'dog', N'chicken', 0, 0, 2.5),
    (N'dog', N'frog', 0, 0, 5.2),
    (N'dog', N'frog', 0, 0, 3.1),
    (N'dog', N'dog', 1, 0, 2.6),
    (N'chicken', N'chicken', 1, 1, 2.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-022', N'demo-na', N'food', DATEADD(SECOND, -20952, @vnMidnight), DATEADD(SECOND, -20892, @vnMidnight), 3, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'frog', 0, 1, 1.6),
    (N'dog', N'dog', 1, 0, 5.0),
    (N'cat', N'cat', 1, 1, 3.4),
    (N'dog', N'dog', 1, 1, 4.5),
    (N'chicken', N'chicken', 1, 1, 3.5),
    (N'cat', N'cat', 1, 1, 1.4),
    (N'sheep', N'sheep', 1, 1, 2.4),
    (N'dog', N'cat', 0, 1, 3.0),
    (N'dog', N'dog', 1, 0, 3.7),
    (N'chicken', N'chicken', 1, 1, 2.4)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-na-023', N'demo-na', N'sound', DATEADD(SECOND, -3293, @now), DATEADD(SECOND, -3228, @now), 3, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'duck', N'duck', 1, 1, 1.5),
    (N'dog', N'cat', 0, 1, 3.3),
    (N'dog', N'dog', 1, 0, 6.1),
    (N'duck', N'duck', 1, 1, 1.8),
    (N'cow', N'cow', 1, 1, 2.0),
    (N'duck', N'duck', 1, 1, 3.2),
    (N'dog', N'chicken', 0, 1, 4.3),
    (N'dog', N'dog', 1, 0, 5.6),
    (N'pig', N'pig', 1, 1, 2.7),
    (N'dog', N'dog', 1, 1, 4.8)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES
    (N'demo-na', N'cat', 0.6824, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'dog', 0.2526, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'chicken', 0.9901, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'duck', 0.9976, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'pig', 0.9942, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'cow', 0.9257, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'sheep', 0.9765, DATEADD(SECOND, -3228, @now)),
    (N'demo-na', N'frog', 0.9985, DATEADD(SECOND, -3228, @now));

INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)
VALUES (N'demo-bin', N'Bin', 5, DATEADD(SECOND, -1080833, @vnMidnight), DATEADD(SECOND, -9774, @now));
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-001', N'demo-bin', N'food', DATEADD(SECOND, -1080773, @vnMidnight), DATEADD(SECOND, -1080669, @vnMidnight), 1, 8, 2);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'frog', N'cow', 0, 1, 2.2),
    (N'frog', N'cow', 0, 0, 2.9),
    (N'frog', N'frog', 1, 0, 3.5),
    (N'pig', N'pig', 1, 1, 7.1),
    (N'cow', N'sheep', 0, 1, 4.4),
    (N'cow', N'cow', 1, 0, 5.9),
    (N'frog', N'sheep', 0, 1, 3.4),
    (N'frog', N'frog', 1, 0, 5.3),
    (N'chicken', N'chicken', 1, 1, 6.2),
    (N'sheep', N'frog', 0, 1, 3.1),
    (N'sheep', N'sheep', 1, 0, 2.6),
    (N'cow', N'pig', 0, 1, 5.1),
    (N'cow', N'cow', 1, 0, 4.7),
    (N'chicken', N'frog', 0, 1, 2.7),
    (N'chicken', N'chicken', 1, 0, 2.9)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-002', N'demo-bin', N'sound', DATEADD(SECOND, -1077322, @vnMidnight), DATEADD(SECOND, -1077227, @vnMidnight), 1, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cat', N'duck', 0, 1, 5.8),
    (N'cat', N'cat', 1, 0, 5.6),
    (N'pig', N'pig', 1, 1, 3.1),
    (N'frog', N'frog', 1, 1, 7.5),
    (N'sheep', N'frog', 0, 1, 6.0),
    (N'sheep', N'sheep', 1, 0, 4.7),
    (N'cow', N'cow', 1, 1, 6.4),
    (N'dog', N'duck', 0, 1, 5.3),
    (N'dog', N'dog', 1, 0, 2.8),
    (N'duck', N'chicken', 0, 1, 5.9),
    (N'duck', N'duck', 1, 0, 2.7),
    (N'chicken', N'chicken', 1, 1, 4.9)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-003', N'demo-bin', N'shadow', DATEADD(SECOND, -921546, @vnMidnight), DATEADD(SECOND, -921467, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'sheep', N'cat', 0, 1, 3.4),
    (N'sheep', N'cat', 0, 0, 3.6),
    (N'sheep', N'sheep', 1, 0, 3.3),
    (N'cow', N'cow', 1, 1, 3.0),
    (N'duck', N'duck', 1, 1, 7.3),
    (N'frog', N'frog', 1, 1, 5.9),
    (N'dog', N'dog', 1, 1, 3.8),
    (N'chicken', N'cow', 0, 1, 1.8),
    (N'chicken', N'chicken', 1, 0, 4.2),
    (N'sheep', N'sheep', 1, 1, 5.3),
    (N'pig', N'pig', 1, 1, 4.9)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-004', N'demo-bin', N'sound', DATEADD(SECOND, -822902, @vnMidnight), DATEADD(SECOND, -822816, @vnMidnight), 1, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'frog', N'cat', 0, 1, 4.2),
    (N'frog', N'frog', 1, 0, 6.5),
    (N'cat', N'duck', 0, 1, 2.5),
    (N'cat', N'cat', 1, 0, 7.1),
    (N'frog', N'frog', 1, 1, 6.9),
    (N'sheep', N'sheep', 1, 1, 4.0),
    (N'dog', N'sheep', 0, 1, 1.8),
    (N'dog', N'dog', 1, 0, 3.2),
    (N'frog', N'frog', 1, 1, 2.8),
    (N'pig', N'cat', 0, 1, 4.4),
    (N'pig', N'pig', 1, 0, 4.1),
    (N'cat', N'cat', 1, 1, 3.8)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-005', N'demo-bin', N'food', DATEADD(SECOND, -640489, @vnMidnight), DATEADD(SECOND, -640387, @vnMidnight), 1, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'pig', N'pig', 1, 1, 5.9),
    (N'cow', N'cow', 1, 1, 4.6),
    (N'duck', N'chicken', 0, 1, 2.4),
    (N'duck', N'duck', 1, 0, 5.2),
    (N'cow', N'sheep', 0, 1, 4.9),
    (N'cow', N'cow', 1, 0, 5.9),
    (N'duck', N'duck', 1, 1, 6.8),
    (N'chicken', N'cat', 0, 1, 4.4),
    (N'chicken', N'cat', 0, 0, 2.7),
    (N'chicken', N'chicken', 1, 0, 5.7),
    (N'dog', N'dog', 1, 1, 5.9),
    (N'cat', N'sheep', 0, 1, 3.6),
    (N'cat', N'cat', 1, 0, 6.5)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-006', N'demo-bin', N'food', DATEADD(SECOND, -626971, @vnMidnight), DATEADD(SECOND, -626856, @vnMidnight), 1, 8, 2);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'chicken', 0, 1, 2.9),
    (N'dog', N'dog', 1, 0, 3.9),
    (N'sheep', N'sheep', 1, 1, 2.9),
    (N'chicken', N'duck', 0, 1, 4.2),
    (N'chicken', N'chicken', 1, 0, 4.1),
    (N'cow', N'sheep', 0, 1, 4.8),
    (N'cow', N'sheep', 0, 0, 4.8),
    (N'cow', N'cow', 1, 0, 3.4),
    (N'pig', N'dog', 0, 1, 4.3),
    (N'pig', N'dog', 0, 0, 1.7),
    (N'pig', N'dog', 0, 0, 4.7),
    (N'pig', N'pig', 1, 0, 5.7),
    (N'dog', N'dog', 1, 1, 6.0),
    (N'cat', N'pig', 0, 1, 3.8),
    (N'cat', N'cat', 1, 0, 3.1),
    (N'cow', N'cat', 0, 1, 3.8),
    (N'cow', N'cow', 1, 0, 4.1)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-007', N'demo-bin', N'food', DATEADD(SECOND, -474798, @vnMidnight), DATEADD(SECOND, -474701, @vnMidnight), 1, 8, 3);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'pig', N'cat', 0, 1, 5.6),
    (N'pig', N'pig', 1, 0, 3.9),
    (N'frog', N'sheep', 0, 1, 4.8),
    (N'frog', N'frog', 1, 0, 3.8),
    (N'cat', N'pig', 0, 1, 4.3),
    (N'cat', N'cat', 1, 0, 6.0),
    (N'cow', N'cow', 1, 1, 6.9),
    (N'frog', N'cat', 0, 1, 4.4),
    (N'frog', N'frog', 1, 0, 2.8),
    (N'cat', N'dog', 0, 1, 1.7),
    (N'cat', N'cat', 1, 0, 7.4),
    (N'dog', N'dog', 1, 1, 2.7),
    (N'pig', N'pig', 1, 1, 6.0)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-008', N'demo-bin', N'food', DATEADD(SECOND, -472216, @vnMidnight), DATEADD(SECOND, -472136, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'sheep', N'duck', 0, 1, 3.3),
    (N'sheep', N'duck', 0, 0, 2.9),
    (N'sheep', N'sheep', 1, 0, 2.9),
    (N'dog', N'dog', 1, 1, 3.7),
    (N'cat', N'cat', 1, 1, 3.5),
    (N'frog', N'frog', 1, 1, 3.5),
    (N'duck', N'duck', 1, 1, 4.2),
    (N'chicken', N'chicken', 1, 1, 2.7),
    (N'frog', N'frog', 1, 1, 5.1),
    (N'cat', N'chicken', 0, 1, 3.5),
    (N'cat', N'cow', 0, 0, 5.0),
    (N'cat', N'cat', 1, 0, 5.6)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-009', N'demo-bin', N'food', DATEADD(SECOND, -364230, @vnMidnight), DATEADD(SECOND, -364129, @vnMidnight), 2, 8, 3);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'dog', 1, 1, 3.4),
    (N'frog', N'frog', 1, 1, 1.7),
    (N'pig', N'cat', 0, 1, 4.2),
    (N'pig', N'dog', 0, 0, 5.9),
    (N'pig', N'pig', 1, 0, 4.9),
    (N'chicken', N'duck', 0, 1, 1.8),
    (N'chicken', N'pig', 0, 0, 5.2),
    (N'chicken', N'chicken', 1, 0, 4.6),
    (N'sheep', N'cow', 0, 1, 2.4),
    (N'sheep', N'sheep', 1, 0, 3.8),
    (N'duck', N'frog', 0, 1, 2.5),
    (N'duck', N'duck', 1, 0, 3.3),
    (N'sheep', N'pig', 0, 1, 5.4),
    (N'sheep', N'sheep', 1, 0, 6.2),
    (N'pig', N'pig', 1, 1, 3.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-010', N'demo-bin', N'sound', DATEADD(SECOND, -221485, @vnMidnight), DATEADD(SECOND, -221393, @vnMidnight), 1, 8, 3);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'duck', N'sheep', 0, 1, 5.0),
    (N'duck', N'duck', 1, 0, 2.8),
    (N'frog', N'pig', 0, 1, 4.1),
    (N'frog', N'frog', 1, 0, 3.1),
    (N'cat', N'cow', 0, 1, 2.5),
    (N'cat', N'cat', 1, 0, 3.4),
    (N'cow', N'sheep', 0, 1, 2.4),
    (N'cow', N'cow', 1, 0, 6.6),
    (N'cat', N'pig', 0, 1, 4.4),
    (N'cat', N'cat', 1, 0, 6.8),
    (N'pig', N'pig', 1, 1, 5.1),
    (N'chicken', N'chicken', 1, 1, 2.6),
    (N'cat', N'cat', 1, 1, 6.3)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-011', N'demo-bin', N'food', DATEADD(SECOND, -203958, @vnMidnight), DATEADD(SECOND, -203880, @vnMidnight), 2, 8, 6);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'pig', N'pig', 1, 1, 1.9),
    (N'duck', N'duck', 1, 1, 4.1),
    (N'chicken', N'chicken', 1, 1, 3.7),
    (N'duck', N'cat', 0, 1, 5.9),
    (N'duck', N'duck', 1, 0, 6.9),
    (N'sheep', N'cow', 0, 1, 5.4),
    (N'sheep', N'cat', 0, 0, 2.1),
    (N'sheep', N'sheep', 1, 0, 5.6),
    (N'cat', N'cat', 1, 1, 5.5),
    (N'cow', N'cow', 1, 1, 3.0),
    (N'chicken', N'chicken', 1, 1, 1.9)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-012', N'demo-bin', N'sound', DATEADD(SECOND, -48119, @vnMidnight), DATEADD(SECOND, -48025, @vnMidnight), 2, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'duck', N'duck', 1, 1, 3.3),
    (N'pig', N'pig', 1, 1, 2.7),
    (N'cat', N'frog', 0, 1, 5.1),
    (N'cat', N'cat', 1, 0, 2.6),
    (N'cow', N'dog', 0, 1, 4.4),
    (N'cow', N'cow', 1, 0, 6.2),
    (N'frog', N'cow', 0, 1, 4.7),
    (N'frog', N'dog', 0, 0, 4.6),
    (N'frog', N'frog', 1, 0, 7.0),
    (N'sheep', N'cow', 0, 1, 2.8),
    (N'sheep', N'sheep', 1, 0, 3.4),
    (N'frog', N'frog', 1, 1, 3.3),
    (N'cat', N'cat', 1, 1, 7.0)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-bin-013', N'demo-bin', N'shadow', DATEADD(SECOND, -9831, @now), DATEADD(SECOND, -9774, @now), 3, 8, 7);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'duck', N'duck', 1, 1, 2.7),
    (N'sheep', N'sheep', 1, 1, 5.6),
    (N'duck', N'duck', 1, 1, 1.5),
    (N'cow', N'cow', 1, 1, 4.6),
    (N'chicken', N'chicken', 1, 1, 1.9),
    (N'cow', N'sheep', 0, 1, 3.2),
    (N'cow', N'cow', 1, 0, 5.6),
    (N'frog', N'frog', 1, 1, 3.4),
    (N'chicken', N'chicken', 1, 1, 1.5)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES
    (N'demo-bin', N'cat', 0.398, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'dog', 0.8288, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'chicken', 0.9112, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'duck', 0.7791, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'pig', 0.8249, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'cow', 0.2546, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'sheep', 0.2412, DATEADD(SECOND, -9774, @now)),
    (N'demo-bin', N'frog', 0.6892, DATEADD(SECOND, -9774, @now));

INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)
VALUES (N'demo-su', N'Su', 7, DATEADD(SECOND, -300859, @vnMidnight), DATEADD(SECOND, -7176, @now));
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-su-001', N'demo-su', N'sound', DATEADD(SECOND, -300799, @vnMidnight), DATEADD(SECOND, -300682, @vnMidnight), 1, 8, 1);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'sheep', N'duck', 0, 1, 5.6),
    (N'sheep', N'sheep', 1, 0, 5.0),
    (N'frog', N'frog', 1, 1, 6.1),
    (N'duck', N'pig', 0, 1, 2.3),
    (N'duck', N'duck', 1, 0, 3.4),
    (N'dog', N'cow', 0, 1, 5.5),
    (N'dog', N'dog', 1, 0, 3.7),
    (N'frog', N'cow', 0, 1, 1.9),
    (N'frog', N'frog', 1, 0, 5.3),
    (N'pig', N'duck', 0, 1, 1.6),
    (N'pig', N'pig', 1, 0, 6.6),
    (N'dog', N'cat', 0, 1, 1.6),
    (N'dog', N'cat', 0, 0, 3.4),
    (N'dog', N'cat', 0, 0, 3.8),
    (N'dog', N'dog', 1, 0, 5.0),
    (N'cat', N'sheep', 0, 1, 5.9),
    (N'cat', N'cat', 1, 0, 3.8)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-su-002', N'demo-su', N'sound', DATEADD(SECOND, -281451, @vnMidnight), DATEADD(SECOND, -281356, @vnMidnight), 1, 8, 2);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'sheep', N'frog', 0, 1, 3.4),
    (N'sheep', N'sheep', 1, 0, 3.3),
    (N'dog', N'pig', 0, 1, 5.6),
    (N'dog', N'dog', 1, 0, 2.9),
    (N'cat', N'duck', 0, 1, 2.5),
    (N'cat', N'cat', 1, 0, 5.7),
    (N'cow', N'sheep', 0, 1, 2.1),
    (N'cow', N'cow', 1, 0, 5.2),
    (N'pig', N'cat', 0, 1, 2.5),
    (N'pig', N'pig', 1, 0, 4.7),
    (N'chicken', N'pig', 0, 1, 2.2),
    (N'chicken', N'chicken', 1, 0, 2.9),
    (N'pig', N'pig', 1, 1, 6.9),
    (N'dog', N'dog', 1, 1, 5.8)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-su-003', N'demo-su', N'food', DATEADD(SECOND, -188569, @vnMidnight), DATEADD(SECOND, -188482, @vnMidnight), 2, 8, 5);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'sheep', N'sheep', 1, 1, 3.4),
    (N'duck', N'duck', 1, 1, 7.3),
    (N'pig', N'pig', 1, 1, 3.0),
    (N'cow', N'dog', 0, 1, 3.4),
    (N'cow', N'cow', 1, 0, 7.0),
    (N'dog', N'dog', 1, 1, 4.4),
    (N'frog', N'frog', 1, 1, 7.0),
    (N'chicken', N'duck', 0, 1, 3.0),
    (N'chicken', N'chicken', 1, 0, 5.7),
    (N'cow', N'chicken', 0, 1, 3.9),
    (N'cow', N'cow', 1, 0, 6.5)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-su-004', N'demo-su', N'shadow', DATEADD(SECOND, -33164, @vnMidnight), DATEADD(SECOND, -33038, @vnMidnight), 1, 8, 2);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'cat', N'cat', 1, 1, 6.1),
    (N'chicken', N'pig', 0, 1, 5.6),
    (N'chicken', N'pig', 0, 0, 5.0),
    (N'chicken', N'chicken', 1, 0, 7.4),
    (N'cat', N'dog', 0, 1, 5.8),
    (N'cat', N'dog', 0, 0, 2.9),
    (N'cat', N'cat', 1, 0, 7.5),
    (N'frog', N'chicken', 0, 1, 2.6),
    (N'frog', N'frog', 1, 0, 2.7),
    (N'dog', N'duck', 0, 1, 4.9),
    (N'dog', N'duck', 0, 0, 5.3),
    (N'dog', N'dog', 1, 0, 5.5),
    (N'chicken', N'cat', 0, 1, 2.3),
    (N'chicken', N'chicken', 1, 0, 3.6),
    (N'duck', N'cow', 0, 1, 4.0),
    (N'duck', N'duck', 1, 0, 4.1),
    (N'frog', N'frog', 1, 1, 4.0)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES (N'demo-su-005', N'demo-su', N'food', DATEADD(SECOND, -7261, @now), DATEADD(SECOND, -7176, @now), 1, 8, 4);
SET @sid = SCOPE_IDENTITY();
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)
SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES
    (N'dog', N'cow', 0, 1, 4.8),
    (N'dog', N'dog', 1, 0, 6.8),
    (N'frog', N'cow', 0, 1, 2.1),
    (N'frog', N'frog', 1, 0, 3.9),
    (N'sheep', N'sheep', 1, 1, 5.1),
    (N'frog', N'frog', 1, 1, 2.6),
    (N'chicken', N'chicken', 1, 1, 5.0),
    (N'duck', N'pig', 0, 1, 2.8),
    (N'duck', N'duck', 1, 0, 5.7),
    (N'pig', N'cat', 0, 1, 4.5),
    (N'pig', N'pig', 1, 0, 5.6),
    (N'frog', N'frog', 1, 1, 1.5)
) AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);
INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES
    (N'demo-su', N'cat', 0.12, DATEADD(SECOND, -7176, @now)),
    (N'demo-su', N'dog', 0.1296, DATEADD(SECOND, -7176, @now)),
    (N'demo-su', N'chicken', 0.2, DATEADD(SECOND, -7176, @now)),
    (N'demo-su', N'duck', 0.072, DATEADD(SECOND, -7176, @now)),
    (N'demo-su', N'pig', 0.288, DATEADD(SECOND, -7176, @now)),
    (N'demo-su', N'sheep', 0.48, DATEADD(SECOND, -7176, @now)),
    (N'demo-su', N'frog', 0.6955, DATEADD(SECOND, -7176, @now));

COMMIT TRANSACTION;

SELECT c.Nickname, COUNT(s.Id) AS SoLuotChoi FROM Children c LEFT JOIN Sessions s ON s.ChildId = c.Id
WHERE c.Id LIKE 'demo-%' GROUP BY c.Nickname;
