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
            Ti.CiscoCommand("exit");

            // Configure VTY
            Ti.CiscoCommand("line vty 0 15");
            Ti.CiscoCommand("password cisco");
            Ti.CiscoCommand("exit");
            
            // Configure enable password
            Ti.CiscoCommand("enable secret cisco");
            Ti.CiscoCommand("banner motd !No access allowed!");

            // Configure VLAN
            Console.WriteLine("Configure VLAN");
            Console.Write("Enter the name for the VLAN: ");
            string vlanInput = Console.ReadLine();
            Ti.CiscoCommand("interface ", vlanInput);

            Console.WriteLine("Enter the IP and Subnet for the VLAN");
            Console.Write("IP: ");
            string ipInput = Console.ReadLine();
            Console.Write("Subnet: ");
            string subnetInput = Console.ReadLine();
            Ti.CiscoCommand("IP Address ", ipInput + "" + subnetInput);
            Ti.CiscoCommand("exit");

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

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
