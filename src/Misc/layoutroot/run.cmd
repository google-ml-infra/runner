:: Copyright 2026 Google LLC
::
:: Licensed under the Apache License, Version 2.0 (the "License");
:: you may not use this file except in compliance with the License.
:: You may obtain a copy of the License at
::
::     https://www.apache.org/licenses/LICENSE-2.0
::
:: Unless required by applicable law or agreed to in writing, software
:: distributed under the License is distributed on an "AS IS" BASIS,
:: WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
:: See the License for the specific language governing permissions and
:: limitations under the License.

@echo off

rem ********************************************************************************
rem Unblock specific files.
rem ********************************************************************************
setlocal
if defined VERBOSE_ARG (
  set VERBOSE_ARG='Continue'
) else (
  set VERBOSE_ARG='SilentlyContinue'
)

rem Unblock files in the root of the layout folder. E.g. .cmd files.
powershell.exe -NoLogo -Sta -NoProfile -NonInteractive -ExecutionPolicy Unrestricted -Command "$VerbosePreference = %VERBOSE_ARG% ; Get-ChildItem -LiteralPath '%~dp0' | ForEach-Object { Write-Verbose ('Unblock: {0}' -f $_.FullName) ; $_ } | Unblock-File | Out-Null"


rem ********************************************************************************
rem Run.
rem ********************************************************************************

:launch_helper
copy "%~dp0run-helper.cmd.template" "%~dp0run-helper.cmd" /Y
call "%~dp0run-helper.cmd" %*
  
if %ERRORLEVEL% EQU 1 (
  echo "Restarting runner..."
  goto :launch_helper
)

if "%ACTIONS_RUNNER_RETURN_VERSION_DEPRECATED_EXIT_CODE%"=="1" (
  if %ERRORLEVEL% EQU 7 (
    echo "Exiting runner with deprecated version error code: %ERRORLEVEL%"
    exit /b %ERRORLEVEL%
  )
)

echo "Exiting runner..."
exit /b 0
