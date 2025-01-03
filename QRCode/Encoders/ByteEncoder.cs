using System.Collections;

namespace QRCodeGen;

public class ByteEncoder : IEncoder
{
    public BitArray[] Encode(string data)
    {
        throw new NotImplementedException();
    }
    
    public BitArray BuildData(QRCodeVersion version, BitArray modeIndicator, BitArray characterCountIndicator, BitArray[] encodedData)
    {
        var bitArray = new BitArray(0);
        
        bitArray.AppendBits(modeIndicator);
        bitArray.AppendBits(characterCountIndicator);
        foreach (var data in encodedData)
        {
            bitArray.AppendBits(data);
        }
        
        return bitArray;
    }
}