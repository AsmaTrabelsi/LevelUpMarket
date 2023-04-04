using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.Models.ViewModel
{
    public class GameVM
    {
        [ValidateNever]
        public Game Game { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> DeveloperList { get; set; }
        [ValidateNever]
        // public IEnumerable<SelectListItem> PlateformeList { get; set; }
        public IEnumerable<Plateforme> PlateformeList { get; set; }

        [ValidateNever]
        public IEnumerable<VoiceLanguage> VoiceLanguagesList { get; set; }
        [ValidateNever]
        public IEnumerable<Subtitle> SubtitleList { get; set; }
        [ValidateNever]
        public IEnumerable<Gender> GenderList { get; set; }

        public IEnumerable<string> SelectedPlateformes { get; set; }
        public IEnumerable<string> SelectedGenders { get; set; }
        public IEnumerable<string> SelectedSubtitle { get; set; }
        public IEnumerable<string> SelectedVoice { get; set; }



    }

}