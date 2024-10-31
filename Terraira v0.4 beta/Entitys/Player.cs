using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Entitys
{
    class Player : Mob
    {
        public float speedMove = 1;
        public float highJump = 2f;
        public Block buildingBlock;
        private Vector2i mousePosition;
        public Player()
        {
            saveInChunk = false;
            size = new Vector2f(Block.sizeBlock, Block.sizeBlock * 2);
            color = new Color(100, 100, 255);
            buildingBlock = RegisteryBlocks.getBlock(blockName.lampGreen);
        }

        public override void start()
        {
            base.start();
        }

        public override void update()
        {
            movePlayer();
            createBlock();
            Program.win.MouseMoved += getMousePosition;
            base.update();
        }

        private void getMousePosition(object sender, MouseMoveEventArgs e)
        {
            mousePosition = new Vector2i(e.X, e.Y);
        }



        public void createBlock()
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Right) || Mouse.IsButtonPressed(Mouse.Button.Left))
            {

                int mouseX = (int)(mousePosition.X + world.camera.X - (Program.win.Size.X / 2));
                int mouseY = (int)(mousePosition.Y + world.camera.Y - (Program.win.Size.Y / 2));

                int blockX = 0;
                int blockY = 0;

                if (mouseX < 0)
                {
                    if (Math.Abs(mouseX * 1.00f / Block.sizeBlock) - Math.Abs(mouseX / Block.sizeBlock) > 0)
                    {
                        blockX = mouseX / Block.sizeBlock - 1;
                    }
                    else
                    {
                        blockX = mouseX / Block.sizeBlock;
                    }
                }
                else
                {
                    blockX = mouseX / Block.sizeBlock;
                }


                if (mouseY < 0)
                {
                    if (Math.Abs(mouseY * 1.00f / Block.sizeBlock) - Math.Abs(mouseY / Block.sizeBlock) > 0)
                    {
                        blockY = mouseY / Block.sizeBlock - 1;
                    }
                    else
                    {
                        blockY = mouseY / Block.sizeBlock;
                    }
                }
                else
                {
                    blockY = mouseY / Block.sizeBlock;
                }
                if (Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    Block block = world.getBlockNotGeneration(blockX, blockY, false);
                    buildingBlock = block;
                    //Console.WriteLine(buildingBlock);
                    //Console.WriteLine("blockX = " + blockX + "; blockY = " + blockY + ";");
                }
                else if(Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    //if (world.getBlockNotGeneration(blockX, blockY, false).canSet)
                    //{
                    world.setBlock(blockX, blockY, buildingBlock.id, false);
                    //Console.WriteLine("blockX = " + blockX + "; blockY = " + blockY + ";");
                    //}
                }

            }
        }

        public void movePlayer() // Лучше в будущем изменить под плавное изменение скорости
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                velocity = new Vector2f(speedMove, velocity.Y);
            }
            else if(Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                velocity = new Vector2f(-speedMove, velocity.Y);
            }
            else
            {
                velocity = new Vector2f(0, velocity.Y);
            }

            if (velocity.Y == 0)
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    velocity.Y = -highJump;
                }

            if (Keyboard.IsKeyPressed(Keyboard.Key.H))
            {
                Thread.Sleep(25);
            }
        }

        public override Entity getNewEntity()
        {
            return new Player();
        }
    }
}
