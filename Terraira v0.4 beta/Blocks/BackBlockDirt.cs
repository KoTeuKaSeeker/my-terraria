using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Blocks
{
    class BackBlockDirt : Block
    {
        public BackBlockDirt()
        {
            texture = Content.blockDirt;
            color = new Color(100, 100, 100);
        }

        public override Block getNewBlock()
        {
            return new BackBlockDirt();
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
