using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelUpMarket.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IDeveloperRepository Developer { get; }
        ICharacterRepository Character { get; }
        IGameRepository Game { get; }
        IPlateformeRepository Plateforme { get; }
        ISubtitleRepository Subtitle { get; }
        IVoiceLanguageRepository VoiceLanguage { get; }
        IGenderRepository Gender { get; }
        IVideoRepository Video { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }

        void Save();
        void deleteFroGamePlateforme(int gameId);
        void deleteFroGameGender(int gameId);

        void deleteFroGameSubtitle(int gameId);

        void deleteFroGameVoiceLanguage(int gameId);

    }
}
