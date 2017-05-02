using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace I_Wanna
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        BasicEffect[] buff_be;
        BasicEffect be;
        SpriteFont sf;
        public bool Pause { get; set; }
        public int delay { get; set; }
        public Action execute;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Main.game = this;
            AsyncManage.Inital();
            AsyncManage.AsyncDelegate(()=> { GameControl.Start(); });
            buff_be = new BasicEffect[24];
            delay = 0;
        }
       
        protected override void Initialize()
        {
            base.Initialize();
        }
        void LoadPic(ref AssetsPic[] ap)
        {
            for (int i = 0; i < 24; i++)
            {
                if (ap[i].reg)
                    if (!ap[i].done)
                    {
                        if (buff_be[i] == null)
                        {
                            buff_be[i] = new BasicEffect(GraphicsDevice);
                            buff_be[i].TextureEnabled = true;
                        }
                        ap[i].t2d = Content.Load<Texture2D>(ap[i].path);
                        buff_be[i].Texture = ap[i].t2d;
                        ap[i].done = true;
                    }
            }
        }
        protected override void Update(GameTime gameTime)
        {
            Main.screenX = GraphicsDevice.ScissorRectangle.Width;
            Main.screenY = GraphicsDevice.ScissorRectangle.Height;
            AsyncManage.AsyncDelegate(GameControl.Update);
            if (execute != null)
            { execute(); execute = null; }
            LoadPic(ref Main.buff_pic);
            base.Update(gameTime);
        }
        void DrawImage(ref ImageProperty[] img)
        {
            for (int i = 0; i < 32; i++)
            {
                if (img[i].reg & img[i].count > 0)
                {
                    int id = img[i].a_id;
                    be = buff_be[id];
                    var pass = be.CurrentTechnique.Passes[0];
                    pass.Apply();
                    int c = img[i].count;
                    GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, img[i].vertex, 0, c);
                }
            }
        }
        void DrawText(ref Textproperty[] t)
        {
            for(int i=0;i<8;i++)
            {
                if(t[i].reg)
                {
                    spriteBatch.DrawString(sf, t[i].text, t[i].screen_pos,t[i].col);
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            if (Pause)
            {
                if (delay > 0)
                    delay--;
                else
                    return;
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawImage(ref Main.buff_img);
            spriteBatch.Begin();
            DrawText(ref Main.buff_t);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sf = Content.Load<SpriteFont>("font");
        }
        protected override void UnloadContent()
        {

        }
    }
}
