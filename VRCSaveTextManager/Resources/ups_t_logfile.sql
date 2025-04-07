INSERT INTO t_logfile (
  log_from,
  log_until,
  logfile,
  created_at,
  created_by,
  updated_at,
  updated_by
)
VALUES (
  @log_from,
  @log_until,
  @logfile,
  UNIXEPOCH('now', 'localtime'),
  @edit_by,
  UNIXEPOCH('now', 'localtime'),
  @edit_by
)
ON CONFLICT (log_from)
DO UPDATE SET
  log_until = EXCLUDED.log_until,
  logfile = EXCLUDED.logfile,
  updated_at = EXCLUDED.updated_at,
  updated_by = EXCLUDED.updated_by
WHERE
  log_until IS DISTINCT FROM EXCLUDED.log_until
  OR logfile IS DISTINCT FROM EXCLUDED.logfile
;
