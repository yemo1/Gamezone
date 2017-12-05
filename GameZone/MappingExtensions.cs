using AutoMapper;
using GameZone.VIEWMODEL;

namespace GameZone
{
    public static class MappingExtensions
    {
        #region GameUser Form

        public static GameVM ToModel(this GameData.Game entity)
        {
            return Mapper.Map<GameData.Game, GameVM>(entity);
        }

        public static GameData.Game ToEntity(this GameVM model)
        {
            return Mapper.Map<GameVM, GameData.Game>(model);
        }

        public static GameData.Game ToEntity(this GameVM model, GameData.Game destination)
        {
            return Mapper.Map(model, destination);
        }

        #endregion
    }
}