# Updating and releasing the public GitHub repository

Official repository: https://github.com/Minerx117/Reach-Community-Client-

The current source package is v0.01.03. Extract the ZIP, then copy its contents into a fresh clone of the existing repository. Keep the repository's `.git` directory. Review `git status` and the file diff before committing.

```powershell
git clone https://github.com/Minerx117/Reach-Community-Client-.git
# Copy the extracted v0.01.03 source contents into the cloned folder.
cd Reach-Community-Client-
git status
git add -A
git commit -m "Update Reach Community Client to v0.01.03"
git push origin main
```

Wait for GitHub Actions CI to pass. To publish the RCC launcher release, create and push the tag from the updated `main` branch:

```powershell
git tag v0.01.03
git push origin v0.01.03
```

The Windows workflow compiles the Avalonia shell and the RCC-owned diagnostic C++ tool. All build steps are defined in this repository.
