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
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public bool OfflinePlayEnable { get; set; }
        public bool Available { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Genres { get; set; }

        [NotMapped]
        public List<Gender> GenresList
        {
            get { return Genres.Split(',').Select(g => (Gender)Enum.Parse(typeof(Gender), g)).ToList(); }
            set { Genres = string.Join(',', value.Select(g => g.ToString())); }
        }
        [Column(TypeName = "nvarchar(200)")]
        public string VoiceLanguages { get; set; }

        [NotMapped]
        public List<Language> VoiceLanguageList
        {
            get { return VoiceLanguages.Split(',').Select(g => (Language)Enum.Parse(typeof(Language), g)).ToList(); }
            set { VoiceLanguages = string.Join(',', value.Select(g => g.ToString())); }
        }
        [Column(TypeName = "nvarchar(200)")]
        public string Subtitles { get; set; }

        [NotMapped]
        public List<Language> SubtitleList
        {
            get { return Subtitles.Split(',').Select(g => (Language)Enum.Parse(typeof(Language), g)).ToList(); }
            set { Subtitles = string.Join(',', value.Select(g => g.ToString())); }
        }
        // navigation properites 
        public  ICollection<Plateforme> Plateformes { get; set; }
        public  ICollection<Image> Images { get; set; }
        public  ICollection<Video> Videos { get; set; }
        public  ICollection<Character> Characters { get; set; }

        public virtual Developer Developer { get; set; }





    }
}
