namespace AstralForge.Extensions;

public static class StringExtensions
{

    public static int LevenshteinDistance(this string a, string b)
    {
        var costs = new int[b.Length + 1];
        for (int i = 0; i <= a.Length; i++)
        {
            var lastValue = i;
            for (int j = 0; j <= b.Length; j++)
            {
                if (i == 0)
                    costs[j] = j;
                else if (j > 0)
                {
                    var newValue = costs[j - 1];
                    if (a[i - 1] != b[j - 1])
                        newValue = Math.Min(Math.Min(newValue, lastValue), costs[j]) + 1;
                    costs[j - 1] = lastValue;
                    lastValue = newValue;
                }
            }
            if (i > 0)
                costs[b.Length] = lastValue;
        }
        return costs[b.Length];
    }
}
