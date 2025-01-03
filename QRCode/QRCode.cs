using System.Collections;

namespace QRCodeGen;

public class QRCode
{
    public struct EncodingData
    {
        public BitArray ModeIndicator { get; init; }
        public BitArray CharacterCountIndicator { get; init; }
        public BitArray[] EncodedData { get; init; }
        
        public EncodingData(BitArray modeIndicator, BitArray characterCountIndicator, BitArray[] encodedData)
        {
            ModeIndicator = modeIndicator;
            CharacterCountIndicator = characterCountIndicator;
            EncodedData = encodedData;
        }

        public override string ToString()
        {
            return $"Mode Indicator: {ModeIndicator.AsString()}\n" +
                   $"Character Count Indicator: {CharacterCountIndicator.AsString()}\n" +
                   $"Encoded Data: {EncodedData.AsString()}";
        }
    }
    
    public QRCodeMode Mode { get; }
    protected QRCodeVersion Version { get; }
    protected ErrorCorrectionLevel ErrorCorrectionLevel { get; }
    protected EncodingData EncodedData { get; }
    
    public QRCode(string data, ErrorCorrectionLevel errorCorrectionLevel)
    {
        ErrorCorrectionLevel = errorCorrectionLevel;
        Mode = GetMostEfficientMode(data);
        Version = GetSmallestVersion(data, errorCorrectionLevel);

        IEncoder encoder = Mode switch
        {
            QRCodeMode.Numeric => new NumericEncoder(),
            QRCodeMode.Alphanumeric => new AlphanumericEncoder(),
            QRCodeMode.Byte => new ByteEncoder(),
            _ => throw new ArgumentOutOfRangeException()
        };

        EncodedData = new EncodingData
        {
            ModeIndicator = Mode.ToModeIndicator(),
            CharacterCountIndicator = GetCharacterCountIndicator(data),
            EncodedData = encoder.Encode(data)
        };
        
        Console.WriteLine($"Mode: {Mode}");
        Console.WriteLine($"Version: {Version.Version}");
        Console.WriteLine($"Error Correction Level: {ErrorCorrectionLevel}");
        Console.WriteLine($"Version Max Data Bits: {Version.GetMaxBits(ErrorCorrectionLevel)}");
        Console.WriteLine(EncodedData);
        
        string finalDataVersion = BuildRawDataBits(Version, EncodedData.ModeIndicator, EncodedData.CharacterCountIndicator, EncodedData.EncodedData).AsString();
        Console.WriteLine($"Final Data Version: {finalDataVersion}");
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
        
        for (var i = 1; i <= VersionLibrary.Versions.Length; i++)
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        throw new Exception("Data is too large to fit in any version of QR code.");
    }
    private BitArray GetCharacterCountIndicator(string data)
    {
        int maxBits = GetMaxBitsForCharacterCountIndicator(Version, Mode);
        BitArray returnValue = new BitArray(maxBits);
        int dataLength = data.Length;

        var binary = Convert.ToString(dataLength, 2);
        returnValue.AppendBits(binary);

        return returnValue;
    }
    private int GetMaxBitsForCharacterCountIndicator(QRCodeVersion version, QRCodeMode mode)
    {
        return version switch
        {
            { Version: <= 9 and > 0} => mode switch
            {
                QRCodeMode.Numeric => 10,
                QRCodeMode.Alphanumeric => 9,
                QRCodeMode.Byte => 8,
                _ => throw new ArgumentOutOfRangeException()
            },
            { Version: <= 26 and > 9 } => mode switch
            {
                QRCodeMode.Numeric => 12,
                QRCodeMode.Alphanumeric => 11,
                QRCodeMode.Byte => 16,
                _ => throw new ArgumentOutOfRangeException()
            },
            { Version: <= 40 and > 26 } => mode switch
            {
                QRCodeMode.Numeric => 14,
                QRCodeMode.Alphanumeric => 13,
                QRCodeMode.Byte => 16,
                _ => throw new ArgumentOutOfRangeException()
            },
            _ => 0
        };
    }
    private BitArray BuildRawDataBits(QRCodeVersion version, BitArray modeIndicator, BitArray characterCountIndicator, BitArray[] encodedData)
    {
        BitArray rawDataBits = new BitArray(0);
        rawDataBits.AppendBits(modeIndicator);
        rawDataBits.AppendBits(characterCountIndicator);
        foreach (var data in encodedData)
        {
            rawDataBits.AppendBits(data);
        }

        int maxBits = version.GetMaxBits(ErrorCorrectionLevel);
        int remainder = maxBits - rawDataBits.Length;

        // Try to add terminator
        if (!TryAddTerminator(ref rawDataBits, remainder))
            return rawDataBits;  // Data is already at max capacity
        
        // Pad to multiple of 8 bits
        MakeBitsMultipleOfEight(ref rawDataBits);
        
        // Add padding bytes until we reach capacity
        AddPaddingBytes(ref rawDataBits, maxBits);

        return rawDataBits;
    }

    private bool TryAddTerminator(ref BitArray bitArray, int freeBits)
    {
        if (freeBits <= 0) return false;
        
        int terminatorBits = Math.Min(freeBits, 4);
        bitArray.AppendBits(0, terminatorBits);
        return true;
    }

    private void MakeBitsMultipleOfEight(ref BitArray bitArray)
    {
        int remainder = bitArray.Length % 8;
        if (remainder == 0) return;
        
        int paddingNeeded = 8 - remainder;
        bitArray.AppendBits(0, paddingNeeded);
    }
    private void AddPaddingBytes(ref BitArray bitArray, int maxBits)
    {
        // Continue adding padding bytes until we reach capacity
        // Alternate between 11101100 and 00010001
        bool useFirstPadding = true;
    
        while (bitArray.Length + 8 <= maxBits)
        {
            if (useFirstPadding)
                bitArray.AppendBits(0xECU, 8);  // 11101100
            else
                bitArray.AppendBits(0x11U, 8);  // 00010001
            
            useFirstPadding = !useFirstPadding;
        }
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