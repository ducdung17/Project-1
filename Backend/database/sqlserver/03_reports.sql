USE NongTrai;
GO

CREATE OR ALTER VIEW dbo.vw_ChildOverview AS
WITH ans AS (
    SELECT s.ChildId,
           SUM(CAST(a.FirstTry AS int))                                        AS Questions,
           SUM(CASE WHEN a.FirstTry = 1 AND a.Correct = 1 THEN 1 ELSE 0 END)  AS FirstTryCorrect,
           AVG(CASE WHEN a.FirstTry = 1 THEN a.Seconds END)                    AS AvgSeconds
    FROM dbo.Sessions s
    JOIN dbo.Answers a ON a.PlaySessionId = s.Id
    GROUP BY s.ChildId
)
SELECT c.Id,
       c.Nickname,
       (SELECT COUNT(*) FROM dbo.Sessions s WHERE s.ChildId = c.Id)                     AS Sessions,
       COALESCE(ans.Questions, 0)                                                       AS Questions,
       CAST(100.0 * ans.FirstTryCorrect / NULLIF(ans.Questions, 0) AS decimal(5, 1))   AS FirstTryPct,
       CAST(ans.AvgSeconds AS decimal(5, 1))                                            AS AvgSeconds,
       (SELECT COUNT(*) FROM dbo.Masteries m WHERE m.ChildId = c.Id AND m.Score >= 0.8) AS MasteredAnimals,
       DATEADD(HOUR, 7, c.LastSeenAt)                                                   AS LastSeenVN
FROM dbo.Children c
LEFT JOIN ans ON ans.ChildId = c.Id;
GO

CREATE OR ALTER VIEW dbo.vw_DailyActivity AS
SELECT s.ChildId,
       CAST(DATEADD(HOUR, 7, s.StartedAt) AS date)                                AS DayVN,
       COUNT(DISTINCT s.Id)                                                       AS Sessions,
       SUM(CAST(a.FirstTry AS int))                                               AS Questions,
       CAST(100.0 * SUM(CASE WHEN a.FirstTry = 1 AND a.Correct = 1 THEN 1 ELSE 0 END)
            / NULLIF(SUM(CAST(a.FirstTry AS int)), 0) AS decimal(5, 1))           AS FirstTryPct
FROM dbo.Sessions s
JOIN dbo.Answers a ON a.PlaySessionId = s.Id
GROUP BY s.ChildId, CAST(DATEADD(HOUR, 7, s.StartedAt) AS date);
GO

CREATE OR ALTER PROCEDURE dbo.sp_ChildReport
    @ChildId nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Children WHERE Id = @ChildId)
    BEGIN
        RAISERROR(N'Không có bé với Id %s', 16, 1, @ChildId);
        RETURN;
    END;

    SELECT * FROM dbo.vw_ChildOverview WHERE Id = @ChildId;

    SELECT m.AnimalId,
           CAST(m.Score * 100 AS decimal(5, 0)) AS ScorePct,
           CASE WHEN m.Score >= 0.8 THEN N'Đã thuộc'
                WHEN m.Score >= 0.4 THEN N'Đang học'
                ELSE N'Mới làm quen' END        AS TrangThai
    FROM dbo.Masteries m
    WHERE m.ChildId = @ChildId
    ORDER BY m.Score DESC;

    SELECT TOP (5) a.AnimalId AS ConDung, a.ChosenId AS BeChonNham, COUNT(*) AS SoLan
    FROM dbo.Answers a
    JOIN dbo.Sessions s ON s.Id = a.PlaySessionId
    WHERE s.ChildId = @ChildId AND a.Correct = 0
    GROUP BY a.AnimalId, a.ChosenId
    ORDER BY COUNT(*) DESC, a.AnimalId;

    DECLARE @todayVN date = CAST(DATEADD(HOUR, 7, SYSUTCDATETIME()) AS date);
    WITH days AS (
        SELECT 0 AS n
        UNION ALL
        SELECT n + 1 FROM days WHERE n < 13
    )
    SELECT DATEADD(DAY, -d.n, @todayVN)  AS DayVN,
           COALESCE(v.Sessions, 0)       AS Sessions,
           COALESCE(v.Questions, 0)      AS Questions,
           v.FirstTryPct
    FROM days d
    LEFT JOIN dbo.vw_DailyActivity v
           ON v.ChildId = @ChildId AND v.DayVN = DATEADD(DAY, -d.n, @todayVN)
    ORDER BY DayVN;
