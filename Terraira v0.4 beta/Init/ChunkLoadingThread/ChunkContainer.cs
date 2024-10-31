using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Init.ChunkLoadingThread
{
    struct ChunkContainer
    {

        public int chunkX;
        public int chunkY;
        public World world;

        public ChunkContainer(int chunkX, int chunkY, World world)
        {
            this.chunkX = chunkX;
            this.chunkY = chunkY;
            this.world = world;
        }

    }
}
