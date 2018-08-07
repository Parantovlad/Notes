#addin nuget:?package=Cake.Kudu.Client

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
			OutputDirectory = "./publish/Notes/",
			NoRestore = true
		};
		DotNetCorePublish("./Notes/Notes.csproj", settings);
		settings.OutputDirectory = "./publish/NotesIdentities/";
		DotNetCorePublish("./NotesIdentities/NotesIdentities.csproj", settings);
	});

Task("Deploy")
	.IsDependentOn("Publish")
	.Does(() => 
	{
		var kuduClient = KuduClient(
			 baseUriApp,
			 userNameApp,
			 passwordApp);
		var sourceDirectoryPath = "./publish/Notes/";
		var remoteDirectoryPath = "/site/wwwroot/";

		kuduClient.ZipUploadDirectory(
			sourceDirectoryPath,
			remoteDirectoryPath);

		kuduClient = KuduClient(
			 baseUriIdent,
			 userNameIdent,
			 passwordIdent);
		sourceDirectoryPath = "./publish/NotesIdentities/";
		remoteDirectoryPath = "/site/wwwroot/";

		kuduClient.ZipUploadDirectory(
			sourceDirectoryPath,
			remoteDirectoryPath);
	});

Task("Default")
	.IsDependentOn("Deploy")
  .Does(() =>
  {
    Information("Your build is done :-)");
  });

RunTarget(target);