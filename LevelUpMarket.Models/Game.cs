using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Intro { get; set; }
        [Required]
        public string Story { get; set; }
        //for testing 
       
        [Required]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public double Price { get; set; }
       
        [Display(Name = "Offline Play")]
        public bool OfflinePlayEnable { get; set; }
        public bool Available { get; set; }

        // navigation properites 
        
        public ICollection<Gender> Genders { get; set; }

        [Display(Name = "Voice Languages")]
        public ICollection<VoiceLanguage> VoiceLanguages { get; set; }
        
        public ICollection<Subtitle> Subtitles { get; set; }
        [ValidateNever]
        public  ICollection<Plateforme> Plateformes { get; set; }
        public  ICollection<Image> Images { get; set; }
        public  ICollection<Video> Videos { get; set; }
        [ValidateNever]
        public  ICollection<Character> Characters { get; set; }
        [Display(Name = "Developer")]
        public int DeveloperId { get; set; }
        [ValidateNever]
        public  Developer Developer { get; set; }





    }
}
