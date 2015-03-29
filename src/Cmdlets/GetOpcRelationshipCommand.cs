using System;
using System.Management.Automation;
using seamless.opc.Model;

namespace seamless.opc.Cmdlets
{
    /// <summary>
    /// Cmdlet for retrieving relations from package parts
    /// </summary>
    [Cmdlet(VerbsCommon.Get, Noun, DefaultParameterSetName = "PackagePart")]
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
            ParameterSetName = "Package",
            HelpMessage = "The package for which the relations should get listed")]
        [ValidateNotNull]
        public OpcPackage Package { get; set; }

        /// <summary>
        /// The Part of the package from which the relationship should get received
        /// </summary>
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "PackagePart",
            HelpMessage = "The part for which the relations should get listed")]
        [ValidateNotNull]
        public OpcPackagePart Part { get; set; }

        /// <summary>
        /// The identifier of the opc packacge or package part
        /// </summary>
        [Parameter(
            Position = 1,
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Package",
            HelpMessage = "Identifier of the opc package or package part")]
        [Parameter(
            Position = 1,
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "PackagePart",
            HelpMessage = "Identifier of the opc package or package part")]
        [ValidateNotNullOrEmpty]
        public string Identifier { get; set; }

        /// <summary>
        /// Process a single record
        /// </summary>
        protected override void ProcessRecord()
        {
            if (Package == null)
                GetPackagePartRelation();
            else
                GetPackageRelation();
        }

        /// <summary>
        /// List package relations
        /// </summary>
        private void GetPackageRelation()
        {
            if (!Package.IsOpen)
            {
                WriteError(new ErrorRecord(new ArgumentException("The Package is not open"),
                        "ERR_COMMON_CLOSED", ErrorCategory.CloseError, Package));

                return;
            }

            if (!String.IsNullOrEmpty(Identifier))
                WriteObject(Package.GetRelation(Identifier));
            else
                WriteObject(Package.GetRelations(), true);
        }

        /// <summary>
        /// List package part relations
        /// </summary>
        private void GetPackagePartRelation()
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