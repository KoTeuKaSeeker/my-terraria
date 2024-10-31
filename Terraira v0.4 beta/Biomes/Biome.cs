using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;
using Terraira_v0._4_beta.WorldGenerationForder;

namespace Terraira_v0._4_beta.Biomes
{
    abstract class Biome
    {
        public World world;
        public RandomCoor rand;

        public Color[] biomeColor;

        //public int offsetHigh = 0;

        public int id;

        public int offsetX = Chunk.sizeChunk;
        public int offsetY = Chunk.sizeChunk;
        public float offsetPosX = 0;
        public float offsetPosY = 0;
        public float sizeNoiseX = 1;
        public float sizeNoiseY = 1;
        public Color backgroundColor = new Color(255, 255, 255);
        public float brightnessK = 1;

        public float high;
        public float highSection;

        public float limitUp;
        public float limitDown;


        public int offsetXLandshaft = Chunk.sizeChunk;
        public int offsetYLandshaft = Chunk.sizeChunk;
        public float sizeNoiseXLandshaft = 1;
        public float sizeNoiseYLandshaft = 1;

        public float highLandshaft = 5;

        public static int biomeOffsetLimited = 0;
        public int offsetSmoothBorder = 10;

        public float offsetPosXAddition = 3452;
        public float offsetPosYAddition = 363;
        public float sizeNoiseXAddition = 1;
        public float sizeNoiseYAddition = 1;

        public float highAddition = 1;
        public float highSectionAddition;

        public List<BlockSpawner> spawnBlock = new List<BlockSpawner>();
        public List<BlockSpawner> spawnBackBlock = new List<BlockSpawner>();

        public Biome(int offsetX, int offsetY, float offsetPosX, float offsetPosY, float sizeNoiseX, float sizeNoiseY, float high, float highSection, float limitUp, float limitDown)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.offsetPosX = offsetPosX;
            this.offsetPosY = offsetPosY;
            this.sizeNoiseX = sizeNoiseX;
            this.sizeNoiseY = sizeNoiseY;
            this.high = high;
            this.highSection = highSection;
            this.limitUp = limitUp;
            this.limitDown = limitDown;

        }

        public void setLandshaft(int offsetXLandshaft, int offsetYLandshaft, float sizeNoiseXLandshaft, float sizeNoiseYLandshaft, float highLandshaft)
        {
            this.offsetXLandshaft = offsetXLandshaft;
            this.offsetYLandshaft = offsetYLandshaft;
            this.sizeNoiseXLandshaft = sizeNoiseXLandshaft;
            this.sizeNoiseYLandshaft = sizeNoiseYLandshaft;
            this.highLandshaft = highLandshaft;
        }

        public abstract Biome getNewBiome();

        public Biome createBiome(World world)
        {
            Biome biome = null;
            biome = getNewBiome();
            biome.id = id;
            biome.world = world;
            biome.rand = world.rand;
            return biome;
        }

        public bool toSpawnBiome(int posX, int posY, RandomCoor rand)
        {

            float answer = PerlinNoise.getPerlinNoise((posX + offsetPosX) / (sizeNoiseX / 5), (posY + offsetPosY) / (sizeNoiseY / 5), (int)offsetX, (int)offsetY, rand);
            answer *= high;
            bool toSpawn = false;

            if (posY < limitDown - biomeOffsetLimited && posY > limitUp + biomeOffsetLimited)
            {
                if (answer > highSection)
                {
                    toSpawn = true;
                }
            }
            else
            {
                float t = 0;
                if (posY >= limitDown - biomeOffsetLimited)
                {
                    t = (biomeOffsetLimited - (limitDown - posY)) / biomeOffsetLimited;
                }
                else if (posY <= limitUp + biomeOffsetLimited)
                {
                    t = (biomeOffsetLimited - (posY - limitUp)) / biomeOffsetLimited;
                }
                else
                {
                    t = 1;
                }

                if (t < 0) t = 0;
                if (t > 1) t = 1;

                float section = highSection + (offsetY * BlockSpawner.kLimit * high * sizeNoiseY - highSection) * t;

                if (answer > section)
                {
                    toSpawn = true;
                }
            }

            return toSpawn;
        }

        //public bool toCreateBlockInMaxHigh(int pX, int pY)
        //{
        //    bool toCreate = false;
        //    float answer = PerlinNoise.getPerlinNoise((offsetPosX + pX) / sizeNoiseX, offsetPosY, offsetX, offsetY, rand);

        //    if(pY >= (answer + world.highWorld + offsetHigh) * sizeNoiseY)
        //    {
        //        toCreate = true;
        //    }

        //    return toCreate;        
        //}

