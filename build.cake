#addin nuget:?package=Cake.Kudu.Client
#addin nuget:?package=Cake.WebDeploy

string  baseUriApp     = EnvironmentVariable("KUDU_CLIENT_BASEURI_APP"),
        userNameApp    = EnvironmentVariable("KUDU_CLIENT_USERNAME_APP"),
		    passwordApp    = EnvironmentVariable("KUDU_CLIENT_PASSWORD_APP"),
        baseUriIdent   = EnvironmentVariable("KUDU_CLIENT_BASEURI_IDENT"),
        userNameIdent  = EnvironmentVariable("KUDU_CLIENT_USERNAME_IDENT"),
		    passwordIdent  = EnvironmentVariable("KUDU_CLIENT_PASSWORD_IDENT");;

var target = Argument("target", "Default");

Task("Clean")
  .Does(() =>
  {	
    DotNetCoreClean("./Notes.sln");
    CleanDirectory("./publish/");
  });

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() => {
		DotNetCoreRestore("./Notes.sln");
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() => 
	{
		var settings = new DotNetCoreBuildSettings
		{
			NoRestore = true,
			Configuration = "Release"
		};
		DotNetCoreBuild("./Notes.sln", settings);
	});

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		var settings = new DotNetCoreTestSettings
		{
			NoBuild = true,
			Configuration = "Release",
			NoRestore = true
		};
		var testProjects = GetFiles("./**/*.Tests.csproj");
		foreach(var project in testProjects)
		{
				DotNetCoreTest(project.FullPath, settings);
		}
	});

Task("Publish")
	.IsDependentOn("Test")
	.Does(() => 
	{
		var settings = new DotNetCorePublishSettings
		{
			Configuration = "Release",
			OutputDirectory = "../ReactNotes/",
			NoRestore = true
		};
		DotNetCorePublish("./Notes/Notes.csproj", settings);
		settings.OutputDirectory = "../ReactNotesIdentities/";
		DotNetCorePublish("./NotesIdentities/NotesIdentities.csproj", settings);
	});

Task("Deploy")
	.IsDependentOn("Publish")
	.Does(() => 
	{
		DeployWebsite(new DeploySettings()
        {
            SourcePath = "../ReactNotes/",
            SiteName = userNameApp,
            ComputerName = "https://" + baseUriApp + "/msdeploy.axd?site=" + userNameApp,
            Username = "$" + userNameApp,
            Password = passwordApp
        });

		DeployWebsite(new DeploySettings()
        {
            SourcePath = "../ReactNotesIdentities/",
            SiteName = userNameIdent,
            ComputerName = "https://" + baseUriIdent + "/msdeploy.axd?site=" + userNameIdent,
            Username = "$" + userNameIdent,
            Password = passwordIdent
        });
	});

Task("Default")
	.IsDependentOn("Deploy")
  .Does(() =>
  {
    Information("Your build is done :-)");
  });

RunTarget(target);