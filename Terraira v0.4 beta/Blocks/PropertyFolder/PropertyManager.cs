using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta.Blocks.PropertyFolder
{
    class PropertyManager
    {

        public List<Property> propertyList;
        public Hashtable propertyTable;
        public int allSize;

        public PropertyManager()
        {
            propertyList = new List<Property>();
            propertyTable = new Hashtable();
        }

        public void addProperty(int value, int sizeProperty, string nameProperty)
        {
            Property property = new Property(value, sizeProperty, nameProperty);
            propertyTable.Add(nameProperty, property);
            propertyList.Add(property);
            allSize += sizeProperty;
        }

        public Property getProperty(string nameProperty)
        {
            return propertyTable[nameProperty] as Property;
        }

        public int getValueProperty(string nameProperty)
        {
            return getProperty(nameProperty).value;
        }

        public void setValueProperty(string nameProperty, int value)
        {
            Property property = getProperty(nameProperty);
            property.value = value;
        }

    }
}
