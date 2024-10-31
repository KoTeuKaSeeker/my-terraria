using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Blocks.BlockInterface;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Blocks
{
    abstract class blockPlant : Block
    {

        public bool isUpdatePlantTexture = true;
        public bool isGrown = true;
        public int idHighPlant;
        public int highPlant;
        public int errorInHigh;

        public int realHigh;

        public int offsetCanSetBlockX = 0;
        public int offsetCanSetBlockY = 0;

        public void growPlant()
        {
            if (realHigh <= 0)
            {
                int high = highPlant + (int)(world.rand.next(pX, pY, 0, errorInHigh));
                realHigh = high;
            }
            setValueProperty("realHigh", realHigh);
            if (idHighPlant < realHigh)
            {
                blockPlant block = world.setBlock(pX, pY - 1, id, false) as blockPlant;
                block.realHigh = realHigh;
                block.idHighPlant = idHighPlant + 1;
                block.setValueProperty("idHigh", block.idHighPlant);
                if (idHighPlant == 0)
                {
                    int biomeId = world.getBiomeWithBorder(pX, pY).id;
                    setValueProperty("biome1Id", biomeId);
                    block.setValueProperty("biome1Id", biomeId);
                }
                else
                {
                    int biomeId = getValueProperty("biome1Id");
                    block.setValueProperty("biome1Id", biomeId);
                }

                updateTexture();
            }
            else
            {
                isGrown = false;
            }

        }

        public blockPlant(int highPlant, int errorInHigh)
        {
            this.highPlant = highPlant;
            this.errorInHigh = errorInHigh;
            isUpdateTexture = false;
            collisionWithBlock = false;
            addProperty(1, 1, "isGrow");
            addProperty(0, 2, "idHigh");
            addProperty(0, 2, "realHigh");
            addProperty(0, 2, "biome1Id");

        }

        protected Block[] getNeighbourPlantsBlock()
        {
            Block[] neighbour = new Block[4];
            neighbour[0] = world.getBlockNotGeneration(pX, pY - 1, false);
            neighbour[1] = world.getBlockNotGeneration(pX + 1, pY, false);
            neighbour[2] = world.getBlockNotGeneration(pX, pY + 1, false);
            neighbour[3] = world.getBlockNotGeneration(pX - 1, pY, false);

            return neighbour;
        }

        public override void start()
        {
            base.start();
        }

        public override void update()
        {
            base.update();
            isGrown = getValueProperty("isGrow") == 1 ? true : false;
            if (isGrown)
            {
                Block oldBlock = world.getBlockNotGeneration(pX, pY - 1, false);
                bool canSet = oldBlock.canSet;
                if (canSet)
                {
                    IGrownPlant grownPlant = this as IGrownPlant;
                    if (grownPlant != null)
                    {
                        grownPlant.grownPlant();
                    }
                    else
                    {
                        growPlant();
                    }
                }
                isGrown = false;
                int number = isGrown ? 1 : 0;
                setValueProperty("isGrow", number);              
            }
            if (getValueProperty("idHigh") != 0)
            {
                Block downBlock = world.getBlockNotGeneration(pX, pY + 1, false);
                if (downBlock != null)
                {
                    if (downBlock.id != id)
                    {
                        destoyBlock();
                    }
                }
            }
        }
    }
}
