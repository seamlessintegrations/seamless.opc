using System.Management.Automation;
using seamless.opc.Model;

namespace seamless.opc.Cmdlets
{
    [Cmdlet(VerbsCommon.Close, Noun)]
    public class CloseOpcPackageCommand : PSCmdlet
    {
        private const string Noun = "OPCPackage";

        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public OpcPackage Package { get; set; }

        protected override void ProcessRecord()
        {
            Package.Close();
        }
    }
}