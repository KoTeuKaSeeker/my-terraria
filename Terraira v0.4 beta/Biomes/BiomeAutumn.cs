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
    class BiomeAutumn : Biome, IBiomeAddition
    {

        public BiomeAutumn() : base(20, 20, 4234, 241, 1, 1, 1, 1, 0, 50)
        {
            biomeColor = new Color[2];
            biomeColor[0] = new Color(255, 0, 0);
            biomeColor[1] = new Color(255, 180, 0);
            addSpawnBlock(RegisteryBlocks.blockDirt, 20, 20, 4324, 1235, 1, -10, 1);
            addSpawnBlock(RegisteryBlocks.blockStone, 20, 20, 4324, 135, 1, 2f, 1);
            addSpawnBlock(RegisteryBlocks.blockAir, 20, 20, 345, 345, 1, 0.95f, 1);
            //backgroundColor = new Color(100, 100, 255);
        }

        public override Biome getNewBiome()
        {
            return new BiomeAutumn();
        }

        public void editChunk(Chunk chunk, Block[,] block, Block[,] backBlock)
        {
            EditingChunkHelper.setGrass(chunk, block, backBlock, new blockName[2] { blockName.grassRed, blockName.grassYellow });
        }
    }
}

