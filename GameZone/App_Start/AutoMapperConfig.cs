using GameZone.VIEWMODEL;

namespace GameZone.App_Start
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<GameData.Game, GameVM>().ReverseMap();
            });
        }
    }
}