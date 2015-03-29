using System;
using System.Linq;
using System.Management.Automation;
using seamless.opc.Model;

namespace seamless.opc.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, Noun)]
    public class GetOpcPackageCommand : PSCmdlet
    {
        private const string Noun = "OPCPackage";

        [Parameter(
            Position = 0,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string PackageName { get; set; }

        protected override void BeginProcessing()
        {
            if (String.IsNullOrWhiteSpace(PackageName))
                WriteObject(OpcPackage.OpenPackages, true);
            else
                WriteObject(from pkg in OpcPackage.OpenPackages
                            where pkg.File.Name.Contains(PackageName)
                            select pkg, true);
        }
    }
}