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
    class BiomeGreenland : Biome
    {
        public BiomeGreenland() : base(20, 20, 3451, 3452, 1, 1, 2, -1, 0, 9999999)
        {
            highLandshaft = 10;
            addSpawnBlock(RegisteryBlocks.blockOreGold, 20, 20, 0, 0, 1, -10, 1);
            addSpawnBlock(RegisteryBlocks.blockAir, 20, 20, 1235, 13, 1, 0.95f, 1);
            addSpawnBlock(RegisteryBlocks.blockDirt, 20, 20, 135, 123, 1, 2, 1);


            addSpawnBackBlock(RegisteryBlocks.backBlockStone, 20, 20, 2342, 135, 1, 1, 1);
        }

        public override Biome getNewBiome()
        {
            return new BiomeGreenland();
        }

    }
}

