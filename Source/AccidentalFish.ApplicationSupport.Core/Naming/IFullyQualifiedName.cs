namespace AccidentalFish.ApplicationSupport.Core.Naming
{
    /// <summary>
    /// Representes a fully qualified name
    /// </summary>
    public interface IFullyQualifiedName
    {
        /// <summary>
        /// The name
        /// </summary>
        string FullyQualifiedName { get; }
    }
}
