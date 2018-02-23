# TfsConfigureNewFeaturesQuerierSeleniumApp
This application will use Selenium to navigate to each Team Project within TFS and check if the project has any New Features to enable or not.

This application will navigate to the TFS Web Portal for a Team Project Collection and for each Project:

1. Navigate to Project Admin page
1. Click the Configure Features button
1. Click Verify button
1. Grab the name of the Assumed WIT and any potential Errors/Warnings

All of that data will be stored in an output file saved in the sub-folder 'Results' after completion.

Changes you will need to make:
- Open the 'settings.json' file and change the values that matter to you
  - instance: The endpoint of the TFS instance to look at. Note it needs to NOT include the 'http://' portion of the url. So only something like 'MyTFS:8080/tfs' is what you want.
  - collectionName: The name of the TFS Collection to look at
  - http: Value will either be 'http' or 'https'. Depending on if you use https or not.
  - projectNamesFilePath: Path to the file that lists all of the projects you want to look at. Project names are new-line delimited.
  - configureIfPossible: Attempt to configure the project if the project can be configured.
  - ImplicitWaitSeconds: Seconds to wait in case something doesn't automatically pop-up
  - MaxProjectsToScan: Maximum amount of projects to scan
- Open the 'projects.txt' file and replace the text within it with the names of the TFS Team Projects you want to check. Each project name on a new line.

