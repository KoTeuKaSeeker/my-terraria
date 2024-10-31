using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraira_v0._4_beta.RegisteryFolder;
using Terraira_v0._4_beta.Worlds;

namespace Terraira_v0._4_beta.Init
{
    class Map : Transformable, Drawable
    {

        public World normalWorld;

        public World activeWorld;
        public Map()
        {
            normalWorld = new NormalWorld(this, 123);
            setActiveWorld(normalWorld);
        }

        public void setActiveWorld(World world)
        {
            activeWorld = world;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(activeWorld);
        }
    }
}
