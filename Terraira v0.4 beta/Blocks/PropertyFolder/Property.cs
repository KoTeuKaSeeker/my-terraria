using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks.PropertyFolder
{
    class Property
    {
        public int value;
        public int sizeProperty;
        public string nameProperty;
        public Property(int value, int sizeProperty, string nameProperty)
        {
            this.value = value;
            this.sizeProperty = sizeProperty;
            this.nameProperty = nameProperty;
        }



    }
}
