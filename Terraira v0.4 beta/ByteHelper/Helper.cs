using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.ByteHelper
{
    class Helper
    {

        public static int setValue2To10(int[] code, int value2)
        {
            int number = 0;
            for (int x = 0; x > code.Length; x++)
            {
                number += code[x] * (int)Math.Pow(value2, x);
            }
            return number;
        }
        public static int set256To10(byte[] code)
        {
            int value = 0;
            for (int x = 0; x < code.Length; x++)
            {
                value += code[x] * (int)Math.Pow(256, code.Length - x - 1);
            }
            return value;
        }

        public static int getSizeCode(int value1, int value2)
        {
            float logCode = (float)Math.Log(value1, value2);
            float logRes = logCode - (int)(logCode);
            int sizeCode = logRes > 0 ? (int)logCode + 1 : (int)logCode;
            if (value1 <= 1) sizeCode = 1;
            return sizeCode;
        }

        public static int[] set10ToValue2(int value, float res)
        {
            float logCode = (float)(Math.Log(value, res)) - (int)(Math.Log(value, res));
            int sizeCode = getSizeCode((int)value, (int)res);
            if (value <= 1)
            {
                sizeCode = 1;
            }
            int[] code = new int[sizeCode];

            if (value > 0)
            {
                if (logCode > 0)
                {
                    float saveValue = value;

                    for (int x = sizeCode - 1; x >= 0; x--)
                    {
                        saveValue = saveValue / (res * 1.0f);
                        float number2 = saveValue - (int)(saveValue); // остаток
                        saveValue = (int)saveValue;
                        int answ = (int)(number2 * res);
                        code[x] = answ;
                    }
                }
                else
                {
                    if (value != 1)
                        code = new int[sizeCode + 1];
                    code[0] = 1;
                }
            }
            else
            {
                code[0] = 0;
            }

            return code;
        }

        public static byte[] set10To256(int value)
        {
            int[] codeInt = set10ToValue2(value, 256);
            byte[] code = new byte[codeInt.Length];
            for (int x = 0; x < codeInt.Length; x++)
            {
                code[x] = (byte)(codeInt[x]);
            }
            return code;
        }

        public static float distance(Vector2f vec1, Vector2f vec2)
        {
            Vector2f dVec = vec2 - vec1;
            return (float)Math.Sqrt(dVec.X * dVec.X + dVec.Y * dVec.Y);
        }

        public static Color HsvToRgb(float h, float s, float v)
        {
            int i;
            float f, p, q, t;

            if (s < float.Epsilon)
            {
                int c = (int)(v * 255);
                return new Color((byte)c, (byte)c, (byte)c);
            }

            h /= 60;
            i = (int)Math.Floor(h);
            f = h - i;
            p = v * (1 - s);
            q = v * (1 - s * f);
            t = v * (1 - s * (1 - f));

            float r, g, b;
            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }

            return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

    }
}
