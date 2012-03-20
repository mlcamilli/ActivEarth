@ECHO OFF

ECHO "Dropping the database: [ActivEarth_Dev] ..."
"%SqlToolsRoot%\sqlcmd.exe" -S localhost -Q "DROP DATABASE [ActivEarth_Dev]"
ECHO "    done."

IF "%1" NEQ "noprompt" PAUSE