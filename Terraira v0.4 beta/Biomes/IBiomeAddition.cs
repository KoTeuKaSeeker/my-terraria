using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Biomes
{
    interface IBiomeAddition
    {   

        void editChunk(Chunk chunk, Block[,] block, Block[,] backBlock);

    }
}
