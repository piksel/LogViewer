using Piksel.LogViewer.Helpers;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Piksel.LogViewer.Logging
{
    /// <summary>
    /// Represents a log message from a log input source
    /// </summary>
    public class LogMessage
    {
        // Exception and message cannot be null
        private string exception = string.Empty;
        private string message = string.Empty;

        /// <summary>
        /// Message part of log message, guaranteed to not be null
        /// </summary>
        public string Message
        {
            get => message;
            set => message = value ?? string.Empty;
        }

        /// <summary>
        /// Exception part of log message, guaranteed to not be null
        /// </summary>
        public string Exception
        {
            get => exception;
            set => exception = value ?? string.Empty;
        }

        /// <summary>
        /// Returns whether <see cref="Exception"/> equals <see cref="string.Empty"/>
        /// </summary>
        public bool HasException 
            => exception == string.Empty;

        public string Source { get; set; }
        public LogLevel LogLevel { get; set; }
        public DateTime Time { get; set; }
        public string RawTime { get; set; }

        protected bool hasTime = true;

        public string FormatLocalTime(string format, IFormatProvider provider)
            => hasTime ? Time.ToString(format, provider) : EmptyDateTime.ToString(format, provider);

        public string FormatLocalTime(string format)
            => FormatLocalTime(format, CultureInfo.CurrentCulture);

        public string FormattedTime => RawTime ?? FormatLocalTime("yyyy-MM-dd HH:mm:ss");

        [Obsolete("LogMessage.Builder should be used instead")]
        public LogMessage(LogLevel logLevel, string source, string message, string exception)
        {
            LogLevel = logLevel;
            Source = source;
            Message = message;
            Exception = exception;
        }

        protected LogMessage()
        {
        }

        public class Builder
        {
            private LogMessage lm = new LogMessage();
            private StringBuilder messageBuilder = new StringBuilder();
            private StringBuilder exceptionBuilder = new StringBuilder();

            // Note: Time offsets are specifed in the amount of time to be subtracted from the input time to make it UTC
            private TimeSpan timeOffset = TimeSpan.Zero;
            private static TimeSpan localOffset;

            protected DateTimeOffset? utcTime;

            public bool IsEmpty => messageBuilder.Length == 0;

            static Builder()
            {
                var localNow = DateTime.Now;
                localOffset = localNow.ToUniversalTime() - localNow;
            }

            public Builder() {}

            /// <summary>
            /// Sets the log message <see cref="LogLevel"/>
            /// </summary>
            /// <param name="logLevel"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithLevel(LogLevel logLevel)
            {
                Debug.Assert(logLevel != LogLevel.None);
                lm.LogLevel = logLevel;
                return this;
            }

            /// <summary>
            /// Sets the log message <see cref="Source"/>
            /// </summary>
            /// <param name="source"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithSource(string source)
            {
                lm.Source = source;
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="time"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithTime(DateTimeOffset time)
            {
                utcTime = time;
                lm.RawTime = null;
                return this;
            }

            /// <summary>
            /// Sets the time to a raw string that is not processed further
            /// </summary>
            /// <param name="time"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithRawTime(string time)
            {
                lm.RawTime = time;
                return this;
            }

            /// <summary>
            /// Sets the log message <see cref="Time"/> from <paramref name="time"/> using current local time zone
            /// </summary>
            /// <param name="time"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithLocalTime(DateTime time)
                => WithTime(new DateTimeOffset(time, localOffset));

            /// <summary>
            /// Sets the log message <see cref="Time"/> from <paramref name="time"/> using current local time zone
            /// </summary>
            /// <param name="time"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithCustomTime(DateTime time)
                => WithTime(new DateTimeOffset(time, timeOffset));

            public Builder WithUniversalTime(DateTime time)
                => WithTime(new DateTimeOffset(time, TimeSpan.Zero));

            /// <summary>
            /// Sets the log message time offset from <paramref name="ts"/> and updates the <see cref="Time"/> accordingly
            /// </summary>
            /// <param name="ts">The time offset of the log input source</param>
            /// <remarks>If the input source uses UTC this should be <see cref="TimeSpan.Zero"/></remarks>
            /// <returns>Returns the builder instance</returns>
            public Builder UsingTimezone(TimeSpan ts)
            {
                // Only update the time if we actually change the timespan to avoid problems with timezone set inside loops
                if (ts != timeOffset)
                {
                    timeOffset = ts;
                    utcTime.HasValue(value => utcTime = value.ToOffset(ts));
                }
                return this;
            }

            /// <summary>
            /// Sets the log message time offset from <paramref name="tzi"/> and updates the <see cref="Time"/> accordingly
            /// </summary>
            /// <param name="tzi">The time zone used in the log input source</param>
            /// <returns>Returns the builder instance</returns>
            public Builder UsingTimezone(TimeZoneInfo tzi)
                => UsingTimezone(tzi.BaseUtcOffset);

            /// <summary>
            /// Appends <paramref name="exception"/> to the <see cref="Exception"/> builder
            /// </summary>
            /// <param name="exception"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithExceptionLine(string exception)
            {
                if (exceptionBuilder.Length > 0) exceptionBuilder.AppendLine();
                exceptionBuilder.Append(exception);
                return this;
            }

            /// <summary>
            /// Appends <paramref name="message"/> to the <see cref="Message"/> builder
            /// </summary>
            /// <param name="message"></param>
            /// <returns>Returns the builder instance</returns>
            public Builder WithMessageLine(string message)
            {
                if (messageBuilder.Length > 0) messageBuilder.AppendLine();
                messageBuilder.Append(message);
                return this;
            }

            /// <summary>
            /// Builds the LogMessage and resets the message and exception values
            /// </summary>
            /// <returns></returns>
            public LogMessage Build()
                => Build(true);

            /// <summary>
            /// Builds the LogMessage and resets the message and exception values if <paramref name="resetLines"/> is <c>true</c>
            /// </summary>
            /// <param name="resetLines">Whether the message and exception values should be reset</param>
            /// <returns></returns>
            public LogMessage Build(bool resetLines)
            {
                lm.Message = messageBuilder.ToString();
                lm.Exception = exceptionBuilder.ToString();
                lm.Time = utcTime?.LocalDateTime ?? DateTime.Now;
                lm.hasTime = utcTime.HasValue;

                var result = lm;

                if (resetLines)
                {
                    messageBuilder.Clear();
                    exceptionBuilder.Clear();

                    lm = new LogMessage()
                    {
                        LogLevel = result.LogLevel,
                        Source = result.Source,
                        Time = result.Time,
                        RawTime = result.RawTime
                    };
                }

                return result;
            }

            public Builder WithoutTime()
            {
                utcTime = null;
                return this;
            }
        }
    }
    
}