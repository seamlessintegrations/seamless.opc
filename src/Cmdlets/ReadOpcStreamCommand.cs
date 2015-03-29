using System;
using System.IO;
using System.Management.Automation;

namespace seamless.opc.Cmdlets
{
    /// <summary>
    /// Reads the given <see cref="Stream"/> and writes it to the pipe
    /// </summary>
    [Cmdlet(VerbsCommunications.Read, Noun)]
    public class ReadOpcStreamCommand : PSCmdlet
    {
        private const string Noun = "OPCStream";

        public ReadOpcStreamCommand()
        {
            Stream = false;
        }

        /// <summary>
        /// The stream to read
        /// </summary>
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The Stream to proces")]
        [ValidateNotNull]
        public Stream InputObject { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Indicates whether the output should be given " +
            "line by line or as a whole. -Stream sets to line-by-line mode.")]
        public SwitchParameter Stream { get; set; }

        /// <summary>
        /// Process a single record
        /// </summary>
        protected override void ProcessRecord()
        {
            if (!InputObject.CanRead)
            {
                WriteError(new ErrorRecord(new ArgumentException("The given stream does not support read access"),
                    "ERR_COMMON_NOREAD", ErrorCategory.InvalidOperation, InputObject));

                return;
            }

            var reader = new StreamReader(InputObject);
            if (!Stream)
                WriteObject(reader.ReadToEnd());
            else
                // line-by-line
                while (!reader.EndOfStream)
                    WriteObject(reader.ReadLine());
        }
    }
}