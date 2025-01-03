using System.Numerics;

namespace QRCodeGen
{
    /// <summary>
    /// Provides a library of QR Code versions supported by this library.
    /// </summary>
    public static class VersionLibrary
    {
        /// <summary>
        /// The QR Code versions supported by this library.
        /// </summary>
        public static QRCodeVersion[] Versions { get; } =
        {
            GetVersion(1),
            GetVersion(2),
            GetVersion(3),
            GetVersion(4),
        };

        /// <summary>
        /// Retrieves the QR Code version information for the specified version number.
        /// </summary>
        /// <param name="version">The version number of the QR Code.</param>
        /// <returns>A <see cref="QRCodeVersion"/> object containing the version information.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the version number is out of range.</exception>
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
                    }, new Dictionary<ErrorCorrectionLevel, int>()
                    {
                        { ErrorCorrectionLevel.L, 152 },
                        { ErrorCorrectionLevel.M, 128 },
                        { ErrorCorrectionLevel.Q, 104 },
                        { ErrorCorrectionLevel.H, 72 },
                    }),
                2 => new QRCodeVersion(2, new Vector2(25, 25),
                    new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                    {
                        { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(77, 47, 32) },
                        { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(63, 38, 26) },
                        { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(48, 29, 20) },
                        { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(34, 20, 14) },
                    }, new Dictionary<ErrorCorrectionLevel, int>()
                    {
                        { ErrorCorrectionLevel.L, 272 },
                        { ErrorCorrectionLevel.M, 224 },
                        { ErrorCorrectionLevel.Q, 176 },
                        { ErrorCorrectionLevel.H, 128 },
                    }),
                3 => new QRCodeVersion(3, new Vector2(29, 29),
                    new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                    {
                        { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(127, 77, 53) },
                        { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(101, 61, 42) },
                        { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(77, 47, 32) },
                        { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(58, 35, 24) },
                    }, new Dictionary<ErrorCorrectionLevel, int>()
                    {
                        { ErrorCorrectionLevel.L, 440 },
                        { ErrorCorrectionLevel.M, 352 },
                        { ErrorCorrectionLevel.Q, 272 },
                        { ErrorCorrectionLevel.H, 208 },
                    }),
                4 => new QRCodeVersion(4, new Vector2(33, 33),
                    new Dictionary<ErrorCorrectionLevel, QRCodeVersion.CharacterCapacity>
                    {
                        { ErrorCorrectionLevel.L, new QRCodeVersion.CharacterCapacity(187, 114, 78) },
                        { ErrorCorrectionLevel.M, new QRCodeVersion.CharacterCapacity(149, 90, 62) },
                        { ErrorCorrectionLevel.Q, new QRCodeVersion.CharacterCapacity(111, 67, 46) },
                        { ErrorCorrectionLevel.H, new QRCodeVersion.CharacterCapacity(82, 50, 34) },
                    }, new Dictionary<ErrorCorrectionLevel, int>()
                    {
                        { ErrorCorrectionLevel.L, 640 },
                        { ErrorCorrectionLevel.M, 512 },
                        { ErrorCorrectionLevel.Q, 384 },
                        { ErrorCorrectionLevel.H, 288 },
                    }),
                _ => throw new ArgumentOutOfRangeException(nameof(version), version, null)
            };
        }
    }

    /// <summary>
    /// Represents a QR Code version.
    /// </summary>
    public struct QRCodeVersion
    {
        /// <summary>
        /// Gets the version number of the QR Code.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Gets the size of the QR Code.
        /// </summary>
        public Vector2 Size { get; }

        /// <summary>
        /// Gets the character capacities for different error correction levels.
        /// </summary>
        public Dictionary<ErrorCorrectionLevel, CharacterCapacity> CharacterCapacities { get; }

        /// <summary>
        /// Gets the maximum data bits for different error correction levels.
        /// </summary>
        public Dictionary<ErrorCorrectionLevel, int> MaxDataBits { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeVersion"/> struct.
        /// </summary>
        /// <param name="version">The version number of the QR Code.</param>
        /// <param name="size">The size of the QR Code.</param>
        /// <param name="characterCapacities">The character capacities for different error correction levels.</param>
        /// <param name="maxDataBits">The maximum data bits for different error correction levels.</param>
        public QRCodeVersion(int version, Vector2 size,
            Dictionary<ErrorCorrectionLevel, CharacterCapacity> characterCapacities,
            Dictionary<ErrorCorrectionLevel, int> maxDataBits)
        {
            Version = version;
            Size = size;
            CharacterCapacities = characterCapacities;
            MaxDataBits = maxDataBits;
        }
        
        public int GetMaxBits(ErrorCorrectionLevel errorCorrectionLevel)
        {
            return MaxDataBits[errorCorrectionLevel];
        }

        /// <summary>
        /// Represents the character capacity for a specific error correction level.
        /// </summary>
        public struct CharacterCapacity
        {
            /// <summary>
            /// Gets the numeric character capacity.
            /// </summary>
            public int Numeric { get; }

            /// <summary>
            /// Gets the alphanumeric character capacity.
            /// </summary>
            public int Alphanumeric { get; }

            /// <summary>
            /// Gets the byte character capacity.
            /// </summary>
            public int Byte { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="CharacterCapacity"/> struct.
            /// </summary>
            /// <param name="numeric">The numeric character capacity.</param>
            /// <param name="alphanumeric">The alphanumeric character capacity.</param>
            /// <param name="byte">The byte character capacity.</param>
            public CharacterCapacity(int numeric, int alphanumeric, int @byte)
            {
                Numeric = numeric;
                Alphanumeric = alphanumeric;
                Byte = @byte;
            }
        }
    }
}