        public void addSpawnBlock(Block block, float offsetX, float offsetY, float offsetPosX, float offsetPosY, float high, float highSection, float limitUp, float limitDown, float sizeNoise)
        {
            BlockSpawner blockSpawner = new BlockSpawner(block, offsetX, offsetY, offsetPosX, offsetPosY, high, highSection, limitUp, limitDown, sizeNoise);
            spawnBlock.Add(blockSpawner);
        }

        public void addSpawnBackBlock(Block block, float offsetX, float offsetY, float offsetPosX, float offsetPosY, float high, float highSection, float limitUp, float limitDown, float sizeNoise)
        {
            BlockSpawner blockSpawner = new BlockSpawner(block, offsetX, offsetY, offsetPosX, offsetPosY, high, highSection, limitUp, limitDown, sizeNoise);
            spawnBackBlock.Add(blockSpawner);
        }

        public void addSpawnBlock(Block block, float offsetX, float offsetY, float offsetPosX, float offsetPosY, float high, float highSection, float sizeNoise)
        {
            BlockSpawner blockSpawner = new BlockSpawner(block, offsetX, offsetY, offsetPosX, offsetPosY, high, highSection, -100000000, 100000000, sizeNoise);
            spawnBlock.Add(blockSpawner);
        }

        public void addSpawnBackBlock(Block block, float offsetX, float offsetY, float offsetPosX, float offsetPosY, float high, float highSection, float sizeNoise)
        {
            BlockSpawner blockSpawner = new BlockSpawner(block, offsetX, offsetY, offsetPosX, offsetPosY, high, highSection, -100000000, 100000000, sizeNoise);
            spawnBackBlock.Add(blockSpawner);
        }

        public Block getBiomeBlock(int blockX, int blockY)
        {
            Block generateBlock = null;
            for (int z = 0; z < spawnBlock.Count; z++)
            {
                bool toCreate = spawnBlock[z].toCreateBlock(blockX, blockY, rand);
                if (toCreate)
                {
                    generateBlock = spawnBlock[z].block;
                }
            }
            if (generateBlock == null)
            {
                generateBlock = RegisteryBlocks.blockAir;
            }
            return generateBlock;
        }

        public Block getBiomeBackBlock(int blockX, int blockY)
        {
            Block generateBlock = null;
            for (int z = 0; z < spawnBackBlock.Count; z++)
            {
                bool toCreate = spawnBlock[z].toCreateBlock(blockX, blockY, rand);
                if (toCreate)
                {
                    generateBlock = spawnBackBlock[z].block;
                }
            }
            if (generateBlock == null)
            {
                generateBlock = RegisteryBlocks.blockAir;
            }
            return generateBlock;
        }

        public Block getBiomeBlock(int blockX, int blockY, float tSection)
        {
            Block generateBlock = null;
            for (int z = 0; z < spawnBlock.Count; z++)
            {
                bool toCreate = spawnBlock[z].toCreateBlock(blockX, blockY, tSection, rand);
                if (toCreate)
                {
                    generateBlock = spawnBlock[z].block;
                }
            }
            if (generateBlock == null)
            {
                generateBlock = RegisteryBlocks.blockAir;
            }

            return generateBlock;
        }

        public bool needCreateBiomeBlock(int blockX, int blockY, float tSection)
        {
            Block generateBlock = null;
            bool toCreateBiomeBlock = true;

            for (int z = 0; z < spawnBlock.Count; z++)
            {
                bool toCreate = spawnBlock[z].toCreateBlock(blockX, blockY, tSection, rand);
                if (toCreate)
                {
                    generateBlock = spawnBlock[z].block;
                }
            }
            if (generateBlock == null)
            {
                toCreateBiomeBlock = false;
            }
            return toCreateBiomeBlock;
        }

        private float lerp(float t, float a, float b)
        {
            return a + (b - a) * t;
        }

