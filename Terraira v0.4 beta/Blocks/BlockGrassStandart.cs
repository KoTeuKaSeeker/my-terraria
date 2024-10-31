using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Blocks
{
    abstract class BlockGrassStandart : Block
    {
        public BlockGrassStandart()
        {
            isUpdateTexture = true;
            texture = Content.blockGrass;
            backTexture = Content.backgroundBlockGrass;
            addProperty(172, 5, "color");

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
