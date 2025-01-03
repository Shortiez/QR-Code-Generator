using System.Collections;

namespace QRCodeGen;

public static class BitArrayExtensions
{
    /// <summary>
    /// Appends the specified number bits of the specified value to this bit array.
    /// <para>
    /// The least significant bits of the specified value are added. They are appended in reverse order,
    /// from the most significant to the least significant one, i.e. bits 0 to <i>len-1</i>
    /// are appended in the order <i>len-1</i>, <i>len-2</i> ... 1, 0.
    /// </para>
    /// <para>
    /// Requires 0 &#x2264; len &#x2264; 31, and 0 &#x2264; val &lt; 2<sup>len</sup>.
    /// </para>
    /// </summary>
    /// <param name="bitArray">The BitArray instance that this method extends.</param>
    /// <param name="val">The value to append.</param>
    /// <param name="len">The number of low-order bits in the value to append.</param>
    /// <exception cref="ArgumentOutOfRangeException">Value or number of bits is out of range.</exception>
    public static void AppendBits(this BitArray bitArray, uint val, int len)
    {
        if (len < 0 || len > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(len), "'len' out of range");
        }

        if (val >> len != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(val), "'val' out of range");
        }

        var bitLength = bitArray.Length;
        bitArray.Length = bitLength + len;
        var mask = 1U << (len - 1);
        for (var i = bitLength; i < bitLength + len; i++) // Append bit by bit
        {
            if ((val & mask) != 0)
            {
                bitArray.Set(i, true);
            }

            mask >>= 1;
        }
    }
    
    public static void AppendBits(this BitArray bitArray, BitArray bits)
    {
        var bitLength = bitArray.Length;
        bitArray.Length = bitLength + bits.Length;
        for (var i = bitLength; i < bitLength + bits.Length; i++) // Append bit by bit
        {
            bitArray.Set(i, bits[i - bitLength]);
        }
    }
    
    public static void AppendBits(this BitArray bitArray, string binary)
    {
        foreach (var c in binary)
        {
            bitArray.AppendBits(c == '1' ? 1U : 0U, 1);
        }
    }
    
    public static string AsString(this BitArray bitArray)
    {
        var sb = new System.Text.StringBuilder(bitArray.Length);
        for (var i = 0; i < bitArray.Length; i++)
        {
            sb.Append(bitArray[i] ? '1' : '0');
        }

        return sb.ToString();
    }
    
    public static string AsString(this BitArray[] bitArrays)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var bitArray in bitArrays)
        {
            sb.Append(bitArray.AsString());
        }

        return sb.ToString();
    }
}