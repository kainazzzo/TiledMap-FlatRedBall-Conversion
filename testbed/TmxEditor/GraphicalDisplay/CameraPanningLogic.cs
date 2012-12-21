using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XnaAndWinforms;
using RenderingLibrary;
using Cursor = InputLibrary.Cursor;
using InputLibrary;

namespace FlatRedBall.SpecializedXnaControls.Input
{
    public class CameraPanningLogic
    {
        RenderingLibrary.Camera mCamera;
        GraphicsDeviceControl mControl;

        Cursor mCursor;
        Keyboard mKeyboard;

        SystemManagers mManagers;

        public CameraPanningLogic(GraphicsDeviceControl graphicsControl, SystemManagers managers, Cursor cursor, Keyboard keyboard)
        {
            mManagers = managers;
            
            mKeyboard = keyboard;

            mCursor = cursor;
            mCursor.Initialize(graphicsControl);
            mCamera = managers.Renderer.Camera;
            mControl = graphicsControl;
            graphicsControl.XnaUpdate += new Action(Activity);

        }

        void Activity()
        {
            
            if (mCursor.MiddleDown && 
                mCursor.IsInWindow)
            {
                mCamera.X -= mCursor.XChange / mManagers.Renderer.Camera.Zoom;
                mCamera.Y -= mCursor.YChange / mManagers.Renderer.Camera.Zoom;
            }

        }


    }
}
