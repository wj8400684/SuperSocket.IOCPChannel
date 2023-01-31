using Microsoft.Extensions.Logging;
using SuperSocket.Channel;
using SuperSocket.Client;
using SuperSocket.IOCPTcpChannel;
using SuperSocket.ProtoBase;
using System.Net;

namespace SuperSocket.IOCPEasyClient;

public class IOCPEasyClient<TPackage, TSendPackage> : EasyClient<TPackage>, IEasyClient<TPackage, TSendPackage>
    where TPackage : class
{
    private IPackageEncoder<TSendPackage> _packageEncoder;
    private IPipelineFilter<TPackage> _pipelineFilter;

    public IOCPEasyClient(IPipelineFilter<TPackage> pipelineFilter, IPackageEncoder<TSendPackage> packageEncoder, ILogger? logger = null)
        : this(pipelineFilter, packageEncoder, new ChannelOptions { Logger = logger })
    {
    }

    public IOCPEasyClient(IPipelineFilter<TPackage> pipelineFilter, IPackageEncoder<TSendPackage> packageEncoder, ChannelOptions options)
        : base(pipelineFilter, options)
    {
        _pipelineFilter = pipelineFilter;
        _packageEncoder = packageEncoder;
    }

    protected override async ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken)
    {
        var connector = GetConnector();
        var state = await connector.ConnectAsync(remoteEndPoint, null, cancellationToken);

        if (state.Cancelled || cancellationToken.IsCancellationRequested)
        {
            OnError($"The connection to {remoteEndPoint} was cancelled.", state.Exception);
            return false;
        }

        if (!state.Result)
        {
            OnError($"Failed to connect to {remoteEndPoint}", state.Exception);
            return false;
        }

        var socket = state.Socket;

        if (socket == null)
            throw new Exception("Socket is null.");

        var channelOptions = Options;
        SetupChannel(new IOCPTcpPipeChannel<TPackage>(socket, _pipelineFilter, channelOptions));
        return true;
    }

    public virtual async ValueTask SendAsync(TSendPackage package)
    {
        await SendAsync(_packageEncoder, package);
    }

    public new IEasyClient<TPackage, TSendPackage> AsClient()
    {
        return this;
    }
}
