using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Blocks.PropertyFolder;
using Terraira_v0._4_beta.ByteHelper;
using Terraira_v0._4_beta.Entitys;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.ConverFolder
{
    class ConvertChunkToData
    {

        /* CHUNK DATA: 
         * 1. Первый биом чанка (2 байта)
         * 2. Второй биом чанка (2 байта)
         * 3. Параметр 2 биома чанка (2 байта)
         * 4. id всех блоков
         * 5. Параметры блоков
         * 6. Количиство ентити в чанке (3 байта)
         * 7. id всех естити
         * 8. Параметры всех ентити
         */

        /* Будут отдельные файлы, содержащие в себе одновременно несколько чанков, которые будут заполняться следующим образом:
         * REGION DATA:
         * 1. Позиция региона X (2 байта)
         * 2. Позиция региона Y (2 байта)
         * 3. Количиство заполненных ссылок под чанки (1 или 2 байта)
         * 4. Ссылки на чанки:
         *       4.1 Позиция чанка по X (2 байта)
         *       4.2 Позиция чанка по Y (2 байта)
         *       4.3 Ссылка на чанк в region data
         * 5. Чанки
         */

        public static byte[] data;

        public static int sizeBiome1 = 2;
        public static int sizeBiome2 = 2;
        public static int sizeBiome2Info = 2;
        public static int sizeCountEntity = 1;
        public static int sizeEntityPos = 3;
        public static int sizeIdEntity = 1;
        public static int sizeIdBlock = 1;

        public static int getValueFromData(int address, int offset)
        {
            byte[] code = new byte[offset];
            for (int x = 0; x < offset; x++)
            {
                code[x] = data[address + x];
            }
            int value = Helper.set256To10(code);
            return value;
        }

        public static void setValueForData(int value, int address, int offset)
        {
            byte[] code = new byte[offset];
            byte[] gotCode = Helper.set10To256(value);
            for (int x = 0; x < gotCode.Length; x++)
            {
                int v = code.Length - gotCode.Length + x;
                code[code.Length - gotCode.Length + x] = gotCode[x];
            }
            for (int x = 0; x < offset; x++)
            {
                data[address + x] = code[x];
            }
        }

        public static int getSizePropertys(Chunk chunk, bool isBackBlock)
        {
            int sizePropertys = 0;
            for(int x = 0; x < Chunk.sizeChunk; x++)
                for(int y = 0; y < Chunk.sizeChunk; y++)
                {
                    if (isBackBlock)
                    {
                        sizePropertys += chunk.backBlock[x, y].propertyManager.allSize;
                    }
                    else
                    {
                        sizePropertys += chunk.block[x, y].propertyManager.allSize;
                    }
                }
            return sizePropertys;
        }

        public static int converPropertysChunk(Chunk chunk, int pointer, bool isBackBlock)
        {
            for(int x = 0; x < Chunk.sizeChunk; x++)
                for(int y = 0; y < Chunk.sizeChunk; y++)
                {
                    PropertyManager propertyManager;

                    if (isBackBlock)
                    {
                        propertyManager = chunk.backBlock[x, y].propertyManager;
                    }
                    else
                    {
                        propertyManager = chunk.block[x, y].propertyManager;
                    }

                    if (propertyManager.allSize > 0)
                    {
                        for(int z = 0; z < propertyManager.propertyList.Count; z++)
                        {
                            setValueForData(propertyManager.propertyList[z].value, pointer, propertyManager.propertyList[z].sizeProperty);
                            pointer += propertyManager.propertyList[z].sizeProperty;
                        }
                    }
                }
            return pointer;
        }

        public static int getCountEntitys(Chunk chunk, World world, out List<Entity> entityInChunk)
        {
            int countEntity = 0;

            List<Entity> entity = world.entity;
            entityInChunk = new List<Entity>();

            for (int x = 0; x < entity.Count; x++)
            {
                if (entity[x].saveInChunk)
                {
                    /////////////////////////////////////////////

                    int pX = 0;
                    int pY = 0;


                    if (entity[x].position.X < 0)
                    {
                        if (Math.Abs(entity[x].position.X * 1.00f / Block.sizeBlock) - Math.Abs(entity[x].position.X / Block.sizeBlock) > 0)
                        {
                            pX = (int)entity[x].position.X / Block.sizeBlock - 1;
                        }
                        else
                        {
                            pX = (int)entity[x].position.X / Block.sizeBlock;
                        }
                    }
                    else
                    {
                        pX = (int)entity[x].position.X / Block.sizeBlock;
                    }


                    if (entity[x].position.Y < 0)
                    {
                        if (Math.Abs(entity[x].position.Y * 1.00f / Block.sizeBlock) - Math.Abs(entity[x].position.Y / Block.sizeBlock) > 0)
                        {
                            pY = (int)entity[x].position.Y / Block.sizeBlock - 1;
                        }
                        else
                        {
                            pY = (int)entity[x].position.Y / Block.sizeBlock;
                        }
                    }
                    else
                    {
                        pY = (int)entity[x].position.Y / Block.sizeBlock;
                    }

                    /////////////////////////////////////////////

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

                    if (chunkX == chunk.chunkX && chunkY == chunk.chunkY)
                    {
                        entityInChunk.Add(entity[x]);
                    }

                }
            }
            countEntity = entityInChunk.Count;

            return countEntity;

        }

        public static int getSizePropertysEntity(List<Entity> entityInChunk)
        {
            int sizePropertys = 0;
            for(int x = 0; x < entityInChunk.Count; x++)
            {
                Entity entity = entityInChunk[x];
                int allSizePropertys = entity.propertyManager.allSize;
                sizePropertys += allSizePropertys;
            }
            return sizePropertys;
        }

        public static int convertEntityChunk(Chunk chunk, List<Entity> entityInChunk, int pointer, World world)
        {
            int countEntity = entityInChunk.Count;
            setValueForData(countEntity, pointer, sizeCountEntity);
            pointer += sizeCountEntity;

            for(int x = 0; x < countEntity; x++)
            {
                int posX = (int)entityInChunk[x].position.X;
                setValueForData(posX + (int)(Math.Pow(2, sizeEntityPos * 8) / 2), pointer, sizeEntityPos);
                pointer += sizeEntityPos;

                int posY = (int)entityInChunk[x].position.Y;
                setValueForData(posY + (int)(Math.Pow(2, sizeEntityPos * 8) / 2), pointer, sizeEntityPos);
                pointer += sizeEntityPos;

                int id = entityInChunk[x].id;
                setValueForData(id, pointer, sizeIdEntity);
                pointer += sizeIdEntity;
            }

            for(int x = 0; x < countEntity; x++)
            {
                Entity entity = entityInChunk[x];
                for(int y = 0; y < entity.propertyManager.propertyList.Count; y++)
                {
                    int sizeProperty = entity.propertyManager.propertyList[y].sizeProperty;
                    int valueProperty = entity.propertyManager.propertyList[y].value;
                    setValueForData(valueProperty, pointer, sizeProperty);
                    pointer += sizeProperty;
                }
            }

            return pointer;
        }

        public static byte[] convertChunk(Chunk chunk, World world)
        {
            int sizePropertysBlock = getSizePropertys(chunk, false);
            int sizePropertysBackBlock = getSizePropertys(chunk, true);
            List<Entity> entityInChunk;
            int countEntity = getCountEntitys(chunk, world, out entityInChunk);
            int sizePropertysEntity = getSizePropertysEntity(entityInChunk);

            data = new byte[sizeBiome1 + sizeBiome2 + sizeBiome2Info + Chunk.sizeChunk * Chunk.sizeChunk * 2 + sizePropertysBlock + sizePropertysBackBlock
                + sizeCountEntity + countEntity * (sizeEntityPos * 2 + sizeIdEntity) + sizePropertysEntity];

            int biome1 = chunk.biome1;
            int biome2 = chunk.biome2;
            int biome2info = chunk.biome2Info;

            int pointer = 0;

            setValueForData(biome1, pointer, sizeBiome1);
            pointer += sizeBiome1;

            setValueForData(biome2, pointer, sizeBiome2);
            pointer += sizeBiome2;

            setValueForData(biome2info, pointer, sizeBiome2Info);
            pointer += sizeBiome2Info;

            for (int x = 0; x < Chunk.sizeChunk; x++)
                for (int y = 0; y < Chunk.sizeChunk; y++)
                {
                    setValueForData(chunk.block[x, y].id, pointer, sizeIdBlock);
                    pointer += sizeIdBlock;
                }

            for (int x = 0; x < Chunk.sizeChunk; x++)
                for (int y = 0; y < Chunk.sizeChunk; y++)
                {
                    setValueForData(chunk.backBlock[x, y].id, pointer, sizeIdBlock);
                    pointer += sizeIdBlock;
                }

            pointer = converPropertysChunk(chunk, pointer, false);
            pointer = converPropertysChunk(chunk, pointer, true);
            pointer = convertEntityChunk(chunk, entityInChunk, pointer, world);

            return data;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public static Chunk recoveryChunk(byte[] chunkData, int chunkX, int chunkY, World world)
        {
            data = chunkData;

            int pointer = 0;

            int biome1;
            int biome2;
            int biome2Info;
            List<Entity> entity = new List<Entity>();

            biome1 = getValueFromData(pointer, sizeBiome1);
            pointer += sizeBiome1;

            biome2 = getValueFromData(pointer, sizeBiome2);
            pointer += sizeBiome2;

            biome2Info = getValueFromData(pointer, sizeBiome2Info);
            pointer += sizeBiome2Info;

            int propertyPointer = sizeBiome1 + sizeBiome2 + sizeBiome2Info + Chunk.sizeChunk * Chunk.sizeChunk * 2;

            Block[,] block = new Block[Chunk.sizeChunk, Chunk.sizeChunk];
            
            for(int x = 0; x < Chunk.sizeChunk; x++)
                for(int y = 0; y < Chunk.sizeChunk; y++)
                {
                    int id = getValueFromData(pointer, sizeIdBlock);
                    pointer += sizeIdBlock;

                    block[x, y] = RegisteryBlocks.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, false, world, id);

                    if(block[x, y].propertyManager.allSize > 0)
                    {
                        PropertyManager propertyManager = block[x, y].propertyManager;
                        for(int z = 0; z < propertyManager.propertyList.Count; z++)
                        {
                            propertyManager.propertyList[z].value = getValueFromData(propertyPointer, propertyManager.propertyList[z].sizeProperty);
                            propertyPointer += propertyManager.propertyList[z].sizeProperty;
                        }
                    }
                    block[x, y].start();
                }

            Block[,] backBlock = new Block[Chunk.sizeChunk, Chunk.sizeChunk];

            for (int x = 0; x < Chunk.sizeChunk; x++)
                for (int y = 0; y < Chunk.sizeChunk; y++)
                {
                    int id = getValueFromData(pointer, sizeIdBlock);
                    pointer += sizeIdBlock;

                    backBlock[x, y] = RegisteryBlocks.createBlock(chunkX * Chunk.sizeChunk + x, chunkY * Chunk.sizeChunk + y, true, world, id);

                    if (backBlock[x, y].propertyManager.allSize > 0)
                    {
                        PropertyManager propertyManager = backBlock[x, y].propertyManager;
                        for (int z = 0; z < propertyManager.propertyList.Count; z++)
                        {
                            propertyManager.propertyList[z].value = getValueFromData(propertyPointer, propertyManager.propertyList[z].sizeProperty);
                            propertyPointer += propertyManager.propertyList[z].sizeProperty;
                        }
                    }
                    backBlock[x, y].start();
                }

            pointer = propertyPointer;


            int countEntity = getValueFromData(pointer, sizeCountEntity);
            pointer += sizeCountEntity;
            propertyPointer = pointer + countEntity * (sizeEntityPos * 2 + sizeIdEntity);

            for(int x = 0; x < countEntity; x++)
            {
                int posX = getValueFromData(pointer, sizeEntityPos) - (int)(Math.Pow(2, sizeEntityPos * 8) / 2);
                pointer += sizeEntityPos;

                int posY = getValueFromData(pointer, sizeEntityPos) - (int)(Math.Pow(2, sizeEntityPos * 8) / 2);
                pointer += sizeEntityPos;

                int id = getValueFromData(pointer, sizeIdEntity);
                pointer += sizeIdEntity;

                Entity localEntity = world.addEntity(posX, posY, id);
                if(localEntity.propertyManager.allSize > 0)
                {
                    PropertyManager propertyManager = localEntity.propertyManager;
                    for(int y = 0; y < propertyManager.propertyList.Count; y++)
                    {
                        int sizeProperty = propertyManager.propertyList[y].sizeProperty;
                        int value = getValueFromData(propertyPointer, sizeProperty);
                        propertyPointer += sizeProperty;

                        localEntity.propertyManager.propertyList[y].value = value;
                    }
                }
                localEntity.start();
            }

            Chunk chunk = new Chunk(chunkX, chunkY, world);
            chunk.biome1 = biome1;
            chunk.biome2 = biome2;
            chunk.biome2Info = biome2Info;
            chunk.block = block;
            chunk.backBlock = backBlock;

            return chunk;
        }


    }
}
