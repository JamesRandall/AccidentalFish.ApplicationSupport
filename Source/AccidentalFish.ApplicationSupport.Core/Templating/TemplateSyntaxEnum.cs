namespace AccidentalFish.ApplicationSupport.Core.Templating
{
    /// <summary>
    /// Template syntax (currently) for emails.
    /// For backwards compatibility Razor is the default template syntax in the v2.x series
    /// however this will changed to Moustache in v3.x (partly due to the temporary file issues of the
    /// Razor engine making it unsuitable for a background worker and my personal preferenence of
    /// the Moustache / Handlebars format for simple text).
    /// </summary>
    public enum TemplateSyntaxEnum
    {
        /// <summary>
        /// Template uses the Razor syntax
        /// </summary>
        Razor,
        /// <summary>
        /// Template uses the Handlebars syntax
        /// </summary>
        Handlebars
    };
}