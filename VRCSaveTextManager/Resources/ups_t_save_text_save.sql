INSERT INTO t_save_text (
  log_at,
  save_text,
  log_from,
  created_at,
  created_by,
  updated_at,
  updated_by
)
VALUES (
  @log_at,
  @save_text,
  @log_from,
  UNIXEPOCH('now', 'localtime'),
  @edit_by,
  UNIXEPOCH('now', 'localtime'),
  @edit_by
)
ON CONFLICT (log_at)
DO UPDATE SET
  save_text = EXCLUDED.save_text,
  log_from = EXCLUDED.log_from,
  updated_at = EXCLUDED.updated_at,
  updated_by = EXCLUDED.updated_by
WHERE
  save_text IS DISTINCT FROM EXCLUDED.save_text
  OR log_from IS DISTINCT FROM EXCLUDED.log_from
;
