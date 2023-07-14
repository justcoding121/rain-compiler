using Microsoft.Extensions.DependencyInjection;
using Rain.Compiler;

var serviceCollection = new ServiceCollection();
DependencyRegistrations.Register(serviceCollection);
var provider = serviceCollection.BuildServiceProvider();

using var scope = provider.CreateScope();
var driver = scope.ServiceProvider.GetRequiredService<Driver>();
driver.Drive("");
