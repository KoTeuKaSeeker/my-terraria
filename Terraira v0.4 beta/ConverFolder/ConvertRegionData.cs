using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks.PropertyFolder;
using Terraira_v0._4_beta.ByteHelper;
using Terraira_v0._4_beta.Entitys;
using Terraira_v0._4_beta.Init;
using Terraira_v0._4_beta.RegisteryFolder;

namespace Terraira_v0._4_beta.ConverFolder
{
    class ConvertRegionData
    {
        public delegate Chunk calledChunk();

        /* CHUNK DATA: 
        * 1. Первый биом чанка (2 байта)
        * 2. Второй биом чанка (2 байта)
        * 3. Параметр 2 биома чанка (2 байта)
        * 4. id всех блоков
        * 5. Параметры блоков
        */

        /* Будут отдельные файлы, содержащие в себе одновременно несколько чанков, которые будут заполняться следующим образом:
         * REGION DATA:
         * 1. Позиция региона X (2 байта)
         * 2. Позиция региона Y (2 байта)
         * 3. Количиство заполненных ссылок под чанки (2 байта)
         * 4. Ссылки на чанки:
         *       4.1 Позиция чанка по X (2 байта)
         *       4.2 Позиция чанка по Y (2 байта)
         *       4.3 Ссылка на чанк в region data (2 байта)
         * 5. Чанки
         */

        public static byte[] data;

        public static int sizeRegion = 15;

        public static int sizeRegionPos = 3;
        public static int sizeAddress = 3;

        public static int offsetEncoding = 688;

        public static string directoryToMaps = "..//Terraria//Maps//";
        public static string directioryToRegions = "Regions//";
        public static string extensionRegion = ".region";
        //public static int offsetEncoding = 1040;

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

