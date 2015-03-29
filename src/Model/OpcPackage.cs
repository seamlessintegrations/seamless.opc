using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;

namespace seamless.opc.Model
{
    /// <summary>
    /// Represents an OPC package
    /// </summary>
    public class OpcPackage
    {
        private readonly Package _package;

        public FileSystemInfo File { get; set; }

        /// <summary>
        /// Gets the core properties of the package
        /// </summary>
        public PackageProperties PackageProperties { get; private set; }

        internal static HashSet<OpcPackage> OpenPackages { get; private set; }

        static OpcPackage()
        {
            OpenPackages = new HashSet<OpcPackage>();
        }

        /// <summary>
        /// Creates an OPC package from an <see cref="Package"/>.
        /// </summary>
        /// <param name="package">the OPC package</param>
        /// <param name="file">the source file</param>
        public OpcPackage(Package package, FileSystemInfo file)
        {
            _package = package;
            File = file;
            PackageProperties = package.PackageProperties;

            OpenPackages.Add(this);
        }

        /// <summary>
        /// Closes the underlying <see cref="Package"/>.
        /// </summary>
        public void Close()
        {
            _package.Close();
            OpenPackages.Remove(this);
        }
    }
}