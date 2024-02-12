using NSubstitute;
using WeatherService.Core.Services;

namespace WeatherService.Testing.NUnit.Integration.Core.Specifications;

public abstract partial class TestSpecification
{
    protected IFileService FileService = default!;

    private void MockFileSystem()
    {
        FileService = Substitute.For<IFileService>();
        AddDependency(FileService);
    }

    [Test]
    public virtual void It_should_save_the_correct_files()
    {
        if (FileService.ReceivedCalls().Any(x => x.GetMethodInfo().Name == nameof(FileService.SaveFileAsync)))
            Assert.Fail($"{nameof(It_should_save_the_correct_files)} must be implemented!");
    }   
    
    [Test]
    public virtual void It_should_get_the_correct_files()
    {
        if (FileService.ReceivedCalls().Any(x => x.GetMethodInfo().Name == nameof(FileService.GetFileAsync)))
            Assert.Fail($"{nameof(It_should_get_the_correct_files)} must be implemented!");
    }
}