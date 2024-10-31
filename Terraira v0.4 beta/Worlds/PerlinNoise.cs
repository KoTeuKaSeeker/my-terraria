using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.WorldGenerationForder
{
    class PerlinNoise
    {

        //private float getPerlinNoise(float posX, float posY, float offset, int seed)
        //{
        //    int a = (int)(posX / offset);
        //    int b = a + 1;
        //    a *= (int)offset;
        //    b *= (int)offset;

        //    int aY = (int)(posY / offset);
        //    int bY = a + 1;
        //    aY *= (int)offset;
        //    bY *= (int)offset;

        //    float t = (posX - a) / (b - a);
        //    t = smoothstep(0, 1, t);

        //    float randA = rand.next(a, aY, 0, 10000, seed) / 10000.000f;
        //    float randB = rand.next(b, bY, 0, 10000, seed) / 10000.000f;

        //    float mA = Mathf.Tan(randA) * (posX - a);
        //    float mB = Mathf.Tan(randB) * (posX - b);

        //    float y = mA + (mB - mA) * t;
        //    return y;

        //}

        private static float lerp(float t, float a, float b)
        {
            return a + (b - a) * t;
        }

        public static float getPerlinNoise(float posX, float posZ, int offsetX, int offsetZ, RandomCoor rand)
        {

            int sXa = (int)(Math.Abs(posX) / offsetX);
            int sXb = sXa + 1;
            int sXc = sXa;
            int sXd = sXb;

            sXa *= offsetX;
            sXb *= offsetX;
            sXc *= offsetX;
            sXd *= offsetX;

            int sZa = (int)(Math.Abs(posZ) / offsetZ);
            int sZb = sZa;
            int sZc = sZa + 1;
            int sZd = sZa + 1;

            sZa *= offsetZ;
            sZb *= offsetZ;
            sZc *= offsetZ;
            sZd *= offsetZ;

            Vector2f serifA = new Vector2f(sXa, sZa);
            Vector2f serifB = new Vector2f(sXb, sZb);
            Vector2f serifC = new Vector2f(sXc, sZc);
            Vector2f serifD = new Vector2f(sXd, sZd);

            Vector2f point = new Vector2f(Math.Abs(posX), Math.Abs(posZ));

            Vector2f sDisA = point - serifA;
            Vector2f sDisB = point - serifB;
            Vector2f sDisC = point - serifC;
            Vector2f sDisD = point - serifD;

            int signX = Math.Sign(posX);
            int signZ = Math.Sign(posZ);

            Vector2f inclineA = new Vector2f(rand.next((int)serifA.X * signX, (int)serifA.Y * signZ, 0, 100) / 100.00f, rand.next((int)(Math.Sin(serifA.X * signX) * rand.sizeNumberMap), (int)(Math.Sin(serifA.Y * signZ) * rand.sizeNumberMap), 0, 100) / 100.00f);
            Vector2f inclineB = new Vector2f(rand.next((int)serifB.X * signX, (int)serifB.Y * signZ, 0, 100) / 100.00f, rand.next((int)(Math.Sin(serifB.X * signX) * rand.sizeNumberMap), (int)(Math.Sin(serifB.Y * signZ) * rand.sizeNumberMap), 0, 100) / 100.00f);
            Vector2f inclineC = new Vector2f(rand.next((int)serifC.X * signX, (int)serifC.Y * signZ, 0, 100) / 100.00f, rand.next((int)(Math.Sin(serifC.X * signX) * rand.sizeNumberMap), (int)(Math.Sin(serifC.Y * signZ) * rand.sizeNumberMap), 0, 100) / 100.00f);
            Vector2f inclineD = new Vector2f(rand.next((int)serifD.X * signX, (int)serifD.Y * signZ, 0, 100) / 100.00f, rand.next((int)(Math.Sin(serifD.X * signX) * rand.sizeNumberMap), (int)(Math.Sin(serifD.Y * signZ) * rand.sizeNumberMap), 0, 100) / 100.00f);

            float mA = inclineA.X * sDisA.X + inclineA.Y * sDisA.Y;
            float mB = inclineB.X * sDisB.X + inclineB.Y * sDisB.Y;
            float mC = inclineC.X * sDisC.X + inclineC.Y * sDisC.Y;
            float mD = inclineD.X * sDisD.X + inclineD.Y * sDisD.Y;

            float tX = sDisA.X / offsetX;
            float tZ = sDisA.Y / offsetZ;

            float polrX1 = lerp(tX, mA, mB);
            float polrX2 = lerp(tX, mC, mD);
            float polr = lerp(tZ, polrX1, polrX2);

            return polr;
        }

        public static float getPerlinNoise2DWithCurve(float posX, float posY, int offsetX, int offsetZ, float T, float kPos, float kOffset, int countNoises, RandomCoor rand)
        {
            float[] yRes = new float[countNoises];
            for (int x = 0; x < countNoises; x++)
            {
                yRes[x] = getPerlinNoise(posX + x * kPos, posY + x * kPos, (int)(offsetX + x * kOffset), (int)(offsetZ + x * kOffset), rand);
            }

            float y = yRes[0];

            for (int x = 1; x < countNoises; x++)
            {
                y = lerp(T, y, yRes[x]);
            }
            return y;
        }



        private static float smoothstep(float edge0, float edge1, float x)
        {
            // Scale, bias and saturate x to 0..1 range
            x = clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
            // Evaluate polynomial
            return x * x * (3 - 2 * x);
        }

        private static float clamp(float x, float lowerlimit, float upperlimit)
        {
            if (x < lowerlimit)
                x = lowerlimit;
            if (x > upperlimit)
                x = upperlimit;
            return x;
        }

    }
}
