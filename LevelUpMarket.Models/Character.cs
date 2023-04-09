using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
       public int CharacterId { get; set; }
        [Required]
       public string CharacterName { get; set; }
        [Required]
       public string Description { get; set; }
        [Required]
       public bool MainCharacter { get; set; }
        [ValidateNever]
       public string ImageUrl { get; set; }
        [Required]
       public CharacterType CharacterType { get; set; }
       public int GameId { get; set; }
        // navigation property
        [ValidateNever]
        public virtual Game Game { get; set; }

    }
}
