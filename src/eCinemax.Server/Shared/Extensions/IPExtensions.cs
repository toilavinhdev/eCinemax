using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace eCinemax.Server.Shared.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IPExtensions
{
    public static string GetLocalIPAddress()
    {
        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
        
        socket.Connect("8.8.8.8", 65530);

        if (socket.LocalEndPoint is IPEndPoint endPoint) {
            return endPoint.Address.ToString();
        }

        return "127.0.0.1";
    }
}