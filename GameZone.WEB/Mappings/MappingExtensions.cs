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

        #region ServiceHeader

        public static ServiceHeaderVM ToModel(this GameData.ServiceHeaders entity)
        {
            return Mapper.Map<GameData.ServiceHeaders, ServiceHeaderVM>(entity);
        }

        public static GameData.ServiceHeaders ToEntity(this ServiceHeaderVM model)
        {
            return Mapper.Map<ServiceHeaderVM, GameData.ServiceHeaders>(model);
        }

        public static GameData.ServiceHeaders ToEntity(this ServiceHeaderVM model, GameData.ServiceHeaders destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion        
    }
}