#addin nuget:?package=Cake.Npm
#addin "Cake.Docker"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////



//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var buildDir = Directory("./_build");
var projectRuntime = "ubuntu.16.04-x64";
var projectDir = Directory("./src/AniSync/");
var dockerTag = "anisync";

//////////////////////////////////////////////////////////////////////
// BUILD
//////////////////////////////////////////////////////////////////////

Task("Gulp").Does(() => {
    NpmInstall(new NpmInstallSettings {
		LogLevel = NpmLogLevel.Silent,
		WorkingDirectory = projectDir,
		Production = false
	});

    NpmRunScript(new NpmRunScriptSettings {
		LogLevel = NpmLogLevel.Silent,
        ScriptName = "prod",
		WorkingDirectory = projectDir
    });
});

Task("Publish").Does(() => {
    DotNetCoreRestore(projectDir, new DotNetCoreRestoreSettings
    {
        Verbosity = DotNetCoreVerbosity.Minimal
    });

    DotNetCorePublish(projectDir, new DotNetCorePublishSettings
    {
        Runtime = projectRuntime,
        SelfContained = true,
        Configuration = "Release",
        OutputDirectory = buildDir,
        Verbosity = DotNetCoreVerbosity.Minimal
    });
});

//////////////////////////////////////////////////////////////////////
// BUILD DOCKER
//////////////////////////////////////////////////////////////////////

Task("DockerBuildImage").Does(() => {
    DockerBuild(new DockerImageBuildSettings {
        BuildArg = new string[] {
            "BUILD_DATE=" + DateTimeOffset.UtcNow.ToString("u"),
            "VERSION=todo"
        }
    }, ".");
});

Task("DockerPublishImage").Does(() => {
    if (AppVeyor.IsRunningOnAppVeyor)
    {
        Information(
            @"Environment:
            ApiUrl: {0}
            Configuration: {1}
            JobId: {2}
            JobName: {3}
            Platform: {4}
            ScheduledBuild: {5}",
            AppVeyor.Environment.ApiUrl,
            AppVeyor.Environment.Configuration,
            AppVeyor.Environment.JobId,
            AppVeyor.Environment.JobName,
            AppVeyor.Environment.Platform,
            AppVeyor.Environment.ScheduledBuild
        );
    }
    else
    {
        Information("Not running on AppVeyor");
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Build")
	.IsDependentOn("Gulp")
	.IsDependentOn("Publish");

Task("Docker")
    .IsDependentOn("DockerBuildImage")
    .IsDependentOn("DockerPublishImage");


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget("Build");
RunTarget("Docker");