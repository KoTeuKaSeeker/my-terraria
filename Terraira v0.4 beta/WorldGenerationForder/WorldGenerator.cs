using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.WorldGenerationForder
{
    class WorldGenerator
    {
        public List<Biome> biome = new List<Biome>();
        public World world;

        public WorldGenerator(World world)
        {
            this.world = world;
        }

        public void addBiome(Biome biome)
        {
            biome.world = world;
            //biome.rand = world.rand;
            this.biome.Add(biome);
        }

        public Chunk generationChunk(int chunkX, int chunkY)
        {
            Chunk chunk = null;
            if (biome.Count > 0)
            {
                Biome generatedBiome = biome[Program.rand.Next(0, biome.Count)];
                //chunk = generatedBiome.generateChunk(chunkX, chunkY);
            }
            return chunk;
        }
    }
}
