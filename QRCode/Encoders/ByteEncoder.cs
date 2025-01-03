using System.Collections;
using System.Text;

namespace QRCodeGen;

public class ByteEncoder : IEncoder
{
    public BitArray[] Encode(string data)
    {
        BitArray[] encodedData = new BitArray[data.Length];

        for (var i = 0; i < data.Length; i++)
        {
            var c = data[i];
            
            var hexBytes = CharToHexadecimalBytes(c);
            var binary = HexadecimalBytesToBinary(hexBytes);
            
            encodedData[i] = binary;
        }
        
        return encodedData;
    }
    
    private string CharToHexadecimalBytes(char c)
    {
        var bytes = Encoding.UTF8.GetBytes(new[] { c });
        return BitConverter.ToString(bytes).Replace("-", "");
    }
    
    private BitArray HexadecimalBytesToBinary(string hex)
    {
        var bytes = new byte[hex.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }

        // Reverse the bits in each byte
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)((bytes[i] * 0x0202020202 & 0x010884422010) % 1023);
        }

        return new BitArray(bytes);
    }
}