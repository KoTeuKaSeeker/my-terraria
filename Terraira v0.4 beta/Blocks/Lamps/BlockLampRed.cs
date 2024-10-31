using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks.Lamps
{
    class BlockLampRed : BlockLamp
    {

        public BlockLampRed()
        {
            texture = Content.blockStone;
            colorLighting = new Color(255, 100, 100);
            lightingK = 10;
            color = colorLighting;
            backColor = colorLighting;
        }

        public override Block getNewBlock()
        {
            return new BlockLampRed();
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
