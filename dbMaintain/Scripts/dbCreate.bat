@ECHO OFF

ECHO "Creating the database: [ActivEarth_Dev] ..."
"%SqlToolsRoot%\sqlcmd.exe" -S localhost -Q "CREATE DATABASE [ActivEarth_Dev]"
ECHO "    done."

IF "%1" NEQ "noprompt" PAUSE
