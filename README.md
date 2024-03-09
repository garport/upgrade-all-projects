# upgrade-all-projects
A .NET tool that runs the Upgrade Assistant against all projects under a given source directory.
## Prequisites
.NET Upgrade Assistant must already be installed 
```dotnet tool install --global upgrade-assistant```

### Install
```dotnet tool install --global Gport.UpgradeAllProjects```

### Usage 
```upgrade-all-projects sdk --sourceDirectory <path>```