END;
GO

SELECT * FROM dbo.vw_ChildOverview ORDER BY FirstTryPct DESC;

EXEC dbo.sp_ChildReport @ChildId = N'demo-na';

WITH pairs AS (
    SELECT s.ChildId, a.AnimalId, a.ChosenId, COUNT(*) AS Times
    FROM dbo.Answers a
    JOIN dbo.Sessions s ON s.Id = a.PlaySessionId
    WHERE a.Correct = 0
    GROUP BY s.ChildId, a.AnimalId, a.ChosenId
), ranked AS (
    SELECT p.*, ROW_NUMBER() OVER (PARTITION BY p.ChildId ORDER BY p.Times DESC, p.AnimalId) AS Rk
    FROM pairs p
)
SELECT c.Nickname, r.AnimalId AS ConDung, r.ChosenId AS BeChonNham, r.Times AS SoLan
FROM ranked r
JOIN dbo.Children c ON c.Id = r.ChildId
WHERE r.Rk <= 3
ORDER BY c.Nickname, r.Rk;

SELECT s.Game,
       COUNT(DISTINCT s.Id)                                                        AS Sessions,
       SUM(CAST(a.FirstTry AS int))                                                AS Questions,
       CAST(100.0 * SUM(CASE WHEN a.FirstTry = 1 AND a.Correct = 1 THEN 1 ELSE 0 END)
            / NULLIF(SUM(CAST(a.FirstTry AS int)), 0) AS decimal(5, 1))            AS FirstTryPct,
       CAST(AVG(CASE WHEN a.FirstTry = 1 THEN a.Seconds END) AS decimal(5, 1))     AS AvgSeconds
FROM dbo.Sessions s
JOIN dbo.Answers a ON a.PlaySessionId = s.Id
GROUP BY s.Game
ORDER BY FirstTryPct;

SELECT a.AnimalId,
       SUM(CAST(a.FirstTry AS int))                                                AS Asked,
       CAST(100.0 * SUM(CASE WHEN a.FirstTry = 1 AND a.Correct = 1 THEN 1 ELSE 0 END)
            / NULLIF(SUM(CAST(a.FirstTry AS int)), 0) AS decimal(5, 1))            AS FirstTryPct,
       CAST(AVG(CASE WHEN a.FirstTry = 1 THEN a.Seconds END) AS decimal(5, 1))     AS AvgSeconds
FROM dbo.Answers a
GROUP BY a.AnimalId
HAVING SUM(CAST(a.FirstTry AS int)) >= 10
ORDER BY FirstTryPct;

WITH tagged AS (
    SELECT s.ChildId, a.FirstTry, a.Correct,
           CASE WHEN CAST(DATEADD(HOUR, 7, s.StartedAt) AS date)
                     >= DATEADD(DAY, -6, CAST(DATEADD(HOUR, 7, SYSUTCDATETIME()) AS date))
                THEN N'2_7NgayGanNhat' ELSE N'1_TruocDo' END AS Period
    FROM dbo.Sessions s
    JOIN dbo.Answers a ON a.PlaySessionId = s.Id
)
SELECT c.Nickname, t.Period,
       SUM(CAST(t.FirstTry AS int))                                                AS Questions,
       CAST(100.0 * SUM(CASE WHEN t.FirstTry = 1 AND t.Correct = 1 THEN 1 ELSE 0 END)
            / NULLIF(SUM(CAST(t.FirstTry AS int)), 0) AS decimal(5, 1))            AS FirstTryPct
FROM tagged t
JOIN dbo.Children c ON c.Id = t.ChildId
GROUP BY c.Nickname, t.Period
ORDER BY c.Nickname, t.Period;

SELECT s.DifficultyLevel,
       COUNT(DISTINCT s.Id)                                                        AS Sessions,
       CAST(100.0 * SUM(CASE WHEN a.FirstTry = 1 AND a.Correct = 1 THEN 1 ELSE 0 END)
            / NULLIF(SUM(CAST(a.FirstTry AS int)), 0) AS decimal(5, 1))            AS FirstTryPct
FROM dbo.Sessions s
JOIN dbo.Answers a ON a.PlaySessionId = s.Id
GROUP BY s.DifficultyLevel
ORDER BY s.DifficultyLevel;

SELECT TOP (20) Id, Game, StartedAt, PerfectCount
FROM dbo.Sessions
WHERE ChildId = N'demo-na'
ORDER BY StartedAt DESC;
