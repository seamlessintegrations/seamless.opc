using System;
using System.IO;
using System.IO.Packaging;

namespace seamless.opc.Model
{
    public class OpcPackagePart
    {
        private readonly PackagePart _part;

        /// <summary>
        /// 
        /// </summary>
        public OpcPackage Package { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public CompressionOption CompressionOption { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="part"></param>
        internal OpcPackagePart(OpcPackage package, PackagePart part)
        {
            Package = package;
            _part = part;

            CompressionOption = _part.CompressionOption;
            ContentType = _part.ContentType;
            Uri = _part.Uri;
        }

        /// <summary>
        /// Returns the stream of the part.
        /// </summary>
        internal Stream GetStream()
        {
            return _part.GetStream();
        }
    }
}