using System;
using System.Management.Automation;
using seamless.opc.Model;

namespace seamless.opc.Cmdlets
{
    /// <summary>
    /// Receives streams of package parts thus providing access to the contents of the parts.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Noun)]
    public class GetOpcPartStreamCommand : PSCmdlet
    {
        private const string Noun = "OPCPartStream";

        /// <summary>
        /// The Part of the package from which the stream should get received
        /// </summary>
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public OpcPackagePart Part { get; set; }

        /// <summary>
        /// Process a single record
        /// </summary>
        protected override void ProcessRecord()
        {
            if (!Part.Package.IsOpen)
                WriteError(new ErrorRecord(new ArgumentException("The Package is not open"),
                    "ERR_COMMON_CLOSED", ErrorCategory.CloseError, Part));
            else
                WriteObject(Part.GetStream());
        }
    }
}