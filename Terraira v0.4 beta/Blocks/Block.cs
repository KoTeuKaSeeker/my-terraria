using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Blocks.PropertyFolder;
using Terraira_v0._4_beta.ByteHelper;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.Blocks
{
    abstract class Block
    {

        public static int sizeBlock = 16;

        public int id;

        public Texture texture;
        public Texture backTexture;
        public Color color = Color.White;
        public Color backColor = Color.White;
        public FloatRect textureRect;
        public FloatRect backTextureRect;

        public Vector2f textureOffset;
        public Vector2f backTextureOffset;

        public Vector2f textureSize = new Vector2f(sizeBlock, sizeBlock);
        public Vector2f backTextureSize = new Vector2f(sizeBlock, sizeBlock);

        public RectangleShape rect;
        public RectangleShape backgroundRect;

        public PropertyManager propertyManager;
        public World world;
        public bool isUpdateTexture = true;

        public Color colorBrightness = new Color(255, 255, 255);
        public float brightnessK = 1;
        public static int fieldNeighbour = 8;
        public float kShadow = 0.02f;

        public Color colorLighting = new Color(0, 0, 0);
        public float lightingK = 1;

        public bool isGetNeighbourShadow = true;
        public bool isUpdateBrightness = true;
        public bool isBrightness = false;

        public int levelUpdated;

        public int pX, pY;
        public bool isBackBlock;

        public bool canSet = false;

        public bool collisionWithBlock = true;

        private IUpdateTexture iUpdateTexture;

        public Block()
        {

            iUpdateTexture = this as IUpdateTexture;
            propertyManager = new PropertyManager();
        }

        public Color getTintColor(Color color, float brightness)
        {

            float r = color.R;
            float g = color.G;
            float b = color.B;

            float k = (brightness * 1.00f) / 255.00f;

            if (k > 1) k = 1; if (k < 0) k = 0;

            r = r * k;
            g = g * k;
            b = b * k;

            if (r < 0) r = 0; if (r > 255) r = 255;
            if (g < 0) g = 0; if (g > 255) g = 255;
            if (b < 0) b = 0; if (b > 255) b = 255;
            return new Color((byte)r, (byte)g, (byte)b);
        }

        public Color getTintColorByColor(Color blockColor, Color biomeColor)
        {
            float r = blockColor.R;
            float g = blockColor.G;
            float b = blockColor.B;

            float kR = (biomeColor.R * 1.00f) / 255.00f;
            float kG = (biomeColor.G * 1.00f) / 255.00f;
            float kB = (biomeColor.B * 1.00f) / 255.00f;


            if (kR > 1) kR = 1; if (kR < 0) kR = 0;
            if (kG > 1) kG = 1; if (kG < 0) kG = 0;
            if (kB > 1) kB = 1; if (kB < 0) kB = 0;

            r = r * kR;
            g = g * kG;
            b = b * kB;

            if (r < 0) r = 0; if (r > 255) r = 255;
            if (g < 0) g = 0; if (g > 255) g = 255;
            if (b < 0) b = 0; if (b > 255) b = 255;
            return new Color((byte)r, (byte)g, (byte)b);
        }

        public void updateBrightness()
        {
            if (color.A < 255 || backColor.A < 255)
            {
                Block backBlock = world.getBlockNotGeneration(pX, pY, true);
                if (backBlock.color.A < 255 || backBlock.backColor.A < 255)
                {
                    Biome biome = RegisteryBiome.getBiome(world, world.getBiomeIdByBlockPosition(pX, pY));
                    float k = 1 - ((color.A * 1.00f / 255) * (backColor.A * 1.00f / 255));
                    float backK = 1 - ((backBlock.color.A * 1.00f / 255) * (backBlock.backColor.A * 1.00f / 255));

                    float r = biome.backgroundColor.R * k * backK;
                    float g = biome.backgroundColor.G * k * backK;
                    float b = biome.backgroundColor.B * k * backK;

                    colorBrightness = new Color((byte)r, (byte)g, (byte)b);
                    brightnessK = biome.brightnessK * k * backK;
                }
                else
                {
                    colorBrightness = colorLighting;
                    brightnessK = lightingK;
                }
            }
            else
            {
                colorBrightness = colorLighting;
                brightnessK = lightingK;
            }
            isBrightness = true;
        }

        public Color getNeighbourBrightnessCheckField()
        {
            float rCB = colorBrightness.R;
            float gCB = colorBrightness.G;
            float bCB = colorBrightness.B;

            float rWorldK = world.worldColor.R * 1.00f / 255;
            float gWorldK = world.worldColor.G * 1.00f / 255;
            float bWorldK = world.worldColor.B * 1.00f / 255;

            float maxDistance = Helper.distance(new Vector2f(0, 0), new Vector2f(fieldNeighbour - 1, fieldNeighbour - 1));

            float countTestBlocks = 0;
            for (int x = pX - fieldNeighbour; x < pX + fieldNeighbour; x++)
                for (int y = pY - fieldNeighbour; y < pY + fieldNeighbour; y++)
                {
                    Block block = world.getBlockByBlockPosition(x, y, false);
                    if (block != null)
                    {
                        if (!block.isBrightness)
                        {
                            block.updateBrightness();
                        }
                        if (block.colorBrightness.R != 0 || block.colorBrightness.G != 0 || block.colorBrightness.B != 0)
                        {

                            float distance = Helper.distance(new Vector2f(pX, pY), new Vector2f(x, y));
                            float t = (distance / maxDistance);
                            if (t < 0) t = 0;
                            if (t > 1) t = 1;
                            countTestBlocks++;

                            float kR = block.brightnessK * block.kShadow * rWorldK;
                            float kG = block.brightnessK * block.kShadow * gWorldK;
                            float kB = block.brightnessK * block.kShadow * bWorldK;

                            float pR = block.colorBrightness.R * kR * (1 - t);
                            float pG = block.colorBrightness.G * kG * (1 - t); // -40 8
                            float pB = block.colorBrightness.B * kB * (1 - t);

                            float t1 = t < 0.5f ? 1 : 1 / -((t - 0.5f) / 0.5f);
                            float t2 = t < 0.5f ? (t / 0.5f) : 1;

                            rCB = rCB + pR;
                            gCB = gCB + pG;
                            bCB = bCB + pB;
                            if (rCB > 255 || gCB > 255 || bCB > 255)
                            {
                                float maxColor = 0;

                                if (maxColor < rCB) maxColor = rCB;
                                if (maxColor < gCB) maxColor = gCB;
                                if (maxColor < bCB) maxColor = bCB;

                                if (maxColor != 0)
                                {
                                    rCB = ((rCB * 1.00f / maxColor) * 1.00f) * 255;
                                    gCB = (gCB * 1.00f / maxColor) * 255;
                                    bCB = (bCB * 1.00f / maxColor) * 255;
                                }
                            }

                            //rCB += (pR - rCB) * t;
                            //gCB += (pG - gCB) * t;
                            //bCB += (pB - bCB) * t;
                        }
                    }
                }
            //rCB = rCB / countTestBlocks;
            //gCB = gCB / countTestBlocks;
            //bCB = bCB / countTestBlocks;
            if (rCB > 255) rCB = 255; if (rCB < 0) rCB = 0;
            if (gCB > 255) gCB = 255; if (gCB < 0) gCB = 0;
            if (bCB > 255) bCB = 255; if (bCB < 0) bCB = 0;
            return new Color((byte)rCB, (byte)gCB, (byte)bCB);
        }

        public List<BlockToShadowUpdate> getCheckingBlocksByFloodFillPropagation(int countChecking)
        {
            float kPermeability = 10;
            float k = 1;
            float km = 0.6f;
            float isChecking = 0;

            List<Block> allBlocks = new List<Block>();
            List<Block> lastIdBlocks = new List<Block>();
            List<Block> lastIdBlocksOld = new List<Block>();
            List<float> tAll = new List<float>();
            List<float> t = new List<float>();
            List<float> tOld = new List<float>();

            List<BlockToShadowUpdate> allReturnBlocks = new List<BlockToShadowUpdate>();


            allBlocks.Add(this);
            tAll.Add(kPermeability);
            lastIdBlocksOld.Add(this);
            tOld.Add(kPermeability);

            while (lastIdBlocksOld.Count != 0 && isChecking < kPermeability)
            {
                for (int x = 0; x < lastIdBlocksOld.Count; x++)
                {
                    Block localBlock = lastIdBlocksOld[x];
                    Block[] checkBlock = new Block[4];

                    checkBlock[0] = world.getBlockByBlockPosition(localBlock.pX, localBlock.pY - 1, false);
                    checkBlock[1] = world.getBlockByBlockPosition(localBlock.pX + 1, localBlock.pY, false);
                    checkBlock[2] = world.getBlockByBlockPosition(localBlock.pX, localBlock.pY + 1, false);
                    checkBlock[3] = world.getBlockByBlockPosition(localBlock.pX - 1, localBlock.pY, false);


                    for (int y = 0; y < 4; y++)
                    {
                        if (checkBlock[y] != null)
                        {
                            if (!allBlocks.Contains(checkBlock[y]))
                            {
                                if (!checkBlock[y].isBrightness)
                                {
                                    checkBlock[y].updateBrightness();
                                }

                                if (true)
                                {

                                    k = (checkBlock[y].color.A * 1.00f / 255.00f) * 1.3f + km;
                                    k = tOld[x] - k;

                                    if (k > 0)
                                    {
                                        t.Add(k);
                                        tAll.Add(k);
                                        lastIdBlocks.Add(checkBlock[y]);
                                        allBlocks.Add(checkBlock[y]);

                                        allReturnBlocks.Add(new BlockToShadowUpdate(checkBlock[y], isChecking));
                                    }
                                }
                            }
                        }
                    }
                }
                lastIdBlocksOld = lastIdBlocks;
                lastIdBlocks = new List<Block>();

                tOld = t;
                t = new List<float>();
                isChecking++;
            }
            return allReturnBlocks;
        }

        /* 
         *  Метод не доделан до конца, так что в будущем можно дописать
         *  + к тому он лагучий, его стоит юзать только на GPU
         */
        public Color getNeighbourColorBrightnessFloodFillPropagation()
        {
            float rCB = colorBrightness.R;
            float gCB = colorBrightness.G;
            float bCB = colorBrightness.B;

            float rWorldK = world.worldColor.R * 1.00f / 255;
            float gWorldK = world.worldColor.G * 1.00f / 255;
            float bWorldK = world.worldColor.B * 1.00f / 255;

            float maxDistance = Helper.distance(new Vector2f(0, 0), new Vector2f(fieldNeighbour - 1, fieldNeighbour - 1));

            float countTestBlocks = 0;

            List<BlockToShadowUpdate> checkBlock = getCheckingBlocksByFloodFillPropagation(10);

            for(int x = 0; x < checkBlock.Count; x++)
                {
                Block block = checkBlock[x].block;
                    if (block != null)
                    {
                        if (!block.isBrightness)
                        {
                            block.updateBrightness();
                        }
                        if (block.colorBrightness.R != 0 || block.colorBrightness.G != 0 || block.colorBrightness.B != 0)
                        {

                            float distance = checkBlock[x].kDistance;
                            float t = (distance * 1.00f / 10.00f);
                            if (t < 0) t = 0;
                            if (t > 1) t = 1;
                            countTestBlocks++;

                            float kR = block.brightnessK * block.kShadow * rWorldK;
                            float kG = block.brightnessK * block.kShadow * gWorldK;
                            float kB = block.brightnessK * block.kShadow * bWorldK;

                            float pR = block.colorBrightness.R * kR * (1 - t);
                            float pG = block.colorBrightness.G * kG * (1 - t); // -40 8
                            float pB = block.colorBrightness.B * kB * (1 - t);

                            float t1 = t < 0.5f ? 1 : 1 / -((t - 0.5f) / 0.5f);
                            float t2 = t < 0.5f ? (t / 0.5f) : 1;

                            rCB = rCB + pR;
                            gCB = gCB + pG;
                            bCB = bCB + pB;
                            if (rCB > 255 || gCB > 255 || bCB > 255)
                            {
                                float maxColor = 0;

                                if (maxColor < rCB) maxColor = rCB;
                                if (maxColor < gCB) maxColor = gCB;
                                if (maxColor < bCB) maxColor = bCB;

                                if (maxColor != 0)
                                {
                                    rCB = (rCB * 1.00f / maxColor) * 255;
                                    gCB = (gCB * 1.00f / maxColor) * 255;
                                    bCB = (bCB * 1.00f / maxColor) * 255;
                                }
                            }
                        }
                    }
                }
            //rCB = rCB / countTestBlocks;
            //gCB = gCB / countTestBlocks;
            //bCB = bCB / countTestBlocks;
            if (rCB > 255) rCB = 255; if (rCB < 0) rCB = 0;
            if (gCB > 255) gCB = 255; if (gCB < 0) gCB = 0;
            if (bCB > 255) bCB = 255; if (bCB < 0) bCB = 0;
            return new Color((byte)rCB, (byte)gCB, (byte)bCB);
        }

        public Color getNeighbourColorBrightness()
        {
            return getNeighbourBrightnessCheckField();
        }

        public void updateShadow()
        {
            updateBrightness();
            if (isUpdateBrightness)
            {
                if (rect != null)
                    rect.FillColor = getTintColorByColor(color, getNeighbourColorBrightness());
                if (backgroundRect != null)
                    backgroundRect.FillColor = getTintColorByColor(backColor, getNeighbourColorBrightness());
            }
        }

        public void createBlockTexture()
        {
            updateTexture();
            rect = new RectangleShape(textureSize);
            backgroundRect = new RectangleShape(backTextureSize);

            rect.Position = new Vector2f(pX * sizeBlock, pY * sizeBlock) + textureOffset;
            rect.Texture = texture;
            rect.TextureRect = (IntRect)textureRect;
            rect.FillColor = color;

            backgroundRect.Position = new Vector2f(pX * sizeBlock, pY * sizeBlock) + backTextureOffset;
            backgroundRect.Texture = backTexture;
            backgroundRect.TextureRect = (IntRect)backTextureRect;
            backgroundRect.FillColor = backColor;
            updateShadow();
        }

        public void destoyBlock() // Если вызвать эту херь, то в будующем должен падать дроп или что нибудь произойти, а пока просто тут заменяется блок на воздух :)
        {
            world.setBlock(pX, pY, 0, false);
        }

        public void updateTexture()
        {
            if(iUpdateTexture != null)
            {
                iUpdateTexture.updateOtherTexture();
                rect = new RectangleShape(textureSize);
                rect.Position = new Vector2f(pX * sizeBlock, pY * sizeBlock) + textureOffset;
                rect.Texture = texture;
                rect.TextureRect = (IntRect)textureRect;
                rect.FillColor = color;

                backgroundRect = new RectangleShape(backTextureSize);
                backgroundRect.Position = new Vector2f(pX * sizeBlock, pY * sizeBlock) + backTextureOffset;
                backgroundRect.Texture = backTexture;
                backgroundRect.TextureRect = (IntRect)backTextureRect;
                backgroundRect.FillColor = backColor;
            }
            else if (isUpdateTexture)
            {
                int pXl = pX;
                int pYl = pY;
                int sX = 1;
                int sY = 1;

                //isBackBlock = false;

                int[] b = new int[8];
                b[0] = world.getBlockNotGeneration(pXl * sX, (pYl - 1) * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration(pXl * sX, (pYl - 1) * sY, isBackBlock).id;
                b[1] = world.getBlockNotGeneration((pXl + 1) * sX, (pYl - 1) * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration((pXl + 1) * sX, (pYl - 1) * sY, isBackBlock).id;
                b[2] = world.getBlockNotGeneration((pXl + 1) * sX, pYl * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration((pXl + 1) * sX, pYl * sY, isBackBlock).id;
                b[3] = world.getBlockNotGeneration((pXl + 1) * sX, (pYl + 1) * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration((pXl + 1) * sX, (pYl + 1) * sY, isBackBlock).id;
                b[4] = world.getBlockNotGeneration(pXl * sX, (pYl + 1) * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration(pXl * sX, (pYl + 1) * sY, isBackBlock).id;
                b[5] = world.getBlockNotGeneration((pXl - 1) * sX, (pYl + 1) * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration((pXl - 1) * sX, (pYl + 1) * sY, isBackBlock).id;
                b[6] = world.getBlockNotGeneration((pXl - 1) * sX, pYl * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration((pXl - 1) * sX, pYl * sY, isBackBlock).id;
                b[7] = world.getBlockNotGeneration((pXl - 1) * sX, (pYl - 1) * sY, isBackBlock) == null ? 0 : world.getBlockNotGeneration((pXl - 1) * sX, (pYl - 1) * sY, isBackBlock).id;

                int tX = 0;
                int tY = 0;


                if (b[6] == id && b[0] == 0 && b[2] == 0 && b[4] == 0)
                {
                    tX = 12;
                    tY = 0;
                }
                else if (b[0] == 0 && b[2] == id && b[4] == 0 && b[6] == 0)
                {
                    tX = 9;
                    tY = 0;
                }
                else if (b[0] == 0 && b[2] == id && b[4] == id && b[6] == 0 && b[7] == 0 || b[0] == 0 && b[2] == id && b[4] == id && b[6] == 0 && b[3] == id)
                {
                    tX = 7;
                    tY = 12;
                }
                else if (b[6] == id && b[4] == id && b[0] == 0 && b[2] == 0 && b[5] == id || b[6] == id && b[4] == id && b[0] == 0 && b[2] == 0 && b[5] == 0)
                {
                    tX = 9;
                    tY = 12;
                }
                else if (b[6] == id && b[7] == id && b[0] == id && b[2] == 0 && b[4] == 0)
                {
                    tX = 9;
                    tY = 14;
                }
                else if (b[0] == 0 && b[2] == id && b[4] == id && b[6] == id)
                {
                    tX = 8;
                    tY = 12;
                }
                else if (b[0] == id && b[2] == id && b[4] == 0 && b[6] == id)
                {
                    tX = 8;
                    tY = 14;
                }
                else if (b[0] == id && b[2] == id && b[4] == id && b[6] == 0)
                {
                    tX = 7;
                    tY = 13;
                }
                else if (b[0] == id && b[2] == 0 && b[4] == id && b[6] == id)
                {
                    tX = 9;
                    tY = 13;
                }
                else if (b[0] == 0 && b[2] != 0 && b[4] != 0 && b[6] != 0)
                {
                    tX = 1;
                    tY = 0;
                }
                else if (b[0] == 0 && b[2] != 0 && b[4] == 0 && b[6] != 0)
                {
                    tX = 6;
                    tY = 4;
                }
                else if (b[6] == 0 && b[1] == id && b[0] == id && b[2] == id && b[4] == 0 || b[6] == 0 && b[1] == 0 && b[0] == id && b[2] == id && b[4] == 0)
                {
                    tX = 7;
                    tY = 14;
                }
                else if (b[0] == 0 && b[2] == 0 && b[4] != 0 && b[6] != 0 && b[1] == 0)
                {
                    tX = 3;
                    tY = 3;
                }
                else if (b[6] != 0 && b[7] != 0 && b[0] != 0 && b[2] == 0 && b[4] == 0)
                {
                    tX = 3;
                    tY = 4;
                }
                else if (b[0] != 0 && b[1] != 0 && b[2] != 0 && b[4] == 0 && b[6] == 0)
                {
                    tX = 2;
                    tY = 4;
                }
                else if (b[7] == 0 && b[4] != 0 && b[2] != 0 && b[6] == 0 && b[0] == 0)
                {
                    tX = 2;
                    tY = 3;
                }
                else if (b[0] != 0 && b[4] == 0 && b[6] != 0 && b[2] != 0)
                {
                    tX = 2;
                    tY = 2;
                }
                else if (b[0] == id && b[1] == id && b[2] == id && b[3] == id && b[4] == id && b[5] == id && b[6] == id && b[7] == id
                    || b[0] == id && b[1] == id && b[2] == id && b[3] == 0 && b[4] == id && b[5] == id && b[6] == id && b[7] == 0 ||
                    b[0] == id && b[1] == 0 && b[2] == id && b[3] == id && b[4] == id && b[5] == 0 && b[6] == id && b[7] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] != 0 && b[1] != 0 && b[2] != 0 && b[3] != 0 && b[4] != 0 && b[5] != 0 && b[6] != 0 && b[7] != 0)
                {
                    tX = 1;
                    tY = 1;
                }
                else if (b[0] != 0 && b[2] == 0 && b[4] != 0 && b[6] != 0)
                {
                    tX = 4;
                    tY = 0 + 1;
                }
                else if (b[0] == id && b[4] == id & b[6] == 0 & b[2] == 0)
                {
                    tX = 5;
                    tY = 0;
                }


                else if (b[0] == id && b[1] == 0 && b[2] == id && b[3] == id && b[4] == id && b[5] == id && b[6] == id && b[7] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] == id && b[1] == id && b[2] == id && b[3] == id && b[4] == id && b[5] == id && b[6] == id && b[7] == 0)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] == id && b[1] == id && b[2] == id && b[3] == id && b[4] == id && b[5] == 0 && b[6] == id && b[7] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] == id && b[1] == id && b[2] == id && b[3] == 0 && b[4] == id && b[5] == id && b[6] == id && b[7] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] != 0 && b[1] == 0 && b[2] != 0 && b[3] != 0 && b[4] != 0 && b[5] != 0 && b[6] != 0 && b[7] != 0)
                {
                    tX = 2;
                    tY = 6;
                }
                else if (b[0] != 0 && b[1] != 0 && b[2] != 0 && b[3] != 0 && b[4] != 0 && b[5] != 0 && b[6] != 0 && b[7] == 0)
                {
                    tX = 3;
                    tY = 6;
                }
                else if (b[0] != 0 && b[1] != 0 && b[2] != 0 && b[3] != 0 && b[4] != 0 && b[5] == 0 && b[6] != 0 && b[7] != 0)
                {
                    tX = 3;
                    tY = 5;
                }
                else if (b[0] != 0 && b[1] != 0 && b[2] != 0 && b[3] == 0 && b[4] != 0 && b[5] != 0 && b[6] != 0 && b[7] != 0)
                {
                    tX = 2;
                    tY = 5;
                }
                else if (b[0] == id && b[1] == 0 && b[2] != 0 && b[3] == 0 && b[4] != 0 && b[5] == 0 && b[6] != 0 && b[7] == 0)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] == id && b[2] == 0 && b[2] == 0 && b[4] == 0 && b[6] == 0)
                {
                    tX = 6;
                    tY = 3;
                }
                else if (b[0] == 0 && b[2] == 0 && b[4] == id && b[6] == 0)
                {
                    tX = 6;
                    tY = 0;
                }
                else if (b[6] == id && b[7] == 0 && b[0] == id && b[2] == 0 && b[4] == 0)
                {
                    tX = 8;
                    tY = 16;
                }
                else if (b[0] == id && b[1] == 0 && b[2] == id && b[3] == 0 && b[4] == id && b[5] == id && b[6] == id && b[7] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[6] == id && b[5] == 0 && b[7] == 0 && b[0] == id && b[4] == id && b[6] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] == id && b[6] == id && b[4] == id && b[2] == id && b[7] == 0 && b[1] == 0)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[4] == id && b[6] == id && b[2] == id && b[5] == 0 && b[3] == 0 && b[0] == id)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[0] == id && b[1] == 0 && b[2] == id && b[3] == 0 && b[4] == id && b[5] != 0 && b[6] != 0 && b[7] != 0)
                {
                    tX = 11;
                    tY = 0;
                }
                else if (b[6] == id && b[5] == 0 && b[7] == 0 && b[0] == id && b[4] == id && b[6] != 0)
                {
                    tX = 10;
                    tY = 1;
                }
                else if (b[0] == id && b[6] == id && b[4] != 0 && b[2] == id && b[7] == 0 && b[1] == 0)
                {
                    tX = 6;
                    tY = 1;
                }
                else if (b[4] == id && b[6] == id && b[2] == id && b[5] == 0 && b[3] == 0 && b[0] != 0)
                {
                    tX = 6;
                    tY = 2;
                }
                else if (b[0] == 0 && b[2] == 0 && b[4] == 0 && b[6] == 0)
                {
                    tX = 9;
                    tY = 3;
                }
                else if (b[6] != 0 && b[0] != 0 && b[4] != 0 && b[2] != 0)
                {
                    tX = 8;
                    tY = 13;
                }
                else if (b[6] != id && b[0] == 0 && b[2] == 0 && b[4] == 0)
                {
                    tX = 0;
                    tY = 13;
                }
                else if (b[6] == 0 && b[4] == 0 && b[0] == 0 && b[2] != id)
                {
                    tX = 3;
                    tY = 13;
                }
                else if (b[4] == 0 && b[6] == 0 && b[2] == 0 && b[0] != id)
                {
                    tX = 6;
                    tY = 8;
                }
                else if (b[0] == 0 && b[6] == 0 && b[4] != id && b[2] == 0)
                {
                    tX = 6;
                    tY = 5;
                }
                float k = (sizeBlock * 1.00f) / (16 * 1.00f);

                tX = (int)(tX * (sizeBlock + 2 * k));
                tY = (int)(tY * (sizeBlock + 2 * k));

                textureRect = new FloatRect(tX, tY, sizeBlock, sizeBlock);
                if (backTexture != null)
                {
                    backTextureRect = new FloatRect(tX, tY, sizeBlock, sizeBlock);
                }
                if (rect != null)
                    rect.TextureRect = new IntRect(tX, tY, sizeBlock, sizeBlock);
                if (backgroundRect != null)
                    backgroundRect.TextureRect = new IntRect(tX, tY, sizeBlock, sizeBlock);
            }
        }

        public abstract Block getNewBlock();

        public Block createBlock(int pX, int pY, bool isBackBlock, World world)
        {
            Block block = null;
            block = getNewBlock();
            block.pX = pX;
            block.pY = pY;
            block.isBackBlock = isBackBlock;
            block.world = world;
            block.id = id;
            return block;
        }

        public virtual void update()
        {

        }

        public virtual void start()
        {

        }

        protected Block addProperty(int value, int sizeProperty, string nameProperty)
        {
            propertyManager.addProperty(value, sizeProperty, nameProperty);
            return this;
        }

        public int getValueProperty(string nameProperty)
        {
            return propertyManager.getValueProperty(nameProperty);
        }

        public void setValueProperty(string nameProperty, int value)
        {
            propertyManager.setValueProperty(nameProperty, value);
        }

        //public BlockState getBlockState(int pX, int pY, bool isBackBlock, World world)
        //{
        //    InitBlockState init = new InitBlockState(this, pX, pY, isBackBlock, world);
        //    BlockState state = init as BlockState;
        //    return state;
        //}

    }
}
