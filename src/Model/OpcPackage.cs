using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;

namespace seamless.opc.Model
{
    /// <summary>
    /// Represents an OPC package
    /// </summary>
    public class OpcPackage
    {
        private readonly Package _package;

        /// <summary>
        /// The File containing the package
        /// </summary>
        public FileSystemInfo File { get; set; }

        /// <summary>
        /// Gets the core properties of the package
        /// </summary>
        public PackageProperties CoreProperties { get; private set; }

        /// <summary>
        /// List of all Parts of this package
        /// </summary>
        public List<OpcPackagePart> Parts { get; set; }

        /// <summary>
        /// Currently open packages
        /// </summary>
        internal static HashSet<OpcPackage> OpenPackages { get; private set; }

        /// <summary>
        /// Indicates whether this package is open or closed
        /// </summary>
        public bool IsOpen { get; set; }

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
            IsOpen = true;
            
            CoreProperties = package.PackageProperties;
            Parts = (from part in _package.GetParts()
                            select new OpcPackagePart(this, part)).ToList();

            OpenPackages.Add(this);
        }

        /// <summary>
        /// Closes the underlying <see cref="Package"/>.
        /// </summary>
        public void Close()
        {
            _package.Close();
            OpenPackages.Remove(this);
            IsOpen = false;
        }
    }
}