using System.Net.NetworkInformation;

namespace BillWare.App.Helpers
{
    public class InternetConnectionHelper
    {
        public static bool IsInternetConnected()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                Ping ping = new Ping();
                try
                {
                    PingReply reply = ping.Send("www.google.com");
                    return reply.Status == IPStatus.Success;
                }
                catch (PingException)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
