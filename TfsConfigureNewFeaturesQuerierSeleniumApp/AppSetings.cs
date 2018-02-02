using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConfigureNewFeaturesQuerierSeleniumApp
{
    public class AppSetings
    {
        public string Instance { get; set; }
        public string CollectionName { get; set; }
        public string Http { get; set; }
        public string ProjectNamesFilePath { get; set; }
        public int ImplicitWaitSeconds { get; set; }
        public int? MaxProjectsToScan { get; set; }
    }
}
