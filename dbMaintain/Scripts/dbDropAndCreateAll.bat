@ECHO OFF

ECHO "Dropping the database: [ActivEarth_Dev] ..."
"%SqlToolsRoot%\sqlcmd.exe" -S localhost -Q "DROP DATABASE [ActivEarth_Dev]"

ECHO "Creating the database: [ActivEarth_Dev] ..."
"%SqlToolsRoot%\sqlcmd.exe" -S localhost -Q "CREATE DATABASE [ActivEarth_Dev]"
ECHO "Inserting tables, stored procedures, and test data..."
%MSBuildRoot%\msbuild.exe "database.deploy.msbuild" /t:DbDeploy /p:ScriptDirectory=Create /p:ScriptRunOrderFilename=script_run_order.txt
ECHO "    done."

IF "%1" NEQ "noprompt" PAUSE