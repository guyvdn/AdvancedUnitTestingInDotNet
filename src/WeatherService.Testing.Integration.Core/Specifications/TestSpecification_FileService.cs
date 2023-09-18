using NSubstitute;
using WeatherService.Core.Services;

namespace WeatherService.Testing.Integration.Core.Specifications;

public abstract partial class TestSpecification
{
    protected IFileService FileService = default!;

    private void MockFileSystem()
    {
        FileService = Substitute.For<IFileService>();
        AddDependency<IFileService>(FileService);
    }
}