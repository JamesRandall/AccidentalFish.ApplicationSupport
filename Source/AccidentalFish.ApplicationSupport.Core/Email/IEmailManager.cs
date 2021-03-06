﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Templating;

namespace AccidentalFish.ApplicationSupport.Core.Email
{
    /// <summary>
    /// Email dispatcher. Emails are initially placed into a queue and are dispatched from a hostable component available in AccidentalFish.ApplicationSupport.Processes
    /// </summary>
    public interface IEmailQueueDispatcher
    {
        /// <summary>
        /// Send an email using a template
        /// </summary>
        /// <param name="to">The to address</param>
        /// <param name="cc">The cc address</param>
        /// <param name="from">The from address</param>
        /// <param name="emailTemplateId">The ID of the template (the email queue processor needs access to this)</param>
        /// <param name="mergeValues">Merge data</param>
        /// <param name="templateSyntax">Syntax of the template. Defaults to Razor in the v2.x series for backwards compatibility but this will be changed to Handlebars from v3.x onwards</param>
        /// <returns>Awaitable task</returns>
        Task SendAsync(string to, string cc, string from, string emailTemplateId, Dictionary<string, string> mergeValues, TemplateSyntaxEnum templateSyntax = TemplateSyntaxEnum.Razor);

        /// <summary>
        /// Send an email using a template
        /// </summary>
        /// <param name="to">The to addresses</param>
        /// <param name="cc">The cc addresses</param>
        /// <param name="from">The from address</param>
        /// <param name="emailTemplateId">The ID of the template (the email queue processor needs access to this)</param>
        /// <param name="mergeValues">Merge data</param>
        /// <param name="templateSyntax">Syntax of the template. Defaults to Razor in the v2.x series for backwards compatibility but this will be changed to Handlebars from v3.x onwards</param>
        /// <returns>Awaitable task</returns>
        Task SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string from, string emailTemplateId, Dictionary<string, string> mergeValues, TemplateSyntaxEnum templateSyntax = TemplateSyntaxEnum.Razor);

        /// <summary>
        /// Send an email using
        /// </summary>
        /// <param name="to">The to address</param>
        /// <param name="cc">The cc address</param>
        /// <param name="from">The from address</param>
        /// <param name="subject">The subject for the email</param>
        /// <param name="htmlBody">The HTML formatted body</param>
        /// <param name="textBody">The text formatted body</param>
        /// <returns>Awaitable task</returns>
        Task SendAsync(string to, string cc, string from, string subject, string htmlBody, string textBody);

        /// <summary>
        /// Send an email using
        /// </summary>
        /// <param name="to">The to addresses</param>
        /// <param name="cc">The cc addresses</param>
        /// <param name="from">The from address</param>
        /// <param name="subject">The subject for the email</param>
        /// <param name="htmlBody">The HTML formatted body</param>
        /// <param name="textBody">The text formatted body</param>
        /// <returns>Awaitable task</returns>
        Task SendAsync(IEnumerable<string> to, IEnumerable<string> cc, string from, string subject, string htmlBody, string textBody);
    }
}
