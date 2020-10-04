using PDCore.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PDCoreNew.Models
{
    [Table("File", Schema = "dbo")]
    [DataContract(Name = "file", Namespace = "")]
    public class FileModel : FileBrief, IModificationHistory
    {       
        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 0, ErrorMessageResourceName = "StringLength_GreaterAndLess", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public int RefId { get; set; }

        public ObjType RefGid { get; set; }

        public string Description { get; set; }

        public int GroupId { get; set; }

        [NotMapped]
        [DataMember(Name = "source")]
        public string Source { get; set; }

        [NotMapped]
        public byte[] Data { get; set; }


        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        [NotMapped]
        public bool IsDirty { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public string GetNameWithExtension() => $"{Name}.{Extension}";
    }

    public enum ObjType { Parent = 1, Child }
}
