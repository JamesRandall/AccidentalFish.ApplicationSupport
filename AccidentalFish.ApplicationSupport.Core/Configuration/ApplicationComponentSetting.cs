using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Application component setting
    /// </summary>
    public class ApplicationComponentSetting
    {
        /// <summary>
        /// Key / name for the setting
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value for the setting
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// If none null then the resource type the setting describes. Valiid values are topic, subscription, brokered-message-queue, 
        /// table, queue, blob-container
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Any additional attributes
        /// </summary>
        public IReadOnlyDictionary<string, string> Attributes { get; set; } 
    }
}
