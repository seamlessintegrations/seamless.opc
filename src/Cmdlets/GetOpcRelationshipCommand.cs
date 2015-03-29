using System;
using System.Management.Automation;
using seamless.opc.Model;

namespace seamless.opc.Cmdlets
{
    /// <summary>
    /// Cmdlet for retrieving relations from package parts
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Noun)]
    public class GetOpcRelationshipCommand : PSCmdlet
    {
        private const string Noun = "OPCRelationship";

        /// <summary>
        /// The Part of the package from which the relationship should get received
        /// </summary>
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The part for which the relations should get received")]
        [ValidateNotNull]
        public OpcPackagePart Part { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Identifier of the opc package part")]
        [ValidateNotNullOrEmpty]
        public string Identifier { get; set; }

        /// <summary>
        /// Process a single record
        /// </summary>
        protected override void ProcessRecord()
        {
            if (!Part.Package.IsOpen)
            {
                WriteError(new ErrorRecord(new ArgumentException("The Package is not open"),
                        "ERR_COMMON_CLOSED", ErrorCategory.CloseError, Part));
                
                return;
            }

            if (!String.IsNullOrEmpty(Identifier))
                WriteObject(Part.GetRelation(Identifier));
            else
                WriteObject(Part.GetRelations(), true);
        }
    }
}