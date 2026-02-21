param(
    [string]$remoteUrl
)

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "Git is not installed or not in PATH. Install Git first: https://git-scm.com/downloads" -ForegroundColor Red
    exit 1
}

Set-Location -Path (Split-Path -Parent $MyInvocation.MyCommand.Definition)
Push-Location -ErrorAction SilentlyContinue

if (-not (Test-Path .git)) {
    git init
    git add --all
    git commit -m "Initial commit"
}

if (Get-Command gh -ErrorAction SilentlyContinue) {
    Write-Host "Detected GitHub CLI (gh). Creating repository and pushing..." -ForegroundColor Cyan
    gh repo create --public --source=. --remote=origin --push
    if ($LASTEXITCODE -ne 0) { Write-Host "gh repo create failed. If you prefer, provide a remote URL via -remoteUrl." -ForegroundColor Yellow }
    exit $LASTEXITCODE
}

if ($remoteUrl) {
    git remote remove origin -ErrorAction SilentlyContinue
    git remote add origin $remoteUrl
    git branch -M main
    git push -u origin main
    exit $LASTEXITCODE
}

Write-Host "No GitHub CLI and no -remoteUrl provided." -ForegroundColor Yellow
Write-Host "Option A (recommended): install GitHub CLI and run this script again to auto-create the repo." -ForegroundColor Gray
Write-Host "Option B: create an empty repo on GitHub and re-run: .\scripts\push_to_github.ps1 -remoteUrl 'https://github.com/youruser/yourrepo.git'" -ForegroundColor Gray
