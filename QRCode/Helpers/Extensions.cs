using System.Collections;
using System.Text;

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
    
    public static int AsAlphaNumeric(char c)
    {
        return c switch
        {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'A' => 10,
            'B' => 11,
            'C' => 12,
            'D' => 13,
            'E' => 14,
            'F' => 15,
            'G' => 16,
            'H' => 17,
            'I' => 18,
            'J' => 19,
            'K' => 20,
            'L' => 21,
            'M' => 22,
            'N' => 23,
            'O' => 24,
            'P' => 25,
            'Q' => 26,
            'R' => 27,
            'S' => 28,
            'T' => 29,
            'U' => 30,
            'V' => 31,
            'W' => 32,
            'X' => 33,
            'Y' => 34,
            'Z' => 35,
            ' ' => 36,
            '$' => 37,
            '%' => 38,
            '*' => 39,
            '+' => 40,
            '-' => 41,
            '.' => 42,
            '/' => 43,
            ':' => 44,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
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