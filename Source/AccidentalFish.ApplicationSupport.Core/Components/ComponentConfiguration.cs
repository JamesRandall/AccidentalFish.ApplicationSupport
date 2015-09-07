using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Describes the configuration of a component in the component host
    /// </summary>
    public class ComponentConfiguration
    {
        /// <summary>
        /// Name of the component (used to instantiate it using the IComponentFactory)
        /// </summary>
        public IComponentIdentity ComponentIdentity { get; set; }

        /// <summary>
        /// Number of instances to run in parallel
        /// </summary>
        public int Instances { get; set; }

        /// <summary>
        /// Function to determins if the component should be restarted. Passed the exception that caused the
        /// failure, the number of times the component has been restarted previously, and should return true
        /// for restart false if not.
        /// </summary>
        public Func<Exception, int, bool> RestartEvaluator { get; set; } 
    }
}
