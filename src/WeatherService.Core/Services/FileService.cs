using OneOf;
using OneOf.Types;

namespace WeatherService.Core.Services;

public interface IFileService
{
    public Task SaveFileAsync(string path, byte[] bytes, CancellationToken cancellationToke);

    public Task<OneOf<byte[], NotFound>> LoadFileAsync(string path, CancellationToken cancellationToken);
}

internal sealed class FileService : IFileService
{
    public async Task SaveFileAsync(string path, byte[] bytes, CancellationToken cancellationToke)
    {
            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory!);

            await File.WriteAllBytesAsync(path, bytes, cancellationToke);
    }

    public async Task<OneOf<byte[], NotFound>> LoadFileAsync(string path, CancellationToken cancellationToken)
    {
        if (File.Exists(path))
            return await File.ReadAllBytesAsync(path ,cancellationToken);
        
        return new NotFound();
    }
}