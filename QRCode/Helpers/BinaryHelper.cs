using System.Collections;
using System.Text;

namespace QRCodeGen;

public static class BinaryHelper
{
    public static BitArray ToBitArray(this string s)
    {
        var bits = new BitArray(s.Length);
        
        for (int i = 0; i < s.Length; i++)
        {
            bits[i] = s[i] == '1';
        }

        return bits;
    }
    
    public static string ToBinaryString(this BitArray bits)
    {
        var sb = new StringBuilder(bits.Length);
        
        foreach (bool bit in bits)
        {
            sb.Append(bit ? '1' : '0');
        }

        return sb.ToString();
    }
    
    // To 10bit binary string
    public static string ToTenBitBinaryString(this int i)
    {
        return Convert.ToString(i, 2).PadLeft(10, '0');
    }
}