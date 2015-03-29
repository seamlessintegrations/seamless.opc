using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;

namespace seamless.opc.Model
{
    /// <summary>
    /// Represents a part of an <see cref="OpcPackage"/>.
    /// </summary>
    public class OpcPackagePart
    {
        private readonly PackagePart _part;

        /// <summary>
        /// The owning <see cref="OpcPackage"/>
        /// </summary>
        public OpcPackage Package { get; private set; }

        /// <summary>
        /// THe compression option used
        /// </summary>
        public CompressionOption CompressionOption { get; set; }

        /// <summary>
        /// The actual content type of the part
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The URI of the part
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Creates a new package part from a given <see cref="OpcPackage"/> and a <see cref="PackagePart"/>.
        /// </summary>
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

        /// <summary>
        /// Returns the relation with the given identifier, if any.
        /// </summary>
        /// <returns></returns>
        internal OpcRelationship GetRelation(string id)
        {
            return new OpcRelationship(this, _part.GetRelationship(id));
        }

        /// <summary>
        /// Returns all relations of this package part
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<OpcRelationship> GetRelations()
        {
            return from rel in _part.GetRelationships()
                   select new OpcRelationship(this, rel);
        }
    }
}