using System.Linq;

namespace Utils
{
    public class StringUtils
    {
        public static string Uncaptalize(string content)
        {
            char[] chars = content.ToCharArray();

            char letter = content.First(char.IsLetter);
            int index = content.IndexOf(letter);

            chars[index] = char.ToLower(chars[index]);

            return new string(chars);
        }

        public static string Captalize(string content)
        {
            char[] chars = content.ToCharArray();

            char letter = content.First(char.IsLetter);
            int index = content.IndexOf(letter);

            chars[index] = char.ToUpper(chars[index]);

            return new string(chars);
        }
    }
}