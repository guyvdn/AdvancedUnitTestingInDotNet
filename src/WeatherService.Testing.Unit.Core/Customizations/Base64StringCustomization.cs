﻿using System.Reflection;
using AutoFixture.Kernel;

namespace WeatherService.Testing.Unit.Core.Customizations;

internal sealed class Base64StringCustomization: ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is PropertyInfo pi && pi.PropertyType == typeof(string) && pi.Name.Contains("Base64"))
        {
            return Build.Base64String();
        }

        return new NoSpecimen();
    }
}