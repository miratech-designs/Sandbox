6 Tasks for Every Project

1. Solution Level .editorconfig
    * Apply code styling for all files and projects within the solution.
    * [https://github.com/dotnet/runtime/blo...](https://github.com/dotnet/runtime)
    * [custome editorconfig](https://gist.github.com/m-jovanovic/417b7d0a641d7dd7d1972550fba298db )
2. Build Configuration
    * Use Directory.Build.props xml file.
    * <packagereference /> properties will apply to all projects (good for code quality packages like Sonar).
3. Central Package Management
    * Use Directory.Packages.props xml file.
    * Set up central package management for the solution.
    * <packageversion /> properties will contain full reference with version.
    * In csproj file, <packagereference /> properties will contain reference without the version.
4. Code Quality
    * Add config in the directory.build.props file to include code quality and set build to break on warnings.
    * Override the warnings in the IDE, and it will add to the editorconfig file.
5. Container Support
    * Available from the solution by right-clicking “add docker support”.
    * Options: Docker Compose or Azure Aspire.
6. CI/CD
    * Use GitHub Actions or other CI/CD tools. 
9. 
