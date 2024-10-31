using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Init
{
    class Chunk
    {

        public static int sizeChunk = 30;

        public Block[,] block;
        public Block[,] backBlock;

        public int chunkX;
        public int chunkY;

        public int biome1;
        public int biome2;
        public int biome2Info;

        public World world;

        public void initBlocks()
        {
            block = new Block[sizeChunk, sizeChunk];
            backBlock = new Block[sizeChunk, sizeChunk];
            for (int x = 0; x < sizeChunk; x++)
                for (int y = 0; y < sizeChunk; y++)
                {
                    block[x, y] = RegisteryBlocks.createBlock(chunkX * sizeChunk + x, chunkY * sizeChunk + y, false, world, 0);
                    backBlock[x, y] = RegisteryBlocks.createBlock(chunkX * sizeChunk + x, chunkY * sizeChunk + y, true, world, 0);
                }
        }

        public void setBlock(int x, int y, int id)
        {
            block[x, y] = RegisteryBlocks.createBlock(chunkX * sizeChunk + x, chunkY * sizeChunk + y, false, world, id);
        }

        public void setBackBlock(int x, int y, int id)
        {
            backBlock[x, y] = RegisteryBlocks.createBlock(chunkX * sizeChunk + x, chunkY * sizeChunk + y, true, world, id);
        }

        public Block[] getNeighboringBlocks(int x, int y)
        {


            Block[] neighboringBlock = new Block[9];
            //neighboringBlock[0] = y - 1 >= 0 ? block[x, y - 1] : RegisteryBlocks.blockAir;
            //neighboringBlock[1] = y - 1 >= 0 && x + 1 < sizeChunk ? block[x + 1, y - 1] : RegisteryBlocks.blockAir;
            //neighboringBlock[2] = x + 1 < sizeChunk ? block[x + 1, y] : RegisteryBlocks.blockAir;
            //neighboringBlock[3] = x + 1 < sizeChunk && y + 1 < sizeChunk ? block[x + 1, y + 1] : RegisteryBlocks.blockAir;
            //neighboringBlock[4] = y + 1 < sizeChunk ? block[x, y + 1] : RegisteryBlocks.blockAir;
            //neighboringBlock[5] = x - 1 >= 0 && y + 1 < sizeChunk ? block[x - 1, y + 1] : RegisteryBlocks.blockAir;
            //neighboringBlock[6] = x - 1 >= 0 ? block[x - 1, y] : RegisteryBlocks.blockAir;
            //neighboringBlock[7] = x - 1 >= 0 && y - 1 >= 0 ? block[x - 1, y - 1] : RegisteryBlocks.blockAir;
            //neighboringBlock[8] = block[x, y];

            //Block[] aroundBlock = new Block[9];

            neighboringBlock[0] = block[x, y - 1];
            neighboringBlock[1] = block[x + 1, y - 1];
            neighboringBlock[2] = block[x + 1, y];
            neighboringBlock[3] = block[x + 1, y + 1];
            neighboringBlock[4] = block[x, y + 1];
            neighboringBlock[5] = block[x - 1, y + 1];
            neighboringBlock[6] = block[x - 1, y];
            neighboringBlock[7] = block[x - 1, y - 1];
            neighboringBlock[8] = block[x, y];

            return neighboringBlock;
        }

        public void updateCloseBlocks()
        {
            for(int x = 0; x < sizeChunk; x++)
                for(int y = 0; y < sizeChunk; y++)
                {
                    ICloseObject closeObject = block[x, y] as ICloseObject;
                    if(closeObject != null)
                    {
                        closeObject.updateObject();
                    }
                }

            for (int x = 0; x < sizeChunk; x++)
                for (int y = 0; y < sizeChunk; y++)
                {
                    ICloseObject closeObject = backBlock[x, y] as ICloseObject;
                    if (closeObject != null)
                    {
                        closeObject.updateObject();
                    }
                }
        }


        public Chunk(int chunkX, int chunkY, World world)
        {
            this.chunkX = chunkX;
            this.chunkY = chunkY;
            this.world = world;
            initBlocks();

            //setBackBlock(2, 1, 2);
            //setBlock(2, 2, 2);
        }
    }
}
