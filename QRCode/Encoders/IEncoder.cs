using System.Collections;

namespace QRCodeGen;

public interface IEncoder
{
    public BitArray[] Encode(string data);
}