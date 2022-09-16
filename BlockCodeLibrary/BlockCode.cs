using System;
using System.Collections;

/// Simple Blockcode Implementation
/// ECC (3,1) repetition code
/// By adding redundant Bits the block code allows the correction of single bit errors
/// https://de.wikipedia.org/wiki/Blockcode#Beispiele_f%C3%BCr_Blockcodes (german)
/// https://en.wikipedia.org/wiki/Error_correction_code#How_it_works (english)
namespace BlockCode
{
    /// <summary>
    /// Codec class: enCOder / DECoder
    /// </summary>
    public class Codec
    {
        /// <summary>
        /// Encode a single Byte by adding redundant Bits. These redundant Bits can be used during the decoding process for fixing single Bit errors.
        /// </summary>
        /// <param name="SingleByteData">Byte data</param>
        /// <returns>Byte[3] array with data and added redundance</returns>
        public static byte[] EncodeByte(byte SingleByteData)
        {
            byte[] BlockCodeByte = new byte[3];
            string EncodedByte = EncodeBits(Bits.ByteToByteString(SingleByteData));
            BlockCodeByte[0] = Bits.ByteStringToByte(EncodedByte.Substring(0, 8));
            BlockCodeByte[1] = Bits.ByteStringToByte(EncodedByte.Substring(7, 8));
            BlockCodeByte[2] = Bits.ByteStringToByte(EncodedByte.Substring(15, 8));
            return BlockCodeByte;
        }

        /// <summary>
        /// Decode an encoded 3-Byte array to a single Byte and fixing potential single Bit errors
        /// </summary>
        /// <param name="ByteData">3-Byte array with encodec Byte data</param>
        /// <returns>(Fixed) Byte value</returns>
        /// <exception cref="Exception">ByteData length must be 3 or exception will be thrown</exception>
        public static byte DecodeBytes(byte[] ByteData)
        {
            if (ByteData.Length != 3)
                throw new Exception("DecodeBytes: incorrect Byte array size");
            string EncodedBytes = Bits.ByteToByteString(ByteData[0]);
            EncodedBytes += Bits.ByteToByteString(ByteData[1]);
            EncodedBytes += Bits.ByteToByteString(ByteData[2]);
            return Bits.ByteStringToByte(DecodeBits(EncodedBytes));
        }

        /// <summary>
        /// Encode a Byte to a Bit string
        /// </summary>
        /// <param name="SingleByteData">8-Bit data String</param>
        /// <returns>24-Bit String with redundant Bits</returns>
        /// <exception cref="Exception">Bit String length must be 8 or exception will be thrown</exception>
        public static string EncodeBits(string SingleByteData)
        {
            if (SingleByteData.Length != 8)
                throw new Exception("EncodeBits: incorrect String size");
            string EncodeString = "";
            for (int i = 0; i < 8; i++)
                EncodeString += new String(SingleByteData[i], 3);
            return EncodeString;
        }

        /// <summary>
        /// Decode an encoded Bit String and fix a potential Bit error
        /// </summary>
        /// <param name="ByteData">24-Bit encoded data String</param>
        /// <returns>8-Bit data String</returns>
        /// <exception cref="Exception">Bit String length must be 24 or exception will be thrown</exception>
        public static string DecodeBits(string ByteData)
        {
            if (ByteData.Length != 24)
                throw new Exception("DecodeBits: incorrect String size");
            string DecodeString = "";
            for (int i = 0; i < 24; i = i + 3)
            {
                string Substring = ByteData.Substring(i, 3);
                if (Substring == "111")
                    DecodeString += "1";
                else if (Substring == "000")
                    DecodeString += "0";
                else if (Substring == "110")
                    DecodeString += "1";
                else if (Substring == "101")
                    DecodeString += "1";
                else if (Substring == "011")
                    DecodeString += "1";
                else if (Substring == "001")
                    DecodeString += "0";
                else if (Substring == "010")
                    DecodeString += "0";
                else if (Substring == "100")
                    DecodeString += "0";
                else
                    DecodeString += "-";
            }
            return DecodeString;
        }
    }

    /// <summary>
    /// Class for Byte<>Bits conversion
    /// </summary>
    public class Bits
    {
        /// <summary>
        /// Convert a Byte to a 8-Bit String
        /// </summary>
        /// <param name="SingleByteData">Byte data</param>
        /// <returns>8-Bit data String</returns>
        public static string ByteToByteString(byte SingleByteData)
        {
            String BitString = "";
            BitArray BArray = new BitArray(new byte[] { SingleByteData });
            for (int i = BArray.Length - 1; i >= 0; i--)
            {
                if (BArray[i])
                    BitString += "1";
                else
                    BitString += "0";
            }
            return BitString;
        }

        /// <summary>
        /// Convert a 8-Bit String to a Byte
        /// </summary>
        /// <param name="SingleByteData">8-Bit String</param>
        /// <returns>Data byte</returns>
        /// <exception cref="Exception">Bit String length must be 8 or exception will be thrown</exception>
        public static byte ByteStringToByte(string SingleByteData)
        {
            if (SingleByteData.Length != 8)
                throw new Exception("ByteStringToByte: incorrect String size");
            BitArray BArray = new BitArray(8, false);
            for (int i = BArray.Length - 1; i >= 0; i--)
            {
                if (SingleByteData[i].ToString() == "1")
                    BArray[7 - i] = true;
            }
            byte[] NewByte = new byte[1];
            BArray.CopyTo(NewByte, 0);
            return NewByte[0];
        }
    }
}
