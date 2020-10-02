using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Models
{
    [Table("File", Schema = "dbo")]
    [DataContract(Name = "file", Namespace = "")]
    public class FileModel : IModificationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id", ResourceType = typeof(Resources.Common))]
        [DataMember(Name = "id")]
        public int ALLFId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Name", ResourceType = typeof(Resources.Common))]
        [DataType(DataType.Text)]
        [StringLength(80, MinimumLength = 1, ErrorMessageResourceName = "StringLength_GreaterAndLess", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceName = "StringLength_GreaterAndLess", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "extension")]
        public string Extension { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceName = "StringLength_GreaterAndLess", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }

        [Range(1, int.MaxValue, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "refId")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        public int RefId { get; set; }

        public ObjType RefGid { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        public string Description { get; set; }

        public int GroupId { get; set; }



        [NotMapped]
        [DataMember(Name = "source")]
        public string Source { get; set; }

        [NotMapped]
        [DataMember(Name = "data")]
        public byte[] Data { get; set; }


        public DateTime DateModified { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDirty { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public enum ObjType { Parent = 1, Child }
}
