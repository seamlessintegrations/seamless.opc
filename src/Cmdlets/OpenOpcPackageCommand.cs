using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;
using seamless.opc.Model;

namespace seamless.opc.Cmdlets
{
    [Cmdlet(VerbsCommon.Open, Noun, DefaultParameterSetName = ParmamSetPath, SupportsShouldProcess = true)]
    public class OpenOpcPackageCommand : PSCmdlet
    {
        private const string Noun = "OPCPackage";
        private const string ParamSetLiteral = "Literal";
        private const string ParmamSetPath = "Path";

        private bool _shouldExpandWildcards;

        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParamSetLiteral)]
        [Alias("PSPath")]
        [ValidateNotNullOrEmpty]
        public string[] LiteralPath { get; set; }

        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = ParmamSetPath)]
        [ValidateNotNullOrEmpty]
        public string[] Path
        {
            get { return LiteralPath; }
            set
            {
                _shouldExpandWildcards = true;
                LiteralPath = value;
            }
        }

        protected override void ProcessRecord()
        {
            foreach (var path in LiteralPath)
            {
                ProviderInfo provider;
                PSDriveInfo drive;
                var filePaths = new List<string>();

                // wildcards?
                if (_shouldExpandWildcards)
                    filePaths.AddRange(GetResolvedProviderPathFromPSPath(path, out provider));
                else
                    // no wildcards, so don't try to expand any * or ? symbols.                    
                    filePaths.Add(SessionState.Path.GetUnresolvedProviderPathFromPSPath(path, out provider, out drive));

                if (!IsFileSystemPath(provider, path))
                    // no, so skip to next path in _paths.
                    continue;

                // at this point, we have a list of paths on the filesystem.
                foreach (var file in from filePath in filePaths where ShouldProcess(filePath, "Get Package") select filePath)
                {
                    // extract document.xml from zip
                    try
                    {
                        var package = Package.Open(file, FileMode.Open, FileAccess.ReadWrite);
                        WriteObject(new OpcPackage(package, new FileInfo(file)));
                    }
                    catch (Exception e)
                    {
                        WriteError(new ErrorRecord(e, "ERR_COMMON_ARG", ErrorCategory.InvalidArgument, file));
                    }
                }
            }
        }

        private bool IsFileSystemPath(ProviderInfo provider, string path)
        {
            // check that this provider is the filesystem
            if (provider.ImplementingType == typeof(FileSystemProvider))
                return true;

            // create a .NET exception wrapping our error text
            var ex = new ArgumentException(path + " does not resolve to a path on the FileSystem provider.");

            // wrap this in a powershell errorrecord
            var error = new ErrorRecord(ex, "InvalidProvider", ErrorCategory.InvalidArgument, path);

            // write a non-terminating error to pipeline
            WriteError(error);

            // tell our caller that the item was not on the filesystem
            return false;
        }
    }
}