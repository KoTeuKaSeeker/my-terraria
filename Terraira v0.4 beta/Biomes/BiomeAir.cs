using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Biomes
{
    class BiomeAir : Biome
    {
        public BiomeAir() : base(20, 20, 2346, 1253, 1, 1, 1, -10, -1000000, 1000000)
        {
            addSpawnBlock(RegisteryBlocks.blockAir, 20, 20, 434, 234, 1, -10, 1);
            //backgroundColor = new Color(100, 100, 255);         
        }

        public override Biome getNewBiome()
        {
            return new BiomeAir();
        }
    }
}
