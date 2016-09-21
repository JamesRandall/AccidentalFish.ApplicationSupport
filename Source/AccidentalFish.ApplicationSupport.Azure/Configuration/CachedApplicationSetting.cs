using System;

namespace AccidentalFish.ApplicationSupport.Azure.Configuration
{
    internal class CachedApplicationSetting
    {
        public DateTimeOffset ExpiresAt { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
