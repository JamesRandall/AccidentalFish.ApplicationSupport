using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    public class ApplicationComponentSetting
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string ResourceType { get; set; }

        public IReadOnlyDictionary<string, string> Attributes { get; set; } 
    }
}
