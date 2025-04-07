UPDATE t_logfile
SET
  log_until = @log_until,
  logfile = @logfile,
  updated_at = UNIXEPOCH('now', 'localtime'),
  updated_by = @edit_by
WHERE
  log_from = @log_from
;
