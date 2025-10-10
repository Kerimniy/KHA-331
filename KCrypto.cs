using System;
using System.Collections;
using System.Text;


namespace KCrypto
{
    public static class KHA331
    {

        public static string GetHashCode(string input)
        {
                       
            string text = input;
            byte[] bytes = new byte[64];
            byte[] output_bytes = new byte[32];
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text);
            string output = "";

            Array.Copy(textBytes, 0, bytes, 0, textBytes.Length);

            for (int j = text.Length; j < 64; j++)
            {

                int s = Convert.ToInt16(MathF.Round(j * text.Length / 64.0f));
                int j1 = 0;
                if (s != 0 & j > 9)
                {
                    j1 = j / (int)Math.Round((s * 1.5)) / 2;

                }
                else
                {
                    if (j > 16)
                    {
                        j1 = j / 2;
                    }
                    else j1 = j;
                }
                int h = 0;
                double shifted = 0;
                try
                {

                    shifted = (long)bytes[j - 1] << (j1) ^ (int)NumSum(bytes[j - 2]);
                    shifted = Convert.ToInt32(shifted) ^ Convert.ToInt32(Last3Symbols((j * j * j).ToString()));
                   
                }
                catch (SystemException)
                {
                    shifted = bytes[s] << (j1 + s) ^ ByteMedium(bytes);
                   
                }
                

                string loged_str = "";
                if (shifted.ToString().Length > 3)
                {
                    loged_str = Convert.ToString(shifted).Substring(shifted.ToString().Length - 3);
                }
                else
                {
                    loged_str = shifted.ToString();
                    while (loged_str.Length < 3)
                    {
                        loged_str = loged_str.Insert(0, "0");
                    }
                }
                shifted = Convert.ToInt16(loged_str);
                while (shifted > 255)
                {
                    shifted -= shifted * 0.6;
                    shifted = Math.Round((float)shifted);
                }

                bytes[j] = (byte)shifted;

            }


            for (byte i = 0; i < 32; i++)
            {
                double multiplied = bytes[i] * bytes[i + 32];
                while (multiplied > 255)
                {
                    multiplied -= multiplied * 0.6;
                    multiplied = Math.Round((float)multiplied);
                }
                output_bytes[i] = Convert.ToByte(multiplied);

            }

            foreach (byte el in output_bytes)
            {
                output = output.Insert(0, ToBase(el, 36));
            }
            return output;
        }


        private static double NumSum(double a)
        {
            return Other.NumSum(a);
        }


        private static byte ByteMedium(byte[] b)
        {
            return Other.ByteMedium(b);
        }


        private static string Last3Symbols(string a)
        {
            return Other.Last3Symbols(a);
        }


        private static string ToBase(ulong number, int _base)
        {
            return NSConvert.ToBase(number, _base);
        }


