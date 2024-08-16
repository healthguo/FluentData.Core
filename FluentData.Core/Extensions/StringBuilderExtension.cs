using System.Text;

namespace FluentData.Core.Extensions
{
    public static class StringBuilderExtension
    {
        /// <summary>
        /// Replaces all occurrences of a specified string in this instance with another 
        /// specified string, using the specified comparison options.
        /// </summary>
        /// <param name="sb">The StringBuilder to perform the operation on.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string to replace with.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies how the search should be performed.</param>
        /// <returns>A reference to the original StringBuilder object.</returns>
        public static StringBuilder Replace(this StringBuilder sb, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (oldValue == null) throw new ArgumentNullException(nameof(oldValue));
            if (newValue == null) throw new ArgumentNullException(nameof(newValue));

            int index;
            var startIndex = 0;
            while ((index = sb.ToString().IndexOf(oldValue, startIndex, comparisonType)) != -1)
            {
                sb.Remove(index, oldValue.Length);
                sb.Insert(index, newValue);
                startIndex = index + newValue.Length;
            }
            return sb;
        }
    }

}
