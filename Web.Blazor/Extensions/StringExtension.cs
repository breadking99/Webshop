using System.Text;
using System.Reflection;

namespace Web.Blazor.Extensions;

public static class StringExtension
{
    public static void AppendJoin<T>(this StringBuilder sb, string separator, IEnumerable<T> values) =>
        sb.Append(string.Join(separator, values));

    public static void AppendJoin<T>(this StringBuilder sb, char separator, IEnumerable<T> values) =>
        AppendJoin(sb, separator.ToString(), values);

    //!? AI_GENERATED_ALGORITHM 
    // Convert a POCO to query string: PropertyName=Value&PropertyName2=Value2
    // - Skips null values
    // - Collections (except string) are joined with comma
    // - DateTime is ISO8601 (round-trip)
    // - Booleans lower-case
    // - Nested complex types are flattened with Property_SubProperty naming
    public static string AddQuery<T>(this StringBuilder sb, T? value)
    {
        if (value == null) return string.Empty;
        bool first = true;
        WriteObject(typeof(T), value, prefix: null);
        return sb.ToString();

        void WriteObject(Type type, object obj, string? prefix)
        {
            foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!prop.CanRead) continue;
                var val = prop.GetValue(obj);
                if (val == null) continue;
                string name = prefix == null ? prop.Name : prefix + "_" + prop.Name;
                if (IsSimple(prop.PropertyType))
                {
                    AppendPair(name, FormatValue(val));
                }
                else if (val is IEnumerable<object> enumerable && prop.PropertyType != typeof(string))
                {
                    var joined = string.Join(',', enumerable.Select(FormatValue));
                    AppendPair(name, joined);
                }
                else
                {
                    // Nested complex type
                    WriteObject(prop.PropertyType, val, name);
                }
            }
        }

        void AppendPair(string key, string val)
        {
            if (!first) sb.Append('&');
            sb.Append(Uri.EscapeDataString(key));
            sb.Append('=');
            sb.Append(Uri.EscapeDataString(val));
            first = false;
        }

        bool IsSimple(Type t) => t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(Guid);

        string FormatValue(object v)
        {
            return v switch
            {
                DateTime dt => dt.ToString("o"),
                bool b => b.ToString().ToLowerInvariant(),
                Enum e => e.ToString(),
                _ => v.ToString() ?? string.Empty
            };
        }
    }
}