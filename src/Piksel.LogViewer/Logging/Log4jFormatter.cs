using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Piksel.LogViewer.Logging
{
    public class Log4jParser: ILogParser
    {
        private readonly XmlReaderSettings xmlSettings;
        private readonly XmlParserContext xmlContext;
        private readonly XmlDocument xmlDoc;


        public Log4jParser()
        {
            xmlSettings = new XmlReaderSettings { NameTable = new NameTable() };
            var xmlns = new XmlNamespaceManager(xmlSettings.NameTable);
            xmlns.AddNamespace("log4j", "http://logging.apache.org/log4j/1.2/apidocs/org/apache/log4j/xml/doc-files/log4j.dtd");
            xmlns.AddNamespace("nlog", "http://www.nlog-project.org/schemas/NLog.xsd");
            xmlContext = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
            xmlDoc = new XmlDocument();
        }

        public LogMessage ParseLogData(byte[] buffer)
        {
            var logLevel = LogLevel.None;
            var message = "";
            var source = "unknown";
            string exception = null;

            using (var ms = new MemoryStream(buffer))
            using (var sr = new StreamReader(ms, Encoding.UTF8))
            using (var xr = XmlReader.Create(sr, xmlSettings, xmlContext))
            {
                xmlDoc.Load(xr);
            }

            var eventNode = xmlDoc.FirstChild;

            switch (eventNode.Attributes["level"].InnerText)
            {
                case "DEBUG":
                    logLevel = LogLevel.Debug;
                    break;

                case "INFO":
                default:
                    logLevel = LogLevel.Info;
                    break;

                case "WARN":
                    logLevel = LogLevel.Warning;
                    break;

                case "FATAL":
                case "ERROR":
                    logLevel = LogLevel.Error;
                    break;

                case "TRACE":
                    logLevel = LogLevel.Trace;
                    break;
            }

            source = eventNode.Attributes["logger"].InnerText;

            foreach (XmlNode child in eventNode.ChildNodes)
            {
                switch (child.Name)
                {
                    case "log4j:message":
                        message = child.InnerText;
                        break;

                    case "log4j:locationInfo":
                    case "log4j:NDC":
                    case "log4j:properties":
                        break;

                    case "nlog:eventSequenceNumber":
                    case "nlog:locationInfo":
                    case "nlog:properties":
                        break;

                    case "log4j:throwable":
                        exception = child.InnerText;

                        break;

                    default:
#if DEBUG
                        message += $"\r\n??? {child.Name} (\"{child.InnerText}\")\r\n";
#endif
                        break;
                }
            }

            return new LogMessage.Builder()
                .WithLevel(logLevel)
                .WithSource(source)
                .WithMessageLine(message)
                .WithExceptionLine(exception)
                .Build();
        }

        
    }
}
