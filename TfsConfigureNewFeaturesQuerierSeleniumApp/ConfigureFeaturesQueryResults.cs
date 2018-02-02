using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConfigureNewFeaturesQuerierSeleniumApp
{
    public class ConfigureFeaturesQueryResults
    {
        public ConfigureFeaturesQueryResults()
        {
            AlreadyConfiguredProjects = new List<string>();
            ConfigurableProjects = new List<ConfigurableProject>();
            UnconfigurableProjects = new List<UnconfigurableProject>();
            ErroredProjects = new List<ErroredProject>();
        }

        /// <summary>
        /// Projects that already have all features configured
        /// </summary>
        public List<string> AlreadyConfiguredProjects { get; set; }
        
        public List<ConfigurableProject> ConfigurableProjects { get; set; }

        public List<UnconfigurableProject> UnconfigurableProjects { get; set; }

        /// <summary>
        /// Projects that had an error occur when trying to query
        /// </summary>
        public List<ErroredProject> ErroredProjects { get; set; }
    }
}
