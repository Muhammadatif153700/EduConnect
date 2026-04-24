dotnet new gitignore
git init
git add .
$commitMessage = "Initial commit: EduConnect Blazor Portal`n`nCompleted all 5 modules including Admin, Faculty, and Student workflows.`n`nCo-authored-by: Mubashir0301 <Mubashir0301@users.noreply.github.com>`nCo-authored-by: Muhammadatif153700 <Muhammadatif153700@users.noreply.github.com>`nCo-authored-by: Abdul-Haxeeb <Abdul-Haxeeb@users.noreply.github.com>"
git commit -m $commitMessage
git branch -M main
git remote add origin https://github.com/Mubashir0301/EduConnect.git
git push -u origin main
