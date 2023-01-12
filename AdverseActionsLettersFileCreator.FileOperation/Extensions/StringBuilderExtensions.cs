using System.Text;

namespace AdverseActionsLettersFileCreator.FileOperation.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder TrimEndByAmount(this StringBuilder sb, int amount)
        {
            if (sb == null || sb.Length == 0) return sb;

            var target = sb.Length - amount;

            var i = sb.Length - 1;
            for (; i >= 0; i--)
                if (!char.IsWhiteSpace(sb[i]) || i + 1 == target)
                    break;

            if (i < sb.Length - 1)
                sb.Length = i + 1;

            return sb;
        }
    }
}
