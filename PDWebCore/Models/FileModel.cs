﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Models
{
    [Table("File")]
    public class FileModel
    {
        [Key]
        public int ALLFId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int RefId { get; set; }
        public ObjType RefGid { get; set; }
        public string Description { get; set; }
        public int GroupId { get; set; }

        [NotMapped]
        public string Source { get; set; }

        [NotMapped]
        public bool GroupIdChanged { get; set; }

        [NotMapped]
        public bool NameChanged { get; set; }

        [NotMapped]
        public bool RefIdChanged { get; set; }

        [NotMapped]
        public byte[] Data { get; set; }
    }

    public enum ObjType { Ticket = 0, Comment }
}
