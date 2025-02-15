@echo off

:: Step 1: Create `Users` directory if it doesn't exist
set "UsersDir=Users"
if not exist "%UsersDir%" (
	mkdir "%UsersDir%"
	echo Created 'Users' directory.
) else (
	echo 'Users' directory already exists.
)

:: Step 2: Replace `lessons` directory with source
set "LessonsDir=lessons"
set "SourceLessonsDir=..\..\..\Data\lessons"

if exist "%LessonsDir%" (
	rmdir /s /q "%LessonsDir%"
	echo Deleted existing 'lessons' directory.
)

if exist "%SourceLessonsDir%" (
	xcopy "%SourceLessonsDir%" "%LessonsDir%\" /E /I
	echo Copied 'lessons' directory from source.
) else (
	echo Source 'lessons' directory does not exist.
)