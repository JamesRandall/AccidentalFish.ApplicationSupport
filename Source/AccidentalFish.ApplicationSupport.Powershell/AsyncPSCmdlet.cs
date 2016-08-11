using System.Management.Automation;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Threading;

namespace AccidentalFish.ApplicationSupport.Powershell
{
    // ReSharper disable once InconsistentNaming
    public class AsyncPSCmdlet : PSCmdlet
    {
        protected virtual Task ProcessRecordAsync()
        {
            return Task.Delay(0);
        }

        protected sealed override void ProcessRecord()
        {
            AsyncPump.Run(async () => await ProcessRecordAsync());
        }
    }    
}
