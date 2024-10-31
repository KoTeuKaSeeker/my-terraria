using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.ByteHelper;

namespace Terraira_v0._4_beta.Blocks.Lamps
{
    class BlockLampRGB : BlockLamp
    {

        public float t;

        public BlockLampRGB()
        {
            texture = Content.blockStone;
            lightingK = 10;
            colorLighting = new Color(255, 0, 0);
            color = colorLighting;
            backColor = colorLighting;
        }

        public override Block getNewBlock()
        {
            return new BlockLampRGB();
        }

        public override void start()
        {
            base.start();
        }

        public override void update()
        {
            base.update();
            updateLightingColor();
        }

        public void updateLightingColor()
        {
            t++;
            if (t >= 360) t = 0;
            float kT = 50;
            if ((t / kT) - (int)(t / kT) == 0)
            {
                colorLighting = Helper.HsvToRgb(t, 1, 1);
                color = colorLighting;
                backColor = colorLighting;
                updateNeighbourBlocks();
            }
        }

        public void updateNeighbourBlocks()
        {
            for (int x = pX - fieldNeighbour; x < pX + fieldNeighbour; x++)
                for (int y = pY - fieldNeighbour; y < pY + fieldNeighbour; y++)
                {
                    Block block = world.getBlockByBlockPosition(x, y, false);
                    block.updateShadow();
                }

            for (int x = pX - fieldNeighbour; x < pX + fieldNeighbour; x++)
                for (int y = pY - fieldNeighbour; y < pY + fieldNeighbour; y++)
                {
                    Block block = world.getBlockByBlockPosition(x, y, true);
                    block.updateShadow();
                }
        }
    }
}
