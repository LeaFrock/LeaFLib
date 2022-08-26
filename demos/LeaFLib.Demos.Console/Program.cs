// See https://aka.ms/new-console-template for more information
using LeaFLib.Demos.Console;
using LeaFLib.Demos.Console.Samples;

Console.WriteLine("App Starts...");

ISample sample = new RandomizeListSample(16, 16000);
await sample.RunAsync();

Console.WriteLine("App Ends...");
Console.ReadKey();