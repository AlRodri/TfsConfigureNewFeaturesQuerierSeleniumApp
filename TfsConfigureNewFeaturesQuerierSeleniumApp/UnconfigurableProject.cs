namespace TfsConfigureNewFeaturesQuerierSeleniumApp
{
    /// <summary>
    /// Project that can't have the new features configured yet becuase of errors
    /// </summary>
    public class UnconfigurableProject
    {
        public string Name { get; set; }
        public string WitName { get; set; }
        public string ErrorsAndWarnings { get; set; }
    }
}