echo "=== PUBLISHING ==="
call %~dp0publish.bat

echo "=== RESOLVE ADDITIONAL DEPENDENCIES ==="
call %~dp0copyDeps.bat

echo "=== COMPRESSING PORTABLE ARCHIVE ==="
call %~dp07zipDist.bat

echo "=== COMPILING DISTRIBUTIVE ==="
call %~dp0nsisDist.bat
