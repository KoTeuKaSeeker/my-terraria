using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Blocks.BlockInterface;
using Terraira_v0._4_beta.ByteHelper;
using Terraira_v0._4_beta.ConverFolder;
using Terraira_v0._4_beta.Entitys;
using Terraira_v0._4_beta.RegisteryFolder;
using Terraira_v0._4_beta.WorldGenerationForder;

namespace Terraira_v0._4_beta.Init
{
    class World : Transformable, Drawable
    {

        public static int countActiveChunks = 60;
        public static int sizeNumberMap = 1000;
        public static PerlinNoise noise;
        public Map map;
        public Chunk[] chunk;
        //public Hashtable chunkTable;
        public int chunkPointer;

        private int k = 0;

        public List<Entity> entity;
        public RectangleShape entityRect;
        public Player player;

        public RandomCoor rand;
        public List<Biome> biome = new List<Biome>();
        public int highWorld = 0;
        public BiomeAir biomeAir;
        public Color worldColor = new Color(255, 255, 255);

        public Vector2f camera;
        public float speedCamera = 25;

        public World(Map map, int seed)
        {
            rand = new RandomCoor(sizeNumberMap, seed);
            biomeAir = new BiomeAir();
            biomeAir.world = this;
            biomeAir.rand = rand;
            this.map = map;
            chunk = new Chunk[countActiveChunks];
            entity = new List<Entity>();

            player = addEntity(0, 0, 0) as Player;
            player.position = new Vector2f(0, 700);
            camera = player.position;

            entityRect = new RectangleShape(new Vector2f(Block.sizeBlock, Block.sizeBlock));
            entityRect.FillColor = Color.Transparent;


        }

        public void addBiome(Biome biome)
        {
            biome.world = this;
            biome.rand = rand;
            this.biome.Add(biome);
        }

        public int getBiomeId(int chunkX, int chunkY)
        {
            Biome generateBiome = biomeAir;
            if (biome.Count > 0)
            {
                for (int x = 0; x < biome.Count; x++)
                {
                    if (biome[x].toSpawnBiome(chunkX, chunkY, rand))
                    {
                        generateBiome = biome[x];
                    }
                }
            }

            return generateBiome.id;
        }

