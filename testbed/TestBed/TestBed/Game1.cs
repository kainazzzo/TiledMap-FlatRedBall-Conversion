using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Utilities;

using TestBed.Screens;
using FlatRedBall.IO;

namespace TestBed
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
			
			BackStack<string> bs = new BackStack<string>();
			bs.Current = string.Empty;
        }

        protected override void Initialize()
        {
            Renderer.UseRenderTargets = false;
            FlatRedBallServices.InitializeFlatRedBall(this, graphics);
			GlobalContent.Initialize();


			Screens.ScreenManager.Start(typeof(TestBed.Screens.TestScreen).FullName);

            SpriteManager.Camera.BackgroundColor = Color.White;
            TiledMapSave tms = TiledMapSave.FromFile("desertgzip.tmx");
            Scene s = tms.ToScene(typeof(TestBed.Screens.TestScreen).FullName);
            s.AddToManagers();

            SpriteManager.Camera.Position.Z += 50;
            SpriteManager.Camera.CameraCullMode = CameraCullMode.None;
            FlatRedBallServices.GraphicsOptions.TextureFilter = TextureFilter.Point;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                SpriteManager.Camera.Y += 3;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                SpriteManager.Camera.Y -= 3;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                SpriteManager.Camera.X -= 3;
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                SpriteManager.Camera.X += 3;
            }
            if (keyboardState.IsKeyDown(Keys.OemPlus))
            {
                SpriteManager.Camera.Z -= 3;
            }
            if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                SpriteManager.Camera.Z += 3;
            }


            FlatRedBallServices.Update(gameTime);

            ScreenManager.Activity();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            FlatRedBallServices.Draw();

            base.Draw(gameTime);
        }
    }
}
