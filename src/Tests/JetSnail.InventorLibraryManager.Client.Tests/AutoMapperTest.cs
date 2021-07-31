using AutoMapper;
using JetSnail.InventorLibraryManager.Client.Profiles;
using Xunit;

namespace JetSnail.InventorLibraryManager.Client.Tests
{
    public class AutoMapperTest
    {
        private static IMapper _mapper;

        public AutoMapperTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new FamilyMappingProfile());
                mc.AddProfile(new GroupMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void MapInventorFamilyToPrototypeTest()
        {
        }

        [Fact]
        public void ConfigurationIsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}