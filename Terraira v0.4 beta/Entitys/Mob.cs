using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;

namespace Terraira_v0._4_beta.Entitys
{
    abstract class Mob : Entity
    {

        public static float maxFallDownSpeed = 12;
        public static float g = 0.02f;

        public override void update()
        {
            base.update();
            velocity.Y += g * DeltaTime.getDeltaTime();
            Vector2f dVelocity = velocity * DeltaTime.getDeltaTime();
            //dVelocity += new Vector2f(0, g) * DeltaTime.getDeltaTime();
            //if (DeltaTime.getDeltaTime() != 0)
            //{
            //    velocity.Y = dVelocity.Y / DeltaTime.getDeltaTime();
            //}

            if(dVelocity.X >= Block.sizeBlock)
            {
                dVelocity.X = Block.sizeBlock - 1;
            }
            else if (dVelocity.X <= -Block.sizeBlock)
            {
                dVelocity.X = -Block.sizeBlock + 1;
            }

            if (dVelocity.Y >= Block.sizeBlock)
            {
                dVelocity.Y = Block.sizeBlock - 1;
            }
            else if(dVelocity.Y <= -Block.sizeBlock)
            {
                dVelocity.Y = -Block.sizeBlock + 1;
            }
            position += dVelocity;
            if (velocity.Y > maxFallDownSpeed * DeltaTime.getDeltaTime()) velocity.Y = maxFallDownSpeed * DeltaTime.getDeltaTime();
            collision();
        }

        public override void start()
        {
            base.start();
        }

        public void collision()
        {
            if (isCollision)
            {
                int pX = (int)(position.X / Block.sizeBlock);
                int pY = (int)(position.Y / Block.sizeBlock);

                for (int x = (int)(pX - 10); x < (int)(pX + 10); x++)
                    for (int y = (int)(pY - 10); y < (int)(pY + 10); y++)
                    {
                        Block state = world.getBlockNotGeneration(x, y, false);
                        if (state != null)
                        {
                            Block block = state;
                            if (block.id != 0)
                            {
                                if (block.collisionWithBlock)
                                {
                                    FloatRect tileRect = new FloatRect(new Vector2f(x * Block.sizeBlock, y * Block.sizeBlock), new Vector2f(Block.sizeBlock, Block.sizeBlock));
                                    FloatRect npcRect = new FloatRect(position, size);

                                    if (npcRect.Intersects(tileRect))
                                    {
                                        Vector2f dVelocity = velocity * DeltaTime.getDeltaTime();
                                        if (dVelocity.X >= Block.sizeBlock)
                                        {
                                            dVelocity.X = Block.sizeBlock - 1;
                                        }
                                        else if (dVelocity.X <= -Block.sizeBlock)
                                        {
                                            dVelocity.X = -Block.sizeBlock + 1;
                                        }

                                        if (dVelocity.Y >= Block.sizeBlock)
                                        {
                                            dVelocity.Y = Block.sizeBlock - 1;
                                        }
                                        else if (dVelocity.Y <= -Block.sizeBlock)
                                        {
                                            dVelocity.Y = -Block.sizeBlock + 1;
                                        }

                                        float offsetTileX = Math.Abs(dVelocity.X) + 1;
                                        float offsetTileY = Math.Abs(dVelocity.Y) + 1;
                                        if (npcRect.Left < tileRect.Left + tileRect.Width && npcRect.Left + npcRect.Width > tileRect.Left && npcRect.Top + npcRect.Height < tileRect.Top + offsetTileY)
                                        {
                                            velocity = new Vector2f(velocity.X, 0);
                                            float offset = tileRect.Top - (npcRect.Top + npcRect.Height);
                                            position += new Vector2f(0, offset);
                                            activeCollisionEvent(Sides.top);
                                        }

                                        if (npcRect.Left + offsetTileX < tileRect.Left + tileRect.Width && npcRect.Left + npcRect.Width > tileRect.Left + offsetTileX && npcRect.Top > tileRect.Top + tileRect.Height - offsetTileY)
                                        {
                                            velocity = new Vector2f(velocity.X, 1);
                                            float offset = (tileRect.Top + tileRect.Height) - npcRect.Top;
                                            position += new Vector2f(0, offset);
                                            activeCollisionEvent(Sides.down);
                                        }

                                        if (npcRect.Left + npcRect.Width < tileRect.Left + offsetTileX && npcRect.Top + offsetTileY < tileRect.Top + tileRect.Height && npcRect.Top + npcRect.Height - offsetTileY > tileRect.Top)
                                        {
                                            velocity = new Vector2f(0, velocity.Y);
                                            float offset = tileRect.Left - (npcRect.Left + npcRect.Width);
                                            position += new Vector2f(offset, 0);
                                            activeCollisionEvent(Sides.right);
                                        }

                                        if (npcRect.Left > tileRect.Left + tileRect.Width - offsetTileX && npcRect.Top + offsetTileY < tileRect.Top + tileRect.Height && npcRect.Top + npcRect.Height - offsetTileY > tileRect.Top)
                                        {
                                            velocity = new Vector2f(0, velocity.Y);
                                            float offset = (tileRect.Left + tileRect.Width) - npcRect.Left;
                                            position += new Vector2f(offset, 0);
                                            activeCollisionEvent(Sides.left);
                                        }
                                    }

                                }
                            }
                        }
                    }
            }
        }
    }
}
