using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System.IO;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace TfsConfigureNewFeaturesQuerierSeleniumApp
{
    class Program
    {
        private static AppSetings AppSettings;

        static void Main(string[] args)
        {
            InitAppSettings();
            var rootUrl = AppSettings.Http + "://" + AppSettings.Instance;
            var collectionUrl = rootUrl + "/" + AppSettings.CollectionName + "/";

            var configureIfPossible = bool.Parse(AppSettings.ConfigureIfPossible);
            
            var projectNames = LoadListOfProjectNames();
            var genericProjectAdminUrl = collectionUrl + "{0}/_admin";

            var webDriver = new ChromeDriver();
            var results = new ConfigureFeaturesQueryResults();

            try
            {
                DeleteExistingResultsFolder();

                //Give you time to log in, in case you need to
                NavigateToCollectionHomepage(webDriver, rootUrl);

                foreach (var projectName in projectNames.Take(AppSettings.MaxProjectsToScan ?? int.MaxValue))
                {
                    try
                    {
                        var projectAdminUrl = string.Format(genericProjectAdminUrl, projectName);
                        webDriver.Navigate().GoToUrl(projectAdminUrl);

                        var configureFeaturesLink = SafeFindElement(() => webDriver.FindElementByLinkText("Configure features"));

                        if (configureFeaturesLink == null)
                        {
                            results.AlreadyConfiguredProjects.Add(projectName);
                            SaveScreenshot(webDriver, $@".\Results\AlreadyConfigured\{projectName}.png");
                        }
                        else
                        {
                            configureFeaturesLink.Click();
                            webDriver.FindElementByXPath("//div/button/span[contains(text(), 'Verify')]").Click();

                            var configureButton = SafeFindElement(() => webDriver.FindElementByXPath("//div/button/span[contains(text(), 'Configure')]"));
                            var witName = SafeFindElement(() => webDriver.FindElementByXPath("//div/p/span[@class='templateName']"));

                            if (configureButton != null)
                            {
                                //We have the Configure button, so verification was successful and we have the ability to try and configure this
                                var configurableProj = new ConfigurableProject
                                {
                                    Name = projectName,
                                    WitName = witName?.Text ?? "UNKNOWN"
                                };
                                results.ConfigurableProjects.Add(configurableProj);
                                SaveScreenshot(webDriver, $@".\Results\Configurable\{projectName}.png");

                                if (configureIfPossible)
                                {
                                    configureButton.Click();
                                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                                    SaveScreenshot(webDriver, $@".\Results\Configurable\{projectName}-AfterConfiguration.png");
                                }
                            }
                            else
                            {
                                //Save off the list of warnings/errors stopping this project from being fully configured
                                var errorTextElm = SafeFindElement(() => webDriver.FindElementById("issues-textarea-id"));
                                string errorText = null;
                                if (errorTextElm != null)
                                {
                                    var getErrorsTextJS = "return document.getElementById('issues-textarea-id').value;";
                                    errorText = webDriver.ExecuteScript(getErrorsTextJS).ToString();
                                }

                                var unconfigurableProj = new UnconfigurableProject
                                {
                                    Name = projectName,
                                    WitName = witName?.Text ?? "UNKNOWN",
                                    ErrorsAndWarnings = errorText ?? "NULL"
                                };
                                results.UnconfigurableProjects.Add(unconfigurableProj);
                                SaveScreenshot(webDriver, $@".\Results\Unconfigurable\{projectName}.png");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var erroredProject = new ErroredProject
                        {
                            Name = projectName,
                            ErrorMessage = ex.Message
                        };

                        results.ErroredProjects.Add(erroredProject);
                        SaveScreenshot(webDriver, $@".\Results\Errored\{projectName}.png");
                    }
                }
            }
            finally
            {
                webDriver.Close();
                webDriver.Dispose();
            }

            WriteOutputFiles(results);
        }

        private static void InitAppSettings()
        {
            var settingsJson = File.ReadAllText(@".\settings.json");
            Program.AppSettings = JsonConvert.DeserializeObject<AppSetings>(settingsJson);
        }

        private static IEnumerable<string> LoadListOfProjectNames()
        {
            return File.ReadLines(AppSettings.ProjectNamesFilePath);
        }

        private static IWebElement SafeFindElement(Func<IWebElement> findElementFunc)
        {
            try
            {
                return findElementFunc();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void DeleteExistingResultsFolder()
        {
            if (Directory.Exists(@".\Results"))
            {
                Directory.Delete(@".\Results", true);
            }
        }

        private static void WriteOutputFiles(ConfigureFeaturesQueryResults results)
        {
            var report = new ConfigureFeaturesQueryReport(results);
            var reportText = Newtonsoft.Json.JsonConvert.SerializeObject(report, Formatting.Indented);
            File.WriteAllText(@".\Results\Report.json", reportText);
        }

        private static void NavigateToCollectionHomepage(ChromeDriver webDriver, string collectionUrl)
        {
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(5);
            webDriver.Navigate().GoToUrl(collectionUrl);
            webDriver.FindElementByXPath("//div/header/h1[contains(text(), 'Projects')]").Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(AppSettings.ImplicitWaitSeconds);
        }

        private static void SaveScreenshot(RemoteWebDriver webDriver, string outputScreenshotPath)
        {
            var erroredScreenshot = webDriver.GetScreenshot();

            var otuputFileDirPath = Path.GetDirectoryName(outputScreenshotPath);
            if (!Directory.Exists(otuputFileDirPath))
            {
                Directory.CreateDirectory(otuputFileDirPath);
            }

            erroredScreenshot.SaveAsFile(outputScreenshotPath, ScreenshotImageFormat.Png);
        }
    }
}

