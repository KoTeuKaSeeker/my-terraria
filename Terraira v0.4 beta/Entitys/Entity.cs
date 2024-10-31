using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.Blocks;
using Terraira_v0._4_beta.Blocks.PropertyFolder;
using Terraira_v0._4_beta.Init;

namespace Terraira_v0._4_beta.Entitys
{
    enum Sides
    {
        top, right, down, left
    }
    abstract class Entity
    {
        public delegate void collisionDelegate(Sides side);

        public PropertyManager propertyManager;
        public int id;

        public Color color = Color.White;
        public Texture texture;
        public IntRect textureRect;
        public Vector2f size;

        public Vector2f position;
        public Vector2f velocity;
        public FloatRect collisionRect;
        public event collisionDelegate collisionEvent = delegate { };
        public bool isCollision = true;

        public bool saveInChunk = true;
        public World world;

        public Entity()
        {
            propertyManager = new PropertyManager();
        }

        protected void activeCollisionEvent(Sides side)
        {
            collisionEvent(side);
        }

        public abstract Entity getNewEntity();

        public Entity createEntity(World world)
        {
            Entity entity = null;
            entity = getNewEntity();
            entity.world = world;
            entity.id = id;
            return entity;
        }

        public virtual void update()
        {

        }

        public virtual void start()
        {

        }

        public Entity addProperty(int value, int sizeProperty, string nameProperty)
        {
            propertyManager.addProperty(value, sizeProperty, nameProperty);
            return this;
        }

        public int getValueProperty(string nameProperty)
        {
            return propertyManager.getValueProperty(nameProperty);
        }

        public void setValueProperty(string nameProperty, int value)
        {
            propertyManager.setValueProperty(nameProperty, value);
        }


    }
}
