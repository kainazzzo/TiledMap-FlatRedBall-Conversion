using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FlatRedBall.Glue.GuiDisplay;
using TMXGlueLib;


namespace TmxEditor.PropertyGridDisplayers
{
    public class MapLayerDisplayer : PropertyGridDisplayer
    {
        #region Fields

        public override object Instance
        {
            get
            {
                return base.Instance;
            }
            set
            {
                base.Instance = value;
                UpdateDisplayedProperties();

                base.PropertyGrid.Refresh();
            }
        }

        MapLayer MapLayerInstance
        {
            get
            {
                return base.Instance as MapLayer;
            }
        }

        #endregion

        #region Properties

        public property CurrentLayerProperty
        {
            get
            {
                if (PropertyGrid.SelectedGridItem == null)
                {
                    return null;
                }
                else
                {
                    string name = PropertyGrid.SelectedGridItem.Label;
                    var property = GetPropertyByName(name);
                    return property;
                }
            }
        }

        #endregion

        public void UpdateDisplayedProperties()
        {
            ExcludeAllMembers();

            this.RefreshOnTimer = false;
            
            if(MapLayerInstance != null)
            {
                foreach (var property in MapLayerInstance.properties)
                {
                    IncludeProperty(property);
                }
            }
        }

        private void IncludeProperty(TMXGlueLib.property property)
        {
            string name = property.name;

            TypeConverter typeConverter = GetTypeConverterForProperty(property);

            IncludeMember(property.name, typeof(string),
                ChangePropertyValue,
                () =>
                {
                    if (MapLayerInstance != null)
                    {
                        var found = MapLayerInstance.properties.FirstOrDefault((candidate) => candidate.name == name);

                        if (property != null)
                        {
                            return property.value;
                        }
                    }
                    return null;
                },
                typeConverter
                );
        }

        private TypeConverter GetTypeConverterForProperty(property property)
        {
            int lastOpen = property.name.LastIndexOf('(');
            int lastClosed = property.name.LastIndexOf(')');
            string type = null;
            if (lastOpen != -1 && lastClosed != -1 && lastClosed > lastOpen)
            {
                type = property.name.Substring(lastOpen + 1, lastClosed - (lastOpen + 1));
            }

            switch (type)
            {
                case "float":
                case "Single":
                case "System.Single":
                    return new System.ComponentModel.SingleConverter();
                    //break;
                case "int":
                case "System.Int32":
                    return new System.ComponentModel.Int32Converter();
                    //break;
                case "bool":
                case "Boolean":
                case "System.Boolean":
                    return new System.ComponentModel.BooleanConverter();
                    //break;
                case "long":
                case "System.Int64":
                    return new System.ComponentModel.Int64Converter();
                    //break;
                case "double":
                case "Double":
                case "System.Double":
                    return new System.ComponentModel.DoubleConverter();
                    // break;
                default:
                    return null;
            }
        }

        void ChangePropertyValue(object sender, MemberChangeArgs args)
        {
            if(MapLayerInstance != null)
            {
                string name = args.Member;

                var property = GetPropertyByName(args.Member);

                if (property != null)
                {
                    if (args.Value == null)
                    {
                        property.value = null;
                    }
                    else
                    {
                        property.value = args.Value.ToString();
                    }
                }
            }
        }

        private property GetPropertyByName(string name)
        {
            property property = null;

            if (MapLayerInstance != null)
            {
                property = MapLayerInstance.properties.First((candidate) => candidate.name == name);
            }
            return property;
        }

    }
}