        public static byte[] setValueForLocalData(int value, int address, int offset, byte[] localData)
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
                localData[address + x] = code[x];
            }
            return localData;
        }

        public static void createRegionFile(int regionX, int regionY)
        {
            data = new byte[sizeRegionPos * 2 + sizeAddress + (sizeRegionPos * 2 + sizeAddress) * sizeRegion * sizeRegion];

            string fileName = "";
            int pointer = 0;

            byte[] fileNameData = new byte[sizeRegionPos];

            fileNameData = setValueForLocalData(regionX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            fileNameData = setValueForLocalData(regionY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            setValueForData(regionX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), pointer, sizeRegionPos);
            pointer += sizeRegionPos;

            setValueForData(regionY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), pointer, sizeRegionPos);
            pointer += sizeRegionPos;

            setValueForData(0, pointer, sizeAddress);
            pointer += sizeAddress;

            setValueForData(0, pointer, (sizeRegionPos * 2 + sizeAddress) * sizeRegion * sizeRegion);

            char[] regionDataChar = new char[data.Length];

            for (int x = 0; x < data.Length; x++)
            {
                regionDataChar[x] = (char)(data[x] + offsetEncoding);

            }

            string regionData = new string(regionDataChar);

            StreamWriter writer = new StreamWriter(directoryToMaps + directioryToRegions + fileName + extensionRegion);
            writer.Write(regionData);
            writer.Close();
        }

        /*Сохранине чанка в RegionData:
        * 1. Находится нужный регион по названию файла и сохранется в ОЗУ
        * 2. Идёт проверка, добавлен ли уже чанк с данной позицией:
        * 3. Указатель перемещается в позицию sizeRegionPos * 2 + sizeFillAddress
        * 4. С помощью цикла while идёт проверка на совпадение chunkX и ChunkY. 
        *    Если совпадения есть, то цикл закрывается и заменяет чанк на полученном адресе на новый чанк.
        *    Если совпадений нету, то цикл закрывается и добавляет новую ссылку на новый чанк.
        *    Затем прибваляет к размеру файла размер чанка и перейдя по добавленной ссылки сохраняет чанк
        * 
        * 
        */
        public static void saveChunkDataToRegion(Chunk chunk, World world)
        {
            int regionX = 0;
            int regionY = 0;

            if (chunk.chunkX >= 0)
            {
                regionX = (chunk.chunkX / ConvertRegionData.sizeRegion);
            }
            else
            {
                if (Math.Abs(chunk.chunkX * 1.00f / ConvertRegionData.sizeRegion) - Math.Abs(chunk.chunkX / ConvertRegionData.sizeRegion) > 0)
                {
                    regionX = chunk.chunkX / ConvertRegionData.sizeRegion - 1;
                }
                else
                {
                    regionX = chunk.chunkX / ConvertRegionData.sizeRegion;
                }
            }

            if (chunk.chunkY >= 0)
            {
                regionY = (chunk.chunkY / ConvertRegionData.sizeRegion);
            }
            else
            {

                if (Math.Abs(chunk.chunkY * 1.00f / ConvertRegionData.sizeRegion) - Math.Abs(chunk.chunkY / ConvertRegionData.sizeRegion) > 0)
                {
                    regionY = chunk.chunkY / ConvertRegionData.sizeRegion - 1;
                }
                else
                {
                    regionY = chunk.chunkY / ConvertRegionData.sizeRegion;
                }
            }

            string fileName = "";
            byte[] fileNameData = new byte[sizeRegionPos];

            fileNameData = setValueForLocalData(regionX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            fileNameData = setValueForLocalData(regionY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            string fileDirectory = directoryToMaps + directioryToRegions + fileName + extensionRegion;

            string fileData = "";

            if (File.Exists(fileDirectory))
            {
                StreamReader reader = new StreamReader(fileDirectory);

                string sLine = "";

                while (sLine != null)
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                    {
                        fileData = sLine;
                    }
                }

                reader.Close();
            }
            else
            {
                createRegionFile(regionX, regionY);
                StreamReader reader = new StreamReader(fileDirectory);
                string sLine = "";

                while (sLine != null)
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                    {
                        fileData = sLine;
                    }
                }

                reader.Close();
            }

            data = new byte[fileData.Length];

            for (int x = 0; x < fileData.Length; x++)
            {
                data[x] = (byte)(fileData[x] - offsetEncoding);
            }

            int countFillChunk = getValueFromData(sizeRegionPos * 2, sizeAddress);

            int pointer = sizeRegionPos * 2 + sizeAddress;

            bool isMatches = false;

            for (int x = 0; x < countFillChunk; x++)
            {
                int chunkX = getValueFromData(pointer, sizeRegionPos) - (int)(Math.Pow(2, sizeRegionPos * 8) / 2);
                pointer += sizeRegionPos;

                int chunkY = getValueFromData(pointer, sizeRegionPos) - (int)(Math.Pow(2, sizeRegionPos * 8) / 2);
                pointer += sizeRegionPos;

                if (chunkX == chunk.chunkX && chunkY == chunk.chunkY)
                {
                    isMatches = true;
                    break;
                }
                else
                {
                    pointer += sizeAddress;
                }
            }

            if (isMatches)
            {
                haveChunk(pointer, chunk, regionX, regionY, world);
            }
            else
            {
                notHaveChunk(pointer, chunk, regionX, regionY, world);
            }
        }

        ///* Если чанка нету в RegionData:
        // * 1. Размер региона изменяется на region.Lenght + sizeNewChunk
        // * 2. pointer перемещается на адрес  (sizeRegionPos * 2 + sizeAddress) + getValueFromData(sizeRegionPos * 2, sizeAddress) * (sizeRegionPos * 2 + sizeAddress), 
        // *  то есть последняя не заполненная ячейка под ссылку
        // * 3. Теперь нужно получить ссылку на последнее пустое место для чанка(lastChunkAddress):
        // *      3.1 Находим адрес на ссылку в RegionReference: regionReference = getValueFromData(pointer - sizeAddress, sizeAddress) 
        // *      3.2 lastChunkAddress = getValueFromData(regionReference + (sizeRegionPos * 2), sizeAddress) 
        // *      ----3.3 Находит размер всех параметров чанка, то есть sizePropertys
        // *      ----3.4 Находит размер чанка, то есть sizeChunk = Chunk.sizeChunk * Chunk.sizeChunk * 2 + sizePropertys
        // *      ----3.5 newChunkAddress = lastChunkAddress + sizeChunk
        // * 4. Изменить адрес на последний чанк, выполнив  setValueForData((pointer - (sizeRegionPos * 2 + sizeAddress)) / (sizeRegionPos * 2 + sizeAddress), sizeRegionPos * 2, sizeAddress);
        // * 5. Затем выполняет setValueForData(chunk.chunkX, pointer, sizeRegionPos). Потом pointer += sizeRegionPos
        // * 6. Затем выполняет setValueForData(chunk.chunkY, pointer, sizeRegionPos). Потом pointer += sizeRegionPos
        // * 7. Затем выполняет setValueForData(lastChunkAddress + sizeChunk, pointer, sizeAddress). Потом pointer += sizeAddress
        // * 8. pointer перемещается на свободное место под чанк, то есть newChunkAddress
        // * 9. Получаем массив byte[] chunkData = ConvertChunkToData.convertChunk(chunk)
        // * 10. Записываем значение chunkData в массив data с помощью цикла с адреса pointer, до адреса data.Lenght
        // * 11. Сохраняем файл с новым data
        // */

        public static void notHaveChunk(int pointer, Chunk chunk, int regionX, int regionY, World world)
        {
            int countFillChunks = getValueFromData(sizeRegionPos * 2, sizeAddress);
            int newChunkXInData = (sizeRegionPos * 2 + sizeAddress) + countFillChunks * (sizeRegionPos * 2 + sizeAddress);
            int oldChunkAddress = getValueFromData(newChunkXInData - sizeAddress, sizeAddress) == 0 ? (sizeRegionPos * 2 + sizeAddress) + sizeRegion * sizeRegion * (sizeRegionPos * 2 + sizeAddress) : getValueFromData(newChunkXInData - sizeAddress, sizeAddress);
            int newChunkAddress = 0;

            int sizeOldChunk = 0;

            int sizeOldPropertys = 0;
            int sizeOldBlockPropertys = 0;
            int sizeOldBackPropertys = 0;
            int sizeOldEntitys = 0;
            int sizeOldPropertysEntity = 0;

            int localOldChunkAddress = oldChunkAddress;
            localOldChunkAddress += ConvertChunkToData.sizeBiome1 + ConvertChunkToData.sizeBiome2 + ConvertChunkToData.sizeBiome2Info;
            if (countFillChunks > 0)
            {

                for (int x = 0; x < Chunk.sizeChunk * Chunk.sizeChunk; x++)
                {
                    int id = getValueFromData(localOldChunkAddress, ConvertChunkToData.sizeIdBlock);
                    int allSizeProperty = RegisteryBlocks.getBlock(id).propertyManager.allSize;
                    sizeOldBlockPropertys += allSizeProperty;
                    localOldChunkAddress += ConvertChunkToData.sizeIdBlock;
                }

                for (int x = 0; x < Chunk.sizeChunk * Chunk.sizeChunk; x++)
                {
                    int id = getValueFromData(localOldChunkAddress, ConvertChunkToData.sizeIdBlock);
                    int allSizeProperty = RegisteryBlocks.getBlock(id).propertyManager.allSize;
                    sizeOldBackPropertys += allSizeProperty;
                    localOldChunkAddress += ConvertChunkToData.sizeIdBlock;
                }
                sizeOldPropertys = sizeOldBlockPropertys + sizeOldBackPropertys;

                localOldChunkAddress += sizeOldPropertys;

                int countEntity = getValueFromData(localOldChunkAddress, ConvertChunkToData.sizeCountEntity);
                localOldChunkAddress += ConvertChunkToData.sizeCountEntity;

                sizeOldEntitys = countEntity * (ConvertChunkToData.sizeEntityPos * 2 + ConvertChunkToData.sizeIdEntity);

                for (int x = 0; x < countEntity; x++)
                {
                    int id = getValueFromData(localOldChunkAddress + ConvertChunkToData.sizeEntityPos * 2, ConvertChunkToData.sizeIdEntity);
                    int sizeAllPropertys = RegisteryEntity.getEntity(id).propertyManager.allSize;
                    sizeOldPropertysEntity += sizeAllPropertys;
                    localOldChunkAddress += ConvertChunkToData.sizeEntityPos * 2 + ConvertChunkToData.sizeIdEntity;
                }


                sizeOldChunk = ConvertChunkToData.sizeBiome1 + ConvertChunkToData.sizeBiome2 + ConvertChunkToData.sizeBiome2Info + Chunk.sizeChunk * Chunk.sizeChunk * 2 + sizeOldPropertys
                    + ConvertChunkToData.sizeCountEntity + sizeOldEntitys + sizeOldPropertysEntity;
                newChunkAddress = oldChunkAddress + sizeOldChunk;
            }
            else
            {
                newChunkXInData = sizeRegionPos * 2 + sizeAddress;
                newChunkAddress = (sizeRegionPos * 2 + sizeAddress) + sizeRegion * sizeRegion * (sizeRegionPos * 2 + sizeAddress);
            }

            byte[] chunkData = ConvertChunkToData.convertChunk(chunk, world);
            int sizeNewChunk = chunkData.Length;

            byte[] newRegionData = new byte[data.Length + sizeNewChunk];
            for (int x = 0; x < data.Length; x++)
            {
                newRegionData[x] = data[x];
            }
            data = newRegionData;

            for (int x = newChunkAddress; x < data.Length; x++)
            {
                data[x] = chunkData[x - newChunkAddress];
            }

            pointer = newChunkXInData;

            setValueForData(countFillChunks + 1, sizeRegionPos * 2, sizeAddress);

            setValueForData(chunk.chunkX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), pointer, sizeRegionPos);
            pointer += sizeRegionPos;

            setValueForData(chunk.chunkY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), pointer, sizeRegionPos);
            pointer += sizeRegionPos;

            setValueForData(newChunkAddress, pointer, sizeAddress);

            string fileName = "";
            byte[] fileNameData = new byte[sizeRegionPos];

            fileNameData = setValueForLocalData(regionX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            fileNameData = setValueForLocalData(regionY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            string fileDirectory = directoryToMaps + directioryToRegions + fileName + extensionRegion;


            char[] regionDataChar = new char[data.Length];

            for (int x = 0; x < data.Length; x++)
            {
                regionDataChar[x] = (char)(data[x] + offsetEncoding);

            }

            string regionData = new string(regionDataChar);

            StreamWriter writer = new StreamWriter(directoryToMaps + directioryToRegions + fileName + extensionRegion);
            writer.Write(regionData);
            writer.Close();
        }

        ///* Если в regionData уже есть чанк, то:
        // * 1. Определсяется размер чанка, который будет заменён
        // *      1.1 Указатель перемещатеся на чанк
        // *      1.2 С помощью функции getSizePropertys записывается размер настроек чанка фоновых блоков и обычных блоков
        // *      1.3 Размер настроек чанка прибавляется к Chunk.sizeChunk * Chunk.sizeChunk * 2. В итоге размер чанка будет такой: 
        // *      Chunk.sizeChunk * Chunk.sizeChunk * 2 + sizePropertysBlock + sizePropertysBackBlock
        // * 2. Размер regionData становится таким: (regionData.Lenght - oldSizeChunk) + newSizeChunk. 
        // * 3. Все чанки, после старого чанка смещаются на размер oldSizeChunk - newSizeChunk(если число отрицательное, то смещать чанки нужно влево)
        // * 4. Новый чанк записывается в regionData с адреса oldAddress до адреса oldAddress + newSizeChunk. 
        // * 5. Все адреса на чанки после адреса чанка, который был заменён смещаются так же на значение oldSizeChunk - newSizeChunk
        // * 
        // */

        public static void haveChunk(int pointer, Chunk chunk, int regionX, int regionY, World world)
        {
            int chunkAddress = getValueFromData(pointer, sizeAddress);
            int chunkXInData = pointer - sizeRegionPos * 2;
            int endChunkAddress = 0;

            int oldSizeChunk = 0;
            int oldSizeBlockPropertys = 0;
            int oldSizeBackPropertys = 0;
            int oldSizePropertys = 0;
            int oldSizeEntitys = 0;
            int oldSizePropertysEntity = 0;

            int newSizeChunk = 0;

            int localChunkAddress = chunkAddress;
            localChunkAddress += ConvertChunkToData.sizeBiome1 + ConvertChunkToData.sizeBiome2 + ConvertChunkToData.sizeBiome2Info;

            for (int x = 0; x < Chunk.sizeChunk * Chunk.sizeChunk; x++)
            {
                int id = getValueFromData(localChunkAddress, ConvertChunkToData.sizeIdBlock);
                int allSizeProperty = RegisteryBlocks.getBlock(id).propertyManager.allSize;
                oldSizeBlockPropertys += allSizeProperty;
                localChunkAddress += ConvertChunkToData.sizeIdBlock;
            }

            for (int x = 0; x < Chunk.sizeChunk * Chunk.sizeChunk; x++)
            {
                int id = getValueFromData(localChunkAddress, ConvertChunkToData.sizeIdBlock);
                int allSizeProperty = RegisteryBlocks.getBlock(id).propertyManager.allSize;
                oldSizeBackPropertys += allSizeProperty;
                localChunkAddress += ConvertChunkToData.sizeIdBlock;
            }
            oldSizePropertys = oldSizeBlockPropertys + oldSizeBackPropertys;

            localChunkAddress += oldSizePropertys;

            int countEntity = getValueFromData(localChunkAddress, ConvertChunkToData.sizeCountEntity);
            localChunkAddress += ConvertChunkToData.sizeCountEntity;

            oldSizeEntitys = countEntity * (ConvertChunkToData.sizeEntityPos * 2 + ConvertChunkToData.sizeIdEntity);

            for(int x = 0; x < countEntity; x++)
            {
                int id = getValueFromData(localChunkAddress + ConvertChunkToData.sizeEntityPos * 2, ConvertChunkToData.sizeIdEntity);
                int sizeAllPropertys = RegisteryEntity.getEntity(id).propertyManager.allSize;
                oldSizePropertysEntity += sizeAllPropertys;
                localChunkAddress += ConvertChunkToData.sizeEntityPos * 2 + ConvertChunkToData.sizeIdEntity;
            }


            oldSizeChunk = ConvertChunkToData.sizeBiome1 + ConvertChunkToData.sizeBiome2 + ConvertChunkToData.sizeBiome2Info + Chunk.sizeChunk * Chunk.sizeChunk * 2 + oldSizePropertys
                + ConvertChunkToData.sizeCountEntity + oldSizeEntitys + oldSizePropertysEntity;

            byte[] chunkData = ConvertChunkToData.convertChunk(chunk, world);

            newSizeChunk = chunkData.Length;

            endChunkAddress = chunkAddress + oldSizeChunk;

            int offsetChunk = newSizeChunk - oldSizeChunk;

            if (offsetChunk > 0) // Смещать вправо
            {
                byte[] newRegionData = new byte[(data.Length - oldSizeChunk) + newSizeChunk];
                for (int x = 0; x < data.Length; x++)
                {
                    newRegionData[x] = data[x];
                }
                data = newRegionData;

                for (int x = data.Length - 1; x >= endChunkAddress + offsetChunk; x--)
                {
                    data[x] = data[x - offsetChunk];
                }

                int countFillChunks = getValueFromData(sizeRegionPos * 2, sizeAddress);
                int numberChunk = (chunkXInData - (sizeRegionPos * 2 + sizeAddress)) / (sizeRegionPos * 2 + sizeAddress);
                numberChunk++;
                int localPointer = (sizeRegionPos * 2 + sizeAddress) + numberChunk * (sizeRegionPos * 2 + sizeAddress);
                for (int x = numberChunk; x < countFillChunks; x++)
                {
                    int address = getValueFromData(localPointer + sizeRegionPos * 2, sizeAddress);
                    address += offsetChunk;
                    setValueForData(address, localPointer + sizeRegionPos * 2, sizeAddress);
                    localPointer += sizeRegionPos * 2 + sizeAddress;
                }
            }
            else if (offsetChunk < 0) // Смещать влево
            {
                for (int x = endChunkAddress + offsetChunk; x < data.Length + offsetChunk; x++)
                {
                    data[x] = data[x - offsetChunk];
                }

                byte[] newRegionData = new byte[(data.Length - oldSizeChunk) + newSizeChunk];
                for (int x = 0; x < newRegionData.Length; x++)
                {
                    newRegionData[x] = data[x];
                }
                data = newRegionData;

                int countFillChunks = getValueFromData(sizeRegionPos * 2, sizeAddress);
                int numberChunk = (chunkXInData - (sizeRegionPos * 2 + sizeAddress)) / (sizeRegionPos * 2 + sizeAddress);
                numberChunk++;
                int localPointer = (sizeRegionPos * 2 + sizeAddress) + numberChunk * (sizeRegionPos * 2 + sizeAddress);
                for (int x = numberChunk; x < countFillChunks; x++)
                {
                    int address = getValueFromData(localPointer + sizeRegionPos * 2, sizeAddress);
                    address += offsetChunk;
                    setValueForData(address, localPointer + sizeRegionPos * 2, sizeAddress);
                    localPointer += sizeRegionPos * 2 + sizeAddress;
                }
            }

            for (int x = chunkAddress; x < chunkAddress + newSizeChunk; x++)
            {
                data[x] = chunkData[x - chunkAddress];
            }




            string fileName = "";
            byte[] fileNameData = new byte[sizeRegionPos];

            fileNameData = setValueForLocalData(regionX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            fileNameData = setValueForLocalData(regionY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            string fileDirectory = directoryToMaps + directioryToRegions + fileName + extensionRegion;


            char[] regionDataChar = new char[data.Length];

            for (int x = 0; x < data.Length; x++)
            {
                regionDataChar[x] = (char)(data[x] + offsetEncoding);

            }

            string regionData = new string(regionDataChar);

            StreamWriter writer = new StreamWriter(directoryToMaps + directioryToRegions + fileName + extensionRegion);
            writer.Write(regionData);
            writer.Close();

        }


        public static Chunk getChunkFromDataRegion(int chunkX, int chunkY, World world)
        {
            int regionX = 0;
            int regionY = 0;

            if (chunkX >= 0)
            {
                regionX = (chunkX / ConvertRegionData.sizeRegion);
            }
            else
            {
                if (Math.Abs(chunkX * 1.00f / ConvertRegionData.sizeRegion) - Math.Abs(chunkX / ConvertRegionData.sizeRegion) > 0)
                {
                    regionX = chunkX / ConvertRegionData.sizeRegion - 1;
                }
                else
                {
                    regionX = chunkX / ConvertRegionData.sizeRegion;
                }
            }

            if (chunkY >= 0)
            {
                regionY = (chunkY / ConvertRegionData.sizeRegion);
            }
            else
            {

                if (Math.Abs(chunkY * 1.00f / ConvertRegionData.sizeRegion) - Math.Abs(chunkY / ConvertRegionData.sizeRegion) > 0)
                {
                    regionY = chunkY / ConvertRegionData.sizeRegion - 1;
                }
                else
                {
                    regionY = chunkY / ConvertRegionData.sizeRegion;
                }
            }

            // Пункт 1
            string fileName = "";
            byte[] fileNameData = new byte[sizeRegionPos];

            fileNameData = setValueForLocalData(regionX + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            fileNameData = setValueForLocalData(regionY + (int)(Math.Pow(2, sizeRegionPos * 8) / 2), 0, sizeRegionPos, fileNameData);
            for (int x = 0; x < fileNameData.Length; x++)
            {
                fileName += (char)(fileNameData[x] + offsetEncoding);
            }

            string fileDirectory = directoryToMaps + directioryToRegions + fileName + extensionRegion;

            string fileData = "";

            if (File.Exists(fileDirectory))
            {
                StreamReader reader = new StreamReader(fileDirectory);

                string sLine = "";

                while (sLine != null)
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                    {
                        fileData = sLine;
                    }
                }

                reader.Close();
            }
            else
            {
                createRegionFile(regionX, regionY);
                StreamReader reader = new StreamReader(fileDirectory);
                string sLine = "";

                while (sLine != null)
                {
                    sLine = reader.ReadLine();
                    if (sLine != null)
                    {
                        fileData = sLine;
                    }
                }

                reader.Close();
            }

            data = new byte[fileData.Length];

            for (int x = 0; x < fileData.Length; x++)
            {
                data[x] = (byte)(fileData[x] - offsetEncoding);
            }

            //Пункт 2
            Chunk chunk = null;

            int pointer = sizeRegionPos * 2 + sizeAddress;
            int countFillChunks = getValueFromData(sizeRegionPos * 2, sizeAddress);
            bool isMatches = false;

            int l = (pointer - (sizeRegionPos * 2 + sizeAddress)) / (sizeRegionPos * 2 + sizeAddress);


            while ((pointer - (sizeRegionPos * 2 + sizeAddress)) / (sizeRegionPos * 2 + sizeAddress) < countFillChunks)
            {
                int regionChunkX = getValueFromData(pointer, sizeRegionPos) - ((int)(Math.Pow(2, sizeRegionPos * 8) / 2));
                pointer += sizeRegionPos;

                int regionChunkY = getValueFromData(pointer, sizeRegionPos) - ((int)(Math.Pow(2, sizeRegionPos * 8) / 2));
                pointer += sizeRegionPos;

                if (chunkX == regionChunkX && chunkY == regionChunkY)
                {
                    isMatches = true;
                    break;
                }
                else
                {
                    pointer += sizeAddress;
                }
            }

            if (isMatches)
            {

                //Пункт 3
                pointer = getValueFromData(pointer, sizeAddress);

                int sizeBlockPropertys = 0;
                int sizeBackPropertys = 0;
                int sizePropertys = 0;
                int sizeEntitys = 0;
                int sizePropertysEntity = 0;
                int sizeChunk = 0;

                int localPointer = pointer + ConvertChunkToData.sizeBiome1 + ConvertChunkToData.sizeBiome2 + ConvertChunkToData.sizeBiome2Info;

                for (int x = 0; x < Chunk.sizeChunk * Chunk.sizeChunk; x++)
                {
                    int id = getValueFromData(localPointer, ConvertChunkToData.sizeIdBlock);
                    int allSizeProperty = RegisteryBlocks.getBlock(id).propertyManager.allSize;
                    sizeBlockPropertys += allSizeProperty;
                    localPointer += ConvertChunkToData.sizeIdBlock;
                }

                for (int x = 0; x < Chunk.sizeChunk * Chunk.sizeChunk; x++)
                {
                    int id = getValueFromData(localPointer, ConvertChunkToData.sizeIdBlock);
                    int allSizeProperty = RegisteryBlocks.getBlock(id).propertyManager.allSize;
                    sizeBackPropertys += allSizeProperty;
                    localPointer += ConvertChunkToData.sizeIdBlock;
                }
                sizePropertys = sizeBlockPropertys + sizeBackPropertys;

                localPointer += sizePropertys;

                int countEntity = getValueFromData(localPointer, ConvertChunkToData.sizeCountEntity);
                localPointer += ConvertChunkToData.sizeCountEntity;

                sizeEntitys = countEntity * (ConvertChunkToData.sizeEntityPos * 2 + ConvertChunkToData.sizeIdEntity);

                for (int x = 0; x < countEntity; x++)
                {
                    int id = getValueFromData(localPointer + ConvertChunkToData.sizeEntityPos * 2, ConvertChunkToData.sizeIdEntity);
                    int sizeAllPropertys = RegisteryEntity.getEntity(id).propertyManager.allSize;
                    sizePropertysEntity += sizeAllPropertys;
                    localPointer += ConvertChunkToData.sizeEntityPos * 2 + ConvertChunkToData.sizeIdEntity;
                }


                sizeChunk = ConvertChunkToData.sizeBiome1 + ConvertChunkToData.sizeBiome2 + ConvertChunkToData.sizeBiome2Info + Chunk.sizeChunk * Chunk.sizeChunk * 2 + sizePropertys
                    + ConvertChunkToData.sizeCountEntity + sizeEntitys + sizePropertysEntity;

                //Пункт 4
                byte[] chunkData = new byte[sizeChunk];

                for(int x = pointer; x < pointer + sizeChunk; x++)
                {
                    chunkData[x - pointer] = data[x];
                }

                //Пункт 5
                chunk = ConvertChunkToData.recoveryChunk(chunkData, chunkX, chunkY, world);



            }
            //Пункт 6
            return chunk;
        }

    }
}
