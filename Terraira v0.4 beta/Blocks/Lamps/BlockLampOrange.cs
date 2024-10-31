using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks.Lamps
{
    class BlockLampOrange : BlockLamp
    {

        public BlockLampOrange()
        {
            texture = Content.blockStone;
            colorLighting = new Color(227, 176, 113);
            lightingK = 20;
            color = colorLighting;
            backColor = colorLighting;
        }

        public override Block getNewBlock()
        {
            return new BlockLampOrange();
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
