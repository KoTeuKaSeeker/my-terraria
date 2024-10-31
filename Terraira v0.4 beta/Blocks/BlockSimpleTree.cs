using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks
{
    class BlockSimpleTree : BlockTree
    {
        public BlockSimpleTree() : base(10, 5)
        {
            texture = Content.blockTreeLeaves;
            backTexture = Content.blockTreeStrunk;
            //backColor = new Color(255, 0, 0);
        }

        public override Block getNewBlock()
        {
            return new BlockSimpleTree();
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
