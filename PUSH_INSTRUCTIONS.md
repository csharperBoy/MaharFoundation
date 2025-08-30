Follow these steps locally to finish repository creation and push to GitHub:

1. Initialize git and set main branch

```powershell
git init
git branch -M main
```

2. Add remote origin (replace <your-username>)

```powershell
git remote add origin https://github.com/<your-username>/MaharFoundation.git
```

3. Add files and commit

```powershell
git add .
git commit -m "feat: initial project structure

- ASP.NET Core 9 backend with modular architecture
- React 18 frontend with TypeScript
- Clean architecture foundation
- CI/CD ready structure"
```

4. Push main and create develop branch

```powershell
git push -u origin main
git checkout -b develop
git push -u origin develop
```

5. In GitHub repository settings enable branch protection, Issues, Discussions, Projects, Wiki, and Security features as required.
