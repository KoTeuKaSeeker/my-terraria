using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;
using Terraira_v0._4_beta.WorldGenerationForder;

namespace Terraira_v0._4_beta.Worlds
{
    class NormalWorld : World
    {


        public NormalWorld(Map map, int seed) : base(map, seed)
        {
            addBiome(RegisteryBiome.createBiome(this, biomeName.herbal));
            addBiome(RegisteryBiome.createBiome(this, biomeName.autumn));
            //addBiome(RegisteryBiome.createBiome(this, biomeName.greenland));
        }


    }
}
