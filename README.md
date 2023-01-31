未作全部测试谨慎使用！

await SuperSocketHostBuilder.Create<StringPackageInfo, CommandLinePipelineFilter>()
    .UseIOCPTcpChannelCreatorFactory()
    .Build()
    .RunAsync();
