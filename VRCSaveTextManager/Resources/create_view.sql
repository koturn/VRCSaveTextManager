DROP VIEW IF EXISTS v_logfile;
CREATE VIEW IF NOT EXISTS v_logfile AS
SELECT
  DATETIME(log_from, 'unixepoch') AS log_from,
  DATETIME(log_until, 'unixepoch') AS log_until,
  logfile,
  DATETIME(created_at, 'unixepoch') AS created_at,
  created_by,
  DATETIME(updated_at, 'unixepoch') AS updated_at,
  updated_by
FROM
  t_logfile
;


DROP VIEW IF EXISTS v_save_text;
CREATE VIEW IF NOT EXISTS v_save_text AS
SELECT
  DATETIME(save.log_at, 'unixepoch') AS log_at,
  save.save_text,
  log.logfile,
  DATETIME(save.created_at, 'unixepoch') AS created_at,
  save.created_by,
  DATETIME(save.updated_at, 'unixepoch') AS updated_at,
  save.updated_by
FROM
  t_save_text save
  INNER JOIN t_logfile log
    ON log.log_from = save.log_from
;
