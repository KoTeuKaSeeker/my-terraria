using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockToShadowUpdate
    {

        public Block block;
        public float kDistance;

        public BlockToShadowUpdate(Block block, float kDistance)
        {
            this.block = block;
            this.kDistance = kDistance;
        }

    }
}
