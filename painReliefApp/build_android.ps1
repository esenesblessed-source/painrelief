param(
    [string]$unityPath = $env:UNITY_EDITOR_PATH,
    [string]$projectPath = "$(Resolve-Path .)"
)

if (-not $unityPath) {
    Write-Host "Please set the path to Unity Editor executable via -unityPath or the UNITY_EDITOR_PATH environment variable." -ForegroundColor Yellow
    Write-Host "Example: ./build_android.ps1 -unityPath 'C:\\Program Files\\Unity\\Hub\\Editor\\2022.3.##\\Editor\\Unity.exe'" -ForegroundColor Gray
    exit 1
}

$unityExe = $unityPath
Write-Host "Using Unity: $unityExe"
Write-Host "Project: $projectPath"

if (-not (Test-Path $unityExe)) {
    Write-Host "Unity executable not found at path: $unityExe" -ForegroundColor Yellow
    Write-Host "Searching common install locations..." -ForegroundColor Gray
    $candidates = @()
    try {
        $candidates += Get-ChildItem -Path 'C:\Program Files\Unity\Hub\Editor\*\Editor\Unity.exe' -File -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName
        $candidates += Get-ChildItem -Path 'C:\Program Files (x86)\Unity\Hub\Editor\*\Editor\Unity.exe' -File -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName
        $candidates += Get-ChildItem -Path 'C:\Program Files\Unity\Editor\Unity.exe' -File -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName
        $candidates += Get-ChildItem -Path 'C:\Program Files' -Filter Unity.exe -Recurse -File -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName
    } catch {
        # ignore errors during search
    }
    $candidates = $candidates | Where-Object { $_ } | Sort-Object -Unique
    if ($candidates.Count -gt 0) {
        Write-Host "Found Unity installations:" -ForegroundColor Green
        $candidates | ForEach-Object { Write-Host " - $_" }
        $unityExe = $candidates[0]
        Write-Host "Auto-selecting Unity executable: $unityExe" -ForegroundColor Cyan
    } else {
        Write-Host "No Unity executable found. Please install Unity or provide the correct path using -unityPath." -ForegroundColor Red
        Write-Host "Example: .\build_android.ps1 -unityPath 'C:\Program Files\Unity\Hub\Editor\2022.3.22f1\Editor\Unity.exe' -projectPath 'C:\path\to\project'" -ForegroundColor Gray
        exit 1
    }
}

$args = @(
    "-batchmode",
    "-quit",
    "-projectPath", $projectPath,
    "-executeMethod", "AndroidAutoBuilder.BuildAndroidAPK",
    "-logFile", "build_android.log"
)

Write-Host "Running Unity in batch mode to build Android APK/AAB..."
try {
    & "$unityExe" @args
    $exit = $LASTEXITCODE
} catch {
    Write-Host "Failed to start Unity executable: $_" -ForegroundColor Red
    exit 1
}
if ($exit -eq 0) { Write-Host "Unity process finished (exit code 0). Check Builds/Android for outputs." -ForegroundColor Green }
else { Write-Host "Unity process exited with code $exit. Check build_android.log for details." -ForegroundColor Red }
exit $exit
