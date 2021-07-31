using AutoMapper;
using JetSnail.InventorLibraryManager.Server.Profiles;
using Xunit;

namespace JetSnail.InventorLibraryManager.Server.Tests
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
        public void ConfigurationIsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}