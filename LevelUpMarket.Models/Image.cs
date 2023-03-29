using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ImageType ImageType { get; set; }    
        
        //navigation properties
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
