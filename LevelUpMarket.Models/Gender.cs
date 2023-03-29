using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models
{
    public class Gender
    {
      public int Id { get; set; }
        [Required]
      public string Name { get; set; }
        //navigation properties
        public virtual ICollection<Game> Games { get; set; }


    }
}
