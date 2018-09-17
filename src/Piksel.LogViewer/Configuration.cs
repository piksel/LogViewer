using System.Collections.Generic;

namespace Piksel.LogViewer
{
    public class Configuration
    {
        public ApplicationConfig Application { get; set; } 

        public class ApplicationConfig
        {
            public bool UseThemedBackground { get; set; }

            public int? WindowY { get; set; }
            public int? WindowX { get; set; }
            public int? WindowWidth { get; set; }
            public int? WindowHeight { get; set; }

            public bool Maximized { get; set; }
        }

        public FileLogConfig FileLog { get; set; }

        public class FileLogConfig
        {
            public ParserOptions DefaultParserOptions { get; set; }

            public class ParserOptions
            {
                public string FieldOrder { get; set; }

                public string PrimaryDelimiter { get; set; }
                public string SecondaryDelimiter { get; set; }
            }

            public Dictionary<string, ParserOptions> PathParserOptions { get; set; }
        }

        public static Configuration Default
            => new Configuration()
            {
                Application = new ApplicationConfig()
                {
                    UseThemedBackground = false,
                    Maximized = false
                },
                FileLog = new FileLogConfig()
                {
                    DefaultParserOptions = new FileLogConfig.ParserOptions()
                    {
                        FieldOrder = "Level, Time, Source, Message",
                        PrimaryDelimiter = " "
                    },
                    PathParserOptions = new Dictionary<string, FileLogConfig.ParserOptions>(),
                }
            };
    }
}