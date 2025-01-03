using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace QRCodeGen;

public static class Extensions
{
    public static BitArray ToModeIndicator(this QRCodeMode mode)
    {
        return mode switch
        {
            QRCodeMode.Numeric => new BitArray(new bool[] {false, false, false, true}),
            QRCodeMode.Alphanumeric => new BitArray(new bool[] {false, false, true, false}),
            QRCodeMode.Byte => new BitArray(new bool[] {false, true, false, false}),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
    
    public static Boolean IsAlphaNumeric(this string strToCheck)
    {
        Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
        return rg.IsMatch(strToCheck);
    }
    
    public static Boolean IsAlphaNumericUpper(this string strToCheck)
    {
        Regex rg = new Regex(@"^[A-Z0-9\s,]*$");
        return rg.IsMatch(strToCheck);
    }
    
    public static string[] SplitInParts(this string str, int partLength)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        if (partLength <= 0)
        {
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));
        }
        
        var result = new List<string>();
        for (int i = 0; i < str.Length; i += partLength)
        {
            result.Add(str.Substring(i, Math.Min(partLength, str.Length - i)));
        }
        
        return result.ToArray();
    }
}