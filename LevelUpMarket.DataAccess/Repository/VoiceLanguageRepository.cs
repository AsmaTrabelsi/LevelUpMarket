using LevelUpMarket.Data;
using LevelUpMarket.DataAccess.Repository.IRepository;
using LevelUpMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository
{
    public class VoiceLanguageRepository : Repository<VoiceLanguage>, IVoiceLanguageRepository
    {
        private ApplicationDbContext _db;
        public VoiceLanguageRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
       

        void IVoiceLanguageRepository.Update(VoiceLanguage voiceLanguage)
        {
            _db.VoiceLanguages.Update(voiceLanguage);
        }
    }
}
