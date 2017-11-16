using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Telnet
{
    class Program
    {
        static void Main(string[] args) {

            // Connect to device
            Console.Write("Please enter the Hostname, you wish to connect: ");
            string ipAddress = Console.ReadLine();
            TelnetConnection Ti = new TelnetConnection(ipAddress, 23);

            // Login to device
            Ti.CiscoLogin("cisco");
            Ti.CiscoEnable("cisco");
            Ti.CiscoCommand("Configure Terminal");

            // Configure hostname
            Console.WriteLine("Configure Hostname");
            Console.Write("Enter the name for the device: ");
            string hostInput = Console.ReadLine();
            Ti.CiscoCommand("Hostname ", hostInput);

            Console.WriteLine();

            // Configure VTY
            Ti.CiscoCommand("line vty 0 15");
            Ti.CiscoCommand("password cisco");
            Ti.CiscoCommand("login");
            Ti.CiscoCommand("exit");
            Console.WriteLine("VTY 0 to 15 - CONFIGURED");

            // Configure enable password
            Ti.CiscoCommand("enable secret cisco");
            Ti.CiscoCommand("banner motd #Unauthorized access to this device is prohibited!# ");
            Console.WriteLine("Enable password  - SET");
            Console.WriteLine("Banner MOTD - SET");

            // Configure VLAN
            Console.WriteLine("\nConfigure VLAN");
            Console.Write("Enter the name for the VLAN: ");
            string vlanInput = Console.ReadLine();
            Ti.CiscoCommand("", vlanInput);
            Ti.CiscoCommand("exit");

            Console.WriteLine();

            //Console.WriteLine("Enter the IP and Subnet for the VLAN");
            //Console.Write("IP and Subnet: ");
            //string ipInput = Console.ReadLine();
            //Ti.CiscoCommand("IP Address ", ipInput);
            //Ti.CiscoCommand("exit");

            Console.WriteLine();

            // Configure Interface
            Console.WriteLine("Configure Interface");
            Console.Write("Do you wish to open of close interfaces: ");
            string interfaceInput = Console.ReadLine();

            if (interfaceInput == "open" || interfaceInput == "Open") {

                Console.Write("First Port: ");
                int firstPort = Convert.ToInt32(Console.ReadLine());
                Console.Write("Last Port: ");
                int lastPort = Convert.ToInt32(Console.ReadLine());
                Ti.CiscoCommand("interface range fa0/" + firstPort + " - " + lastPort);
                Ti.CiscoCommand("No shutdown");
                Ti.CiscoCommand("exit");
                Ti.CiscoCommand("exit");
            }
            else if (interfaceInput == "close" || interfaceInput == "Close") {

                Console.Write("First Port: ");
                int firstPort = Convert.ToInt32(Console.ReadLine());
                Console.Write("Last Port: ");
                int lastPort = Convert.ToInt32(Console.ReadLine());
                Ti.CiscoCommand("interface range fa0/" + firstPort + " - " + lastPort);
                Ti.CiscoCommand("Shutdown");
                Ti.CiscoCommand("exit");
                Ti.CiscoCommand("exit");
            }

            Console.WriteLine();

            // Configure Ports to VLAN
            Ti.CiscoCommand("Configure Terminal");
            Console.WriteLine("Assign ports to VLAN");
            Console.WriteLine("Enter the range you wish to assign");
            Console.Write("First Port: ");
            int portFirstVlan = Convert.ToInt32(Console.ReadLine());
            Console.Write("Last Port: ");
            int portLastVlan = Convert.ToInt32(Console.ReadLine());
            Ti.CiscoCommand("interface range fa0/" + portFirstVlan + " - " + portLastVlan);
            Ti.CiscoCommand("No shutdown");
            Ti.CiscoCommand("Switchport mode access");
            Ti.CiscoCommand("switchport access " + vlanInput);
            Ti.CiscoCommand("exit");
            Ti.CiscoCommand("exit");

            // Show Running-Config
            List<String> Result = new List<String>();
            Result = Ti.CiscoCommand("Show running");

            Result.ForEach(delegate (String line) {
                Console.WriteLine(line);
            });

            Console.ReadKey();
        }
    }
}
