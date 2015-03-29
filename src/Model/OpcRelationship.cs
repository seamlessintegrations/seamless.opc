using System;
using System.IO.Packaging;

namespace seamless.opc.Model
{
    /// <summary>
    /// Represents a relationship betweeen package elements
    /// </summary>
    public class OpcRelationship
    {
        private OpcPackage _opcPackage;
        private readonly OpcPackagePart _opcPackagePart;
        private readonly PackageRelationship _packageRelationship;

        /// <summary>
        /// Identifier of the relationship
        /// </summary>
        public string Identifier
        {
            get { return _packageRelationship.Id; }
        }

        /// <summary>
        /// The fully qualified type of the relation ship
        /// </summary>
        public string Type
        {
            get { return _packageRelationship.RelationshipType; }
        }

        /// <summary>
        /// The containing package
        /// </summary>
        public OpcPackage Package
        {
            get { return _opcPackagePart != null ? _opcPackagePart.Package : _opcPackage; }
        }

        /// <summary>
        /// Uri of the source element
        /// </summary>
        public Uri SourceUri
        {
            get { return _opcPackagePart != null ? _opcPackagePart.Uri : null; }
        }

        /// <summary>
        /// Uri of the target element
        /// </summary>
        public Uri TargetUri
        {
            get { return _packageRelationship.TargetUri; }
        }

        /// <summary>
        /// Gets a value indicating whether the target is internal or external to the package
        /// </summary>
        public TargetMode TargetMode
        {
            get { return _packageRelationship.TargetMode; }
        }

        /// <summary>
        /// Constructs an <see cref="OpcRelationship"/> object from a given <see cref="PackageRelationship"/>.
        /// </summary>
        /// <param name="opcPackagePart">The part that owns the relationship</param>
        /// <param name="packageRelationship">the package relationship</param>
        internal OpcRelationship(OpcPackagePart opcPackagePart, PackageRelationship packageRelationship)
        {
            _opcPackagePart = opcPackagePart;
            _packageRelationship = packageRelationship;
        }

        /// <summary>
        /// Constructs an <see cref="OpcRelationship"/> object from a given <see cref="PackageRelationship"/>.
        /// </summary>
        /// <param name="opcPackage">The package that owns the relationship</param>
        /// <param name="packageRelationship">the package relationship</param>
        public OpcRelationship(OpcPackage opcPackage, PackageRelationship packageRelationship)
        {
            _opcPackage = opcPackage;
            _packageRelationship = packageRelationship;
        }
    }
}