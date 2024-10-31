using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Entitys;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.RegisteryFolder
{

    enum entityName
    {
        player,
        slime
    }
    class RegisteryEntity
    {

        public static List<Entity> entity = new List<Entity>();
        public static Hashtable idTable = new Hashtable();

        public static Player player = new Player();
        public static Slime slime = new Slime();

        public static Entity getEntity(int id)
        {
            return entity[id];
        }

        public static Entity getEntity(entityName name)
        {
            int id = (int)idTable[name];
            return entity[id];
        }

        public static Entity createEntity(World world, int id)
        {
            Entity entity = RegisteryEntity.entity[id];
            return entity.createEntity(world);
        }

        public static Entity createEntity(World world, entityName name)
        {
            Entity entity = RegisteryEntity.entity[(int)idTable[name]];
            return entity.createEntity(world);
        }

        public static void addEntity(Entity entityLocal, entityName name)
        {
            entity.Add(entityLocal);
            entityLocal.id = entity.Count - 1;
            idTable.Add(name, entityLocal.id);
        }

        public static void initEntitys()
        {
            addEntity(player, entityName.player);
            addEntity(slime, entityName.slime);
        }

    }
}
