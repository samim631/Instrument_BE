using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.IO;
// Fikk masse hjelp av Christian Hovden og Odd Smith-Jahansen
namespace Instrument_BE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Introdoction
            Console.WriteLine("instrumentBE has started..");
            Console.WriteLine("Pleaee enter TCP prot number :");
            string serverPort = Console.ReadLine();
            
           
            
            // serial configrutation  load fro mfile 
           

            Console.WriteLine("started");
            /*
            string serialResp = serialCommand("COM3", "readconf");
            Console.WriteLine(serialResp);
            Console.ReadKey();
            */
            string portName = "COM3";
            string commandReceived = "";

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 5000);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Bind(endpoint);
                server.Listen(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                
            }

            Console.WriteLine("Server started");

            while (true)
            {
                Socket client = server.Accept();
                Console.WriteLine("Client connected.");

                //received data
                byte[] buffer = new byte[1024];
                int bytesReceived = client.Receive(buffer);
                commandReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                Console.WriteLine("Received command: " + commandReceived);
                client.Send(Encoding.ASCII.GetBytes(serialCommand(portName, commandReceived)));
                client.Close();
                Console.WriteLine("Client disconnected...");
            }



            string serialCommand(string port, string command)
            {

                int baudRate = 9600;
                string serialResponse = "";
                SerialPort serialPort = new SerialPort(portName, baudRate);
                try
                {

                    serialPort.Open();
                    serialPort.WriteLine(command);
                    serialResponse = serialPort.ReadLine();
                    serialPort.Close();

                }
                catch (System.IO.IOException) 
                {

                    serialResponse = "serialport failed...";
                }
                return serialResponse;
            }
        }
    }
}
