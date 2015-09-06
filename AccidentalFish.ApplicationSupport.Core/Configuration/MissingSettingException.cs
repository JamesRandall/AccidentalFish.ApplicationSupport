using System;

namespace AccidentalFish.ApplicationSupport.Core.Configuration
{
    /// <summary>
    /// Thrown if a setting is not supplied during configuration merging
    /// </summary>
    [Serializable]
    public class MissingSettingException : Exception
    {
    }
}
