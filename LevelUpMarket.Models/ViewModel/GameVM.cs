﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public IEnumerable<SelectListItem> PlateformeList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VoiceLanguagesList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SubtitleList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }

        public IEnumerable<string> SelectedPlateformes { get; set; }
        public IEnumerable<string> SelectedGenders { get; set; }
        public IEnumerable<string> SelectedSubtitle { get; set; }
        public IEnumerable<string> SelectedVoice { get; set; }



    }

}