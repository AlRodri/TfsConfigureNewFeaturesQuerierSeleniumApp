# TfsConfigureNewFeaturesQuerierSeleniumApp
This application will use Selenium to navigate to each Team Project within TFS and check if the project has any New Features to enable or not.

This application will navigate to the TFS Web Portal for a Team Project Collection and for each Project:

1. Navigate to Project Admin page
1. Click the Configure Features button
1. Click Verify button
1. Grab the name of the Assumed WIT and any potential Errors/Warnings

All of that data will be stored in an output file saved in the sub-folder 'Results' after completion.

In order to get started, open the 'settings.json' file and modify the 'instance' property to point to your on-prem TFS instance. Then make changes to any other properties that you may need to change.

