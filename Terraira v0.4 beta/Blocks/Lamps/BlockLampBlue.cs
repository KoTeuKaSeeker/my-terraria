using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks.Lamps
{
    class BlockLampBlue : BlockLamp
    {
        public BlockLampBlue()
        {
            texture = Content.blockStone;
            colorLighting = new Color(100, 100, 255);
            lightingK = 40;
            color = colorLighting;
            backColor = colorLighting;
        }

        public override Block getNewBlock()
        {
            return new BlockLampBlue();
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
