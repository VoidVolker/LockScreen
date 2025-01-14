;NSIS Modern User Interface
;Multilingual Example Script
;Written by Joost Verburg

!pragma warning error all

;--------------------------------
;Compression

  SetCompressor /SOLID lzma
  SetCompressorDictSize 64
  SetDatablockOptimize ON

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;Include other modules

  ; !include x64.nsh
  ; ${If} ${RunningX64}
  ; ${Else}
  ; ${EndIf}

;--------------------------------
;General

  !define SHORT_APP_NAME "LockScreen"
  !define COMPANY_NAME "VoidVolker"
  !define APP_NAME "Lock Screen"
  ;Properly display all languages (Installer will not work on Windows 95, 98 or ME!)
  Unicode true

  ;Name and file
  Name "Lock Screen ${PLATFORM_ID}"
  OutFile "..\..\LockScreen\bin\dist\LockScreen.${PLATFORM_ID}.exe"

  ;Default installation folder
  ; InstallDir "$LOCALAPPDATA\${COMPANY_NAME}\${SHORT_APP_NAME}"
  InstallDir "${PLATFORM_PROGRAMFILES}\${COMPANY_NAME}\${SHORT_APP_NAME}"
  !define LOGON_SERVICE_EXE "$INSTDIR\LogonService\LogonService.exe"

  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "Software\${COMPANY_NAME}\${SHORT_APP_NAME}" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

  ;DPI scaling fix
  ManifestDPIAware True

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

  ;Show all languages, despite user's codepage
  !define MUI_LANGDLL_ALLLANGUAGES

;--------------------------------
;Language Selection Dialog Settings
  !define LANG_EN 1033
  !define LANG_RU 1049

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKLM"
  !define MUI_LANGDLL_REGISTRY_KEY "Software\${COMPANY_NAME}\${SHORT_APP_NAME}"
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

  !define I18n_License "License text"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
  ; !insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"
  !insertmacro MUI_PAGE_LICENSE $(I18n_License)
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  !define MUI_FINISHPAGE_RUN "$INSTDIR\LockScreen.exe"
  !define MUI_FINISHPAGE_RUN_TEXT $(I18n_StartLockScreen)
  !insertmacro MUI_PAGE_FINISH

  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  ; !insertmacro MUI_UNPAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"
  !insertmacro MUI_UNPAGE_LICENSE $(I18n_License)
  !insertmacro MUI_UNPAGE_COMPONENTS
  !insertmacro MUI_UNPAGE_DIRECTORY
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "English" ; The first language is the default language
  !insertmacro MUI_LANGUAGE "Russian"

  LicenseLangString   I18n_License              ${LANG_ENGLISH} "I18n\en-us\license.txt"
  LangString          I18n_SectionLockScreen    ${LANG_ENGLISH} "Application main files"
  LangString          I18n_SectionLogonService  ${LANG_ENGLISH} "Install system service 'Logon Service'"
  LangString          I18n_StartLockScreen      ${LANG_ENGLISH} "Start Lock Screen"

  LicenseLangString   I18n_License              ${LANG_RUSSIAN} "I18n\ru-ru\license.txt"
  LangString          I18n_SectionLockScreen    ${LANG_RUSSIAN} "Основные файлы"
  LangString          I18n_SectionLogonService  ${LANG_RUSSIAN} "Установить системный сервис 'Logon Service'"
  LangString          I18n_StartLockScreen      ${LANG_RUSSIAN} "Запустить Lock Screen"

;--------------------------------
;Reserve Files

  ;If you are using solid compression, files that are required before
  ;the actual installation should be stored first in the data block,
  ;because this will make your installer start faster.

  !insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections

;Application main files
Section "Lock Screen" SectionLockScreen

  ;Readonly section
  SectionIn RO

  SetOutPath "$INSTDIR"
  ;ADD YOUR OWN FILES HERE...
  IfFileExists "${LOGON_SERVICE_EXE}" 0 +2
  ExecWait '"${LOGON_SERVICE_EXE}" /stop'

  FILE /r "..\..\LockScreen\bin\publish\${PLATFORM_ID}\*"

  ;Store installation folder
  WriteRegStr HKLM "Software\${COMPANY_NAME}\${SHORT_APP_NAME}" "" $INSTDIR

  CreateDirectory '$SMPROGRAMS\${APP_NAME}'
  CreateShortCut '$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk' '$INSTDIR\LockScreen.exe' "" '$INSTDIR\LockScreen.exe' 0
  CreateShortCut '$SMPROGRAMS\${APP_NAME}\Uninstall ${APP_NAME}.lnk' '$INSTDIR\Uninstall.exe' "" '$INSTDIR\Uninstall.exe' 0

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;Logon service installation
Section "Logon Service" SectionLogonService

  SetOutPath "$INSTDIR"

  ;Install logon service
  ; ExecWait '"${LOGON_SERVICE_EXE}" /install'
  nsExec::ExecToStack '"${LOGON_SERVICE_EXE}" /install'
  Pop $0 # return value/error/timeout
  Pop $1 # printed text, up to ${NSIS_MAX_STRLEN}
  DetailPrint '"${LOGON_SERVICE_EXE}" /install result:'
  DetailPrint "$1"
  DetailPrint ""
  DetailPrint "       Return value: $0"
  DetailPrint ""

  ;Start logon service if it was already installed
  ; ExecWait '"${LOGON_SERVICE_EXE}" /start'
  nsExec::ExecToStack '"${LOGON_SERVICE_EXE}" /start'
  Pop $0 # return value/error/timeout
  Pop $1 # printed text, up to ${NSIS_MAX_STRLEN}
  DetailPrint '"${LOGON_SERVICE_EXE}" /start result:'
  DetailPrint "$1"
  DetailPrint "       Return value: $0"
  DetailPrint ""

SectionEnd


;--------------------------------
;Installer Functions

Function .onInit
  ;Turn on logger
  LogSet on

  !insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd

;--------------------------------
;Descriptions

  ;USE A LANGUAGE STRING IF YOU WANT YOUR DESCRIPTIONS TO BE LANGUAGE SPECIFIC

  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionLockScreen} $(I18n_SectionLockScreen)
    !insertmacro MUI_DESCRIPTION_TEXT ${SectionLogonService} $(I18n_SectionLogonService)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END


;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...

  ;Stop logon service
  ExecWait '"$INSTDIR\LogonService\LogonService.exe" /uninstall' $0
  SetErrorLevel $0

  Delete '$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk'
  Delete '$SMPROGRAMS\${APP_NAME}\Uninstall ${APP_NAME}.lnk'

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKLM "Software\${COMPANY_NAME}\${SHORT_APP_NAME}"

SectionEnd

;--------------------------------
;Uninstaller Functions

Function un.onInit

  !insertmacro MUI_UNGETLANGUAGE

FunctionEnd
