using SFML.Graphics;
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
    class BiomeHerbal : Biome, IBiomeAddition
    {
        public BiomeHerbal() : base(20, 20, 34234, 4532, 1, 1, 1, -10, 0, 50)
        {
            biomeColor = new Color[1];
            biomeColor[0] = new Color(0, 200, 255);
            highLandshaft = 10;

            addSpawnBlock(RegisteryBlocks.blockStone, 20, 20, 0, 0, 1, -10, 1);
            //addSpawnBlock(RegisteryBlocks.blockDirt, 20, 20, 2346, 2345, 1, 3, 1);
            //addSpawnBlock(RegisteryBlocks.blockOreIron, 15, 15, 2345, 1236, 20, 50, 0.98f);
            //addSpawnBlock(RegisteryBlocks.blockOreGold, 10, 10, 2345, 1236, 20, 50, 0.98f);
            //addSpawnBackBlock(RegisteryBlocks.backBlockStone, 20, 20, 3234, 2525, 1, -10, 1);

            addSpawnBlock(RegisteryBlocks.blockDirt, 20, 20, 425, 741, 1, 2f, 1);
            addSpawnBlock(RegisteryBlocks.blockGrassRed, 20, 20, 123, 2346, 1, 5, 1);

            addSpawnBlock(RegisteryBlocks.blockOreIron, 15, 15, 2345, 1236, 20, 50, 0.98f);
            addSpawnBlock(RegisteryBlocks.blockOreGold, 10, 10, 2345, 1236, 20, 50, 0.98f);

            addSpawnBlock(RegisteryBlocks.blockAir, 20, 20, 345, 345, 1, 0.5f, 1);

            addSpawnBackBlock(RegisteryBlocks.backBlockStone, 20, 20, 345, 345, 1, -10, 1);
            //backgroundColor = new Color(100, 100, 255);

        }

        public override Biome getNewBiome()
        {
            return new BiomeHerbal();
        }

        public void editChunk(Chunk chunk, Block[,] block, Block[,] backBlock)
        {
            EditingChunkHelper.setGrass(chunk, block, backBlock, blockName.grassBlue);
        }

    }
}
