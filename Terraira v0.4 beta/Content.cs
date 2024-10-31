using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;

namespace Terraira_v0._4_beta
{
    class Content
    {
        public static string directoryToBlocks = "..//Terraria//Textures//Blocks//";
        public static string extension = ".png";

        public static Texture blockStone;
        public static Texture blockGrass;
        public static Texture blockGreenGrass;
        public static Texture blockDirt;

        public static Texture blockTreeStrunk;
        public static Texture blockTreeLeaves;

        public static Texture backgroundBlockGrass;

        public static void loadBlocksTextures()
        {
            blockStone = new Texture(directoryToBlocks + "Stone" + extension);
            blockGrass = new Texture(directoryToBlocks + "Grass" + extension);
            blockGreenGrass = new Texture(directoryToBlocks + "GreenGrass" + extension);
            blockDirt = new Texture(directoryToBlocks + "Dirt" + extension);

            blockTreeStrunk = new Texture(directoryToBlocks + "TreeStrunk" + extension);
            blockTreeLeaves = new Texture(directoryToBlocks + "TreeLeaves" + extension);

            backgroundBlockGrass = new Texture(directoryToBlocks + "BackgroundGrass" + extension);
        }

        public static void loadAllTextures()
        {
            loadBlocksTextures();
        }

    }
}
