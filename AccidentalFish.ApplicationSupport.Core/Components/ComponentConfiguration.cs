using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public class ComponentConfiguration
    {
        public IComponentIdentity ComponentIdentity { get; set; }

        public int Instances { get; set; }

        /// <summary>
        /// Function to determins if the component should be restarted. Passed the exception that caused the
        /// failure, the number of times the component has been restarted previously, and should return true
        /// for restart false if not.
        /// </summary>
        public Func<Exception, int, bool> RestartEvaluator { get; set; } 
    }
}
