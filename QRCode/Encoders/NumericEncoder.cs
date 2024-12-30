namespace QRCodeGen;

public class NumericEncoder : IEncoder
{
    public string[] Encode(string data)
    {
        string[] groupsOfThree = data.SplitInParts(3);
        string[] encodedData = new string[groupsOfThree.Length];

        for (var i = 0; i < groupsOfThree.Length; i++)
        {
            var group = groupsOfThree[i];
            int number = int.Parse(group);
            string binary = number.ToTenBitBinaryString();

            encodedData[i] = binary;
        }

        return encodedData;
    }
}