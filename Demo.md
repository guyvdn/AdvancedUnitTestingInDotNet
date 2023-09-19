# Demo

## Prerequisites

- [ ] Start Docker
- [ ] Run Unit Tests
- [ ] Start Application
- [ ] Collapse Solution Explorer

### Shortcuts

- Collapse all code: Ctrl + M, O
- Expand all code: Ctrl + M, P
- Toggle showing whitespace: CTRL+R, CTRL+W

## 00. Subject Under Test

### Show the Application

- [ ] Call AuditLogs to show Authoriziation is needed
- [ ] Authorize
- [ ] Try again without a value to show validation works
- [ ] Try again with value 10 to show validation is still broken
- [ ] Try again with cvalue 7 to show it works
- [ ] Post an Image
- [ ] Download the Image
- [ ] Get a WeatherForecast
- [ ] Explain this is allready a lot of work to do these manual tests for such a small application, we can do better.

### Show the Code

- [ ] Api talks to Core via MediatR
- [ ] There is a Validation Behaviour
- [ ] There is ExceptionHandlingMiddleware
- [ ] There is a Representation with DTOs

# 01. Unit Testing Frameworks

- [ ] Show used Nuget packages with short explanation
- [ ] Explain Analyzers are also available 

## 10. Unit Testing Core

### Specifications

- [ ] Explain TestClassesTestSpecification, we care about performance
- [ ] Explain TestSpecificationBase, and skip the Customizations for now
- [ ] Explain TestSpecification to enforce AAA pattern
- [ ] Explain TestSpecification<SUT>

### Dependencies

- [ ] Explain DependencyList and DependencyBuilder
- [ ] Explain FakeLogger and FakeLoggerBuilder

### Build

- [ ] Explain Build

### Customizations

- [ ] Explain Customizations

### EquivalencySteps

- [ ] Explain EquivalencySteps

## 20. Unit Testing Features

### TestClassTests

- [ ] Show TestClassTests
- [ ] Show TestSetupFixture and explain OneTimeSetUp

### AddImageTests.When_adding_an_image

- [ ] Show the AddImage handler
- [ ] Explain the AAA style
- [ ] Explain AutoFixture
- [ ] Explain CancellationToken
- [ ] Explain FakeLoggerAssertions
- [ ] Explain FileServiceAssertions

### AddImageTests.When_getting_an_image

- [ ] Explain ExplicitDependencies
- [ ] Explain AndDoes
- [ ] Explain GetImage with Regex

### AddImageTests.When_getting_an_image_that_does_not_exist

- [ ] Explain OneOfAssertions 

### GetCurrentWeatherTests.When_getting_a_WeatherForecast

- [ ] Explain Freeze
- [ ] Explain strict mocking
- [ ] Explain loose mocking

### MapperTests.When_mapping_an_AuditLog

- [ ] Explain AAA inline and AuditLogMessageEquivalencySteps

### MapperTests.When_mapping_an_Image

- [ ] Explain AutoData = one line of code for mapper test

### ValidatorTests.When_validating_a_GetAuditLogs_request

- [ ] Show the Validator
- [ ] Explain FluentValidation TestHelper

### ValidatorTests.When_validating_an_AddImage_request

- [ ] Show Base64Content validation

## 30. Unit Testing Service Registrations

- [ ] Explain ServiceCollectionTestSpecification
- [ ] Run WeatherServiceCoreServiceCollectionTest

## 40. Unit Testing Controllers 

- [ ] Explain ControllerTestSpecification

## 50. Integration Testing Core

- [ ] Explain TestSpecification<TController, TRequest, TResponse>
- [ ] Explain Dependencies, FileService, HttpClient
- [ ] Explain TestApplicationFactory
- [ ] Show DatabaseContext and explain risks of using InMemoryDb

## 60. Integration Testing EF Core Configuration

### WeatherApiDbContextTests

- [ ] Explain Integration tests are useless if the configuration is not correct when not using code first.
- [ ] Explain TestCategory
- [ ] Explain EfSchemaCompare and show kind of errors

## 70. Integration Testing Features

### GetCurrentWeatherTests.When_all_is_good

- [ ] RichardSzalay.MockHttp
- [ ] Use Should().BeCloseTo()
- [ ] TestSink

### GetCurrentWeatherTests.When_the_request_is_invalid

- [ ] Should().HaveProblemDetails()

### GetCurrentWeatherTests.When_the_user_is_not_authenticated

- [ ] Unauthorized

### ImageTests.When_all_is_good (skip if needbe)

- [ ] Some more FluentAssertions extensions

## 80. Integration Testing Seeding

### We use Bogus for seeding

- [ ] Show Fakers 

### When_a_lot_of_data_is_stored_in_the_database

- [ ] LongRunning
- [ ] FirstResponderKit
- [ ] Assert.Fail with CREATE INDEX script