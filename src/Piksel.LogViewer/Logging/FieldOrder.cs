using Piksel.LogViewer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Piksel.LogViewer.Logging
{
    public partial class FieldOrder
    {

        byte[] fields;

        public byte? Level { get => this[LogField.Level]; set => this[LogField.Level] = value; }

        public byte? this[LogField field]
        {
            get => fields[(byte)field] >= 0 ? (byte?)fields[(byte)field] : null;
            
            set
            {
                if (value.HasValue)
                {
                    byte index = (byte)(value.Value + 1);
                    // Unset all other fields using this index
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] == index)
                        {
                            fields[i] = 0;
                        }
                    }
                    fields[(byte)field] = index;
                }
                else
                {
                    fields[(byte)field] = 0;
                }
            }
        }

        public FieldOrder()
        {
            fields = Enum.GetValues(typeof(LogField))
                .Cast<byte>()
                .Select(b => (byte)(b + 1))
                .ToArray();
        }

        public bool HasIndex(LogField field)
            => fields[(byte)field] >= 0;

        public IEnumerable<LogField> GetOrderedFields()
        {
            for(int ii=1; ii<=fields.Length; ii++)
            {
                for (int fi = 0; fi < fields.Length; fi++)
                {
                    if (fields[fi] == ii) yield return (LogField)fi;
                }
            }
        }

        public int GroupIndex(LogField field)
            => fields[(byte)field] >= 0
            ? fields[(byte)field]
            : throw new InvalidOperationException("The field \"{field}\" does not have an order index.");

        public bool TrySetFromString(string text)
        {
            var parts = text.Split(',');
            var newFields = new byte[fields.Length];
            for(byte i = 0; i< parts.Length; i++)
            {
                if (Enum.TryParse(parts[i], out LogField field))
                {
                    newFields[(byte)field] = (byte)(i + 1);
                }
                else
                {
                    return false;
                }
            }

            fields = newFields;
            return true;
        }

        public override string ToString()
            => string.Join(", ", GetOrderedFields().Select(f => f.ToString()).ToArray());
    }
}