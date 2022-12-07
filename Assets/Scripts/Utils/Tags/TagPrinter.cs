using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pickup.Utils.Tags
{
    public class TagPrinter
    {
        private string m_Indent = "";
        private readonly StringBuilder m_Sb = new ();

        public override string ToString() => m_Sb.ToString();

        private static string TypeToString(Type type)
        {
            if (type == typeof(byte))
                return "byte";
            if (type == typeof(short))
                return "short";
            if (type == typeof(int))
                return "int";
            if (type == typeof(long))
                return "long";
            if (type == typeof(float))
                return "float";
            if (type == typeof(double))
                return "double";
            if (type == typeof(string))
                return "string";
            if (type == typeof(byte[]))
                return "byte[]";
            if (type == typeof(int[]))
                return "int[]";
            if (type == typeof(TagCompound))
                return "object";
            if (type == typeof(IList))
                return "list";
            throw new ArgumentException("Unknown Type: " + type);
        }

        private void WriteList<T>(char start, char end, bool multiline, IEnumerable<T> list, Action<T> write)
        {
            m_Sb.Append(start);
            m_Indent += "  ";
            var flag = true;
            foreach (T obj in list)
            {
                if (flag)
                    flag = false;
                else
                    m_Sb.Append(multiline ? "," : ", ");
                if (multiline)
                    m_Sb.AppendLine().Append(m_Indent);
                write(obj);
            }

            // string m_Indent
            // m_Indent.Substring(2) == m_Indent[2..]
            m_Indent = m_Indent[2..];
            if (multiline && !flag)
                m_Sb.AppendLine().Append(m_Indent);
            m_Sb.Append(end);
        }

        private void WriteEntry(KeyValuePair<string, object> entry)
        {
            if (entry.Value == null)
            {
                m_Sb.Append('"').Append(entry.Key).Append("\" = null");
            }
            else
            {
                Type type = entry.Value.GetType();
                bool flag = entry.Value is IList && !(entry.Value is Array);
                m_Sb.Append(TypeToString(flag ? type.GetGenericArguments()[0] : type));
                m_Sb.Append(" \"").Append(entry.Key).Append("\" ");
                if (type != typeof(TagCompound) && !flag)
                    m_Sb.Append("= ");
                WriteValue(entry.Value);
            }
        }

        private void WriteValue(object elem)
        {
            switch (elem)
            {
                case byte num1:
                {
                    m_Sb.Append(num1);
                    break;
                }
                case short num2:
                {
                    m_Sb.Append(num2);
                    break;
                }
                case int num3:
                {
                    m_Sb.Append(num3);
                    break;
                }
                case long num4:
                {
                    m_Sb.Append(num4);
                    break;
                }
                case float num5:
                {
                    m_Sb.Append(num5);
                    break;
                }
                case double num6:
                {
                    m_Sb.Append(num6);
                    break;
                }
                case string text:
                {
                    m_Sb.Append('"').Append(text).Append('"');
                    break;
                }
                case byte[] bytes:
                {
                    m_Sb.Append('[').Append(string.Join(", ", bytes)).Append(']');
                    break;
                }
                case int[] ints:
                {
                    m_Sb.Append('[').Append(string.Join(", ", ints)).Append(']');
                    break;
                }
                case TagCompound compound:
                {
                    WriteList('{', '}', true,
                        compound,
                        WriteEntry);
                    break;
                }
                case IList list:
                {
                    var type = list.GetType().GetGenericArguments()[0];
                    WriteList('[', ']',
                        type == typeof(string) || type == typeof(TagCompound) || typeof(IList).IsAssignableFrom(type),
                        list.Cast<object>(), o =>
                        {
                            if (type == typeof(IList))
                                m_Sb.Append(TypeToString(o.GetType().GetGenericArguments()[0])).Append(' ');
                            WriteValue(o);
                        });
                    break;
                }
            }
        }

        public static string Print(TagCompound tag)
        {
            var tagPrinter = new TagPrinter();
            tagPrinter.WriteValue(tag);
            return tagPrinter.ToString();
        }

        public static string Print(KeyValuePair<string, object> entry)
        {
            var tagPrinter = new TagPrinter();
            tagPrinter.WriteEntry(entry);
            return tagPrinter.ToString();
        }
    }
}