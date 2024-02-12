namespace WeatherService.Testing.XUnit.Unit.Core.Dependencies;

public interface IDependencyBuilder
{
    public object? Build(Type type);
}

public abstract class DependencyBuilder<TDependency, TImplementation> : DependencyBuilder<TDependency>
    where TImplementation : TDependency, new()
{
    protected override TDependency BuildInternal()
    {
        return new TImplementation();
    }
}

public abstract class DependencyBuilder<TDependency> : IDependencyBuilder
{
    public object? Build(Type type)
    {
        if (type.IsAssignableTo(typeof(TDependency)))
        {
            return BuildInternal();
        }

        return null;
    }

    protected abstract TDependency BuildInternal();
}