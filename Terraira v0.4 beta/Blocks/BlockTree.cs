using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Blocks
{
    
    abstract class BlockTree : blockPlant, IUpdateTexture
    {

        public BlockTree(int highPlant, int errorInHigh) : base(highPlant, errorInHigh)
        {
            offsetCanSetBlockX = 2;
            offsetCanSetBlockY = 4;
        }

        public void updateOtherTexture()
        {
            if (isUpdatePlantTexture)
            {
                Block[] neighbour = getNeighbourPlantsBlock();

                Block blockUp = neighbour[0];
                Block blockDown = neighbour[2];

                int saveIdHigh = getValueProperty("idHigh");
                int saveRealHigh = getValueProperty("realHigh");

                Vector2f tPos = new Vector2f(0, 0);
                Vector2f size = new Vector2f(16, 16);

                Vector2f[] strunkPos = new Vector2f[5];
                Vector2f[] strunkSize = new Vector2f[5];
                Vector2f[] strunkOffset = new Vector2f[5];

                int numberStrunk = 0;

                numberStrunk = world.rand.getProbably(pX + 2342, pY + 242, 100, 60) ? 2 : numberStrunk;
                numberStrunk = world.rand.getProbably(pX + 3452, pY + 34, 100, 60) ? 3 : numberStrunk;
                numberStrunk = world.rand.getProbably(pX + 263, pY + 6345, 100, 50) ? 4 : numberStrunk;
                numberStrunk = world.rand.getProbably(pX + 246, pY + 625, 100, 3) ? 0 : numberStrunk;
                numberStrunk = world.rand.getProbably(pX + 352, pY + 243, 100, 3) ? 1 : numberStrunk;

                strunkPos[0] = new Vector2f(14, 585); // Ветка налево
                strunkSize[0] = new Vector2f(46, 28);
                strunkOffset[0] = new Vector2f(-31, -9);

                strunkPos[1] = new Vector2f(45, 541); // Ветка направо
                strunkSize[1] = new Vector2f(46, 28);
                strunkOffset[1] = new Vector2f(0, -9);

                strunkPos[2] = new Vector2f(45, 93); // Чистный ствол
                strunkSize[2] = new Vector2f(16, 20);
                strunkOffset[2] = new Vector2f(0, 0);

                strunkPos[3] = new Vector2f(45, 528); // С выпоклостью вправо
                strunkSize[3] = new Vector2f(18, 20);
                strunkOffset[3] = new Vector2f(0, 0);

                strunkPos[4] = new Vector2f(43, 103); // С выпоклостью влево
                strunkSize[4] = new Vector2f(18, 20);
                strunkOffset[4] = new Vector2f(-2, 0);

                Vector2f[] endStrunkPos = new Vector2f[3];
                Vector2f[] endStrunkSize = new Vector2f[3];
                Vector2f[] endStrunkOffset = new Vector2f[3];

                endStrunkPos[0] = new Vector2f(86, 76); //Обрубленное дерево 1
                endStrunkSize[0] = new Vector2f(16, 16); 
                endStrunkOffset[0] = new Vector2f(0, 1);

                endStrunkPos[1] = new Vector2f(86, 76 + 16); //Обрубленное дерево 2
                endStrunkSize[1] = new Vector2f(16, 16);
                endStrunkOffset[1] = new Vector2f(0, 1);

                endStrunkPos[2] = new Vector2f(86, 76 + 32); //Обрубленное дерево 3
                endStrunkSize[2] = new Vector2f(16, 16);
                endStrunkOffset[2] = new Vector2f(0, 1);

                int numberEndStrunk = (int)(world.rand.next(pX, pY, 0, endStrunkPos.Length));

                bool isEndTrunk = false;

                if (blockUp != null)
                {

                    if (saveIdHigh != saveRealHigh && blockUp.id != id)
                    {
                        tPos = endStrunkPos[numberEndStrunk];
                        size = endStrunkSize[numberEndStrunk];
                        textureOffset = endStrunkOffset[numberEndStrunk];
                        isEndTrunk = true;
                    }
                }

                if (!isEndTrunk)
                {
                    if (saveIdHigh == saveRealHigh - 1 && saveIdHigh != 0)
                    {
                        tPos = new Vector2f(0, 0);
                        size = new Vector2f(100, 72);
                        textureOffset = new Vector2f(-60 + 16, -70 + 16);
                    }
                    else if (saveIdHigh > 0 && saveIdHigh < saveRealHigh)
                    {
                        tPos = strunkPos[numberStrunk];
                        size = strunkSize[numberStrunk];
                        textureOffset = strunkOffset[numberStrunk];
                    }
                    else if (saveIdHigh == 0)
                    {
                        tPos = new Vector2f(45, 613);
                        size = new Vector2f(32, 16);
                    }
                }

                textureRect = new FloatRect(tPos, size);
                textureSize = size;

                backTextureRect = textureRect;
                backTextureSize = textureSize;
                backTextureOffset = textureOffset;

                Biome biome = RegisteryBiome.getBiome(world, getValueProperty("biome1Id"));
                if (biome.biomeColor != null)
                {
                    Color[] biomeColor = biome.biomeColor;
                    color = biomeColor[(int)(world.rand.next(pX, pY, 0, biomeColor.Length))];
                }
            }
        }

        public override void update() // ОБЯЗАТЕЛЬНО АКТИВИРОВАТЬ В IBlockState.update()
        {
            base.update();

            int saveIdHigh = getValueProperty("idHigh");
            int saveRealHigh = getValueProperty("realHigh");

            if (saveIdHigh == saveRealHigh && saveIdHigh != 0)
            {
                for (int x = pX - offsetCanSetBlockX; x < pX + offsetCanSetBlockX + 1; x++)
                    for (int y = pY - offsetCanSetBlockY; y < pY + offsetCanSetBlockY + 1; y++)
                    {
                        world.getBlockNotGeneration(x, y, false).canSet = false;
                    }
            }
            else
            {
                for (int x = pX - offsetCanSetBlockX; x < pX + offsetCanSetBlockX + 1; x++)
                {
                    world.getBlockNotGeneration(x, pY, false).canSet = false;
                }
            }
        }

        public override void start()
        {
            base.start();
        }


    }
}
