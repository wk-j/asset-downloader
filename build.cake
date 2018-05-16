#addin "wk.StartProcess"
#addin "wk.ProjectParser"

using PS = StartProcess.Processor;
using ProjectParser;

var name = "AssetDownloader";
var project = $"src/{name}/{name}.fsproj";
var info = Parser.Parse(project);
var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var currentDir = new System.IO.DirectoryInfo(".").FullName;

Task("Publish").Does(() => {
    CleanDirectory("publish");
    DotNetCorePublish(project, new DotNetCorePublishSettings {
        OutputDirectory = "publish/out"
    });
});

Task("Pack").Does(() => {
    CleanDirectory("publish");
    DotNetCorePack(project, new DotNetCorePackSettings {
        OutputDirectory = "publish"
    });
});

Task("Publish-Nuget")
    .IsDependentOn("Pack")
    .Does(() => {
        var npi = EnvironmentVariable("npi");
        var nupkg = new DirectoryInfo("publish").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
});

Task("Uninstall").Does(() => {
    PS.StartProcess($"dotnet tool uninstall -g wk.{name}");
});

Task("Install")
    .IsDependentOn("Pack")
    .Does(() => {
        Information(info.Version);
        PS.StartProcess($"dotnet tool install -g wk.{name} --source-feed {currentDir}/publish --version {info.Version}");
});

Task("Reinstall")
    .IsDependentOn("Uninstall")
    .IsDependentOn("Install");

var target = Argument("target", "default");
RunTarget(target);