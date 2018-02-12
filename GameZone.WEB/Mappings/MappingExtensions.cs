using AutoMapper;
using GameZone.VIEWMODEL;

namespace GameZone.WEB.Mappings
{
    public static class MappingExtensions
    {
        #region AppUser

        public static AppUserVM ToModel(this GameData.AppUser entity)
        {
            return Mapper.Map<GameData.AppUser, AppUserVM>(entity);
        }        

        public static GameData.AppUser ToEntity(this AppUserVM model)
        {
            return Mapper.Map<AppUserVM, GameData.AppUser>(model);
        }

        public static GameData.AppUser ToEntity(this AppUserVM model, GameData.AppUser destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion        
    }
}