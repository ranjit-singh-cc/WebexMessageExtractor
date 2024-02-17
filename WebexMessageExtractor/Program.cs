// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using WebexMessageExtractor;

var config = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: false)
.Build();

WebexMessageExtractorMain.Start(config);
