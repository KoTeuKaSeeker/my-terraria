﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockGrassYellow : BlockGrassStandart
    {
        public BlockGrassYellow()
        {
            color = new Color(255, 180, 0);
        }

        public override Block getNewBlock()
        {
            return new BlockGrassYellow();
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
