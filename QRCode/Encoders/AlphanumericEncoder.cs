using System.Collections;

namespace QRCodeGen;

public class AlphanumericEncoder : IEncoder
{
    private static readonly Dictionary<char, int> AlphanumericTable = new Dictionary<char, int>
    {
        {'0', 0}, {'1', 1}, {'2', 2}, {'3', 3}, {'4', 4}, {'5', 5}, {'6', 6}, {'7', 7}, {'8', 8}, {'9', 9},
        {'A', 10}, {'B', 11}, {'C', 12}, {'D', 13}, {'E', 14}, {'F', 15}, {'G', 16}, {'H', 17}, {'I', 18}, {'J', 19},
        {'K', 20}, {'L', 21}, {'M', 22}, {'N', 23}, {'O', 24}, {'P', 25}, {'Q', 26}, {'R', 27}, {'S', 28}, {'T', 29},
        {'U', 30}, {'V', 31}, {'W', 32}, {'X', 33}, {'Y', 34}, {'Z', 35}, {' ', 36}, {'$', 37}, {'%', 38}, {'*', 39},
        {'+', 40}, {'-', 41}, {'.', 42}, {'/', 43}, {':', 44}
    };
    
    public BitArray[] Encode(string data)
    {
        List<BitArray> encodedData = new List<BitArray>();
        
        for (int i = 0; i < data.Length; i += 2)
        {
            // If there are two characters left in the data
            if (i + 1 < data.Length)
            {
                int firstValue = AlphanumericTable[data[i]];
                int secondValue = AlphanumericTable[data[i + 1]];
                int combinedValue = (firstValue * 45) + secondValue;
                
                string binaryString = Convert.ToString(combinedValue, 2).PadLeft(11, '0');
                
                encodedData.Add(new BitArray(binaryString.Select(c => c == '1').ToArray()));
            }
            // If there is only one character left in the data
            else
            {
                int value = AlphanumericTable[data[i]];
                string binaryString = Convert.ToString(value, 2).PadLeft(6, '0');
                
                encodedData.Add(new BitArray(binaryString.Select(c => c == '1').ToArray()));
            }
        }
        
        Console.WriteLine("Alphanumeric Encoded Data:");
        foreach (var bitArray in encodedData)
        {
            Console.WriteLine(bitArray.AsString());
        }
        
        return encodedData.ToArray();
    }
}