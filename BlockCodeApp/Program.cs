///------
/// Start
///------
Start:
Console.Title = "CodeBlock class demo for a ECC (3,1) repetition code";
Console.Clear();
#pragma warning disable CA1416
if (System.Runtime.InteropServices.OSPlatform.Windows == System.Runtime.InteropServices.OSPlatform.Windows)
    Console.WriteLine(Console.Title + Environment.NewLine);
#pragma warning restore CA1416
Console.WriteLine("The 1st run shows the result of the encoding of a Byte value with redundant bits\nand the decoding back removing the additional Bits.");
Console.WriteLine("The 2nd run shows the result of the encoding of a Byte and the fixing of a Bit error\nduring decoding back to the original Byte value.");

//Initialize a random data Byte value
Random Randomizer = new Random();
byte Data = (byte)Randomizer.Next(Byte.MinValue, Byte.MaxValue);
byte[] EncodedData = new byte[3];

///------------------------
///1. Run with no error Bit
///------------------------
Console.WriteLine(Environment.NewLine + "1. Run with no error (only encoding/decoding)");

//Show start data
Console.WriteLine("Data value (not encoded): {0}", Data.ToString());

//Encode the data Byte
EncodedData = BlockCode.Codec.EncodeByte(Data);
Console.WriteLine("Data value (encoded):     {0} {1} {2}", EncodedData[0].ToString(), EncodedData[1].ToString(), EncodedData[2].ToString());
Console.WriteLine("Data value (encoded):     {0} {1} {2}", BlockCode.Bits.ByteToByteString(EncodedData[0]), BlockCode.Bits.ByteToByteString(EncodedData[1]), BlockCode.Bits.ByteToByteString(EncodedData[2]));

//Decode the encoded data again
Console.WriteLine("Data value (decoded):     {0}", BlockCode.Codec.DecodeBytes(EncodedData).ToString());

///------------------------
///2. Run with error Bit
///------------------------
Console.WriteLine(Environment.NewLine + "2. Run with Bit error (encoding/decoding/fixing)");

//Show start data
Console.WriteLine("Data value (no error):    {0}", Data.ToString());

//Encode the data Byte
EncodedData = BlockCode.Codec.EncodeByte(Data);
Console.WriteLine("Data value (encoded):     {0} {1} {2}", EncodedData[0].ToString(), EncodedData[1].ToString(), EncodedData[2].ToString());
Console.WriteLine("Data value (encoded):     {0} {1} {2}", BlockCode.Bits.ByteToByteString(EncodedData[0]), BlockCode.Bits.ByteToByteString(EncodedData[1]), BlockCode.Bits.ByteToByteString(EncodedData[2]));

//Manipulate the encoded data by flipping the first Bit to simulate Bit error
string BitString = BlockCode.Bits.ByteToByteString(EncodedData[0]);
if (BitString.Substring(0, 1) == "1")
    BitString = "0" + BitString.Substring(1, 7);
else
    BitString = "1" + BitString.Substring(1, 7);
EncodedData[0] = BlockCode.Bits.ByteStringToByte(BitString);
Console.WriteLine("Data value (Bit 1 error): {0} {1} {2}", EncodedData[0].ToString(), EncodedData[1].ToString(), EncodedData[2].ToString());
Console.WriteLine("Data value (Bit 1 error): {0} {1} {2}", BlockCode.Bits.ByteToByteString(EncodedData[0]), BlockCode.Bits.ByteToByteString(EncodedData[1]), BlockCode.Bits.ByteToByteString(EncodedData[2]));

//Decode and fix the encoded data again
Console.WriteLine("Data value (fixed):       {0}", BlockCode.Codec.DecodeBytes(EncodedData).ToString());

///----
/// End
///----
Console.WriteLine(Environment.NewLine + "Press SPACE for repeat");
Console.WriteLine("Press any other key for quit");
ConsoleKeyInfo KeyInfo = Console.ReadKey();
if (KeyInfo.Key == ConsoleKey.Spacebar)
    goto Start;
