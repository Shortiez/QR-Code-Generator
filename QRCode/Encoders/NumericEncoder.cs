using System.Collections;

namespace QRCodeGen;

public class NumericEncoder : IEncoder
{
    public BitArray[] Encode(string data)
    {
        string[] groupsOfThree = data.SplitInParts(3);
        BitArray[] encodedData = new BitArray[groupsOfThree.Length];

        for (var i = 0; i < groupsOfThree.Length; i++)
        {
            var group = groupsOfThree[i];
            var groupInt = uint.Parse(group);
            
            // Determine number of bits based on group length
            int numBits = group.Length switch
            {
                3 => 10,  // 3 digits = 10 bits
                2 => 7,   // 2 digits = 7 bits
                1 => 4,   // 1 digit = 4 bits
                _ => throw new ArgumentException("Invalid group length")
            };

            var bitArray = new BitArray(0);
            bitArray.AppendBits(groupInt, numBits);
            encodedData[i] = bitArray;
        }

        return encodedData;
    }
}