        public Block getBiomeBlockWithTransition(int blockX, int blockY, float tSection, bool isBackBlock, Biome biome)
        {

            Block generateBlock = null;

            bool isHaveSpawner = false;
            BlockSpawner findSpawner = null;
            BlockSpawner selfSpawner = null;
            if (!isBackBlock)
            {

                int idBlockSpawner = 0;
                for (int z = 0; z < spawnBlock.Count; z++)
                {
                    bool toCreate = spawnBlock[z].toCreateBlock(blockX, blockY, rand);
                    if (toCreate)
                    {
                        idBlockSpawner = z;
                    }
                }

                BlockSpawner spawner = spawnBlock[idBlockSpawner];
                selfSpawner = spawner;
                for (int x = 0; x < biome.spawnBlock.Count; x++)
                {
                    if (biome.spawnBlock[x].block == spawner.block)
                    {
                        isHaveSpawner = true;
                        findSpawner = biome.spawnBlock[x];
                        break;
                    }
                }
            }
            else
            {
                if (spawnBackBlock.Count > 0)
                {
                    int idBlockSpawner = 0;
                    for (int z = 0; z < spawnBackBlock.Count; z++)
                    {
                        bool toCreate = spawnBackBlock[z].toCreateBlock(blockX, blockY, rand);
                        if (toCreate)
                        {
                            idBlockSpawner = z;
                        }
                    }

                    BlockSpawner spawner = spawnBackBlock[idBlockSpawner];
                    selfSpawner = spawner;
                    for (int x = 0; x < biome.spawnBackBlock.Count; x++)
                    {
                        if (biome.spawnBackBlock[x].block == spawner.block)
                        {
                            isHaveSpawner = true;
                            findSpawner = biome.spawnBackBlock[x];
                            break;
                        }
                    }
                }
            }

            if (isHaveSpawner)
            {
                BlockSpawner transitionSpawner = new BlockSpawner(findSpawner.block, lerp(tSection, selfSpawner.offsetX, findSpawner.offsetX), lerp(tSection, selfSpawner.offsetY, findSpawner.offsetY),
                    lerp(tSection, selfSpawner.offsetPosX, findSpawner.offsetPosX), lerp(tSection, selfSpawner.offsetPosY, findSpawner.offsetPosY), lerp(tSection, selfSpawner.high, findSpawner.high),
                    lerp(tSection, selfSpawner.highSection, findSpawner.highSection), lerp(tSection, selfSpawner.limitUp, findSpawner.limitUp), lerp(tSection, selfSpawner.limitDown, findSpawner.limitDown),
                    lerp(tSection, selfSpawner.sizeNoise, findSpawner.sizeNoise));
                bool toCreateBlock = transitionSpawner.toCreateBlock(blockX, blockY, rand);
                if (toCreateBlock)
                {
                    generateBlock = transitionSpawner.block;
                }
                else
                {
                    generateBlock = RegisteryBlocks.createBlock(blockX, blockY, isBackBlock, world, blockName.air);
                }
            }
            else
            {
                if (!isBackBlock)
                {
                    generateBlock = getBiomeBlock(blockX, blockY, tSection);
                }
                else
                {
                    generateBlock = getBiomeBackBlock(blockX, blockY, tSection);
                }
            }


            return generateBlock;
        }

        public Block getBiomeBackBlock(int blockX, int blockY, float tSection)
        {
            Block generateBlock = null;
            for (int z = 0; z < spawnBackBlock.Count; z++)
            {
                bool toCreate = spawnBackBlock[z].toCreateBlock(blockX, blockY, tSection, rand);
                if (toCreate)
                {
                    generateBlock = spawnBackBlock[z].block;
                }
            }
            if (generateBlock == null)
            {
                generateBlock = RegisteryBlocks.blockAir;
            }
            return generateBlock;
        }

