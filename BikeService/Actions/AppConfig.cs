using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BikeService.Actions
{
    public static class AppConfig
    {
        private static string Path { get; set;}
        private static XDocument ConfigXml { get; set; }

        static AppConfig()
        {
            Path = "C:/Repozytorium/BikeService/BikeService/wwwroot/config.xml";
            ConfigXml = XDocument.Load(Path);
        }

        public static void LastUpdatedRequestSave(string requestId)
        {
            ConfigXml.Root.Element("LastUpdatedRequest").Value = requestId;
            ConfigXml.Save(Path);
        }

        public static string LastUpdatedRequestGet()
        {
            string lastUpdatedRequest = ConfigXml.Root.Element("LastUpdatedRequest").Value;
            ConfigXml.Root.Element("LastUpdatedRequest").Value = "";
            return lastUpdatedRequest;
        }
    }
}
