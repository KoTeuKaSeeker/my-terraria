using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraira_v0._4_beta.ConverFolder;
using Terraira_v0._4_beta.Init.ChunkLoadingThread;

namespace Terraira_v0._4_beta.Init
{
    class ChunkThread
    {
        public Thread thread;
        public List<Chunk> chunkToSave;
        public List<ChunkContainer> chunkContainer;

        public ChunkThread()
        {
            chunkToSave = new List<Chunk>();
            chunkContainer = new List<ChunkContainer>();
            Queue<Chunk> q = new Queue<Chunk>();
            Stack<Chunk> s = new Stack<Chunk>();
        }

        public void start()
        {
            thread = new Thread(update);
            thread.Start();
        }

        private void update()
        {
            while (Program.win.IsOpen)
            {
                if (chunkToSave.Count > 0)
                {
                    int id = 0;
                    lock (chunkToSave)
                    {
                        id = chunkToSave.Count - 1;
                    }
                    toSaveChunk(chunkToSave[id]);
                    lock (chunkToSave)
                    {
                        chunkToSave.Remove(chunkToSave[id]);
                    }
                }
                if (chunkContainer.Count > 0)
                {
                    int id = 0;
                    lock (chunkContainer)
                    {
                        id = chunkContainer.Count - 1;
                    }
                    loadChunkToWorld(chunkContainer[id]);
                    lock (chunkContainer)
                    {
                        chunkContainer.Remove(chunkContainer[id]);
                    }
                }
            }
        }

        public void addChunkToSave(Chunk chunk)
        {
            lock (chunkToSave)
            {
                if (!chunkToSave.Contains(chunk)) chunkToSave.Add(chunk);
            }
        }

        public void addChunkContainer(int chunkX, int chunkY, World world)
        {
            lock (chunkContainer)
            {
                ChunkContainer containter = new ChunkContainer(chunkX, chunkY, world);
                if (!chunkContainer.Contains(containter))
                {
                    chunkContainer.Add(containter);
                }
            }
        }

        public void toSaveChunk(Chunk chunk)
        {
            ConvertRegionData.saveChunkDataToRegion(chunk, chunk.world);     
        }

        public void loadChunkToWorld(ChunkContainer container)
        {
            World world = container.world;
            Chunk localChunk = ConvertRegionData.getChunkFromDataRegion(container.chunkX, container.chunkY, world);

            if (localChunk == null)
            {
                localChunk = world.generationChunk(container.chunkX, container.chunkY);
            }

            world.chunkPointer++;
            if (world.chunkPointer > World.countActiveChunks - 1)
            {
                world.chunkPointer = 0;
            }

            if (world.chunk[world.chunkPointer] != null)
            {
                world.updateCloseChunk(world.chunk[world.chunkPointer]);
                toSaveChunk(world.chunk[world.chunkPointer]);
            }

            world.chunk[world.chunkPointer] = localChunk;        
        }

        public void close()
        {
            thread.Abort();
        }

    }
}
