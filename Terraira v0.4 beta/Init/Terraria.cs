using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Init
{
    class Terraria : Transformable, Drawable
    {

        public Map map;

        public Terraria()
        {
            map = new Map();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(map, states);
        }
    }
}
