import os
import random

ANIMALS = ["cat", "dog", "chicken", "duck", "pig", "cow", "sheep", "frog"]
CONFUSE = {"cow": "sheep", "sheep": "cow", "chicken": "duck", "duck": "chicken", "cat": "dog", "dog": "cat"}
GAMES = ["sound", "shadow", "food"]
FAST, GAIN_FAST, GAIN_SLOW, LOSS, MASTERED = 4.0, 0.35, 0.20, 0.40, 0.8
WINDOW, UP, DOWN = 6, 0.8, 0.5
QUESTIONS = 8

KIDS = [
    ("demo-na", "Na", 2, 0.2, list(range(13, -1, -1)), (1, 3)),
    ("demo-bin", "Bin", 5, 0.00, [13, 11, 10, 8, 6, 5, 3, 1, 0], (1, 2)),
    ("demo-su", "Su", 7, -0.08, [4, 3, 1, 0], (1, 2)),
]

def update(score, correct, seconds):
    if correct:
        gain = GAIN_FAST if seconds <= FAST else GAIN_SLOW
        return min(1.0, score + (1 - score) * gain)
    return max(0.0, score * (1 - LOSS))

def simulate(rng):
    kids = []
    for kid_id, name, avatar, skill, days, per_day in KIDS:
        score = {a: 0.0 for a in ANIMALS}
        level, recent, last_target = 1, [], None
        sessions = []
        for d in days:
            n = rng.randint(*per_day)
            if d == 0:
                starts = sorted((-rng.randint(1800, 3 * 3600) for _ in range(n)))
            else:
                starts = sorted((-d * 86400 + rng.randint(8 * 3600, 20 * 3600) for _ in range(n)))
            for start in starts:
                game = rng.choice(GAMES)
                answers = []
                elapsed = 0.0
                for _ in range(QUESTIONS):
                    pool = [a for a in ANIMALS if a != last_target]
                    weights = [1 - score[a] + 0.1 for a in pool]
                    target = rng.choices(pool, weights)[0]
                    last_target = target
                    n_opt = level + 1
                    others = [a for a in ANIMALS if a != target]
                    if level == 3 and target in CONFUSE:
                        opts = [CONFUSE[target]] + rng.sample([a for a in others if a != CONFUSE[target]], n_opt - 2)
                    else:
                        opts = rng.sample(others, n_opt - 1)
                    p = 0.42 + 0.5 * score[target] + skill - 0.06 * (n_opt - 2)
                    if CONFUSE.get(target) in opts:
                        p -= 0.15
                    p = max(0.15, min(0.97, p))

                    first = True
                    while True:
                        correct = rng.random() < (p if first else 0.8)
                        if correct:
                            secs = round(rng.uniform(1.4, 3.8) if score[target] > 0.5 else rng.uniform(2.5, 7.5), 1)
                            chosen = target
                        else:
                            secs = round(rng.uniform(1.5, 6.0), 1)
                            wrong = [o for o in opts if o not in {x[1] for x in answers if x[0] == target and not x[2]}]
                            chosen = CONFUSE[target] if CONFUSE.get(target) in wrong and rng.random() < 0.7 else rng.choice(wrong or opts)
                        answers.append((target, chosen, correct, first, secs))
                        elapsed += secs + 2.5
                        if first:
                            score[target] = update(score[target], correct, secs)
                            recent = (recent + [correct])[-WINDOW:]
                            if len(recent) == WINDOW:
                                rate = sum(recent) / WINDOW
                                if rate >= UP and level < 3:
                                    level, recent = level + 1, []
                                elif rate <= DOWN and level > 1:
                                    level, recent = level - 1, []
                        if correct:
                            break
                        first = False
                sessions.append({
                    "csid": f"{kid_id}-{len(sessions) + 1:03d}",
                    "game": game, "base": "now" if d == 0 else "day", "start": start, "end": start + int(elapsed) + 5,
                    "level": level, "perfect": sum(1 for a in answers if a[2] and a[3]),
                    "answers": answers,
                })
        kids.append({"id": kid_id, "name": name, "avatar": avatar, "sessions": sessions,
                     "mastery": {a: round(v, 4) for a, v in score.items() if v > 0}})
    return kids

