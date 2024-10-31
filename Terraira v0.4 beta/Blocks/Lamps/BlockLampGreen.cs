using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks.Lamps
{
    class BlockLampGreen : BlockLamp
    {

        public BlockLampGreen()
        {
            texture = Content.blockStone;
            colorLighting = new Color(100, 255, 100);
            lightingK = 10;
        }

        public override Block getNewBlock()
        {
            return new BlockLampGreen();
        }

        public override void start()
        {
            base.start();
            color = colorLighting;
            backColor = colorLighting;
        }

        public override void update()
        {
            base.update();
        }

    }
}
