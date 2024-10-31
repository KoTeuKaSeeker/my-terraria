using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Entitys
{
    class Slime : Mob
    {
        public float highJump = 4;
        public float speedMove = 6;
        public int isRightDirection;
        public Slime()
        {
            addProperty(0, 1, "speedMove").addProperty(0, 1, "isRightDirection");

            color = new Color((byte)(50 + Program.rand.Next(0, 200)), (byte)(50 + Program.rand.Next(0, 200)), (byte)(50 + Program.rand.Next(0, 200)));
            size = new Vector2f(Block.sizeBlock, Block.sizeBlock / 1.5f);
            collisionEvent += setDirection;
        }

        public override Entity getNewEntity()
        {
            return new Slime();
        }

        public void setDirection(Sides side)
        {
            if(side == Sides.right)
            {
                isRightDirection = 1;
            }
            else if(side == Sides.left)
            {
                isRightDirection = 2;
            }
        }

        public override void start()
        {
            int localSpeedMove = getValueProperty("speedMove");
            int localIsRightDirection = getValueProperty("isRightDirection");

            if (localSpeedMove == 0)
            {
                speedMove = Program.rand.Next(2, (int)speedMove);
            }
            else
            {
                speedMove = localSpeedMove;
            }

            if (localIsRightDirection == 0)
            {
                isRightDirection = Program.rand.Next(0, 1) + 1;
            }
            else
            {
                isRightDirection = localIsRightDirection;
            }

            setValueProperty("speedMove", (int)speedMove);
            setValueProperty("isRightDirection", isRightDirection);
            base.start();
        }

        public override void update()
        {
            if(velocity.Y == 0)
            {
                velocity.Y = -highJump;
            }

            if (isRightDirection == 2)
            {
                velocity.X = speedMove;
            }
            else
            {
                velocity.X = -speedMove;
            }
            base.update();
        }
    }
}
