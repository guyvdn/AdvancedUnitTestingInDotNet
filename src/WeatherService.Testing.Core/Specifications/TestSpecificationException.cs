﻿namespace WeatherService.Testing.Unit.Core.Specifications;

[Serializable]
public class TestSpecificationException : Exception
{
    public TestSpecificationException(string? message) : base(message) { }
}