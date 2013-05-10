using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using Wcf;

namespace FSDownloadClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DateTime start = new DateTime();
            System.Console.WriteLine("Press <ENTER> to start the download.");
            System.Console.ReadLine();
            string baseAddress = "net.tcp://buildServer01:2008/FileService";
            ChannelFactory<IFileManager> factory = new ChannelFactory<IFileManager>(MyHost.GetTcpBinding(),
                                                                                    new EndpointAddress(baseAddress));
            IFileManager manager = factory.CreateChannel();
            string remoteFileName = "11.txt";
           
            start = DateTime.Now;
            Console.WriteLine("start time:" + start);
            for (int i = 0; i < 10000; i++)
            {
                MyFileInfo fileInfo = manager.DownloadFile(new DownloadFileRequest(remoteFileName));
               // using (FileStream fs = File.Create("Client_"+i.ToString() + fileInfo.FileName))
                using (MemoryStream memoryStream=new MemoryStream())
                {
                    int bytesRead = 0;
                    long totalBytesRead = 0;
                    byte[] buffer = new byte[1000];
                    // Console.Read();
                    do
                    {
                        bytesRead = fileInfo.Stream.Read(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer,0,bytesRead);
                       // fs.Write(buffer, 0, bytesRead);
                        //totalBytesRead += bytesRead;
                        //double currentProgress = ((double)totalBytesRead) / fileInfo.FileSize;
                        // Console.WriteLine("Read so far {0:.00}% of the file", currentProgress * 100);
                    } while (bytesRead > 0);
                } 
            }
           
            Console.WriteLine("end time:" + DateTime.Now + ",total time:" + (DateTime.Now - start) + ".");
            ((IClientChannel) manager).Close();
            factory.Close();

            Console.ReadKey();
        }
    }
}
