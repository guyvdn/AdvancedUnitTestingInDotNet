using FluentAssertions.Primitives;

namespace WeatherService.Testing.Unit.Core.Assertions;

public static class FileServiceAssertionsExtensions
{
    public static FileServiceAssertions Should(this IFileService fileService)
    {
        return new FileServiceAssertions(fileService);
    }
}

public sealed partial class FileServiceAssertions : ReferenceTypeAssertions<IFileService, FileServiceAssertions>
{
    protected override string Identifier => nameof(IFileService);

    public FileServiceAssertions(IFileService subject) : base(subject)
    {
    }

    public AndConstraint<FileServiceAssertions> SaveFile(string fileName, byte[] bytes, CancellationToken cancellationToken)
    {
        Subject.Received().SaveFileAsync(
            fileName,
            Arg.Is<byte[]>(predicate: x => x.SequenceEqual(bytes)),
            cancellationToken);

        return new AndConstraint<FileServiceAssertions>(parentConstraint: this);
    }

    public AndConstraint<FileServiceAssertions> GetFile(string fileName, CancellationToken cancellationToken)
    {
        Subject.Received().GetFileAsync(fileName, cancellationToken);

        return new AndConstraint<FileServiceAssertions>(parentConstraint: this);
    }

    public AndConstraint<FileServiceAssertions> GetImage(CancellationToken cancellationToken)
    {
        Subject.Received().GetFileAsync(Arg.Is<string>(x => ImageRegex().IsMatch(x)), cancellationToken);

        return new AndConstraint<FileServiceAssertions>(parentConstraint: this);
    }

    [GeneratedRegex(@"Images\/.+\.png$")]
    private static partial Regex ImageRegex();
}