//using System.Runtime.InteropServices.JavaScript;
//using FluentAssertions.Primitives;
//using NSubstitute;

//namespace WeatherService.Testing.NUnit.Core.Specifications;

//public abstract class DependencyAssertions<TDependency, TAssertions> 
//    : ReferenceTypeAssertions<TSubject, TAssertions>
//    where TDependency : class
//    where TAssertions : DependencyAssertions<TDependency, TAssertions>
//{
//    protected DependencyAssertions(TDependency subject)
//        : base(subject)
//    {
//    }

//    public void NotHaveInvocations()
//    {
//        Subject.ReceivedCalls().Should().HaveCount(0);
//    }
//}