using SuperSocket.IOCPEasyClient;

namespace SuperSocket.IOCPTcpChannelCreatorFactory;

public static class HostBuilderExtensions
{
    public static ISuperSocketHostBuilder UseIOCPTcpChannelCreatorFactory(this ISuperSocketHostBuilder hostBuilder)
    {
        return hostBuilder.UseChannelCreatorFactory<TcpIocpChannelCreatorFactory>();
    }

    public static ISuperSocketHostBuilder UseIOCPTcpChannelCreatorFactory(this ISuperSocketHostBuilder hostBuilder,
        int maxThreadCount)
    {
        TheadPoolEx.ResetThreadPool(maxThreadCount, maxThreadCount, maxThreadCount, maxThreadCount);
        return hostBuilder.UseChannelCreatorFactory<TcpIocpChannelCreatorFactory>();
    }
}