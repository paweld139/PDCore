using Newtonsoft.Json;
using PDCore.Interfaces;
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
    public class UserDataModel : IByDateFindable
    {
        [Key]
        public int ULId { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }
        public string OperatingSystem { get; set; }
        public DeviceType Device { get; set; }
        public string PhoneModel { get; set; }
        public string PhoneManufacturer { get; set; }
        public string Platform { get; set; }
        public string Resolution { get; set; }
        public string Language { get; set; }
        public string Browser { get; set; }
        public bool ServiceUnresponded { get; set; }

        //[JsonProperty("ip")]
        //public string Ip { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("continent_code")]
        public string ContinentCode { get; set; }

        [JsonProperty("continent_name")]
        public string ContinentName { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }

        [JsonProperty("location")]
        public virtual Location Location { get; set; }
    }

    [Table("Language")]
    public class Language
    {
        [Key]
        public int LAId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("native")]
        public string Native { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
    }

    [Table("Location")]
    public class Location
    {
        [Key]
        public int LOId { get; set; }

        [JsonProperty("geoname_id")]
        public int? GeonameId { get; set; }

        [JsonProperty("capital")]
        public string Capital { get; set; }

        [JsonProperty("languages")]
        public virtual ICollection<Language> Languages { get; set; }

        [JsonProperty("country_flag")]
        public string CountryFlag { get; set; }

        [JsonProperty("country_flag_emoji")]
        public string CountryFlagEmoji { get; set; }

        [JsonProperty("country_flag_emoji_unicode")]
        public string CountryFlagEmojiUnicode { get; set; }

        [JsonProperty("calling_code")]
        public string CallingCode { get; set; }

        [JsonProperty("is_eu")]
        public bool? IsEu { get; set; }
    }

    public enum DeviceType { Desktop = 1, Mobile }
}
