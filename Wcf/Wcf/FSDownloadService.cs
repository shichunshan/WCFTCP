using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;
namespace Wcf
{
    [MessageContract]
    public class MyFileInfo
    {
        [MessageHeader]
        public string FileName;
        [MessageHeader]
        public long FileSize;
        [MessageBodyMember]
        public Stream Stream;
        public MyFileInfo() { }
        public MyFileInfo(Stream stream, string fileName, long fileSize)
        {
            this.Stream = stream;
            this.FileSize = fileSize;
            this.FileName = fileName;
        }
    }

    [MessageContract]
    public class DownloadFileRequest
    {
        [MessageBodyMember]
        public readonly string FileName;
        public DownloadFileRequest() { }
        public DownloadFileRequest(string fileName)
        {
            this.FileName = fileName;
        }
    }
    [ServiceContract]
    public interface IFileManager
    {
        [OperationContract]
        MyFileInfo DownloadFile(DownloadFileRequest request);
    }
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class MyService : IFileManager
    {
        public MyFileInfo DownloadFile(DownloadFileRequest request)
        {
            FileInfo fi = new FileInfo(request.FileName);
            MyFileInfo result = new MyFileInfo(File.OpenRead(request.FileName), request.FileName, fi.Length);
            return result;
        }
    }

    public class MyHost
    {
        static ServiceHost host = null;
        public static void Open()
        {
            string baseAddress = "net.tcp://localhost:2008/FileService";
            host = new ServiceHost(typeof(MyService), new Uri(baseAddress));
            host.AddServiceEndpoint(typeof(IFileManager), GetTcpBinding(), "");
            host.Open();
        }
        public static void Close()
        {
            if (host != null && host.State == CommunicationState.Opened)
            {
                host.Close();
            }
            host = null;
        }
        public static Binding GetTcpBinding()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.None;
            //binding.TransferMode = TransferMode.Streamed;
            binding.TransferMode = TransferMode.Buffered;
            binding.ReliableSession.Enabled = true;
        
            binding.MaxReceivedMessageSize = int.MaxValue;
            return binding;
        }
    }
}