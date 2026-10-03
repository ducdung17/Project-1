PRAGMA foreign_keys = ON;
BEGIN TRANSACTION;

DELETE FROM Answers WHERE PlaySessionId IN (SELECT Id FROM Sessions WHERE ChildId LIKE 'demo-%');
DELETE FROM Sessions  WHERE ChildId LIKE 'demo-%';
DELETE FROM Masteries WHERE ChildId LIKE 'demo-%';
DELETE FROM Children  WHERE Id LIKE 'demo-%';

INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)
VALUES ('demo-na', 'Na', 2, datetime('now', '+7 hours', 'start of day', '-7 hours', '-1073523 seconds'), datetime('now', '-3228 seconds'));
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-001', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-1073463 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-1073381 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'frog', 'cow', 0, 1, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'frog', 'cow', 0, 0, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'frog', 'frog', 1, 0, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'cow', 'cow', 1, 1, 6.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'duck', 'duck', 1, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'sheep', 'sheep', 1, 1, 7.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'duck', 'duck', 1, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'frog', 'chicken', 0, 1, 6.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'frog', 'frog', 1, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'duck', 'duck', 1, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-001'), 'cow', 'cow', 1, 1, 6.1);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-002', 'demo-na', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-980330 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-980208 seconds'), 1, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'chicken', 'chicken', 1, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'pig', 'cow', 0, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'pig', 'frog', 0, 0, 4.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'pig', 'pig', 1, 0, 6.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cow', 'frog', 0, 1, 5.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cow', 'cow', 1, 0, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'dog', 0, 1, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'chicken', 0, 0, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'cat', 1, 0, 5.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'duck', 'duck', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'frog', 'frog', 1, 1, 6.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'chicken', 0, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'chicken', 0, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'chicken', 0, 0, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'cat', 'cat', 1, 0, 6.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-002'), 'pig', 'pig', 1, 1, 7.0);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-003', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-977784 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-977679 seconds'), 1, 8, 3);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'dog', 'dog', 1, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'sheep', 'sheep', 1, 1, 7.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cat', 'pig', 0, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cat', 'cat', 1, 0, 7.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'pig', 'dog', 0, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'pig', 'dog', 0, 0, 1.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'pig', 'pig', 1, 0, 7.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cat', 'cat', 1, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'dog', 'cow', 0, 1, 4.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'dog', 'dog', 1, 0, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cow', 'frog', 0, 1, 5.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cow', 'cow', 1, 0, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cat', 'cow', 0, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-003'), 'cat', 'cat', 1, 0, 3.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-004', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-883194 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-883114 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'pig', 'duck', 0, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'pig', 'pig', 1, 0, 7.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'chicken', 'chicken', 1, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'duck', 'duck', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'pig', 'pig', 1, 1, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'cat', 'cat', 1, 1, 6.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'dog', 'dog', 1, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'pig', 'dog', 0, 1, 4.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'pig', 'pig', 1, 0, 7.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-004'), 'cat', 'cat', 1, 1, 6.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-005', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-803492 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-803398 seconds'), 3, 8, 5);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'cow', 'cat', 0, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'cow', 'cow', 1, 0, 6.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'chicken', 'chicken', 1, 1, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'duck', 'duck', 1, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'sheep', 'sheep', 1, 1, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'pig', 'cat', 0, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'pig', 'cow', 0, 0, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'pig', 'cow', 0, 0, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'pig', 'pig', 1, 0, 6.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'frog', 'frog', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'pig', 'pig', 1, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'dog', 'cat', 0, 1, 4.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-005'), 'dog', 'dog', 1, 0, 6.4);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-006', 'demo-na', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-720223 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-720140 seconds'), 2, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cat', 'dog', 0, 1, 2.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cat', 'cat', 1, 0, 3.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'sheep', 'sheep', 1, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cat', 'pig', 0, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cat', 'cat', 1, 0, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cow', 'cow', 1, 1, 4.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'pig', 'sheep', 0, 1, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'pig', 'pig', 1, 0, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cow', 'cow', 1, 1, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'frog', 'frog', 1, 1, 6.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cat', 'sheep', 0, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-006'), 'cat', 'cat', 1, 0, 7.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-007', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-642940 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-642867 seconds'), 3, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'chicken', 'cat', 0, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'chicken', 'chicken', 1, 0, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'pig', 'pig', 1, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'cow', 'cow', 1, 1, 5.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'dog', 'dog', 1, 1, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'cat', 'cat', 1, 1, 5.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'frog', 'frog', 1, 1, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'cat', 'cat', 1, 1, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'chicken', 'dog', 0, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-007'), 'chicken', 'chicken', 1, 0, 6.8);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-008', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-558619 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-558516 seconds'), 2, 8, 3);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'cow', 'chicken', 0, 1, 1.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'cow', 'cow', 1, 0, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'cow', 0, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'duck', 0, 0, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'chicken', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'pig', 'pig', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'cat', 0, 1, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'chicken', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'cat', 'cat', 1, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'cow', 'cow', 1, 1, 6.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'cow', 0, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'cow', 0, 0, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'chicken', 'chicken', 1, 0, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'dog', 'cat', 0, 1, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-008'), 'dog', 'dog', 1, 0, 5.2);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-009', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-553672 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-553587 seconds'), 1, 8, 5);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'chicken', 'dog', 0, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'chicken', 'pig', 0, 0, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'chicken', 'chicken', 1, 0, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'dog', 'dog', 1, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'cow', 'cow', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'chicken', 'chicken', 1, 1, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'cow', 'cow', 1, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'chicken', 'frog', 0, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'chicken', 'chicken', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'dog', 'dog', 1, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'sheep', 'cow', 0, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-009'), 'sheep', 'sheep', 1, 0, 7.4);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-010', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-534516 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-534453 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'pig', 'pig', 1, 1, 1.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'chicken', 'chicken', 1, 1, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'frog', 'frog', 1, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'dog', 'dog', 1, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'frog', 'frog', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'chicken', 'dog', 0, 1, 4.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'chicken', 'chicken', 1, 0, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'cat', 'cat', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'chicken', 'duck', 0, 1, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-010'), 'chicken', 'chicken', 1, 0, 4.5);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-011', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-453573 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-453506 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'duck', 'duck', 1, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'chicken', 'sheep', 0, 1, 4.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'chicken', 'chicken', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'cat', 'cat', 1, 1, 1.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'pig', 'pig', 1, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'cow', 'cow', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'sheep', 'sheep', 1, 1, 6.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'chicken', 'sheep', 0, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'chicken', 'chicken', 1, 0, 6.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-011'), 'cow', 'cow', 1, 1, 1.6);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-012', 'demo-na', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-402454 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-402404 seconds'), 3, 8, 7);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'dog', 'dog', 1, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'cow', 'cow', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'duck', 'duck', 1, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'cat', 'cat', 1, 1, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'frog', 'frog', 1, 1, 2.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'chicken', 'duck', 0, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'chicken', 'chicken', 1, 0, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'pig', 'pig', 1, 1, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-012'), 'sheep', 'sheep', 1, 1, 1.5);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-013', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-390902 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-390850 seconds'), 3, 8, 8);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'cow', 'cow', 1, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'chicken', 'chicken', 1, 1, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'pig', 'pig', 1, 1, 2.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'cat', 'cat', 1, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'frog', 'frog', 1, 1, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'sheep', 'sheep', 1, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'cow', 'cow', 1, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-013'), 'pig', 'pig', 1, 1, 3.4);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-014', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-367879 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-367794 seconds'), 2, 8, 5);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'chicken', 'duck', 0, 1, 4.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'chicken', 'chicken', 1, 0, 6.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'dog', 'cat', 0, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'dog', 'frog', 0, 0, 5.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'dog', 'dog', 1, 0, 7.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'chicken', 'pig', 0, 1, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'chicken', 'chicken', 1, 0, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'dog', 'dog', 1, 1, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'chicken', 'chicken', 1, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'pig', 'pig', 1, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'chicken', 'chicken', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-014'), 'cat', 'cat', 1, 1, 3.4);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-015', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-314960 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-314884 seconds'), 2, 8, 5);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'dog', 'dog', 1, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'sheep', 'sheep', 1, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'dog', 'cat', 0, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'dog', 'sheep', 0, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'dog', 'dog', 1, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'frog', 'frog', 1, 1, 1.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'dog', 'chicken', 0, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'dog', 'dog', 1, 0, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'chicken', 'chicken', 1, 1, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'sheep', 'sheep', 1, 1, 2.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'cat', 'dog', 0, 1, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-015'), 'cat', 'cat', 1, 0, 3.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-016', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-306065 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-306011 seconds'), 3, 8, 8);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'dog', 'dog', 1, 1, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'cat', 'cat', 1, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'frog', 'frog', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'pig', 'pig', 1, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'dog', 'dog', 1, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'chicken', 'chicken', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'frog', 'frog', 1, 1, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-016'), 'cat', 'cat', 1, 1, 2.7);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-017', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-229205 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-229160 seconds'), 3, 8, 8);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'frog', 'frog', 1, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'sheep', 'sheep', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'dog', 'dog', 1, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'sheep', 'sheep', 1, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'dog', 'dog', 1, 1, 2.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'duck', 'duck', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'chicken', 'chicken', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-017'), 'dog', 'dog', 1, 1, 2.8);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-018', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-227373 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-227316 seconds'), 3, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cat', 'cat', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'pig', 'pig', 1, 1, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cat', 'dog', 0, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cat', 'cat', 1, 0, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'chicken', 'chicken', 1, 1, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cat', 'cat', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cow', 'sheep', 0, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cow', 'cow', 1, 0, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'cat', 'cat', 1, 1, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-018'), 'dog', 'dog', 1, 1, 2.0);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-019', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-133017 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-132957 seconds'), 3, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'cow', 'cow', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'dog', 'dog', 1, 1, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'cat', 'cat', 1, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'chicken', 'chicken', 1, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'duck', 'duck', 1, 1, 1.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'cat', 'duck', 0, 1, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'cat', 'dog', 0, 0, 2.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'cat', 'cat', 1, 0, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'dog', 'cat', 0, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'dog', 'dog', 1, 0, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-019'), 'cat', 'cat', 1, 1, 2.0);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-020', 'demo-na', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-108183 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-108122 seconds'), 3, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'chicken', 'chicken', 1, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'frog', 'frog', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'cow', 'cow', 1, 1, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'dog', 'cat', 0, 1, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'dog', 'dog', 1, 0, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'pig', 'pig', 1, 1, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'frog', 'frog', 1, 1, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'dog', 'cow', 0, 1, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'dog', 'dog', 1, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-020'), 'frog', 'frog', 1, 1, 2.4);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-021', 'demo-na', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-108123 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-108019 seconds'), 2, 8, 5);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cow', 'cow', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cat', 'dog', 0, 1, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cat', 'frog', 0, 0, 4.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cat', 'pig', 0, 0, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cat', 'cat', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'dog', 'dog', 1, 1, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'frog', 'frog', 1, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cat', 'chicken', 0, 1, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'cat', 'cat', 1, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'duck', 'duck', 1, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'dog', 'cat', 0, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'dog', 'chicken', 0, 0, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'dog', 'frog', 0, 0, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'dog', 'frog', 0, 0, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'dog', 'dog', 1, 0, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-021'), 'chicken', 'chicken', 1, 1, 2.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-022', 'demo-na', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-20952 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-20892 seconds'), 3, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'dog', 'frog', 0, 1, 1.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'dog', 'dog', 1, 0, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'cat', 'cat', 1, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'dog', 'dog', 1, 1, 4.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'chicken', 'chicken', 1, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'cat', 'cat', 1, 1, 1.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'sheep', 'sheep', 1, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'dog', 'cat', 0, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'dog', 'dog', 1, 0, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-022'), 'chicken', 'chicken', 1, 1, 2.4);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-na-023', 'demo-na', 'sound', datetime('now', '-3293 seconds'), datetime('now', '-3228 seconds'), 3, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'duck', 'duck', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'dog', 'cat', 0, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'dog', 'dog', 1, 0, 6.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'duck', 'duck', 1, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'cow', 'cow', 1, 1, 2.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'duck', 'duck', 1, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'dog', 'chicken', 0, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'dog', 'dog', 1, 0, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'pig', 'pig', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-na-023'), 'dog', 'dog', 1, 1, 4.8);
INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES
    ('demo-na', 'cat', 0.6824, datetime('now', '-3228 seconds')),
    ('demo-na', 'dog', 0.2526, datetime('now', '-3228 seconds')),
    ('demo-na', 'chicken', 0.9901, datetime('now', '-3228 seconds')),
    ('demo-na', 'duck', 0.9976, datetime('now', '-3228 seconds')),
    ('demo-na', 'pig', 0.9942, datetime('now', '-3228 seconds')),
    ('demo-na', 'cow', 0.9257, datetime('now', '-3228 seconds')),
    ('demo-na', 'sheep', 0.9765, datetime('now', '-3228 seconds')),
    ('demo-na', 'frog', 0.9985, datetime('now', '-3228 seconds'));

INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)
VALUES ('demo-bin', 'Bin', 5, datetime('now', '+7 hours', 'start of day', '-7 hours', '-1080833 seconds'), datetime('now', '-9774 seconds'));
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-001', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-1080773 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-1080669 seconds'), 1, 8, 2);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'frog', 'cow', 0, 1, 2.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'frog', 'cow', 0, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'frog', 'frog', 1, 0, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'pig', 'pig', 1, 1, 7.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'cow', 'sheep', 0, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'cow', 'cow', 1, 0, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'frog', 'sheep', 0, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'frog', 'frog', 1, 0, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'chicken', 'chicken', 1, 1, 6.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'sheep', 'frog', 0, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'sheep', 'sheep', 1, 0, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'cow', 'pig', 0, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'cow', 'cow', 1, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'chicken', 'frog', 0, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-001'), 'chicken', 'chicken', 1, 0, 2.9);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-002', 'demo-bin', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-1077322 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-1077227 seconds'), 1, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'cat', 'duck', 0, 1, 5.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'cat', 'cat', 1, 0, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'pig', 'pig', 1, 1, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'frog', 'frog', 1, 1, 7.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'sheep', 'frog', 0, 1, 6.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'sheep', 'sheep', 1, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'cow', 'cow', 1, 1, 6.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'dog', 'duck', 0, 1, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'dog', 'dog', 1, 0, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'duck', 'chicken', 0, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'duck', 'duck', 1, 0, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-002'), 'chicken', 'chicken', 1, 1, 4.9);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-003', 'demo-bin', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-921546 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-921467 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'sheep', 'cat', 0, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'sheep', 'cat', 0, 0, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'sheep', 'sheep', 1, 0, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'cow', 'cow', 1, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'duck', 'duck', 1, 1, 7.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'frog', 'frog', 1, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'dog', 'dog', 1, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'chicken', 'cow', 0, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'chicken', 'chicken', 1, 0, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'sheep', 'sheep', 1, 1, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-003'), 'pig', 'pig', 1, 1, 4.9);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-004', 'demo-bin', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-822902 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-822816 seconds'), 1, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'frog', 'cat', 0, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'frog', 'frog', 1, 0, 6.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'cat', 'duck', 0, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'cat', 'cat', 1, 0, 7.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'frog', 'frog', 1, 1, 6.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'sheep', 'sheep', 1, 1, 4.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'dog', 'sheep', 0, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'dog', 'dog', 1, 0, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'frog', 'frog', 1, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'pig', 'cat', 0, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'pig', 'pig', 1, 0, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-004'), 'cat', 'cat', 1, 1, 3.8);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-005', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-640489 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-640387 seconds'), 1, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'pig', 'pig', 1, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'cow', 'cow', 1, 1, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'duck', 'chicken', 0, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'duck', 'duck', 1, 0, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'cow', 'sheep', 0, 1, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'cow', 'cow', 1, 0, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'duck', 'duck', 1, 1, 6.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'chicken', 'cat', 0, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'chicken', 'cat', 0, 0, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'chicken', 'chicken', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'dog', 'dog', 1, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'cat', 'sheep', 0, 1, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-005'), 'cat', 'cat', 1, 0, 6.5);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-006', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-626971 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-626856 seconds'), 1, 8, 2);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'dog', 'chicken', 0, 1, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'dog', 'dog', 1, 0, 3.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'sheep', 'sheep', 1, 1, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'chicken', 'duck', 0, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'chicken', 'chicken', 1, 0, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cow', 'sheep', 0, 1, 4.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cow', 'sheep', 0, 0, 4.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cow', 'cow', 1, 0, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'pig', 'dog', 0, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'pig', 'dog', 0, 0, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'pig', 'dog', 0, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'pig', 'pig', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'dog', 'dog', 1, 1, 6.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cat', 'pig', 0, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cat', 'cat', 1, 0, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cow', 'cat', 0, 1, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-006'), 'cow', 'cow', 1, 0, 4.1);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-007', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-474798 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-474701 seconds'), 1, 8, 3);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'pig', 'cat', 0, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'pig', 'pig', 1, 0, 3.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'frog', 'sheep', 0, 1, 4.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'frog', 'frog', 1, 0, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'cat', 'pig', 0, 1, 4.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'cat', 'cat', 1, 0, 6.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'cow', 'cow', 1, 1, 6.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'frog', 'cat', 0, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'frog', 'frog', 1, 0, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'cat', 'dog', 0, 1, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'cat', 'cat', 1, 0, 7.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'dog', 'dog', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-007'), 'pig', 'pig', 1, 1, 6.0);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-008', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-472216 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-472136 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'sheep', 'duck', 0, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'sheep', 'duck', 0, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'sheep', 'sheep', 1, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'dog', 'dog', 1, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'cat', 'cat', 1, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'frog', 'frog', 1, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'duck', 'duck', 1, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'chicken', 'chicken', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'frog', 'frog', 1, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'cat', 'chicken', 0, 1, 3.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'cat', 'cow', 0, 0, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-008'), 'cat', 'cat', 1, 0, 5.6);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-009', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-364230 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-364129 seconds'), 2, 8, 3);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'dog', 'dog', 1, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'frog', 'frog', 1, 1, 1.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'pig', 'cat', 0, 1, 4.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'pig', 'dog', 0, 0, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'pig', 'pig', 1, 0, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'chicken', 'duck', 0, 1, 1.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'chicken', 'pig', 0, 0, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'chicken', 'chicken', 1, 0, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'sheep', 'cow', 0, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'sheep', 'sheep', 1, 0, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'duck', 'frog', 0, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'duck', 'duck', 1, 0, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'sheep', 'pig', 0, 1, 5.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'sheep', 'sheep', 1, 0, 6.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-009'), 'pig', 'pig', 1, 1, 3.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-010', 'demo-bin', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-221485 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-221393 seconds'), 1, 8, 3);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'duck', 'sheep', 0, 1, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'duck', 'duck', 1, 0, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'frog', 'pig', 0, 1, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'frog', 'frog', 1, 0, 3.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cat', 'cow', 0, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cat', 'cat', 1, 0, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cow', 'sheep', 0, 1, 2.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cow', 'cow', 1, 0, 6.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cat', 'pig', 0, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cat', 'cat', 1, 0, 6.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'pig', 'pig', 1, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'chicken', 'chicken', 1, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-010'), 'cat', 'cat', 1, 1, 6.3);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-011', 'demo-bin', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-203958 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-203880 seconds'), 2, 8, 6);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'pig', 'pig', 1, 1, 1.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'duck', 'duck', 1, 1, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'chicken', 'chicken', 1, 1, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'duck', 'cat', 0, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'duck', 'duck', 1, 0, 6.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'sheep', 'cow', 0, 1, 5.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'sheep', 'cat', 0, 0, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'sheep', 'sheep', 1, 0, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'cat', 'cat', 1, 1, 5.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'cow', 'cow', 1, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-011'), 'chicken', 'chicken', 1, 1, 1.9);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-012', 'demo-bin', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-48119 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-48025 seconds'), 2, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'duck', 'duck', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'pig', 'pig', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'cat', 'frog', 0, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'cat', 'cat', 1, 0, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'cow', 'dog', 0, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'cow', 'cow', 1, 0, 6.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'frog', 'cow', 0, 1, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'frog', 'dog', 0, 0, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'frog', 'frog', 1, 0, 7.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'sheep', 'cow', 0, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'sheep', 'sheep', 1, 0, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'frog', 'frog', 1, 1, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-012'), 'cat', 'cat', 1, 1, 7.0);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-bin-013', 'demo-bin', 'shadow', datetime('now', '-9831 seconds'), datetime('now', '-9774 seconds'), 3, 8, 7);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'duck', 'duck', 1, 1, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'sheep', 'sheep', 1, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'duck', 'duck', 1, 1, 1.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'cow', 'cow', 1, 1, 4.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'chicken', 'chicken', 1, 1, 1.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'cow', 'sheep', 0, 1, 3.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'cow', 'cow', 1, 0, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'frog', 'frog', 1, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-bin-013'), 'chicken', 'chicken', 1, 1, 1.5);
INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES
    ('demo-bin', 'cat', 0.398, datetime('now', '-9774 seconds')),
    ('demo-bin', 'dog', 0.8288, datetime('now', '-9774 seconds')),
    ('demo-bin', 'chicken', 0.9112, datetime('now', '-9774 seconds')),
    ('demo-bin', 'duck', 0.7791, datetime('now', '-9774 seconds')),
    ('demo-bin', 'pig', 0.8249, datetime('now', '-9774 seconds')),
    ('demo-bin', 'cow', 0.2546, datetime('now', '-9774 seconds')),
    ('demo-bin', 'sheep', 0.2412, datetime('now', '-9774 seconds')),
    ('demo-bin', 'frog', 0.6892, datetime('now', '-9774 seconds'));

INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)
VALUES ('demo-su', 'Su', 7, datetime('now', '+7 hours', 'start of day', '-7 hours', '-300859 seconds'), datetime('now', '-7176 seconds'));
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-su-001', 'demo-su', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-300799 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-300682 seconds'), 1, 8, 1);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'sheep', 'duck', 0, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'sheep', 'sheep', 1, 0, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'frog', 'frog', 1, 1, 6.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'duck', 'pig', 0, 1, 2.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'duck', 'duck', 1, 0, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'dog', 'cow', 0, 1, 5.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'dog', 'dog', 1, 0, 3.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'frog', 'cow', 0, 1, 1.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'frog', 'frog', 1, 0, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'pig', 'duck', 0, 1, 1.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'pig', 'pig', 1, 0, 6.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'dog', 'cat', 0, 1, 1.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'dog', 'cat', 0, 0, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'dog', 'cat', 0, 0, 3.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'dog', 'dog', 1, 0, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'cat', 'sheep', 0, 1, 5.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-001'), 'cat', 'cat', 1, 0, 3.8);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-su-002', 'demo-su', 'sound', datetime('now', '+7 hours', 'start of day', '-7 hours', '-281451 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-281356 seconds'), 1, 8, 2);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'sheep', 'frog', 0, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'sheep', 'sheep', 1, 0, 3.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'dog', 'pig', 0, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'dog', 'dog', 1, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'cat', 'duck', 0, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'cat', 'cat', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'cow', 'sheep', 0, 1, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'cow', 'cow', 1, 0, 5.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'pig', 'cat', 0, 1, 2.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'pig', 'pig', 1, 0, 4.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'chicken', 'pig', 0, 1, 2.2),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'chicken', 'chicken', 1, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'pig', 'pig', 1, 1, 6.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-002'), 'dog', 'dog', 1, 1, 5.8);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-su-003', 'demo-su', 'food', datetime('now', '+7 hours', 'start of day', '-7 hours', '-188569 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-188482 seconds'), 2, 8, 5);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'sheep', 'sheep', 1, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'duck', 'duck', 1, 1, 7.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'pig', 'pig', 1, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'cow', 'dog', 0, 1, 3.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'cow', 'cow', 1, 0, 7.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'dog', 'dog', 1, 1, 4.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'frog', 'frog', 1, 1, 7.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'chicken', 'duck', 0, 1, 3.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'chicken', 'chicken', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'cow', 'chicken', 0, 1, 3.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-003'), 'cow', 'cow', 1, 0, 6.5);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-su-004', 'demo-su', 'shadow', datetime('now', '+7 hours', 'start of day', '-7 hours', '-33164 seconds'), datetime('now', '+7 hours', 'start of day', '-7 hours', '-33038 seconds'), 1, 8, 2);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'cat', 'cat', 1, 1, 6.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'chicken', 'pig', 0, 1, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'chicken', 'pig', 0, 0, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'chicken', 'chicken', 1, 0, 7.4),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'cat', 'dog', 0, 1, 5.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'cat', 'dog', 0, 0, 2.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'cat', 'cat', 1, 0, 7.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'frog', 'chicken', 0, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'frog', 'frog', 1, 0, 2.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'dog', 'duck', 0, 1, 4.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'dog', 'duck', 0, 0, 5.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'dog', 'dog', 1, 0, 5.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'chicken', 'cat', 0, 1, 2.3),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'chicken', 'chicken', 1, 0, 3.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'duck', 'cow', 0, 1, 4.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'duck', 'duck', 1, 0, 4.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-004'), 'frog', 'frog', 1, 1, 4.0);
INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)
VALUES ('demo-su-005', 'demo-su', 'food', datetime('now', '-7261 seconds'), datetime('now', '-7176 seconds'), 1, 8, 4);
INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'dog', 'cow', 0, 1, 4.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'dog', 'dog', 1, 0, 6.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'frog', 'cow', 0, 1, 2.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'frog', 'frog', 1, 0, 3.9),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'sheep', 'sheep', 1, 1, 5.1),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'frog', 'frog', 1, 1, 2.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'chicken', 'chicken', 1, 1, 5.0),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'duck', 'pig', 0, 1, 2.8),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'duck', 'duck', 1, 0, 5.7),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'pig', 'cat', 0, 1, 4.5),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'pig', 'pig', 1, 0, 5.6),
    ((SELECT Id FROM Sessions WHERE ClientSessionId = 'demo-su-005'), 'frog', 'frog', 1, 1, 1.5);
INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES
    ('demo-su', 'cat', 0.12, datetime('now', '-7176 seconds')),
    ('demo-su', 'dog', 0.1296, datetime('now', '-7176 seconds')),
    ('demo-su', 'chicken', 0.2, datetime('now', '-7176 seconds')),
    ('demo-su', 'duck', 0.072, datetime('now', '-7176 seconds')),
    ('demo-su', 'pig', 0.288, datetime('now', '-7176 seconds')),
    ('demo-su', 'sheep', 0.48, datetime('now', '-7176 seconds')),
    ('demo-su', 'frog', 0.6955, datetime('now', '-7176 seconds'));

COMMIT;
