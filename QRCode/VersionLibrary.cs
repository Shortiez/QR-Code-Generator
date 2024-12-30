using System.Numerics;

namespace QRCodeGen;

public static class VersionLibrary
{
    public static QRCodeVersion[] Versions { get; } =
    {
        GetVersion(1),
        GetVersion(2),
        GetVersion(3),
        GetVersion(4),
    };
    
    public static QRCodeVersion GetVersion(int version)
    {
        return version switch
        {
            1 => new QRCodeVersion(1, new Vector2(21, 21),
                new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                {
                    { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(41, 25, 17) },
                    { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(34, 20, 14) },
                    { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(27, 16, 11) },
                    { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(17, 10, 7) },
                }),
            2 => new QRCodeVersion(2, new Vector2(25, 25),
                new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                {
                    { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(77, 47, 32) },
                    { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(63, 38, 26) },
                    { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(48, 29, 20) },
                    { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(34, 20, 14) },
                }),
            3 => new QRCodeVersion(3, new Vector2(29, 29),
                new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                {
                    { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(127, 77, 53) },
                    { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(101, 61, 42) },
                    { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(77, 47, 32) },
                    { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(58, 35, 24) },
                }),
            4 => new QRCodeVersion(4, new Vector2(33, 33),
                new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                {
                    { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(187, 114, 78) },
                    { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(149, 90, 62) },
                    { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(111, 67, 46) },
                    { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(82, 50, 34) },
                }),
        };
    }

}

public struct QRCodeVersion
{
    public int Version { get; }
    public Vector2 Size { get; }
    public Dictionary<ErrorCorrectionLevel, CharacterCapacity> CharacterCapacities { get; }

    public QRCodeVersion(int version, Vector2 size,
        Dictionary<ErrorCorrectionLevel, CharacterCapacity> characterCapacities)
    {
        Version = version;
        Size = size;
        CharacterCapacities = characterCapacities;
    }

    public struct CharacterCapacity
    {
        public int Numeric { get; }
        public int Alphanumeric { get; }
        public int Byte { get; }

        public CharacterCapacity(int numeric, int alphanumeric, int @byte)
        {
            Numeric = numeric;
            Alphanumeric = alphanumeric;
            Byte = @byte;
        }
    }
}