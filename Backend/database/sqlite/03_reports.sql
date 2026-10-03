DROP VIEW IF EXISTS vw_ChildOverview;
CREATE VIEW vw_ChildOverview AS
WITH ans AS (
    SELECT s.ChildId,
           SUM(a.FirstTry)                       AS Questions,
           SUM(a.FirstTry * a.Correct)           AS FirstTryCorrect,
           ROUND(AVG(CASE WHEN a.FirstTry = 1 THEN a.Seconds END), 1) AS AvgSeconds
    FROM Sessions s
    JOIN Answers a ON a.PlaySessionId = s.Id
    GROUP BY s.ChildId
)
SELECT c.Id,
       c.Nickname,
       (SELECT COUNT(*) FROM Sessions s WHERE s.ChildId = c.Id)                     AS Sessions,
       COALESCE(ans.Questions, 0)                                                   AS Questions,
       ROUND(100.0 * ans.FirstTryCorrect / NULLIF(ans.Questions, 0), 1)            AS FirstTryPct,
       ans.AvgSeconds,
       (SELECT COUNT(*) FROM Masteries m WHERE m.ChildId = c.Id AND m.Score >= 0.8) AS MasteredAnimals,
       datetime(c.LastSeenAt, '+7 hours')                                           AS LastSeenVN
FROM Children c
LEFT JOIN ans ON ans.ChildId = c.Id;

DROP VIEW IF EXISTS vw_DailyActivity;
CREATE VIEW vw_DailyActivity AS
SELECT s.ChildId,
       date(s.StartedAt, '+7 hours')                                         AS DayVN,
       COUNT(DISTINCT s.Id)                                                  AS Sessions,
       SUM(a.FirstTry)                                                       AS Questions,
       ROUND(100.0 * SUM(a.FirstTry * a.Correct) / NULLIF(SUM(a.FirstTry), 0), 1) AS FirstTryPct
FROM Sessions s
JOIN Answers a ON a.PlaySessionId = s.Id
GROUP BY s.ChildId, date(s.StartedAt, '+7 hours');

SELECT * FROM vw_ChildOverview ORDER BY FirstTryPct DESC;

SELECT DayVN, Sessions, Questions, FirstTryPct
FROM vw_DailyActivity
WHERE ChildId = 'demo-na' AND DayVN >= date('now', '+7 hours', '-13 days')
ORDER BY DayVN;

WITH pairs AS (
    SELECT s.ChildId, a.AnimalId, a.ChosenId, COUNT(*) AS Times
    FROM Answers a
    JOIN Sessions s ON s.Id = a.PlaySessionId
    WHERE a.Correct = 0
    GROUP BY s.ChildId, a.AnimalId, a.ChosenId
), ranked AS (
    SELECT p.*, ROW_NUMBER() OVER (PARTITION BY p.ChildId ORDER BY p.Times DESC, p.AnimalId) AS Rk
    FROM pairs p
)
SELECT c.Nickname, r.AnimalId AS ConDung, r.ChosenId AS BeChonNham, r.Times AS SoLan
FROM ranked r
JOIN Children c ON c.Id = r.ChildId
WHERE r.Rk <= 3
ORDER BY c.Nickname, r.Rk;

SELECT s.Game,
       COUNT(DISTINCT s.Id)                                                       AS Sessions,
       SUM(a.FirstTry)                                                            AS Questions,
       ROUND(100.0 * SUM(a.FirstTry * a.Correct) / NULLIF(SUM(a.FirstTry), 0), 1) AS FirstTryPct,
       ROUND(AVG(CASE WHEN a.FirstTry = 1 THEN a.Seconds END), 1)                 AS AvgSeconds
FROM Sessions s
JOIN Answers a ON a.PlaySessionId = s.Id
GROUP BY s.Game
ORDER BY FirstTryPct;

SELECT a.AnimalId,
       SUM(a.FirstTry)                                                            AS Asked,
       ROUND(100.0 * SUM(a.FirstTry * a.Correct) / NULLIF(SUM(a.FirstTry), 0), 1) AS FirstTryPct,
       ROUND(AVG(CASE WHEN a.FirstTry = 1 THEN a.Seconds END), 1)                 AS AvgSeconds
FROM Answers a
GROUP BY a.AnimalId
HAVING SUM(a.FirstTry) >= 10
ORDER BY FirstTryPct;

WITH tagged AS (
    SELECT s.ChildId, a.FirstTry, a.Correct,
           CASE WHEN date(s.StartedAt, '+7 hours') >= date('now', '+7 hours', '-6 days')
                THEN '2_7NgayGanNhat' ELSE '1_TruocDo' END AS Period
    FROM Sessions s
    JOIN Answers a ON a.PlaySessionId = s.Id
)
SELECT c.Nickname, t.Period,
       SUM(t.FirstTry)                                                            AS Questions,
       ROUND(100.0 * SUM(t.FirstTry * t.Correct) / NULLIF(SUM(t.FirstTry), 0), 1) AS FirstTryPct
FROM tagged t
JOIN Children c ON c.Id = t.ChildId
GROUP BY c.Nickname, t.Period
ORDER BY c.Nickname, t.Period;

SELECT s.DifficultyLevel,
       COUNT(DISTINCT s.Id)                                                       AS Sessions,
       ROUND(100.0 * SUM(a.FirstTry * a.Correct) / NULLIF(SUM(a.FirstTry), 0), 1) AS FirstTryPct
FROM Sessions s
JOIN Answers a ON a.PlaySessionId = s.Id
GROUP BY s.DifficultyLevel
ORDER BY s.DifficultyLevel;

SELECT m.AnimalId,
       ROUND(m.Score * 100) AS ScorePct,
       CASE WHEN m.Score >= 0.8 THEN 'Đã thuộc'
            WHEN m.Score >= 0.4 THEN 'Đang học'
            ELSE 'Mới làm quen' END AS TrangThai
FROM Masteries m
WHERE m.ChildId = 'demo-na'
ORDER BY m.Score DESC;
