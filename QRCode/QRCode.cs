using System.Collections;

namespace QRCodeGen;

public class QRCode
{
    protected QRCodeMode Mode { get; }
    protected QRCodeVersion Version { get; }
    protected int ModeIndicator { get; }
    protected int CharacterCountIndicator { get; }
    
    public QRCode(string data, ErrorCorrectionLevel errorCorrectionLevel)
    {
        Mode = GetMostEfficientMode(data);
        Version = GetSmallestVersion(data, errorCorrectionLevel);
        
        ModeIndicator = Mode.ToModeIndicator();
        CharacterCountIndicator = GetCharacterCountIndicator(data);
        
        // Encoding data
        IEncoder encoder = Mode switch
        {
            QRCodeMode.Numeric => new NumericEncoder(),
            QRCodeMode.Alphanumeric => new AlphanumericEncoder(),
            QRCodeMode.Byte => new ByteEncoder(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        string[] encodedData = encoder.Encode(data);
        foreach (var binary in encodedData)
        {
            Console.WriteLine(binary);
        }
    }
    
    private QRCodeMode GetMostEfficientMode(string data)
    {
        if (data.All(char.IsDigit))
        {
            return QRCodeMode.Numeric;
        }
        
        if (data.All(char.IsLetterOrDigit))
        {
            return QRCodeMode.Alphanumeric;
        }
        
        return QRCodeMode.Byte;
    }
    private QRCodeVersion GetSmallestVersion(string data, ErrorCorrectionLevel errorCorrectionLevel)
    {
        // TODO: Implement upper limit for data length
        
        for (int i = 1; i <= VersionLibrary.Versions.Length; i++)
        {
            var version = VersionLibrary.GetVersion(i);
            var characterCapacity = version.CharacterCapacities[errorCorrectionLevel];
            
            switch (Mode)
            {
                case QRCodeMode.Numeric:
                    if (data.Length <= characterCapacity.Numeric)
                    {
                        return version;
                    }
                    break;
                case QRCodeMode.Alphanumeric:
                    if (data.Length <= characterCapacity.Alphanumeric)
                    {
                        return version;
                    }
                    break;
                case QRCodeMode.Byte:
                    if (data.Length <= characterCapacity.Byte)
                    {
                        return version;
                    }
                    break;
            }
        }
        
        throw new Exception("Data is too large to fit in any version of QR code.");
    }
    private Int16 GetCharacterCountIndicator(string data)
    {
        // TODO: Implement character count indicator
        
        return 0;
    }
}

public enum QRCodeMode
{
    Numeric,
    Alphanumeric,
    Byte,
}

public enum ErrorCorrectionLevel
{
    L,
    M,
    Q,
    H,
}