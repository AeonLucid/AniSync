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
var dockerPublisher = "AeonLucid";
var dockerTagAppveyor = "anisync-appveyor";
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
        },
        Tag = new string[] {
            "anisync-appveyor"
        }
    }, ".");
});

Task("DockerPublishImage").Does(() => {
    // Publish requirements
    //  - Running on AppVeyor
    //      - not a pull request
    //      - master (branch)
    //          - tagged commit
    //      - develop (branch)

    if (!AppVeyor.IsRunningOnAppVeyor) {
        Warning("Skipping DockerPublishImage because we are not building on AppVeyor.");
        return;
    }

    if (AppVeyor.Environment.PullRequest.IsPullRequest) {
        Warning("Skipping DockerPublishImage because this is a pull request.");
        return;
    }

    var branch = AppVeyor.Environment.Repository.Branch;
    var publishTags = new List<string>();
    var publishAllowed = false;

    switch (branch)
    {
        case "master":
            if (!AppVeyor.Environment.Repository.Tag.IsTag) {
                Warning($"Skipping DockerPublishImage on master because the commit is not tagged.");
                break;
            }

            publishTags.Add(dockerTag + ":latest");
            publishTags.Add(dockerTag + ":" + AppVeyor.Environment.Repository.Tag.Name);
            publishAllowed = true;
            break;
        
        case "develop":
            publishTags.Add(dockerTag + ":develop");
            publishAllowed = true;
            break;

        default:
            Warning($"Skipping DockerPublishImage because of an unknown branch '{branch}'.");
            break;
    }

    publishTags.Add(dockerTag + ":develop");
    publishAllowed = true;

    if (publishAllowed) {
        DockerLogin(
            EnvironmentVariable("DOCKER_USER"),
            EnvironmentVariable("DOCKER_PASS")
        );

        foreach (var publishTag in publishTags)
        {
            var dockerImageWithTag = $"{dockerPublisher}/{publishTag}";

            DockerTag(dockerTagAppveyor, dockerImageWithTag);
            DockerPush(dockerImageWithTag);
        }
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