# 🚨This package is obsolete and no longer maintained🚨

With .NET 10 is now possible to expose endpoints using Kestrel. Therefore this package is no longer needed. Please update to dotnet 10 and see my blog post for more details https://blog.netwatwezoeken.nl/integration-testing-with-dotnet-10-and-playwright/ and see how to leverage the new `UseKestrel()`.

# Mvc.Testing
Alternative to [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing) implementation to enable testing using actual endpoints.

This package provides an alternative WebApplicationFactory implementation. Instead of using a [TestServer](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver) it spins up Kestrel to expose listen to a port over the network. This allows a browser to access the application in a test scenario. Ideal to use in combination with [Playwright](https://playwright.dev/dotnet/) for instance.

While the [same is possible using Microsoft.AspNetCore.Mvc.Testing](https://danieldonbavand.com/2022/06/13/using-playwright-with-the-webapplicationfactory-to-test-a-blazor-application/). The method leaves you with a duplicate Host and will cause `ConfigureWebHost` to be invoked twice. This project is a copy of [Microsoft's WebApplicationFactory](https://github.com/dotnet/aspnetcore/tree/main/src/Mvc/Mvc.Testing/src) but it invokes Kerstel instead of the TestServer. The advantage is that you no longer need to worry about any side effect that might occur when `ConfigureWebHost` is called twice.  

## Usage

1. Add this package in you test project

`dotnet add package Nwwz.Mvc.Testing`
2. Create a custom class that inherits from the `WebApplicationFactory`:

```csharp
internal class CustomWebApplicationFactory : Nwwz.Mvc.Testing.WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
            {
                // Add testservices here
            });
}
```

3. A simple (xUnit)test like below will start the app, start a browsers and visit the home page
```csharp
public class Testing
{
    [Fact]
    public async Task Test()
    {
        var factory = new CustomWebApplicationFactory();
        var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        var page = await browser.NewPageAsync();
        
        await page.GotoAsync(factory.ServerAddress);
    }
}
```
4. Starting the whole app and a browser for each test might be a bit much performance wise. Make sure that the app and browser are only started once  and reused for each test. For example by leveraging [xUnit's ICollectionFixture](https://xunit.net/docs/shared-context#collection-fixture)

## Compatibility

The code is fully compatible with Microsoft.AspNetCore.Mvc.Testing. For example, `CreateClient()` will still return a client that connects to the application under test.

The fact that is expose real endpoints comes with the need for a couple of extensions though.

### Server address
The property `ServerAddress` expose the endpoint as can be seen in the example above.

### Toggle https
Toggle between http and https. The default is https.
To use http set `UseHttps` to false.
```csharp
internal class CustomWebApplicationFactory : Nwwz.Mvc.Testing.WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {
        // Optionally specify to use https or not, default is true
        UseHttps = false;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
            {
                // Add testservices here
            });
}
```

## Example

For a full example please see https://github.com/netwatwezoeken/full-integration-testing/tree/main