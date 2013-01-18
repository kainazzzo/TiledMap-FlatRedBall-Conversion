using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TmxEditor.PropertyGridDisplayers;
using TmxEditor.UI;
using TMXGlueLib;

namespace TmxEditor.Controllers
{
    public class LayersController : Singleton<LayersController>
    {
        #region Fields

        TreeView mLayerTreeView;


        TiledMapSave mTiledMapSave;

        MapLayerDisplayer mDisplayer;

        #endregion

        #region Properties

        public TiledMapSave TiledMapSave
        {
            set
            {
                mTiledMapSave = value;
                RefreshAll();
            }
        }

        public MapLayer CurrentMapLayer
        {
            get
            {
                TreeNode selectedNode = mLayerTreeView.SelectedNode;
                if (selectedNode != null)
                {
                    return selectedNode.Tag as MapLayer;
                }
                return null;
            }
        }

        public property CurrentLayerProperty
        {
            get
            {
                return mDisplayer.CurrentLayerProperty;


            }
        }

        #endregion

        #region Events
        public event EventHandler AnyTileMapChange;
        #endregion

        public void Initialize(TreeView layerTreeView, PropertyGrid propertyGrid)
        {
            mLayerTreeView = layerTreeView;

            mLayerTreeView.AfterSelect += mLayerTreeView_Click;

            mDisplayer = new MapLayerDisplayer();
            mDisplayer.PropertyGrid = propertyGrid;

            mDisplayer.PropertyGrid.PropertyValueChanged += HandlePropertyValueChangeInternal;
        }

        void HandlePropertyValueChangeInternal(object s, PropertyValueChangedEventArgs e)
        {
            if (AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }

        }

        void mLayerTreeView_Click(object sender, EventArgs e)
        {
            MapLayer mapLayer = null;

            if (mLayerTreeView.SelectedNode != null)
            {
                mapLayer = mLayerTreeView.SelectedNode.Tag as MapLayer;
                mDisplayer.Instance = mapLayer;
            }
        }

        public void RefreshAll()
        {
            // We'll do the inefficient way for now, but move to an efficient way when it matters

            mLayerTreeView.Nodes.Clear();

            if (mTiledMapSave != null)
            {
                foreach (var layer in mTiledMapSave.Layers)
                {
                    TreeNode node = new TreeNode(layer.Name);
                    node.Tag = layer;
                    mLayerTreeView.Nodes.Add(node);
                }
            }

        }

        internal void HandleAddPropertyClick()
        {
            var layer = AppState.Self.CurrentMapLayer;
            if (layer == null)
            {
                MessageBox.Show("You must first select a Layer");
            }
            else
            {
                NewPropertyWindow window = new NewPropertyWindow();
                DialogResult result = window.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string name = window.ResultName;
                    string type = window.ResultType;
                    var newProperty = new TMXGlueLib.property();
                    if (!string.IsNullOrEmpty(type))
                    {
                        newProperty.name = name + " (" + type + ")";

                    }
                    else
                    {
                        newProperty.name = name;
                    }
                    layer.properties.Add(newProperty);
                    mDisplayer.UpdateDisplayedProperties();
                    mDisplayer.PropertyGrid.Refresh();

                    if (AnyTileMapChange != null)
                    {
                        AnyTileMapChange(this, null);
                    }
                }

            }
        }

        internal void HandleRemovePropertyClick()
        {
            property property = AppState.Self.CurrentLayerProperty;

            if(property != null)
            {
                var result = 
                    MessageBox.Show("Are you sure you'd like to remove the property " + property.name + "?", "Remove property?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    AppState.Self.CurrentMapLayer.properties.Remove(property);
                    mDisplayer.UpdateDisplayedProperties();
                    mDisplayer.PropertyGrid.Refresh();
                    if (AnyTileMapChange != null)
                    {
                        AnyTileMapChange(this, null);
                    }
                }
            }
        }
    }
}