        public int getBiomeIdByBlockPosition(int pX, int pY)
        {

            int chunkX = 0;
            int chunkY = 0;

            if (pX < 0)
            {
                if (Math.Abs(pX * 1.00f / Chunk.sizeChunk) - Math.Abs(pX / Chunk.sizeChunk) > 0)
                {
                    chunkX = pX / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkX = pX / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkX = pX / Chunk.sizeChunk;
            }


            if (pY < 0)
            {
                if (Math.Abs(pY * 1.00f / Chunk.sizeChunk) - Math.Abs(pY / Chunk.sizeChunk) > 0)
                {
                    chunkY = pY / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkY = pY / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkY = pY / Chunk.sizeChunk;
            }

            Biome generateBiome = biomeAir;
            if (biome.Count > 0)
            {
                for (int x = 0; x < biome.Count; x++)
                {
                    if (biome[x].toSpawnBiome(chunkX, chunkY, rand))
                    {
                        generateBiome = biome[x];
                    }
                }
            }

            return generateBiome.id;
        }


        public Chunk generationChunk(int chunkX, int chunkY)
        {
            Chunk chunk = null;
            if (biome.Count > 0)
            {
                chunk = RegisteryBiome.getBiome(this, getBiomeId(chunkX, chunkY)).generateChunk(chunkX, chunkY);
            }
            return chunk;
        }

        public Biome getBiomeWithBorder(int posX, int posY)
        {
            int chunkX = 0;
            int chunkY = 0;

            if (posX < 0)
            {
                if (Math.Abs(posX * 1.00f / Chunk.sizeChunk) - Math.Abs(posX / Chunk.sizeChunk) > 0)
                {
                    chunkX = posX / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkX = posX / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkX = posX / Chunk.sizeChunk;
            }


            if (posY < 0)
            {
                if (Math.Abs(posY * 1.00f / Chunk.sizeChunk) - Math.Abs(posY / Chunk.sizeChunk) > 0)
                {
                    chunkY = posY / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkY = posY / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkY = posY / Chunk.sizeChunk;
            }

            int rpX = posX - chunkX * Chunk.sizeChunk;
            int rpY = posY - chunkY * Chunk.sizeChunk;


            Biome biomeUp = RegisteryBiome.getBiome(this, getBiomeId(chunkX, chunkY - 1));
            Biome biomeRight = RegisteryBiome.getBiome(this, getBiomeId(chunkX + 1, chunkY));
            Biome biomeDown = RegisteryBiome.getBiome(this, getBiomeId(chunkX, chunkY + 1));
            Biome biomeLeft = RegisteryBiome.getBiome(this, getBiomeId(chunkX - 1, chunkY));

            Biome centreBiome = RegisteryBiome.getBiome(this, getBiomeId(chunkX, chunkY));

            int side = 4;

            if (rpY < biomeUp.offsetSmoothBorder)
            {
                if (centreBiome.id < biomeUp.id)
                {
                    side = 0;
                }
            }
            else if (rpX > Chunk.sizeChunk - biomeRight.offsetSmoothBorder)
            {
                if (centreBiome.id < biomeRight.id)
                {
                    side = 1;
                }
            }
            else if (rpY > Chunk.sizeChunk - biomeDown.offsetSmoothBorder)
            {
                if (centreBiome.id < biomeDown.id)
                {
                    side = 2;
                }
            }
            else if (rpX < biomeLeft.offsetSmoothBorder)
            {
                if (centreBiome.id < biomeLeft.id)
                {
                    side = 3;
                }
            }

            Biome biome = centreBiome;

            if (side == 0)
            {
                biome = biomeUp;
            }
            else if (side == 1)
            {
                biome = biomeRight;
            }
            else if (side == 2)
            {
                biome = biomeDown;
            }
            else if (side == 3)
            {
                biome = biomeLeft;
            }

            return biome;
        }



        public Chunk getChunk(int chunkX, int chunkY)
        {

            Chunk localChunk = null;
            bool chunkFinded = false;

            //localChunk = chunkTable[new Vector2f(chunkX, chunkY)] as Chunk;

            for(int x = 0; x < countActiveChunks; x++)
            {
                if (chunk[x] != null)
                {
                    if (chunkX == chunk[x].chunkX && chunkY == chunk[x].chunkY)
                    {
                        localChunk = chunk[x];
                        chunkFinded = true;
                        break;
                    }
                }
            }

            if (chunkFinded == false)
            {        
                Program.chunkThread.addChunkContainer(chunkX, chunkY, this);         
            }

            return localChunk;

        }

        public Chunk getChunkNotGeneration(int chunkX, int chunkY)
        {
            Chunk localChunk = null;

            for (int x = 0; x < countActiveChunks; x++)
            {
                if (chunk[x] != null)
                {
                    if (chunkX == chunk[x].chunkX && chunkY == chunk[x].chunkY)
                    {
                        localChunk = chunk[x];
                        break;
                    }
                }
            }


            return localChunk;
        }

        public Chunk getChunkByBlockPosition(int posX, int posY)
        {
            Chunk localChunk = null;

            int chunkX = 0;
            int chunkY = 0;

            if(posX < 0)
            {
                if(Math.Abs(posX * 1.00f / Chunk.sizeChunk) - Math.Abs(posX / Chunk.sizeChunk) > 0)
                {
                    chunkX = posX / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkX = posX / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkX = posX / Chunk.sizeChunk;
            }


            if (posY < 0)
            {
                if (Math.Abs(posY * 1.00f / Chunk.sizeChunk) - Math.Abs(posY / Chunk.sizeChunk) > 0)
                {
                    chunkY = posY / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkY = posY / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkY = posY / Chunk.sizeChunk;
            }

            localChunk = getChunk(chunkX, chunkY);

            return localChunk;
        }

        public Chunk getChunkByBlockPositionNotGeneration(int posX, int posY)
        {
            Chunk localChunk = null;

            int chunkX = 0;
            int chunkY = 0;

            if (posX < 0)
            {
                if (Math.Abs(posX * 1.00f / Chunk.sizeChunk) - Math.Abs(posX / Chunk.sizeChunk) > 0)
                {
                    chunkX = posX / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkX = posX / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkX = posX / Chunk.sizeChunk;
            }


            if (posY < 0)
            {
                if (Math.Abs(posY * 1.00f / Chunk.sizeChunk) - Math.Abs(posY / Chunk.sizeChunk) > 0)
                {
                    chunkY = posY / Chunk.sizeChunk - 1;
                }
                else
                {
                    chunkY = posY / Chunk.sizeChunk;
                }
            }
            else
            {
                chunkY = posY / Chunk.sizeChunk;
            }

            localChunk = getChunkNotGeneration(chunkX, chunkY);

            return localChunk;
        }

        public Block getBlockByBlockPosition(int posX, int posY, bool isBackBlock)
        {

            Chunk localChunk = getChunkByBlockPosition(posX, posY);

            if(localChunk != null)
            {
                int blockX = 0;
                int blockY = 0;

                if(localChunk.chunkX < 0)
                {
                    blockX = Math.Abs(localChunk.chunkX * Chunk.sizeChunk) - Math.Abs(posX);
                }
                else
                {
                    blockX = posX - localChunk.chunkX * Chunk.sizeChunk;
                }




                if (localChunk.chunkY < 0)
                {
                    blockY = Math.Abs(localChunk.chunkY * Chunk.sizeChunk) - Math.Abs(posY);
                }
                else
                {
                    blockY = posY - localChunk.chunkY * Chunk.sizeChunk;
                }

                if (isBackBlock)
                {
                    return localChunk.backBlock[blockX, blockY];
                }
                else
                {
                    return localChunk.block[blockX, blockY];
                }
            }
            else
            {
                return null;
            }

        }

        public Block getBlockNotGeneration(int posX, int posY, bool isBackBlock)
        {

            Chunk localChunk = getChunkByBlockPositionNotGeneration(posX, posY);

            if (localChunk != null)
            {
                int blockX = 0;
                int blockY = 0;

                if (localChunk.chunkX < 0)
                {
                    blockX = Math.Abs(localChunk.chunkX * Chunk.sizeChunk) - Math.Abs(posX);
                }
                else
                {
                    blockX = posX - localChunk.chunkX * Chunk.sizeChunk;
                }




                if (localChunk.chunkY < 0)
                {
                    blockY = Math.Abs(localChunk.chunkY * Chunk.sizeChunk) - Math.Abs(posY);
                }
                else
                {
                    blockY = posY - localChunk.chunkY * Chunk.sizeChunk;
                }

                if (isBackBlock)
                {
                    return localChunk.backBlock[blockX, blockY];
                }
                else
                {
                    return localChunk.block[blockX, blockY];
                }
            }
            else
            {
                return null;
            }

        }

        public Block setBlock(int blockX, int blockY, int id, bool isBackBlock)
        {
            Chunk chunk = getChunkByBlockPosition(blockX, blockY);

            int blockXInChunk = 0;
            int blockYInChunk = 0;

            if (chunk.chunkX < 0)
            {
                blockXInChunk = Math.Abs(chunk.chunkX * Chunk.sizeChunk) - Math.Abs(blockX);
            }
            else
            {
                blockXInChunk = blockX - chunk.chunkX * Chunk.sizeChunk;
            }

            if (chunk.chunkY < 0)
            {
                blockYInChunk = Math.Abs(chunk.chunkY * Chunk.sizeChunk) - Math.Abs(blockY);
            }
            else
            {
                blockYInChunk = blockY - chunk.chunkY * Chunk.sizeChunk;
            }

            Block localBlock = RegisteryBlocks.createBlock(blockX, blockY, isBackBlock, this, id);
            localBlock.start();
            Color lastColorBrightess = chunk.block[blockXInChunk, blockYInChunk].colorBrightness;
            chunk.block[blockXInChunk, blockYInChunk] = localBlock;

            int pX = blockX;
            int pY = blockY;

            Block[] b = new Block[8];
            b[0] = getBlockNotGeneration(pX, pY - 1, isBackBlock);
            b[1] = getBlockNotGeneration(pX + 1, pY - 1, isBackBlock);
            b[2] = getBlockNotGeneration(pX + 1, pY, isBackBlock);
            b[3] = getBlockNotGeneration(pX + 1, pY + 1, isBackBlock);
            b[4] = getBlockNotGeneration(pX, pY + 1, isBackBlock);
            b[5] = getBlockNotGeneration(pX - 1, pY + 1, isBackBlock);
            b[6] = getBlockNotGeneration(pX - 1, pY, isBackBlock);
            b[7] = getBlockNotGeneration(pX - 1, pY - 1, isBackBlock);

            for (int x = 0; x < b.Length; x++)
            {
                if (b[x] != null)
                {
                    b[x].updateTexture();
                    b[x].levelUpdated += 1;
                }
            }

            localBlock.updateShadow();
            if (localBlock.colorBrightness.R != 0 || localBlock.colorBrightness.G != 0 || localBlock.colorBrightness.B != 0
                || lastColorBrightess.R != 0 || lastColorBrightess.G != 0 || lastColorBrightess.B != 0)
            {
                for (int x = pX - Block.fieldNeighbour; x < pX + Block.fieldNeighbour; x++)
                    for (int y = pY - Block.fieldNeighbour; y < pY + Block.fieldNeighbour; y++)
                    {
                        Block block = getBlockByBlockPosition(x, y, false);
                        if (block != null)
                            block.updateShadow();
                    }

                for (int x = pX - Block.fieldNeighbour; x < pX + Block.fieldNeighbour; x++)
                    for (int y = pY - Block.fieldNeighbour; y < pY + Block.fieldNeighbour; y++)
                    {
                        Block block = getBlockByBlockPosition(x, y, true);
                        if (block != null) //?//////////////////////////////////////////////////////////////////// НУЖНО ИСПРАВИТЬ, ЧТО БЫ ОСВЕЩЁННОСТЬ ОБНАВЛЯЛАСЬ ТОГДА, КОГДА ПРОГРУЗИТСЯ ЧАНК
                            block.updateShadow();
                    }
            }

            return localBlock;
        }

        public void setBlocks(int blockX, int blockY, int width, int height, int id, bool isBackBlock)
        {
            for(int x = blockX; x < blockX + width; x++)
                for(int y = blockY; y < blockY + height; y++)
                {
                    setBlock(x, y, id, isBackBlock);
                }
        }

        public Entity addEntity(int posX, int posY, int id)
        {
            Entity localEntity = RegisteryEntity.createEntity(this, id);
            localEntity.position = new Vector2f(posX, posY);
            entity.Add(localEntity);
            return localEntity;
        }

        public Entity addEntity(int posX, int posY, entityName name)
        {
            Entity localEntity = RegisteryEntity.createEntity(this, name);
            localEntity.position = new Vector2f(posX, posY);
            entity.Add(localEntity);
            return localEntity;
        }


        public void renderBlock(int x, int y, bool isBackBlock, RenderTarget target, RenderStates states)
        {
            Block localBlock = getBlockByBlockPosition(x, y, isBackBlock);


            if (localBlock != null)
            {
                localBlock.update();

                //localBlock.updateShadow();

                //if (localBlock.isUpdateTexture)
                //{
                //    if (localBlock.levelUpdated < 2)
                //    {
                //        localBlock.updateTexture();
                //        //localBlock.updateShadow();
                //        localBlock.levelUpdated++;
                //    }
                //}

                if (localBlock.rect == null || localBlock.backgroundRect == null)
                {
                    localBlock.createBlockTexture();
                }

                if (localBlock.backgroundRect.Texture != null)
                {
                    target.Draw(localBlock.backgroundRect, states);
                }
                if (localBlock.rect.Texture != null)
                {
                    target.Draw(localBlock.rect, states);
                }
            }
        }

        public void renderEntity(Entity entity)
        {
            entityRect.Texture = entity.texture;
            entityRect.FillColor = entity.color;
            entityRect.TextureRect = (IntRect)entity.textureRect;
            entityRect.Size = entity.size;
            entityRect.Position = entity.position;
        }

        public void saveActiveChunks()
        {
            int regionXLocal = 0;
            int regionYLocal = 0;

            for (int x = 0; x < countActiveChunks; x++)
            {
                if (chunk[x] != null)
                {
                    if (chunk[x].chunkX < 0)
                    {
                        if (Math.Abs(chunk[x].chunkX * 1.00f / ConvertRegionData.sizeRegion) - Math.Abs(chunk[x].chunkX / ConvertRegionData.sizeRegion) > 0)
                        {
                            regionXLocal = chunk[x].chunkX / ConvertRegionData.sizeRegion - 1;
                        }
                        else
                        {
                            regionXLocal = chunk[x].chunkX / ConvertRegionData.sizeRegion;
                        }
                    }
                    else
                    {
                        regionXLocal = chunk[x].chunkX / ConvertRegionData.sizeRegion;
                    }


                    if (chunk[x].chunkY < 0)
                    {
                        if (Math.Abs(chunk[x].chunkY * 1.00f / ConvertRegionData.sizeRegion) - Math.Abs(chunk[x].chunkY / ConvertRegionData.sizeRegion) > 0)
                        {
                            regionYLocal = chunk[x].chunkY / ConvertRegionData.sizeRegion - 1;
                        }
                        else
                        {
                            regionYLocal = chunk[x].chunkY / ConvertRegionData.sizeRegion;
                        }
                    }
                    else
                    {
                        regionYLocal = chunk[x].chunkY / ConvertRegionData.sizeRegion;
                    }


                    updateCloseActiveChunks();
                    ConvertRegionData.saveChunkDataToRegion(chunk[x], this);
                }
            }
        }

        public void updateCloseChunk(Chunk chunk)
        {
            chunk.updateCloseBlocks();
            List<Entity> entityInChunk = new List<Entity>();
            ConvertChunkToData.getCountEntitys(chunk, this, out entityInChunk);
            for(int x = 0; x < entityInChunk.Count; x++)
            {
                ICloseObject closeObject = entityInChunk[x] as ICloseObject;
                if(closeObject != null)
                {
                    closeObject.updateObject();
                }
            }
        }

        public void updateCloseActiveChunks()
        {
            for(int x = 0; x < countActiveChunks; x++)
            {
                if (chunk[x] != null)
                {
                    updateCloseChunk(chunk[x]);
                }
            }
        }

        public void cameraMove(float k)
        {
            //if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            //{
            //    camera += new Vector2f(-speedCamera, 0);
            //}
            //else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            //{
            //    camera += new Vector2f(speedCamera, 0);
            //}

            //if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            //{
            //    camera += new Vector2f(0, -speedCamera);
            //}
            //else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            //{
            //    camera += new Vector2f(0, speedCamera);
            //}

            Vector2f velocity = (player.position - camera) * k;
            camera += velocity * DeltaTime.getDeltaTime();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            int offsetBlocks = 3;
            Vector2f sizeWindow = (Vector2f)Program.win.Size;
            int blockX = (int)((camera.X - sizeWindow.X / 2) / Block.sizeBlock) - offsetBlocks;
            int blockY = (int)((camera.Y - sizeWindow.Y / 2) / Block.sizeBlock) - offsetBlocks;
            int blockSizeX = (int)(sizeWindow.X / Block.sizeBlock) + offsetBlocks * 2;
            int blockSizeY = (int)(sizeWindow.Y / Block.sizeBlock) + offsetBlocks * 2;


            for(int x = blockX; x < blockX + blockSizeX; x++)
                for(int y = blockY; y < blockY + blockSizeY; y++)
                {
                    renderBlock(x, y, true, target, states);
                }

            for (int x = blockX; x < blockX + blockSizeX; x++)
                for (int y = blockY; y < blockY + blockSizeY; y++)
                {
                    renderBlock(x, y, false, target, states);

                }
            

            for (int x = 0; x < entity.Count; x++)
            {
                renderEntity(entity[x]);
                entity[x].update();
                target.Draw(entityRect, states);
            }

            cameraMove(player.speedMove * 0.005f);
            //cameraMove(0.005f);
            Position = -camera + new Vector2f(sizeWindow.X / 2, sizeWindow.Y / 2);
        }
    }
}
