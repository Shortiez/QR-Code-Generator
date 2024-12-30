using System.Collections;

namespace QRCodeGen;

public class QRCode
{
    public struct EncodingData
    {
        public int ModeIndicator { get; set; }
        public int CharacterCountIndicator { get; set; }
        public string[] EncodedData { get; set; }
        
        public string BitString => GetBitString();
        
        public EncodingData(int modeIndicator, int characterCountIndicator, string[] encodedData)
        {
            ModeIndicator = modeIndicator;
            CharacterCountIndicator = characterCountIndicator;
            EncodedData = encodedData;
        }

        private string GetBitString()
        {
            var returnValue = "";
            
            returnValue += ModeIndicator;
            returnValue += CharacterCountIndicator.ToTenBitBinaryString();
            returnValue += EncodedData;
            
            return returnValue; 
        }
    }
    
    public QRCodeMode Mode { get; }
    protected QRCodeVersion Version { get; }
    protected EncodingData EncodedData { get; }
    
    public QRCode(string data, ErrorCorrectionLevel errorCorrectionLevel)
    {
        Mode = GetMostEfficientMode(data);
        Version = GetSmallestVersion(data, errorCorrectionLevel);

        IEncoder encoder = Mode switch
        {
            QRCodeMode.Numeric => new NumericEncoder(),
            QRCodeMode.Alphanumeric => new AlphanumericEncoder(),
            QRCodeMode.Byte => new ByteEncoder(),
            _ => throw new ArgumentOutOfRangeException()
        };

        EncodedData = new EncodingData()
        {
            ModeIndicator = Mode.ToModeIndicator(),
            CharacterCountIndicator = GetCharacterCountIndicator(data),
            EncodedData = encoder.Encode(data)
        };
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