using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models
{
    public class Character
    {
        [Key]
       public int Id { get; set; }
        [Required]
       public string Name { get; set; }
        [Required]
       public string Description { get; set; }
        [Required]
       public bool MainCharacter { get; set; }
        [Required]
       public CharacterType CharacterType { get; set; }
       public int GameId { get; set; }
        // navigation property
        public virtual Game Game { get; set; }

    }
}
