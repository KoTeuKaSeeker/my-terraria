using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockOreIron : Block
    {
        public BlockOreIron()
        {
            texture = Content.blockStone;
            color = new Color(255, 209, 201);

        }

        public override Block getNewBlock()
        {
            return new BlockOreIron();
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
