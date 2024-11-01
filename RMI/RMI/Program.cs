using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;

namespace RMI
{
    internal static class Program
    {
        [ServiceContract]
        public interface IMessageService
        {
            [OperationContract]
            string[] GetMessage();
        }
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class MessageService : IMessageService
        { 
            public string[] GetMessage()
            {
                return new string[] { "server1", "server2", "server3" };
            }
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            var uris = new Uri[1];
            string address = "net.tcp://localhost:6565/MessageService";
            uris[0] = new Uri(address);
            IMessageService message = new MessageService();
            ServiceHost host = new ServiceHost(message, uris);
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IMessageService), binding, "");
            host.Open();

        }

        private static void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Message service started");
        }
    }
}
