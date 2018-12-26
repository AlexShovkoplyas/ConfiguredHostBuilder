using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ConfigReader
{
    class Program
    {
        static void Main(string[] args)
        {            
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddXmlFile("app.config", false, true)
                .Build();

            //config files could be dynamically changed in runtime
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("From Json\n");

                Console.WriteLine("----- Dynamicaly typed configuration");
                Console.WriteLine($" Organization { config["organization"] } !");
                Console.WriteLine($" Name { config["person:firstName"] } !");

                Console.WriteLine("\n----- Strongly typed configuration");
                var person = config.GetSection("person").Get<Person>();
                Console.WriteLine($" Full name : {person.FullName } !");

                Console.WriteLine("\n\nFrom Xml\n");

                Console.WriteLine("----- Dynamicaly typed configuration");
                Console.WriteLine($" Organization { config["organization"] } !");
                Console.WriteLine($" Name { config["person:firstName"] } !");

                //strange bechaviour when same settings section in different configuration files
                Console.WriteLine("\n----- Strongly typed configuration");
                var person2 = config.GetSection("person").Get<Person>();
                Console.WriteLine($" Full name : {person2.FullName } !");

                Console.WriteLine("\n" + config["connectionStrings:add:localConnection:connectionString"]);

                Console.ReadLine();
            }

            
        }
    }
}
