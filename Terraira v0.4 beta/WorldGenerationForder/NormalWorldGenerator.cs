using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.WorldGenerationForder
{
    class NormalWorldGenerator : WorldGenerator
    {
        public NormalWorldGenerator(World world) : base(world)
        {
            addBiome(new BiomeHerbal());
        }
    }
}
