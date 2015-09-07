using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Thrown when a component identity is required but could not be found
    /// </summary>
    [Serializable]
    public class MissingComponentIdentityException : Exception
    {
    }
}
