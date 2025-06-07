@echo off
CLS
ECHO #########################################
ECHO ##     TESTOWANIE ENDPOINTOW API     ##
ECHO #########################################
ECHO.

REM ============================================================================
REM  KROK 1: KONFIGURACJA TOKENU
REM ============================================================================
ECHO.
ECHO *** KROK 1: USTAWIENIE TOKENU ***
ECHO.

REM Edytuj poni¿sz¹ liniê i wklej token JWT uzyskany z endpointu /api/auth/login
SET "%TOKEN%"=="TUTAJ_WKLEJ_SWOJ_TOKEN"

IF "%TOKEN%"=="TUTAJ_WKLEJ_SWOJ_TOKEN" (
    ECHO !!! OSTRZEZENIE: Nie wklejono tokenu do skryptu!
    ECHO !!! Aby testy autoryzacji zadzialaly, najpierw zaloguj sie w Swaggerze,
    ECHO !!! skopiuj token, a nastepnie wyedytuj ten plik i wklej go w odpowiednie miejsce.
    ECHO.
) ELSE (
    ECHO Token zostal ustawiony.
)
ECHO.
pause


REM ============================================================================
REM  TEST 2: Pobieranie wszystkich postów (publiczny)
REM ============================================================================
ECHO.
ECHO *** TEST 2: Pobieranie wszystkich postów ***
ECHO.
curl -X GET "https://localhost:7270/api/posts" -k
ECHO.
ECHO.
pause


REM ============================================================================
REM  TEST 3: Tworzenie nowego posta (autoryzowany)
REM ============================================================================
ECHO.
ECHO *** TEST 3: Tworzenie nowego posta ***
ECHO.
curl -X POST "https://localhost:7270/api/posts" -H "Content-Type: application/json" -H "Authorization: Bearer %TOKEN%" -d "{\"title\":\"Post ze skryptu .bat\",\"content\":\"Treœæ posta dodanego przez skrypt\"}" -k
ECHO.
ECHO.
pause


REM ============================================================================
REM  TEST 4: Próba usuniêcia posta o ID 4 jako zwykly uzytkownik (oczekiwany blad)
REM Zak³adaj¹c, ¿e wklejony token nale¿y do TestUser, a nie TestAdmin
REM ============================================================================
ECHO.
ECHO *** TEST 4: Próba usuniêcia posta o ID 4 (oczekiwany blad 403 Forbidden) ***
ECHO.
curl -X DELETE "https://localhost:7270/api/posts/4" -H "Authorization: Bearer %TOKEN%" -k
ECHO.
ECHO.
ECHO UWAGA: Aby przetestowac poprawne usuwanie (jako Admin),
ECHO wklej na gorze skryptu token dla 'TestAdmin' i uruchom ponownie.
ECHO.
pause


ECHO.
ECHO Koniec testow. Nacisnij dowolny klawisz, aby zamknac to okno.
ECHO.