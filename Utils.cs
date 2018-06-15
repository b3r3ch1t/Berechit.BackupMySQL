using MySql.Data.Types;
using System;
using System.Globalization;
using System.Text;

namespace Berechit.BackupMySQL
{
    public static class Utils
    {
        private static readonly DateTimeFormatInfo DateFormatInfo = new DateTimeFormatInfo()
        {
            DateSeparator = "-",
            TimeSeparator = ":"
        };

        private static readonly NumberFormatInfo NumberFormatInfo = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = string.Empty
        };

        public static string ConvertToSqlFormat(object obj)
        {
            var sb = new StringBuilder();

            var typeCode = Type.GetTypeCode(obj.GetType());
            string result;

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    sb.AppendFormat(Convert.ToInt32(obj).ToString());
                    break;
                case TypeCode.Byte:
                    if (((byte[])obj).Length == 0)
                    {

                        sb.AppendFormat("''");
                    }
                    else
                    {
                        result = BitConverter.ToString((byte[])obj).Replace("-", "");

                        sb.AppendFormat("X'{0}'", result);
                    }
                    break;

                case TypeCode.DateTime:
                    sb.AppendFormat("'");
                    sb.AppendFormat(((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss", DateFormatInfo));
                    sb.AppendFormat("'");
                    break;
                case TypeCode.DBNull:
                    sb.AppendFormat("NULL");
                    break;
                case TypeCode.Decimal:
                    sb.AppendFormat(((decimal)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.Double:
                    sb.AppendFormat(((double)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.Int16:
                    sb.AppendFormat(((short)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.Int32:
                    sb.AppendFormat(((int)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.Int64:
                    sb.AppendFormat(((long)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.SByte:
                    sb.AppendFormat(((sbyte)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.Single:
                    sb.AppendFormat(((float)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.String:
                case TypeCode.Char:
                    result = EscapeStringSequence(obj.ToString());
                    sb.AppendFormat("'{0}'", result);
                    break;
                case TypeCode.UInt16:
                    sb.AppendFormat(((ushort)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.UInt32:
                    sb.AppendFormat(((uint)obj).ToString(NumberFormatInfo));
                    break;
                case TypeCode.UInt64:
                    sb.AppendFormat(((ulong)obj).ToString(NumberFormatInfo));
                    break;
                default:


                    if (obj is byte[] bytes)
                    {
                        if (bytes.Length == 0)
                        {

                            sb.AppendFormat("''");
                        }
                        else
                        {
                            

                            result = BitConverter.ToString((byte[])obj).Replace("-", "");

                            sb.AppendFormat("X'{0}'", result);
                        }
                        break;
                    }
                    switch (obj)
                    {
                        case TimeSpan _:
                            var ts = (TimeSpan)obj;
                            sb.AppendFormat("'");

                            sb.AppendFormat(ts.Hours.ToString().PadLeft(2, '0'));
                            sb.AppendFormat(":");
                            sb.AppendFormat(ts.Minutes.ToString().PadLeft(2, '0'));
                            sb.AppendFormat(":");
                            sb.AppendFormat(ts.Seconds.ToString().PadLeft(2, '0'));

                            sb.AppendFormat("'");
                            break;
                        case Guid _:

                            sb.AppendFormat("'");
                            sb.Append(obj);
                            sb.AppendFormat("'");

                            break;
                    }


                    break;

            }

            return sb.ToString();
        }

        public static string EscapeStringSequence(string data)
        {
            var builder = new StringBuilder();
            foreach (var ch in data)
            {
                switch (ch)
                {
                    case '\\': // Backslash
                        builder.AppendFormat("\\\\");
                        break;
                    case '\r': // Carriage return
                        builder.AppendFormat("\\r");
                        break;
                    case '\n': // New Line
                        builder.AppendFormat("\\n");
                        break;
                    //case '\a': // Vertical tab
                    //    builder.AppendFormat("\\a");
                    //    break;
                    case '\b': // Backspace
                        builder.AppendFormat("\\b");
                        break;
                    //case '\f': // Formfeed
                    //    builder.AppendFormat("\\f");
                    //    break;
                    case '\t': // Horizontal tab
                        builder.AppendFormat("\\t");
                        break;
                    //case '\v': // Vertical tab
                    //    builder.AppendFormat("\\v");
                    //    break;
                    case '\"': // Double quotation mark
                        builder.AppendFormat("\\\"");
                        break;
                    case '\'': // Single quotation mark
                        builder.AppendFormat("''");
                        break;
                    default:
                        builder.Append(ch);
                        break;
                }
            }

            return builder.ToString();
        }
    }
}
