﻿--https://stackoverflow.com/questions/3930338/sql-server-get-table-primary-key-using-sql-query
SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE	OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA+'.'+QUOTENAME(CONSTRAINT_NAME)),'IsPrimaryKey')=1
AND		TABLE_NAME='Directors'