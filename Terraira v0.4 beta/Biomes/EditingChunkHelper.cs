using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Biomes
{
    class EditingChunkHelper
    {

        public static Block[] getNeighboringBlock(int x, int y, Block[,] block)
        {
            int sizeX = block.GetLength(0);
            int sizeY = block.GetLength(1);

            Block[] neighboringBlock = new Block[9];

            if (y - 1 >= 0)
                neighboringBlock[0] = block[x, y - 1];
            if (x + 1 < sizeX && y - 1 >= 0)
                neighboringBlock[1] = block[x + 1, y - 1];
            if (x + 1 < sizeX)
                neighboringBlock[2] = block[x + 1, y];
            if (x + 1 < sizeX && y + 1 < sizeY)
                neighboringBlock[3] = block[x + 1, y + 1];
            if (y + 1 < sizeY)
                neighboringBlock[4] = block[x, y + 1];
            if (x - 1 >= 0 && y + 1 < sizeY)
                neighboringBlock[5] = block[x - 1, y + 1];
            if (x - 1 >= 0)
                neighboringBlock[6] = block[x - 1, y];
            if (x - 1 >= 0 && y - 1 >= 0)
                neighboringBlock[7] = block[x - 1, y - 1];
            neighboringBlock[8] = block[x, y];

            return neighboringBlock;
        }


        public static void setGrass(Chunk chunk, Block[,] block, Block[,] backBlock, blockName[] nameGrass)
        {
            int sizeX = block.GetLength(0);
            int sizeY = block.GetLength(1);

            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                {
                    blockName name = nameGrass[(int)chunk.world.rand.next(block[x, y].pX, block[x, y].pY, 0, nameGrass.Length)];

                    if (name != blockName.NA)
                    {

                        Block[] neighboringBlock = getNeighboringBlock(x, y, block);


                        Block top = neighboringBlock[0];
                        Block right = neighboringBlock[2];
                        Block down = neighboringBlock[4];
                        Block left = neighboringBlock[6];
                        Block center = neighboringBlock[8];

                        if (top != null && right != null && down != null && left != null && center != null)
                        {
                            if (center.id != 0)
                            {
                                int blockX = block[x, y].pX;
                                int blockY = block[x, y].pY;
                                if (top.id == 0)
                                    center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, name);

                                if (right.id == 0)
                                    center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, name);

                                if (down.id == 0)
                                    center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, name);

                                if (left.id == 0)
                                    center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, name);

                                block[x, y] = center;
                            }
                        }

                    }
                }
        }

        public static void setGrass(Chunk chunk, Block[,] block, Block[,] backBlock, blockName nameGrass)
        {

            int sizeX = block.GetLength(0);
            int sizeY = block.GetLength(1);

            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                {

                    if (nameGrass != blockName.NA)
                    {

                        Block[] neighboringBlock = getNeighboringBlock(x, y, block);


                        Block top = neighboringBlock[0];
                        Block right = neighboringBlock[2];
                        Block down = neighboringBlock[4];
                        Block left = neighboringBlock[6];
                        Block center = neighboringBlock[8];

                        if(top != null && right != null & down != null && left != null && center != null)
                        if (center.id != 0)
                        {
                            int blockX = block[x, y].pX;
                            int blockY = block[x, y].pY;
                            if (top.id == 0)
                                center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, nameGrass);

                            if (right.id == 0)
                                center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, nameGrass);

                            if (down.id == 0)
                                center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, nameGrass);

                            if (left.id == 0)
                                center = RegisteryBlocks.createBlock(blockX, blockY, false, chunk.world, nameGrass);

                            block[x, y] = center;
                        }
                    }
                }
        }

    }
}
