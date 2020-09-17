using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Models
{
    [DataContract(Name = "rowInfo")]
    public class RowInfo
    {
        public RowInfo(IModificationHistory modificationHistory)
        {
            RowVersion = modificationHistory.RowVersion;
        }

        [DataMember(Name = "rowVersion")]
        public byte[] RowVersion { get; set; }
    }
}
