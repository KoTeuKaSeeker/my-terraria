using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockDirt : Block
    {
        public BlockDirt()
        {
            texture = Content.blockDirt;
        }

        public override Block getNewBlock()
        {
            return new BlockDirt();
        }

        public override void start()
        {
            base.start();
        }

        public override void update()
        {
            base.update();
        }
    }
}
