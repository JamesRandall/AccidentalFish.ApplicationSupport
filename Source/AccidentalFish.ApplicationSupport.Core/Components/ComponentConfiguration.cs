using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Describes the configuration of a component in the component host
    /// </summary>
    public class ComponentConfiguration
    {
        /// <summary>
        /// Name of the component. If no factory is specified this will be used to instantiate the name via the IComponentFactory
        /// </summary>
        public IComponentIdentity ComponentIdentity { get; set; }

        /// <summary>
        /// If this is specified then the supplied factory will be used to instantiate the component
        /// </summary>
        public Func<IHostableComponent> Factory { get; set; }

        /// <summary>
        /// Number of instances to run in parallel
        /// </summary>
        public int Instances { get; set; }

        /// <summary>
        /// Function to determins if the component should be restarted. Passed the exception that caused the
        /// failure, the number of times the component has been restarted previously, and should return true
        /// for restart false if not.
        /// 
        /// If null then the default restart handler will be used. This will log the error and restart the
        /// component pausing for 30 seconds on every fifth error.
        /// </summary>
        public Func<Exception, int, bool> RestartEvaluator { get; set; } 
    }
}
