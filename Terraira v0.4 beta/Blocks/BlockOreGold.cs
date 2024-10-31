using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockOreGold : Block
    {
        public BlockOreGold()
        {
            texture = Content.blockStone;
            color = new Color(255, 248, 107);

        }

        public override Block getNewBlock()
        {
            return new BlockOreGold();
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
