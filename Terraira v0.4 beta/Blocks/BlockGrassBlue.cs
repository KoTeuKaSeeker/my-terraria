using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockGrassBlue : BlockGrassStandart
    {
        public BlockGrassBlue()
        {
            color = new Color(0, 200, 255);
        }

        public override Block getNewBlock()
        {
            return new BlockGrassBlue();
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
