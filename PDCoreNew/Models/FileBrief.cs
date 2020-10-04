using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PDCoreNew.Models
{
    [DataContract(Name = "file", Namespace = "")]
    public class FileBrief
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id", ResourceType = typeof(Resources.Common))]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "Name", ResourceType = typeof(Resources.Common))]
        [DataType(DataType.Text)]
        [StringLength(100, MinimumLength = 1, ErrorMessageResourceName = "StringLength_GreaterAndLess", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        //[Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataType(DataType.Text)]
        [StringLength(20, MinimumLength = 1, ErrorMessageResourceName = "StringLength_GreaterAndLess", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [DataMember(Name = "extension")]
        public string Extension { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }
    }
}
