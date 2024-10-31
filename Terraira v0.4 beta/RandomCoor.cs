using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta
{
    class RandomCoor
    {
        public float[,] numberMap;
        public float sizeNumberMap;
        public float seed;

        public RandomCoor(int sizeNumberMap, int seed)
        {
            this.sizeNumberMap = sizeNumberMap;
            this.seed = seed;

            numberMap = new float[sizeNumberMap, sizeNumberMap];
            System.Random rand = new System.Random(seed);
            for (int x = 0; x < sizeNumberMap; x++)
                for (int y = 0; y < sizeNumberMap; y++)
                {
                    numberMap[x, y] = rand.Next(0, 10000) / 10000.0000f;
                }
        }

        public bool getProbably(int posX, int posY, int maxSize, int probably)
        {
            float value = next(posX, posY, 0, maxSize);
            return value < probably;
        }

        public float next(int posX, int posY, float min, float max)
        {
            float posXF = posX + 100000;
            float posYF = posY + 100000;
            if (posXF >= sizeNumberMap)
            {
                posXF = ((posXF / sizeNumberMap) - (int)(posXF / sizeNumberMap)) * sizeNumberMap;
            }

            if (posYF >= sizeNumberMap)
            {
                posYF = ((posYF / sizeNumberMap) - (int)(posYF / sizeNumberMap)) * sizeNumberMap;
            }

            if(posXF < 0)
            {
                posXF = (Math.Abs(posXF) - (int)(Math.Abs(posXF) / sizeNumberMap)) > 0 ? ((Math.Abs(posXF) - (int)(Math.Abs(posXF) / sizeNumberMap)) + 1) * sizeNumberMap : 0;
            }
            if (posYF < 0)
            {
                posYF = (Math.Abs(posYF) - (int)(Math.Abs(posYF) / sizeNumberMap)) > 0 ? ((Math.Abs(posYF) - (int)(Math.Abs(posYF) / sizeNumberMap)) + 1) * sizeNumberMap : 0;
            }
            float number = numberMap[(int)posXF, (int)posYF];
            float answer = min + (max - min) * number;

            return answer;
        }

    }
}
