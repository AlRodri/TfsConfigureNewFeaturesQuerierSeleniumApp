using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConfigureNewFeaturesQuerierSeleniumApp
{
    public class ConfigureFeaturesQueryReport
    {
        public ConfigureFeaturesQueryReport(ConfigureFeaturesQueryResults results)
        {
            Results = results;
            var congiurableNames = results.ConfigurableProjects.Select(x => x.WitName).ToList();
            congiurableNames.AddRange(results.UnconfigurableProjects.Select(x => x.WitName));
            DistinctWits = congiurableNames
                        .GroupBy(x => x)
                        .Select(x => new WitGroup { Count = x.Count(), Wit = x.Key })
                        .ToImmutableList();

            DistinctUnconfigurableErrors = results.UnconfigurableProjects.Select(x => x.ErrorsAndWarnings)
                                            .GroupBy(x => x)
                                            .Select(x => new UnconfigurableErrorsGroup { Count = x.Count(), ErrorsWarningsText = x.Key })
                                            .ToImmutableList();
        }

        public ConfigureFeaturesQueryResults Results { get; private set; }
        public ImmutableList<WitGroup> DistinctWits { get; private set; }
        public ImmutableList<UnconfigurableErrorsGroup> DistinctUnconfigurableErrors { get; private set; }
    }
}