        private static ulong FromBase(string input, int _base)
        {
            return NSConvert.FromBase(input, _base);
        }
    }

    public class SEDA128
    {
        private int[] w = new int[64];
        private string key;

        public SEDA128(string key)
        {
            this.key = key;
            PadKey();
            CreateWeights(this.key);
        }

        private void CreateWeights(string key)
        {

            w[0] = (key[0] + key[62] + key[63]) / 3;
            w[1] = (key[0] + key[1] + key[63]) / 3;
            for (int i = 2; i < w.Length; i++)
            {

                float de = ((int)key[i] + (int)w[i - 1]) * (float)Math.Sqrt(i);
                float di = (w[i - 2]) + 1;
                if (de / di < 10)
                {
                    di /= 1000;
                }
                float r = (de / di);
                if (r < 500 && (r * 10) % 10 != 0)
                {
                    r *= 10;
                }
                w[i] = (int)r;
            }
        }
        private void PadKey()
        {
            StringBuilder sb = new StringBuilder(key);

            for (int i = key.Length; i < 64; i++)
            {
                sb.Append($"{(char)((i * i * 0.7 + i) / (0.7 * i + i + 1))}");
            }

            key = sb.ToString();
        }
        public string Encode(string input, bool fixedlength = false)
        {

            StringBuilder res = new StringBuilder();
            int il = input.Length;
            int l = il;
            if (fixedlength)
            {
                l = 64;

            }

            for (int i = 0; i < l; i++)
            {
                res.Append(((char)(input[i % il] ^ w[i])).ToString());
            }

            if (fixedlength)
            {
                return $"{input.Length}x{res.ToString()}";
            }

            return res.ToString();
        }
        public string Decode(string input, bool fixedlength = false)
        {
            StringBuilder res = new StringBuilder();
            int il = input.Length;
            if (!fixedlength)
            {
                for (int i = 0; i < il; i++)
                {
                    res.Append(((char)(input[i % il] ^ w[i])).ToString());
                }

                return res.ToString();
            }
            else
            {
                int dl = 0;
                for (int j = 0; j < input.Length - 64 - 1; j++)
                {
                    dl = (dl * 10) + (input[j] - '0');
                }
                input = input.Substring(input.Length - 64, 64);
                char[] r = new char[dl];

                for (int i = 63; i > -1; i--)
                {
                    r[i % dl] = (char)(input[i % dl] ^ w[i]);
                }
                return new string(r);
            }
        }
    }

    public static class Other
    {
        class SimpleXOREDA()
        {
            private static int autoKey = 0;

            static SimpleXOREDA()
            {
                Random rand = new Random();
                autoKey = rand.Next(1, 999999999);
            }

            public static byte[] EnCrypt(string input, int key)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);

                int[] INTbytes1 = new int[bytes.Length];
                byte[] output = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    int v = i % 8 + 1 + Convert.ToInt32(key.ToString()[0].ToString());

                    INTbytes1[i] = (int)bytes[i];
                    if (i % 2 == 0)
                    {
                        INTbytes1[i] = INTbytes1[i] ^ key;
                        INTbytes1[i] = INTbytes1[i] + key;
                    }
                    else
                    {
                        INTbytes1[i] = INTbytes1[i] ^ (key + v);
                        INTbytes1[i] = INTbytes1[i] + (key + v);
                    }
                    output[i] = (byte)INTbytes1[i];
                }
                return output;
            }


            public static byte[] EnCrypt(string input)
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
                int[] INTbytes1 = new int[bytes.Length];
                byte[] output = new byte[bytes.Length];
                for (int i = 0; i < bytes.Length; i++)
                {
                    int v = i % 8 + 1 + Convert.ToInt32(autoKey.ToString()[0].ToString());

                    INTbytes1[i] = (int)bytes[i];
                    if (i % 2 == 0)
                    {
                        INTbytes1[i] = INTbytes1[i] ^ autoKey;
                        INTbytes1[i] = INTbytes1[i] + autoKey;
                    }
                    else
                    {
                        INTbytes1[i] = INTbytes1[i] ^ (autoKey + v);
                        INTbytes1[i] = INTbytes1[i] + (autoKey + v);
                    }
                    output[i] = (byte)INTbytes1[i];

                }
                return output;
            }


            public static string DeCrypt(byte[] bytes, int key)
            {
                int[] INTbytesEN = new int[bytes.Length];

                byte[] bytesEN = new byte[INTbytesEN.Length];

                for (int i = 0; i < INTbytesEN.Length; i++)
                {
                    int v = i % 8 + 1 + Convert.ToInt32(key.ToString()[0].ToString());
                    INTbytesEN[i] = bytes[i];
                    if (i % 2 == 0)
                    {
                        INTbytesEN[i] = INTbytesEN[i] - key;
                        INTbytesEN[i] = INTbytesEN[i] ^ key;
                    }
                    else
                    {
                        INTbytesEN[i] = INTbytesEN[i] - (key + v);
                        INTbytesEN[i] = INTbytesEN[i] ^ (key + v);
                    }
                    bytesEN[i] = (byte)INTbytesEN[i];
                }
                string output = Encoding.UTF8.GetString(bytesEN);

                return output;
            }


            public static string DeCrypt(byte[] bytes)
            {
                int[] INTbytesEN = new int[bytes.Length];

                byte[] bytesEN = new byte[INTbytesEN.Length];

                for (int i = 0; i < INTbytesEN.Length; i++)
                {
                    int v = i % 8 + 1 + Convert.ToInt32(autoKey.ToString()[0].ToString());
                    INTbytesEN[i] = bytes[i];
                    if (i % 2 == 0)
                    {
                        INTbytesEN[i] = INTbytesEN[i] - autoKey;
                        INTbytesEN[i] = INTbytesEN[i] ^ autoKey;
                    }
                    else
                    {
                        INTbytesEN[i] = INTbytesEN[i] - (autoKey + v);
                        INTbytesEN[i] = INTbytesEN[i] ^ (autoKey + v);
                    }
                    bytesEN[i] = (byte)INTbytesEN[i];
                }
                string output = Encoding.UTF8.GetString(bytesEN);

                return output;
            }

        }

        public static double NumSum(double a)
        {
            double sum = 0;
            string a_s = a.ToString();
            for (byte i = 0; i < a_s.Length; i++)
            {
                sum += Convert.ToDouble(Convert.ToInt16(a_s[i]));
            }
            return sum;
        }

        public static byte ByteMedium(byte[] b)
        {
            byte medium = 0;
            int sum = 0;
            for (byte i = 0; i < b.Length; i++)
            {
                sum += b[i];
            }
            medium = Convert.ToByte(sum / b.Length);
            return medium;
        }

        public static string Last3Symbols(string a)
        {
            double shifted = Convert.ToDouble(a);
            string loged_str = "";
            if (shifted.ToString().Length > 3)
            {
                loged_str = Convert.ToString(shifted).Substring(shifted.ToString().Length - 3);
            }
            else
            {
                loged_str = shifted.ToString();
                while (loged_str.Length < 3)
                {
                    loged_str = loged_str.Insert(0, "0");
                }
            }

            shifted = Convert.ToInt16(loged_str);
            return loged_str;
        }

       
    }

    public static class NSConvert
    {
        public static string ToBase(ulong number, int _base)
        {
            if (_base == 0 || _base > 36)
            {
                return "OutOfDegreeRange";
            }

            string output = "";
            char[] charbase = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            decimal _decimal = number;
            for (byte i = 0; i < number.ToString().Length; i++)
            {

                output = output.Insert(0, charbase[Convert.ToInt32(_decimal % _base)].ToString());
                _decimal = Math.Floor((_decimal / _base));
            }

            string new_output = "";

            if (output[0] == '0')
            {
                for (byte j = 1; j < output.Length; j++)
                {
                    new_output = new_output.Insert(j - 1, output[j].ToString());
                }

            }
            else new_output = output;

            return new_output;
        }


        public static ulong  FromBase(string input, int _base)
        {
            if (_base == 0 || _base > 36)
            {
                return 1;
            }
            if (input.Length > 32)
            {
                string b1 = input.Remove(32);
                input = b1;
            }
            char[] charbase = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string numbers = "";
            ulong output = 0;
            List<int> dynamicArray = new List<int>();
            for (byte m = 0; m < input.Length; m++)
            {
                for (byte i = 0; i < charbase.Length; i++)
                {
                    if (charbase[i] == input[m])
                    {

                        numbers = numbers.Insert(0, i.ToString());
                        dynamicArray.Add(i);

                    }

                }

            }
            for (int i = 0; i < dynamicArray.Count; i++)
            {

                int j = dynamicArray.Count - i;
                output += Convert.ToUInt64(dynamicArray[i]) * Convert.ToUInt64(MathF.Pow(_base, j - 1));
            }

            return output;
        }
    }



}



   





