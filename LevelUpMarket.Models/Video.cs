﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string URL { get; set; }
        public VideoType Type { get; set; }

        //navigation properties
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
