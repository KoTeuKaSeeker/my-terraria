using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Blocks.Lamps;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.RegisteryFolder
{
    enum blockName
    {
        NA, air, stone, grassRed, grassYellow, grassBlue, grassGreen, dirt, backDirt, backStone,
        iron, gold, simpleTree, lampRed, lampGreen, lampBlue, lampRGB, lampOrange
    }

    class RegisteryBlocks
    {

        public static List<Block> block = new List<Block>();
        public static Hashtable blockId = new Hashtable();

        public static BlockAir blockAir = new BlockAir();
        public static BlockStone blockStone = new BlockStone();
        public static BlockGrassRed blockGrassRed = new BlockGrassRed();
        public static BlockGrassYellow blockGrassYellow = new BlockGrassYellow();
        public static BlockGrassBlue blockGrassBlue = new BlockGrassBlue();
        public static BlockGrassGreen blockGrassGreen = new BlockGrassGreen();
        public static BlockDirt blockDirt = new BlockDirt();
        public static BlockOreIron blockOreIron = new BlockOreIron();
        public static BlockOreGold blockOreGold = new BlockOreGold();
        public static BlockSimpleTree blockSimpleTree = new BlockSimpleTree();
        public static BlockLampRed blockLampRed = new BlockLampRed();
        public static BlockLampGreen blockLampGreen = new BlockLampGreen();
        public static BlockLampBlue blockLampBlue = new BlockLampBlue();
        public static BlockLampRGB blockLampRGB = new BlockLampRGB();
        public static BlockLampOrange blockLampOrange = new BlockLampOrange();


        public static BackBlockDirt backBlockDirt = new BackBlockDirt();
        public static BackBlockStone backBlockStone = new BackBlockStone();

        public static Block getBlock(int id)
        {
            return block[id];
        }

        public static Block getBlock(blockName name)
        {
            return block[(int)blockId[name]];
        }

        public static Block createBlock(int pX, int pY, bool isBackBlock, World world, int id)
        {
            Block stateBlock = getBlock(id);
            Block block = stateBlock.createBlock(pX, pY, isBackBlock, world);
            block.isBackBlock = isBackBlock;
            return block;
        }

        public static Block createBlock(int pX, int pY, bool isBackBlock, World world, blockName name)
        {
            int id = (int)blockId[name];
            Block stateBlock = getBlock(id);
            Block block = stateBlock.createBlock(pX, pY, isBackBlock, world);
            block.isBackBlock = isBackBlock;
            return block;
        }

        private static void addBlock(Block blockLocal, blockName name)
        {
            block.Add(blockLocal);
            blockLocal.id = block.Count - 1;
            blockId.Add(name, blockLocal.id);
        }

        public static void initBlocks()
        {
            addBlock(blockAir, blockName.air);
            addBlock(blockStone, blockName.stone);
            addBlock(blockGrassGreen, blockName.grassGreen);
            addBlock(blockDirt, blockName.dirt);
            addBlock(blockGrassYellow, blockName.grassYellow);
            addBlock(blockGrassBlue, blockName.grassBlue);
            addBlock(blockGrassRed, blockName.grassRed);
            addBlock(backBlockDirt, blockName.backDirt);
            addBlock(backBlockStone, blockName.backStone);
            addBlock(blockOreIron, blockName.iron);
            addBlock(blockOreGold, blockName.gold);
            addBlock(blockSimpleTree, blockName.simpleTree);
            addBlock(blockLampRed, blockName.lampRed);
            addBlock(blockLampGreen, blockName.lampGreen);
            addBlock(blockLampBlue, blockName.lampBlue);
            addBlock(blockLampRGB, blockName.lampRGB);
            addBlock(blockLampOrange, blockName.lampOrange);
        }

    }
}
