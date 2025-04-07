SELECT
  log_at
FROM
  t_save_text save
  INNER JOIN t_logfile log
    ON log.log_from = save.log_from
    AND log.logfile = @logfile
ORDER BY
  save.log_at DESC
;
