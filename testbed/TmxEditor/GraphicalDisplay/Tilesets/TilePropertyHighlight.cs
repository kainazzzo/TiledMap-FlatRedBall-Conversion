using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RenderingLibrary.Math.Geometry;
using RenderingLibrary;
using RenderingLibrary.Graphics;

namespace TmxEditor.GraphicalDisplay.Tilesets
{
    public class TilePropertyHighlight : IPositionedSizedObject
    {
        #region Fields



        SystemManagers mManagers;
        LineRectangle mLineRectangle;
        Text mCountDisplay;

        #endregion

        public List<IPositionedSizedObject> Children
        {
            get;
            private set;
        }

        public float Height
        {
            get
            {
                return mLineRectangle.Height;
            }
            set
            {
                mLineRectangle.Height = value;
                mCountDisplay.Height = value;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public IPositionedSizedObject Parent
        {
            get;
            set;
        }

        public object Tag
        {
            get;
            set;
        }

        public float Width
        {
            get
            {
                return mLineRectangle.Width;
            }
            set
            {
                mLineRectangle.Width = value;
                mCountDisplay.Width = value;
            }
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public float Z
        {
            get;
            set;
        }

        public int Count
        {
            set
            {
                mCountDisplay.RawText = value.ToString();
            }
        }

        public float Rotation
        {
            get;
            set;
        }

        public TilePropertyHighlight(SystemManagers managers)
        {
            Children = new List<IPositionedSizedObject>();
            mManagers = managers;
            mLineRectangle = new LineRectangle(mManagers);
            mLineRectangle.Width = 16;
            mLineRectangle.Height = 16;
            mLineRectangle.Z = 1;
            mLineRectangle.Parent = this;

            mLineRectangle.Color = new Microsoft.Xna.Framework.Color(1, 1, 1, .3f);


            mCountDisplay = new Text(mManagers, "99");
            mCountDisplay.HorizontalAlignment = HorizontalAlignment.Left;
            mCountDisplay.VerticalAlignment = VerticalAlignment.Top;
            mCountDisplay.Z = 1;
            mCountDisplay.Parent = this;
        }


        public void AddToManagers()
        {
            mManagers.ShapeManager.Add(mLineRectangle);

            mManagers.TextManager.Add(mCountDisplay);
        }

        public void RemoveFromManagers()
        {
            mManagers.ShapeManager.Remove(this.mLineRectangle);
            mManagers.TextManager.Remove(this.mCountDisplay);

        }




        void IPositionedSizedObject.SetParentDirect(IPositionedSizedObject newParent)
        {
            // do nothing
        }
    }
}
