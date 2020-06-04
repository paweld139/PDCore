using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Models
{
    [Table("UserData")]
    public class UserDataModel
    {
        [Key]
        public int ULId { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }
        public string OperatingSystem { get; set; }
        public DeviceType Device { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public string MetroCode { get; set; }
        public string TimeZone { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PhoneModel { get; set; }
        public string PhoneManufacturer { get; set; }
        public string Platform { get; set; }
        public string Resolution { get; set; }
        public string Language { get; set; }
        public string Browser { get; set; }
        public bool ServiceUnresponded { get; set; }
    }

    public enum DeviceType { Desktop = 1, Mobile }
}
