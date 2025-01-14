using MagicVillaWebApi.Models.Dto;

namespace MagicVillaWebApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto> {
            new VillaDto { Id=1, Name="villa1", sqft=10, occupancy=1000},
            new VillaDto { Id=2, Name="villa2", sqft=15, occupancy=2000}
        };
    }
}
