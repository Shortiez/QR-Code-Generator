using System.Collections;

namespace QRCodeGen;

public interface IEncoder
{
    public string[] Encode(string data);
}