def ss_time(sec, base):
    return f"DATEADD(SECOND, {sec}, {'@now' if base == 'now' else '@vnMidnight'})"

def write_sqlserver(kids, path):
    L = []
    w = L.append
    w("/* =====================================================================")
    w("   Nông Trại Của Bé – DỮ LIỆU DEMO cho SQL Server")
    w("   3 bé (Na, Bin, Su), 14 ngày gần nhất tính tới hôm nay. Chạy lại được nhiều lần:")
    w("   script xóa dữ liệu demo cũ (Id bắt đầu bằng 'demo-') rồi thêm mới, không đụng dữ liệu thật.")
    w("   Sinh tự động bởi database/tools/generate_demo.py")
    w("   ===================================================================== */")
    w("USE NongTrai;")
    w("SET NOCOUNT ON;")
    w("SET XACT_ABORT ON;  -- lỗi giữa chừng thì hủy toàn bộ, không để dữ liệu dở dang")
    w("BEGIN TRANSACTION;")
    w("")
    w("DECLARE @now datetime2 = SYSUTCDATETIME();  -- giờ UTC lúc chạy script")
    w("-- 0h hôm nay theo giờ Việt Nam, đổi ra UTC (VN = UTC+7). Các lượt ngày trước tính từ mốc này.")
    w("DECLARE @vnMidnight datetime2 = DATEADD(HOUR, -7, CAST(CAST(DATEADD(HOUR, 7, @now) AS date) AS datetime2));")
    w("DECLARE @sid int;")
    w("")
    w("-- 1) Xóa dữ liệu demo cũ (theo thứ tự con trước, cha sau)")
    w("DELETE a FROM Answers a JOIN Sessions s ON s.Id = a.PlaySessionId WHERE s.ChildId LIKE 'demo-%';")
    w("DELETE FROM Sessions  WHERE ChildId LIKE 'demo-%';")
    w("DELETE FROM Masteries WHERE ChildId LIKE 'demo-%';")
    w("DELETE FROM Children  WHERE Id LIKE 'demo-%';")
    w("")
    for k in kids:
        first, last = k["sessions"][0], k["sessions"][-1]
        w(f"-- ===== Bé {k['name']}: {len(k['sessions'])} lượt chơi")
        w("INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)")
        w(f"VALUES (N'{k['id']}', N'{k['name']}', {k['avatar']}, {ss_time(first['start'] - 60, first['base'])}, {ss_time(last['end'], last['base'])});")
        for s in k["sessions"]:
            w("INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)")
            w(f"VALUES (N'{s['csid']}', N'{k['id']}', N'{s['game']}', {ss_time(s['start'], s['base'])}, {ss_time(s['end'], s['base'])}, {s['level']}, {QUESTIONS}, {s['perfect']});")
            w("SET @sid = SCOPE_IDENTITY();  -- Id của lượt vừa thêm")
            w("INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds)")
            w("SELECT @sid, v.AnimalId, v.ChosenId, v.Correct, v.FirstTry, v.Seconds FROM (VALUES")
            rows = [f"    (N'{t}', N'{c}', {int(ok)}, {int(ft)}, {sec})" for t, c, ok, ft, sec in s["answers"]]
            w(",\n".join(rows))
            w(") AS v(AnimalId, ChosenId, Correct, FirstTry, Seconds);")
        w("INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES")
        rows = [f"    (N'{k['id']}', N'{a}', {v}, {ss_time(last['end'], last['base'])})" for a, v in k["mastery"].items()]
        w(",\n".join(rows) + ";")
        w("")
    w("COMMIT TRANSACTION;")
    w("")
    w("SELECT c.Nickname, COUNT(s.Id) AS SoLuotChoi FROM Children c LEFT JOIN Sessions s ON s.ChildId = c.Id")
    w("WHERE c.Id LIKE 'demo-%' GROUP BY c.Nickname;")
    open(path, "w", encoding="utf-8-sig", newline="\r\n").write("\n".join(L) + "\n")

