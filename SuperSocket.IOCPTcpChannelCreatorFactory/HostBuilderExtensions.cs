namespace SuperSocket.IOCPTcpChannelCreatorFactory;

public static class HostBuilderExtensions
{
    // move to extensions
    public static ISuperSocketHostBuilder UseIOCPTcpChannelCreatorFactory(this ISuperSocketHostBuilder hostBuilder)
    {
        return hostBuilder.UseChannelCreatorFactory<TcpIocpChannelCreatorFactory>();
    }
}