        public void setBorderChunk(Chunk chunk)
        {
            int chunkX = chunk.chunkX;
            int chunkY = chunk.chunkY;
            Biome biomeUp = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX, chunkY - 1));
            Biome biomeUpLeft = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX - 1, chunkY - 1));
            Biome biomeUpRight = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX + 1, chunkY - 1));

            Biome biomeRight = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX + 1, chunkY));

            Biome biomeDown = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX, chunkY + 1));
            Biome biomeDownLeft = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX - 1, chunkY + 1));
            Biome biomeDownRight = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX + 1, chunkY + 1));

            Biome biomeLeft = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX - 1, chunkY));

            float t = 0;
            Block localBlock = null;
            if (biomeUp.id > id)
            {
                int offsetSmoothBorder = biomeUp.offsetSmoothBorder;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                for (int x = 0; x < Chunk.sizeChunk; x++)
                    for (int y = 0; y < offsetSmoothBorder; y++)
                    {
                        t = y / (offsetSmoothBorder * 1.00f);
                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);
                        localBlock = biomeUp.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (localBlock.id != 0 && biomeUp.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y))
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeUp.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);

                        if (localBlock.id != 0 && biomeUp.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y))
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }


            if (biomeUpLeft.id > id)
            {
                offsetSmoothBorder = biomeUpLeft.offsetSmoothBorder;
                offsetSmoothBorder = offsetSmoothBorder / 2;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;
                for (int x = 0; x < offsetSmoothBorder; x++)
                    for (int y = 0; y < offsetSmoothBorder; y++)
                    {
                        float dY = y;
                        float dX = x;

                        float maxT = offsetSmoothBorder * 2;
                        t = (dX + dY) / maxT;

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);

                        localBlock = biomeUpLeft.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (biomeUpLeft.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeUpLeft.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);

                        if (biomeUpLeft.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }

            if (biomeUpRight.id > id)
            {
                offsetSmoothBorder = biomeUpRight.offsetSmoothBorder;
                offsetSmoothBorder = offsetSmoothBorder / 2;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;
                for (int x = Chunk.sizeChunk - offsetSmoothBorder; x < Chunk.sizeChunk; x++)
                    for (int y = 0; y < offsetSmoothBorder; y++)
                    {
                        float dY = y;
                        float dX = offsetSmoothBorder - (x - (Chunk.sizeChunk - offsetSmoothBorder));

                        float maxT = offsetSmoothBorder * 2;
                        t = (dX + dY) / maxT;

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);

                        localBlock = biomeUpRight.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (biomeUpRight.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeUpRight.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);
                        if (biomeUpRight.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }


            if (biomeRight.id > id)
            {
                offsetSmoothBorder = biomeRight.offsetSmoothBorder;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;

                for (int x = Chunk.sizeChunk - offsetSmoothBorder; x < Chunk.sizeChunk; x++)
                    for (int y = 0; y < Chunk.sizeChunk; y++)
                    {
                        t = (offsetSmoothBorder - (x - (Chunk.sizeChunk - offsetSmoothBorder))) / (offsetSmoothBorder * 1.00f);

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);
                        localBlock = biomeRight.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);

                        if (biomeRight.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeRight.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);
                        if (biomeRight.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }

            if (biomeDown.id > id)
            {
                offsetSmoothBorder = biomeDown.offsetSmoothBorder;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;
                for (int x = 0; x < Chunk.sizeChunk; x++)
                    for (int y = Chunk.sizeChunk - offsetSmoothBorder; y < Chunk.sizeChunk; y++)
                    {
                        t = ((offsetSmoothBorder - (y - (Chunk.sizeChunk - offsetSmoothBorder))) / (offsetSmoothBorder * 1.00f));

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);

                        localBlock = biomeDown.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (biomeDown.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;
                        localBlock = biomeDown.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);

                        if (biomeDown.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }

            if (biomeDownLeft.id > id)
            {
                offsetSmoothBorder = biomeDownLeft.offsetSmoothBorder;
                offsetSmoothBorder = offsetSmoothBorder / 2;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;
                for (int x = 0; x < offsetSmoothBorder; x++)
                    for (int y = Chunk.sizeChunk - offsetSmoothBorder; y < Chunk.sizeChunk; y++)
                    {
                        float dY = offsetSmoothBorder - (y - (Chunk.sizeChunk - offsetSmoothBorder));
                        float dX = x;

                        float maxT = offsetSmoothBorder * 2;
                        t = (dX + dY) / maxT;

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);

                        localBlock = biomeDownLeft.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (biomeDownLeft.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeDownLeft.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);
                        if (biomeDownLeft.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }


            if (biomeDownRight.id > id)
            {
                offsetSmoothBorder = biomeDownRight.offsetSmoothBorder;
                offsetSmoothBorder = offsetSmoothBorder / 2;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;
                for (int x = Chunk.sizeChunk - offsetSmoothBorder; x < Chunk.sizeChunk; x++)
                    for (int y = Chunk.sizeChunk - offsetSmoothBorder; y < Chunk.sizeChunk; y++)
                    {
                        float dY = offsetSmoothBorder - (y - (Chunk.sizeChunk - offsetSmoothBorder));
                        float dX = offsetSmoothBorder - (x - (Chunk.sizeChunk - offsetSmoothBorder));

                        float maxT = offsetSmoothBorder * 2;
                        t = (dX + dY) / maxT;

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);

                        localBlock = biomeDownRight.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (biomeDownRight.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeDownRight.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);
                        if (biomeDownRight.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }


            if (biomeLeft.id > id)
            {
                offsetSmoothBorder = biomeLeft.offsetSmoothBorder;
                if (offsetSmoothBorder > Chunk.sizeChunk - 1) offsetSmoothBorder = Chunk.sizeChunk - 1;
                t = 0;
                for (int x = 0; x < offsetSmoothBorder; x++)
                    for (int y = 0; y < Chunk.sizeChunk; y++)
                    {
                        t = x / (offsetSmoothBorder * 1.00f);

                        bool toCreateBiomeBlock = needCreateBiomeBlock(x, y, t);

                        localBlock = biomeLeft.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, false, this);
                        if (biomeLeft.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.block[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world);
                        }

                        Block realBlock = localBlock;

                        localBlock = biomeLeft.getBiomeBlockWithTransition(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, t, true, this);
                        if (biomeLeft.toCreateBlockInLandshaft(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y) && localBlock.id != 0)
                        {
                            chunk.backBlock[x, y] = localBlock.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world);
                        }
                    }
            }

        }

        public bool toCreateBlockInLandshaft(int blockX, int blockY)
        {
            bool toCreate = false;

            float maxHighMountains = offsetYLandshaft * highLandshaft * sizeNoiseYLandshaft * 0.2f;
            float answer = PerlinNoise.getPerlinNoise(blockX + offsetPosX * Chunk.sizeChunk, offsetPosY * Chunk.sizeChunk, offsetXLandshaft, offsetYLandshaft, rand) * highLandshaft;
            answer = maxHighMountains - answer;
            float answerDown = PerlinNoise.getPerlinNoise(blockX + offsetPosX * Chunk.sizeChunk + 32451, offsetPosY * Chunk.sizeChunk + 2346, offsetXLandshaft, offsetYLandshaft, rand) * highLandshaft;
            answer = maxHighMountains - answer;

            int highPoint = (int)(limitUp * Chunk.sizeChunk + maxHighMountains);
            int lowPoint = (int)(limitDown * Chunk.sizeChunk - maxHighMountains);

            if (blockY > answer + highPoint && blockY < lowPoint - answer)
            {
                toCreate = true;
            }
            return toCreate;
        }

        public void setLandshaftChunk(Chunk chunk)
        {
            int chunkX = chunk.chunkX;
            int chunkY = chunk.chunkY;

            for (int x = 0; x < Chunk.sizeChunk; x++)
                for (int y = 0; y < Chunk.sizeChunk; y++)
                {
                    int blockX = chunkX * Chunk.sizeChunk + x;
                    int blockY = chunkY * Chunk.sizeChunk + y;

                    if (!toCreateBlockInLandshaft(blockX, blockY))
                    {
                        chunk.block[x, y] = RegisteryBlocks.createBlock(blockX, blockY, false, world, blockName.air);
                        chunk.backBlock[x, y] = RegisteryBlocks.createBlock(blockX, blockY, false, world, blockName.air);
                    }
                }
        }

        public void setAdditionChunk(Chunk chunk)
        {
            int chunkX = chunk.chunkX;
            int chunkY = chunk.chunkY;
            Biome biomeUp = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX, chunkY - 1));
            Biome biomeRight = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX + 1, chunkY));
            Biome biomeDown = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX, chunkY + 1));
            Biome biomeLeft = RegisteryBiome.createBiome(world, world.getBiomeId(chunkX - 1, chunkY));


            //Biome[] biomeOlder = new Biome[5];

            //biomeOlder[0] = biomeUp;
            //biomeOlder[1] = biomeRight;
            //biomeOlder[2] = biomeDown;
            //biomeOlder[3] = biomeLeft;
            //biomeOlder[4] = this;

            //int pointer = 1;

            //for (int x = 0; x < biomeOlder.Length - pointer; x++)
            //{
            //    for (int y = 0; y < biomeOlder.Length - pointer; y++)
            //    {
            //        if (biomeOlder[y].id > biomeOlder[y + 1].id)
            //        {
            //            Biome lastBiome = biomeOlder[y + 1];
            //            biomeOlder[y + 1] = biomeOlder[y];
            //            biomeOlder[y] = lastBiome;
            //        }
            //    }
            //    pointer++;
            //}

            IBiomeAddition addSelf = this as IBiomeAddition;

            Block[,] localBlock = null;
            Block[,] localBackBlock = null;

            if (addSelf != null)
                addSelf.editChunk(chunk, chunk.block, chunk.backBlock);
            float t = 0;


            IBiomeAddition addUp = biomeUp as IBiomeAddition;
            if (addUp != null)
            {
                if (biomeUp.id > id)
                {
                    localBlock = new Block[Chunk.sizeChunk, biomeUp.offsetSmoothBorder];
                    localBackBlock = new Block[Chunk.sizeChunk, biomeUp.offsetSmoothBorder];

                    for (int x = 0; x < Chunk.sizeChunk; x++)
                        for (int y = 0; y < biomeUp.offsetSmoothBorder; y++)
                        {
                            localBlock[x, y] = chunk.block[x, y].createBlock(Chunk.sizeChunk * chunkX + x, Chunk.sizeChunk * chunkY + y, false, chunk.world);
                            localBackBlock[x, y] = chunk.backBlock[x, y].createBlock(Chunk.sizeChunk * chunkX + x, Chunk.sizeChunk * chunkY + y, true, chunk.world);
                        }

                    addUp.editChunk(chunk, localBlock, localBackBlock);

                    if (addSelf != null)
                    {

                        for (int x = 0; x < Chunk.sizeChunk; x++)
                            for (int y = 0; y < biomeUp.offsetSmoothBorder; y++)
                            {
                                t = 1 - (y / biomeUp.offsetSmoothBorder);
                                if (rand.next((int)(Chunk.sizeChunk * chunkX + x + biomeUp.offsetPosX), (int)(Chunk.sizeChunk * chunkY + y + biomeUp.offsetPosY), 0, 100) < t * 100)
                                {
                                    if (localBlock[x, y].id != 0)
                                    {
                                        chunk.block[x, y] = localBlock[x, y];
                                    }
                                    if (localBackBlock[x, y].id != 0)
                                    {
                                        chunk.backBlock[x, y] = localBackBlock[x, y];
                                    }
                                }
                            }
                    }
                    else
                    {
                        for (int x = 0; x < Chunk.sizeChunk; x++)
                            for (int y = 0; y < biomeUp.offsetSmoothBorder; y++)
                            {

                                if (localBlock[x, y].id != 0)
                                {
                                    chunk.block[x, y] = localBlock[x, y];
                                }
                                if (localBackBlock[x, y].id != 0)
                                {
                                    chunk.backBlock[x, y] = localBackBlock[x, y];
                                }
                            }
                    }
                }
            }


            IBiomeAddition addRight = biomeRight as IBiomeAddition;

            if(addRight != null)
            {
                if(biomeRight.id > id)
                {
                    localBlock = new Block[biomeRight.offsetSmoothBorder, Chunk.sizeChunk];
                    localBackBlock = new Block[biomeRight.offsetSmoothBorder, Chunk.sizeChunk];

                    for(int x = 0; x < biomeRight.offsetSmoothBorder; x++)
                        for(int y = 0; y < Chunk.sizeChunk; y++)
                        {
                            int localX = Chunk.sizeChunk - biomeRight.offsetSmoothBorder + x;
                            int localY = y;
                            localBlock[x, y] = chunk.block[localX, localY].createBlock(chunk.block[localX, localY].pX, chunk.block[localX, localY].pY, false, chunk.world);
                            localBackBlock[x, y] = chunk.backBlock[localX, localY].createBlock(chunk.backBlock[localX, localY].pX, chunk.backBlock[localX, localY].pY, true, chunk.world);
                        }
                    addRight.editChunk(chunk, localBlock, localBackBlock);
                    if (addSelf != null)
                    {
                        for (int x = 0; x < biomeRight.offsetSmoothBorder; x++)
                            for (int y = 0; y < Chunk.sizeChunk; y++)
                            {
                                int localX = Chunk.sizeChunk - biomeRight.offsetSmoothBorder + x;
                                int localY = y;
                                t = 1 - (x / (Chunk.sizeChunk - biomeRight.offsetSmoothBorder));
                                if(rand.next((int)(Chunk.sizeChunk * chunkX + x + biomeRight.offsetPosX), (int)(Chunk.sizeChunk * chunkY + y + biomeRight.offsetPosY), 0, 100) < t * 100)
                                {
                                    if (localBlock[x, y].id != 0)
                                    {
                                        chunk.block[localX, localY] = localBlock[x, y];
                                    }
                                    if (localBackBlock[x, y].id != 0)
                                    {
                                        chunk.backBlock[localX, localY] = localBackBlock[x, y];
                                    }
                                }
                            }
                    }
                    else
                    {
                        for (int x = 0; x < biomeRight.offsetSmoothBorder; x++)
                            for (int y = 0; y < Chunk.sizeChunk; y++)
                            {
                                int localX = Chunk.sizeChunk - biomeRight.offsetSmoothBorder + x;
                                int localY = y;
                                if (localBlock[x, y].id != 0)
                                {
                                    chunk.block[localX, localY] = localBlock[x, y];
                                }
                                if (localBackBlock[x, y].id != 0)
                                {
                                    chunk.backBlock[localX, localY] = localBackBlock[x, y];
                                }
                            }
                    }
                }
            }


            IBiomeAddition addDown = biomeDown as IBiomeAddition;

            if (addDown != null)
            {
                if (biomeDown.id > id)
                {
                    localBlock = new Block[Chunk.sizeChunk, biomeDown.offsetSmoothBorder];
                    localBackBlock = new Block[Chunk.sizeChunk, biomeDown.offsetSmoothBorder];

                    for (int x = 0; x < Chunk.sizeChunk; x++)
                        for (int y = 0; y < biomeDown.offsetSmoothBorder; y++)
                        {
                            int localX = x;
                            int localY = Chunk.sizeChunk - biomeDown.offsetSmoothBorder + y;
                            localBlock[x, y] = chunk.block[localX, localY].createBlock(chunk.block[localX, localY].pX, chunk.block[localX, localY].pY, false, chunk.world);
                            localBackBlock[x, y] = chunk.backBlock[localX, localY].createBlock(chunk.backBlock[localX, localY].pX, chunk.backBlock[localX, localY].pY, true, chunk.world);
                        }
                    addDown.editChunk(chunk, localBlock, localBackBlock);
                    if (addSelf != null)
                    {
                        for (int x = 0; x < Chunk.sizeChunk; x++)
                            for (int y = 0; y < biomeDown.offsetSmoothBorder; y++)
                            {
                                int localX = x;
                                int localY = Chunk.sizeChunk - biomeDown.offsetSmoothBorder + y;
                                t = x / biomeDown.offsetSmoothBorder;
                                if (rand.next((int)(Chunk.sizeChunk * chunkX + x + biomeDown.offsetPosX), (int)(Chunk.sizeChunk * chunkY + y + biomeDown.offsetPosY), 0, 100) < t * 100)
                                {
                                    if (localBlock[x, y].id != 0)
                                    {
                                        chunk.block[localX, localY] = localBlock[x, y];
                                    }
                                    if (localBackBlock[x, y].id != 0)
                                    {
                                        chunk.backBlock[localX, localY] = localBackBlock[x, y];
                                    }
                                }
                            }
                    }
                    else
                    {
                        for (int x = 0; x < Chunk.sizeChunk; x++)
                            for (int y = 0; y < biomeDown.offsetSmoothBorder; y++)
                            {
                                int localX = x;
                                int localY = Chunk.sizeChunk - biomeDown.offsetSmoothBorder + y;
                                if (localBlock[x, y].id != 0)
                                {
                                    chunk.block[localX, localY] = localBlock[x, y];
                                }
                                if (localBackBlock[x, y].id != 0)
                                {
                                    chunk.backBlock[localX, localY] = localBackBlock[x, y];
                                }
                            }
                    }
                }
            }

            IBiomeAddition addLeft = biomeLeft as IBiomeAddition;

            if (addLeft != null)
            {
                if (biomeLeft.id > id)
                {
                    localBlock = new Block[biomeLeft.offsetSmoothBorder, Chunk.sizeChunk];
                    localBackBlock = new Block[biomeLeft.offsetSmoothBorder, Chunk.sizeChunk];

                    for (int x = 0; x < biomeLeft.offsetSmoothBorder; x++)
                        for (int y = 0; y < Chunk.sizeChunk; y++)
                        {
                            int localX = x;
                            int localY = y;
                            localBlock[x, y] = chunk.block[localX, localY].createBlock(chunk.block[localX, localY].pX, chunk.block[localX, localY].pY, false, chunk.world);
                            localBackBlock[x, y] = chunk.backBlock[localX, localY].createBlock(chunk.backBlock[localX, localY].pX, chunk.backBlock[localX, localY].pY, true, chunk.world);
                        }
                    addLeft.editChunk(chunk, localBlock, localBackBlock);
                    if (addSelf != null)
                    {
                        for (int x = 0; x < biomeLeft.offsetSmoothBorder; x++)
                            for (int y = 0; y < Chunk.sizeChunk; y++)
                            {
                                int localX = x;
                                int localY = y;
                                t = x / (Chunk.sizeChunk - biomeLeft.offsetSmoothBorder);
                                if (rand.next((int)(Chunk.sizeChunk * chunkX + x + biomeLeft.offsetPosX), (int)(Chunk.sizeChunk * chunkY + y + biomeLeft.offsetPosY), 0, 100) < t * 100)
                                {
                                    if (localBlock[x, y].id != 0)
                                    {
                                        chunk.block[localX, localY] = localBlock[x, y];
                                    }
                                    if (localBackBlock[x, y].id != 0)
                                    {
                                        chunk.backBlock[localX, localY] = localBackBlock[x, y];
                                    }
                                }
                            }
                    }
                    else
                    {
                        for (int x = 0; x < biomeLeft.offsetSmoothBorder; x++)
                            for (int y = 0; y < Chunk.sizeChunk; y++)
                            {
                                int localX = x;
                                int localY = y;
                                if (localBlock[x, y].id != 0)
                                {
                                    chunk.block[localX, localY] = localBlock[x, y];
                                }
                                if (localBackBlock[x, y].id != 0)
                                {
                                    chunk.backBlock[localX, localY] = localBackBlock[x, y];
                                }
                            }
                    }
                }
            }

            //else
            //{
            //    chunkBlock = new Block[Chunk.sizeChunk, biomeUp.offsetSmoothBorder];
            //    chunkBackBlock = new Block[Chunk.sizeChunk, biomeUp.offsetSmoothBorder];

            //    for (int x = 0; x < Chunk.sizeChunk; x++)
            //        for (int y = 0; y < biomeUp.offsetSmoothBorder; y++)
            //        {
            //            chunkBlock[x, y] = chunk.block[x, y];
            //            chunkBackBlock[x, y] = chunk.backBlock[x, y];
            //        }

            //    addUp.editChunk(chunkBlock, chunkBackBlock);
            //}
        }

        public Chunk generateChunk(int chunkX, int chunkY)
        {
            Chunk chunk = new Chunk(chunkX, chunkY, world);
            chunk.biome1 = id;

            int blockChunkX = chunk.chunkX * Chunk.sizeChunk;
            int blockChunkY = chunk.chunkY * Chunk.sizeChunk;

            Block[,] block = new Block[Chunk.sizeChunk, Chunk.sizeChunk];

            for (int x = 0; x < Chunk.sizeChunk; x++)
                for (int y = 0; y < Chunk.sizeChunk; y++)
                {
                    block[x, y] = getBiomeBlock(blockChunkX + x, blockChunkY + y).createBlock(blockChunkX + x, blockChunkY + y, false, world);
                }

            chunk.block = block;

            Block[,] backBlock = new Block[Chunk.sizeChunk, Chunk.sizeChunk];

            for (int x = 0; x < Chunk.sizeChunk; x++)
                for (int y = 0; y < Chunk.sizeChunk; y++)
                {
                    backBlock[x, y] = getBiomeBackBlock(blockChunkX + x, blockChunkY + y).createBlock(blockChunkX + x, blockChunkY + y, true, world);
                }

            chunk.backBlock = backBlock;

            setLandshaftChunk(chunk);
            setBorderChunk(chunk);
            setAdditionChunk(chunk);

            //IBiomeAddition biomeAddition = this as IBiomeAddition;

            //if (biomeAddition != null)
            //{
            //    for (int x = 1; x < Chunk.sizeChunk - 1; x++)
            //        for (int y = 1; y < Chunk.sizeChunk - 1; y++)
            //        {
            //            Block[] aroundBlock = new Block[9];
            //            aroundBlock[0] = chunk.block[x, y - 1];
            //            aroundBlock[1] = chunk.block[x + 1, y - 1];
            //            aroundBlock[2] = chunk.block[x + 1, y];
            //            aroundBlock[3] = chunk.block[x + 1, y + 1];
            //            aroundBlock[4] = chunk.block[x, y + 1];
            //            aroundBlock[5] = chunk.block[x - 1, y + 1];
            //            aroundBlock[6] = chunk.block[x - 1, y];
            //            aroundBlock[7] = chunk.block[x - 1, y - 1];
            //            aroundBlock[8] = chunk.block[x, y];

            //            //aroundBlock[0] = y - 1 >= 0 ? chunk.block[x, y - 1] : RegisteryBlocks.blockAir;
            //            //aroundBlock[1] = y - 1 >= 0 && x + 1 < Chunk.sizeChunk ? chunk.block[x + 1, y - 1] : RegisteryBlocks.blockAir;
            //            //aroundBlock[2] = x + 1 < Chunk.sizeChunk ? chunk.block[x + 1, y] : RegisteryBlocks.blockAir;
            //            //aroundBlock[3] = x + 1 < Chunk.sizeChunk && y + 1 < Chunk.sizeChunk ? chunk.block[x + 1, y + 1] : RegisteryBlocks.blockAir;
            //            //aroundBlock[4] = y + 1 < Chunk.sizeChunk ? chunk.block[x, y + 1] : RegisteryBlocks.blockAir;
            //            //aroundBlock[5] = x - 1 >= 0 && y + 1 < Chunk.sizeChunk ? chunk.block[x - 1, y + 1] : RegisteryBlocks.blockAir;
            //            //aroundBlock[6] = x - 1 >= 0 ? chunk.block[x - 1, y] : RegisteryBlocks.blockAir;
            //            //aroundBlock[7] = x - 1 >= 0 && y - 1 >= 0 ? chunk.block[x - 1, y - 1] : RegisteryBlocks.blockAir;
            //            //aroundBlock[8] = chunk.block[x, y];

            //            Block localBlock = biomeAddition.editBlock(aroundBlock);
            //            if (localBlock != null)
            //            {
            //                block[x, y] = localBlock.createBlock(blockChunkX + x, blockChunkY + y, false, world);
            //            }
            //        }
            //    chunk.block = block;

            return chunk;
        }


    }

}
