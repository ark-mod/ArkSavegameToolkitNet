msbuild ArkSavegameToolkitNet.sln /p:Configuration=Release /p:OutputPath="C:/Temp/ArkSavegameToolkitNet/dist/Release/"
msbuild ArkSavegameToolkitNet.sln /p:Configuration=Debug /p:OutputPath="C:/Temp/ArkSavegameToolkitNet/dist/Debug/"

Nuget
---
Packaging
nuget pack ArkSavegameToolkitNet.Domain.csproj -Prop Configuration=Release -Symbols -SymbolPackageFormat snupkg -IncludeReferencedProjects

Push
nuget push ArkSavegameToolkitNet.<version>.nupkg -Source nuget.org