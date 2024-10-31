using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Biomes;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.RegisteryFolder
{

    enum biomeName
    {
        air, herbal, autumn, greenland
    }

    class RegisteryBiome
    {

        public static List<Biome> biome = new List<Biome>();
        public static Hashtable idTable = new Hashtable();

        private static BiomeAir biomeAir = new BiomeAir();
        private static BiomeHerbal biomeHerbal = new BiomeHerbal();
        private static BiomeAutumn biomeAutumn = new BiomeAutumn();
        private static BiomeGreenland biomeGreenland = new BiomeGreenland();

        public static void addBiome(Biome biome, biomeName name)
        {
            RegisteryBiome.biome.Add(biome);
            int id = RegisteryBiome.biome.Count - 1;
            idTable.Add(name, id);
            biome.id = id;
        }

        public static Biome getBiome(World world, int id)
        {
            Biome localBiome = biome[id];
            localBiome.world = world;
            localBiome.rand = world.rand;
            return biome[id];
        }

        public static Biome getBiome(World world, biomeName name)
        {
            return getBiome(world, (int)idTable[name]);
        }

        public static Biome createBiome(World world, int id)
        {
            Biome biome = getBiome(world, id).createBiome(world);
            return biome;
        }

        public static Biome createBiome(World world, biomeName name)
        {
            Biome biome = getBiome(world, name).createBiome(world);
            return biome;
        }

        public static void initBiome()
        {
            addBiome(biomeAir, biomeName.air);
            addBiome(biomeHerbal, biomeName.herbal);
            addBiome(biomeAutumn, biomeName.autumn);
            addBiome(biomeGreenland, biomeName.greenland);
        }

    }
}
