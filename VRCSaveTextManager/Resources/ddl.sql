CREATE TABLE IF NOT EXISTS t_logfile (  /* Log file information */
  log_from INTEGER NOT NULL,  /* First log timestamp (UNIX Timestamp) */
  log_until INTEGER NOT NULL,  /* Last log timestamp (UNIX Timestamp) */
  logfile TEXT NOT NULL,  /* Log file name */
  created_at INTEGER NOT NULL DEFAULT (UNIXEPOCH('now', 'localtime')),  /* Record creation date and time (UNIX Timestamp) */
  created_by TEXT NOT NULL,  /* Record creator name */
  updated_at INTEGER NOT NULL DEFAULT (UNIXEPOCH('now', 'localtime')),  /* Record update time (UNIX Timestamp) */
  updated_by TEXT NOT NULL,  /* Record updater name */
  PRIMARY KEY (log_from)
);


CREATE TABLE IF NOT EXISTS t_save_text (  /* Save data text table */
  log_at INTEGER NOT NULL,  /* Log timestamp (UNIX Timestamp) */
  save_text TEXT NOT NULL,  /* Save data text */
  log_from INTEGER NOT NULL,  /* First log timestamp (UNIX Timestamp) */
  created_at INTEGER NOT NULL,  /* Record creation date and time (UNIX Timestamp) */
  created_by TEXT NOT NULL DEFAULT (UNIXEPOCH('now', 'localtime')),  /* Record creator name */
  updated_at INTEGER NOT NULL,  /* Record update time (UNIX Timestamp) */
  updated_by TEXT NOT NULL DEFAULT (UNIXEPOCH('now', 'localtime')),  /* Record updater name */
  PRIMARY KEY (log_at),
  FOREIGN KEY (log_from) REFERENCES t_logfile (log_from)
);

CREATE INDEX IF NOT EXISTS t_save_text_idx1 ON t_save_text (log_from);
