using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockAir : Block
    {
        public BlockAir()
        {
            color = new Color(0, 0, 0, 0);
            canSet = true;
            isUpdateBrightness = false;
        }

        public World World { get; }

        public override Block getNewBlock()
        {
            return new BlockAir();
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
