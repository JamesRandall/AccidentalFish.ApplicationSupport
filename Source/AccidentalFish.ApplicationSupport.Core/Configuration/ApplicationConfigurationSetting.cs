namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Represents an application setting
    /// </summary>
    public class ApplicationConfigurationSetting
    {
        /// <summary>
        /// The key of the setting
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The value of the setting
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Does the setting represent a secret?
        /// </summary>
        public bool IsSecret { get; set; }
    }
}
