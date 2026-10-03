import re, sqlite3, sys, os
here = os.path.dirname(os.path.abspath(__file__))
c = sqlite3.connect(':memory:')
c.execute('PRAGMA foreign_keys = ON')
c.executescript(open(os.path.join(here, 'sqlite_schema_for_test.sql')).read())
seed = open(os.path.join(here, '..', 'sqlite', '02_seed_demo.sql'), encoding='utf-8').read()
c.executescript(seed); c.executescript(seed)
txt = open(os.path.join(here, '..', 'sqlite', '03_reports.sql'), encoding='utf-8').read()
idx = txt.index('PHẦN 2'); idx = txt.rindex('\n', 0, idx)
c.executescript(txt[:idx])
for st in re.split(r';\s*\n', txt[idx:]):
    lines = st.strip().splitlines()
    title = next((l for l in lines if l.startswith('-- [')), None)
    sql = '\n'.join(l for l in lines if not l.strip().startswith('--')).strip()
    if not sql: continue
    cur = c.execute(sql)
    print('\n' + title); print([d[0] for d in cur.description])
    for r in cur.fetchall()[:9]: print(r)
print('\nSessions:', c.execute('select count(*) from Sessions').fetchone()[0])
