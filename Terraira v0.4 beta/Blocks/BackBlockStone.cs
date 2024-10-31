using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks
{
    class BackBlockStone : Block
    {
        public BackBlockStone()
        {
            texture = Content.blockStone;
            color = new Color(100, 100, 100);
        }

        public override Block getNewBlock()
        {
            return new BackBlockStone();
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