def lt_time(sec, base):
    if base == "now":
        return f"datetime('now', '{sec:+d} seconds')"
    return f"datetime('now', '+7 hours', 'start of day', '-7 hours', '{sec:+d} seconds')"

def write_sqlite(kids, path):
    L = []
    w = L.append
    w("-- =====================================================================")
    w("-- Nông Trại Của Bé – DỮ LIỆU DEMO cho SQLite (file nongtrai.db)")
    w("-- 3 bé (Na, Bin, Su), 14 ngày gần nhất. Chạy lại được: xóa dữ liệu demo cũ rồi thêm mới.")
    w("-- Cách chạy: tắt server, mở nongtrai.db bằng DB Browser for SQLite > tab Execute SQL > dán > Run,")
    w("-- rồi bấm Write Changes. Sinh tự động bởi database/tools/generate_demo.py")
    w("-- =====================================================================")
    w("PRAGMA foreign_keys = ON;")
    w("BEGIN TRANSACTION;")
    w("")
    w("DELETE FROM Answers WHERE PlaySessionId IN (SELECT Id FROM Sessions WHERE ChildId LIKE 'demo-%');")
    w("DELETE FROM Sessions  WHERE ChildId LIKE 'demo-%';")
    w("DELETE FROM Masteries WHERE ChildId LIKE 'demo-%';")
    w("DELETE FROM Children  WHERE Id LIKE 'demo-%';")
    w("")
    for k in kids:
        first, last = k["sessions"][0], k["sessions"][-1]
        w(f"-- ===== Bé {k['name']}: {len(k['sessions'])} lượt chơi")
        w("INSERT INTO Children (Id, Nickname, AvatarId, CreatedAt, LastSeenAt)")
        w(f"VALUES ('{k['id']}', '{k['name']}', {k['avatar']}, {lt_time(first['start'] - 60, first['base'])}, {lt_time(last['end'], last['base'])});")
        for s in k["sessions"]:
            w("INSERT INTO Sessions (ClientSessionId, ChildId, Game, StartedAt, EndedAt, DifficultyLevel, QuestionCount, PerfectCount)")
            w(f"VALUES ('{s['csid']}', '{k['id']}', '{s['game']}', {lt_time(s['start'], s['base'])}, {lt_time(s['end'], s['base'])}, {s['level']}, {QUESTIONS}, {s['perfect']});")
            w("INSERT INTO Answers (PlaySessionId, AnimalId, ChosenId, Correct, FirstTry, Seconds) VALUES")
            rows = [f"    ((SELECT Id FROM Sessions WHERE ClientSessionId = '{s['csid']}'), '{t}', '{c}', {int(ok)}, {int(ft)}, {sec})"
                    for t, c, ok, ft, sec in s["answers"]]
            w(",\n".join(rows) + ";")
        w("INSERT INTO Masteries (ChildId, AnimalId, Score, UpdatedAt) VALUES")
        rows = [f"    ('{k['id']}', '{a}', {v}, {lt_time(last['end'], last['base'])})" for a, v in k["mastery"].items()]
        w(",\n".join(rows) + ";")
        w("")
    w("COMMIT;")
    open(path, "w", encoding="utf-8", newline="\n").write("\n".join(L) + "\n")

if __name__ == "__main__":
    here = os.path.dirname(os.path.abspath(__file__))
    data = simulate(random.Random(2026))
    write_sqlserver(data, os.path.join(here, "..", "sqlserver", "02_seed_demo.sql"))
    write_sqlite(data, os.path.join(here, "..", "sqlite", "02_seed_demo.sql"))
    for k in data:
        ft = [a for s in k["sessions"] for a in s["answers"] if a[3]]
        half = len(ft) // 2
        print("  nua dau", round(sum(a[2] for a in ft[:half]) / half, 2), "nua sau", round(sum(a[2] for a in ft[half:]) / (len(ft) - half), 2))
        print(k["name"], len(k["sessions"]), "luot,", len(ft), "cau, dung lan dau",
              round(sum(a[2] for a in ft) / len(ft), 2), "thuoc", sum(v >= MASTERED for v in k["mastery"].values()),
              "level cuoi", k["sessions"][-1]["level"])
