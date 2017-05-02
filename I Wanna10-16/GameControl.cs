using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace I_Wanna
{
    class GameControl : Main
    {
        enum Canvas
        {
            Start,Editor,Game
        }
        static Archive save;
        protected static KeyboardState ks;
        readonly static float dx=0.01f, dy=0.021f;
        protected static Imgid iid = new Imgid();
        static LandDate[] buff_land;
        static LandDate[] buff_needle;
        //static LandDate[] buff_door;
        static LandDate[] buff_record;
        struct State
        {
            public int interval;
            public Canvas can;
        }
        static State current;
        static void Initial()
        {
            LandDate l = new LandDate()
            {
                active = true,
                location = new Point2(0, -0.94f),
                w = 2,
                h = 0.12f,
                distance = 1.2f
            };
            buff_land = new LandDate[] { l };
            buff_needle = null;
            LoadLand(ref buff_land, iid.land);
            LoadRole();
            buff_img[iid.needle].count = 0;
            buff_t[0].reg = false;
            game.execute = () => { GamePage.page.ShowDeclare(); };
        }
        public static void Start()
        {
            AsyncManage.AsyncDelegate(() =>{ FileManage.LoadAppLayout("001.png" ,(t)=> { maps = t; }); });
            int pid = RegPicture("polygon");
            iid.land = RegImage(pid, 0);
            buff_land = new LandDate[64];
            VertexPositionTexture[] vpt = new VertexPositionTexture[384];//64*6
            for (int i = 1; i < 384; i++)
            {
                vpt[i].TextureCoordinate.Y = 1;
                i++;
                vpt[i].TextureCoordinate.X = 0.25f;
                i++;
                vpt[i].TextureCoordinate.X = 0.25f;
                i++;
                vpt[i].TextureCoordinate.Y = 1;
                i++;
                vpt[i].TextureCoordinate.X = 0.25f;
                vpt[i].TextureCoordinate.Y = 1;
                i++;
            }
            buff_img[iid.land].vertex = vpt;
            iid.needle= RegImage(pid, 0);
            vpt = new VertexPositionTexture[384];//128*3
            for (int i = 0; i < 384; i++)
            {
                vpt[i].TextureCoordinate.X = 0.25f;
                vpt[i].TextureCoordinate.Y = 1;
                i++;
                vpt[i].TextureCoordinate.X = 0.375f;
                i++;
                vpt[i].TextureCoordinate.X = 0.5f;
                vpt[i].TextureCoordinate.Y = 1;
            }
            buff_img[iid.needle].vertex = vpt;
            save = FileManage.LoadArichive();
            role = new Role();
            role.points = new Point2[4];
            role.pid = RegPicture("wanna");
            role.imgid = RegImage(role.pid, 0);
            vpt = new VertexPositionTexture[6];
            buff_img[role.imgid].vertex = vpt;
            SetTex_PosA(ref buff_img[role.imgid].vertex, 0, Vector2.Zero);
            Initial();
            buff_t[0].col.A = 255;
            buff_t[0].col.R = 255;
        }
        public static void Update()
        {
            ks = Keyboard.GetState();
            if(ks.IsKeyDown(Keys.Escape))
            {
                if(current.interval < 0)
                {
                    if(current.can==Canvas.Start)
                    game.Exit();
                    else if(current.can==Canvas.Game)
                        Initial();
                    else
                    {
                        Editor.Dispose();
                        Initial();
                    }
                    current.can = Canvas.Start;
                    current.interval = 15;
                }
            }
            if(ks.IsKeyDown(Keys.F12))
            {
                if (current.interval < 0)
                    if (current.can == Canvas.Start)
                    {
                        Editor.Initial();
                        game.execute = GamePage.page.HideDeclare;
                        current.can = Canvas.Editor;
                        current.interval = 15;
                    }
            }
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                mousedown = true;
            else mousedown = false;
            current.interval--;
            Point p = Mouse.GetState().Position;
            mouseX = p.X;
            mouseY = p.Y;
            if(current.can==Canvas.Editor)
                Editor.EventUpdate();
            else UpdateRole();
        }

        #region land
        protected static void SetDefRect(ref Point2[] p, Point2 location)
        {
            float x = location.x - 0.03375f;
            float x1 = location.x + 0.03375f;
            float y = location.y - 0.06f;
            float y1 = location.y + 0.06f;
            p[0].x = x;
            p[0].y = y;
            p[1].x = x;
            p[1].y = y1;
            p[2].x = x1;
            p[2].y = y1;
            p[3].x = x1;
            p[3].y = y;
        }
        protected static void LoadLand(ref LandDate[] l,int imgid)
        {
            VertexPositionTexture[] vpt = buff_img[iid.land].vertex;
            int count = -1;
            for (int i = 0; i < l.Length; i++)
            {
                if(l[i].active)
                {
                    Point2[] p = new Point2[4];
                    float w = l[i].w * 0.5f;
                    float h = l[i].h * 0.5f;
                    float x = l[i].location.x - w;
                    float x1 = l[i].location.x + w;
                    float y = l[i].location.y - h;
                    float y1 = l[i].location.y + h;
                    p[0].x = x;
                    p[0].y = y;
                    p[1].x = x;
                    p[1].y = y1;
                    p[2].x = x1;
                    p[2].y = y1;
                    p[3].x = x1;
                    p[3].y = y;
                    l[i].points = p;
                    CaculRect_Pos(ref buff_img[imgid].vertex, l[i].location, l[i].w, l[i].h, i * 6);
                    count = i;
                }
                else
                {
                    int c = i * 6;
                    for(int t=0;t<6;t++)
                    {
                        vpt[c].Position.X = 0;
                        vpt[c].Position.Y = 0;
                        c++;
                    }
                }
            }
            buff_img[imgid].count = (count+1)*2;
        }
        static void CaculLand()
        {
            role.flooer = false;
            for(int i=0;i<buff_land.Length;i++)
            {
                if(buff_land[i].active)
                {
                    float w = buff_land[i].location.x - role.location.x;
                    if (w < 0)
                        w = -w;
                    if(w<buff_land[i].w*2)
                    {
                        w = buff_land[i].location.y - role.location.y;
                        if (w < 0)
                            w = -w;
                        if(w<buff_land[i].h*2)
                        CheckLandA(ref buff_land[i].points);
                    }
                }
            }
        }
        static void CheckLandA(ref Point2[] p)
        {
            if(PToP2A(ref p,ref motion))
            {
                Point2 v1 = new Point2();
                Point2 v2 = new Point2();
                Point2[][] t = new Point2[4][];
                for(int i=0;i<4;i++)
                    t[i] = new Point2[2];
                t[0][0] = p[0];
                t[0][1] = p[1];
                t[1][0] = p[1];
                t[1][1] = p[2];
                t[2][0] = p[2];
                t[2][1] = p[3];
                t[3][0] = p[3];
                t[3][1] = p[0];
                float d=10;
                int c ,a=0;
                for(c=0;c<4;c++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (LineToLine(ref t[i], ref alex[c], ref v1))
                        {
                            float x = role.location.x - v1.x;
                            x *= x;
                            float y = role.location.y - v1.y;
                            y *= y;
                            x += y;
                            if (x < d)
                            { d = x; v2 = v1;a = i;}
                        }
                    }
                }
                if(a==0)
                    role.motion.x = -0.0001f;
                else if(a==1)
                {
                    role.motion.y = 0.000001f;
                    role.flooer = true;
                    role.jump = 1;
                }
                else if(a==2)
                    role.motion.x = 0.0001f;
                else role.motion.y = -0.001f;
            }
        }
        #endregion

        #region role
        protected static Role role;
        static Point2[] line_x1, line_x2, line_y1, line_y2;
        static Point2[][] alex;//anti line extend
        static Point2[] motion;
        protected static void LoadRole()
        {
            role.origin.y = -0.82f;
            role.location.y = -0.82f;
            int id = role.imgid;
            CaculRect_PosA(ref buff_img[id].vertex, role.location, 0.05625f, 0.1f, 0);
            buff_img[id].count = 2;
            GetPoints(new Point2(), ref role.points);
            line_x1 = new Point2[2];
            line_x2 = new Point2[2];
            line_y1 = new Point2[2];
            line_y2 = new Point2[2];
            alex = new Point2[4][];
            for(int i=0;i<4;i++)
                alex[i] = new Point2[2];
            motion = new Point2[4];
        }
        static void SetTex_Pos(ref VertexPositionTexture[] v, int s, Vector2 p)
        {
            float x2 = p.X + 0.1666666f;
            //(x,y2)(x,y)(x2,y2) (x2,y2)(x,y)(x2,y)
            v[s].TextureCoordinate.X = p.X;
            v[s].TextureCoordinate.Y = 1;
            s++;
            v[s].TextureCoordinate.X = p.X;
            v[s].TextureCoordinate.Y = 0;
            s++;
            v[s].TextureCoordinate.X = x2;
            v[s].TextureCoordinate.Y = 1;
            s++;
            v[s].TextureCoordinate.X = x2;
            v[s].TextureCoordinate.Y = 1;
            s++;
            v[s].TextureCoordinate.X = p.X;
            v[s].TextureCoordinate.Y = 0;
            s++;
            v[s].TextureCoordinate.X = x2;
            v[s].TextureCoordinate.Y = 0;
        }
        protected static void SetTex_Pos(ref VertexPositionTexture[] v, int s, float x,float y,float x1,float y1)
        {
            //(x,y1)(x,y)(x1,y1) (x1,y1)(x,y)(x1,y)
            v[s].TextureCoordinate.X = x;
            v[s].TextureCoordinate.Y = y1;
            s++;
            v[s].TextureCoordinate.X = x;
            v[s].TextureCoordinate.Y = y;
            s++;
            v[s].TextureCoordinate.X = x1;
            v[s].TextureCoordinate.Y = y1;
            s++;
            v[s].TextureCoordinate.X = x1;
            v[s].TextureCoordinate.Y = y1;
            s++;
            v[s].TextureCoordinate.X = x;
            v[s].TextureCoordinate.Y = y;
            s++;
            v[s].TextureCoordinate.X = x1;
            v[s].TextureCoordinate.Y = y;
        }
        static void SetTex_PosA(ref VertexPositionTexture[] v, int s, Vector2 p)
        {
            float x2 = p.X + 0.1666666f;
            //(x,y2)(x2,y2)(x,y) (x,y)(x2,y2)(x2,y)
            v[s].TextureCoordinate.X = p.X;
            v[s].TextureCoordinate.Y = 1;
            s++;
            v[s].TextureCoordinate.X = x2;
            v[s].TextureCoordinate.Y = 1;
            s++;
            v[s].TextureCoordinate.X = p.X;
            v[s].TextureCoordinate.Y = 0;
            s++;
            v[s].TextureCoordinate.X = p.X;
            v[s].TextureCoordinate.Y = 0;
            s++;
            v[s].TextureCoordinate.X = x2;
            v[s].TextureCoordinate.Y = 1;
            s++;
            v[s].TextureCoordinate.X = x2;
            v[s].TextureCoordinate.Y = 0;
        }
        protected static void GetPoints(Point2 p, ref Point2[] v)
        {
            float x1 = p.x - 0.015f;
            float x2 = p.x + 0.015f;
            float y1 = p.y - 0.04f;
            float y2 = p.y + 0.03f;
            v[0].x = x1;
            v[0].y = y1;
            v[1].x = x1;
            v[1].y = y2;
            v[2].x = x2;
            v[2].y = y2;
            v[3].x = x2;
            v[3].y = y1;
        }
        static void GetMotionLine()
        {
            float x = role.location.x + role.motion.x;
            float y = role.location.y + role.motion.y;
            float x1 = x+0.016f;
            x -= 0.016f;
            float y1 = y+0.03f;
            y -= 0.044f;
            motion[0].x = x;
            motion[0].y = y;
            motion[1].x = x;
            motion[1].y = y1;
            motion[2].x = x1;
            motion[2].y = y1;
            motion[3].x = x1;
            motion[3].y = y;
            for (int i=0;i<4;i++)
            {
                alex[i][0].x = role.points[i].x;
                alex[i][0].y = role.points[i].y;
                alex[i][1].x = motion[i].x;
                alex[i][1].y = motion[i].y;
            }
        }
        public static void UpdateRole()
        {
            role.time--;
            if (ks.IsKeyDown(Keys.J))
            {
                if(role.jump>0 & role.time<0)
                {
                    role.time = 15;
                    if(!role.flooer)
                       role.jump--;
                    role.motion.y = dy;
                }
            }
            if (ks.IsKeyDown(Keys.K))
            {
                if (role.jump > 0 & role.time < 0)
                {
                    role.time = 15;
                    if (!role.flooer)
                        role.jump--;
                    role.motion.y = dy*0.8f;
                }
            }
            if(ks.IsKeyDown(Keys.F1))
            {
                if(role.time<0)
                {
                    role.time = 15;
                    role.loc_back = role.location;
                    save.achi_count++;
                    save.loaction = role.location;
                    buff_t[0].text = "Die:" + save.die_count.ToString() +
                                        "\r\n" + "Archive:" + save.achi_count.ToString();
                    buff_t[0].screen_pos.X = screenX * 0.5f - 40;
                }
            }
            if(ks.IsKeyDown(Keys.R))
            {
                if (role.time < 0)
                {
                    role.time = 15;
                    role.location = role.origin;
                    save.achi_count++;
                    save.loaction = role.location;
                }
            }
            if (ks.IsKeyDown(Keys.Left)| ks.IsKeyDown(Keys.A))
            {
                role.backword = true;
                if(role.motion.x>-dx)
                   role.motion.x -= 0.0008f;
                role.index++;
                if (role.index >= 24)
                    role.index = 5;
            }
            else
            if (ks.IsKeyDown(Keys.Right)| ks.IsKeyDown(Keys.D))
            {
                role.backword = false;
                if (role.motion.x < dx)
                    role.motion.x += 0.0008f;
                role.index++;
                if (role.index >= 24)
                    role.index = 5;
            }
            else { role.motion.x = 0;role.index = 0; }
            if (role.motion.y>-0.022f)
              role.motion.y -= 0.001f;
            GetMotionLine();
            CaculLand();
            CaculNeedle();
            float x= role.location.x + role.motion.x;
            if (x > -1 & x <1)
                role.location.x = x;
            else if(x>1)
            {
                if(save.level<maps.Length)
                {
                    buff_t[0].reg = true;
                    buff_t[0].text = "Die:" + save.die_count.ToString() +
                                        "\r\n" + "Archive:" + save.achi_count.ToString();
                    buff_t[0].screen_pos.X = screenX * 0.5f - 40;
                    if (current.can == Canvas.Game)
                        save.level++;
                    else game.execute = () => { GamePage.page.HideDeclare(); };
                    LoadLayOut(maps[save.level]);
                    save.loaction = role.origin;
                    role.loc_back = role.origin;
                    current.can = Canvas.Game;
                    FileManage.SaveArichive(save);
                    return;
                }
            }
            x= role.location.y + role.motion.y;
            if (x > -1 & x < 1)
                role.location.y = x;
            GetPoints(role.location,ref role.points);
            int c = role.index / 5;
            Vector2 v = Vector2.Zero;
            v.X = c * 0.1666666f;
            if(role.backword)
            {
                SetTex_Pos(ref buff_img[role.imgid].vertex, 0, v);
                CaculRect_Pos(ref buff_img[role.imgid].vertex, role.location, 0.05625f, 0.1f, 0);
            }
            else
            {
                SetTex_PosA(ref buff_img[role.imgid].vertex, 0, v);
                CaculRect_PosA(ref buff_img[role.imgid].vertex, role.location, 0.05625f, 0.1f, 0);
            }
        }
        #endregion

        #region needle
        static void CaculNeedle()
        {
            if (buff_needle != null)
                for (int i = 0; i < buff_needle.Length; i++)
                {
                    if(buff_needle[i].active)
                    {
                        float x = role.location.x - buff_needle[i].location.x;
                        if (x < 0)
                            x = -x;
                        if (x < buff_needle[i].distance)
                        {
                            x = role.location.y - buff_needle[i].location.y;
                            if (x < 0)
                                x = -x;
                            if (x < buff_needle[i].distance)
                            {
                                if (PToP2A(ref buff_needle[i].points, ref motion))
                                {
                                    role.location = role.loc_back;
                                    role.motion.x = 0;
                                    role.motion.y = 0;
                                    save.die_count++;
                                    buff_t[0].text = "Die:" + save.die_count.ToString() +
                                        "\r\n" + "Archive:" + save.achi_count.ToString();
                                    buff_t[0].screen_pos.X = screenX * 0.5f - 40;
                                    FileManage.SaveArichive(save);
                                    return;
                                }
                            }
                        }
                    }
                }
        }
        protected static void LoadNeedle(ref LandDate[] l,int imgid)
        {
            VertexPositionTexture[] v = buff_img[iid.needle].vertex;
            int count = -1;
            for(int i=0;i<l.Length;i++)
            {
                if(l[i].active)
                {
                    Point2[] p = new Point2[3];
                    float scale = l[i].w;
                    if (scale != 0)//0==def 1
                    {
                        l[i].distance = scale * 0.12f + 0.06f;
                        float a = l[i].h;
                        if (a != 0)//angle
                        {
                            p[0].x = tri_mod[0].x;
                            p[0].y = scale * tri_mod[0].y;
                            p[1].x = tri_mod[1].x;
                            p[1].y = scale * tri_mod[1].y;
                            p[2].x = tri_mod[2].x;
                            p[2].y = scale * tri_mod[2].y;
                            p = RotatePoint2(ref p, ref l[i].location, a,0.5625f);
                            l[i].points = p;
                        }
                        else
                        {
                            float x = l[i].location.x;
                            float y = l[i].location.y;
                            p[0].x = scale * tri_modA[0].x + x;
                            p[0].y = y;
                            p[1].x = x;
                            p[1].y = scale * tri_modA[1].y + y;
                            p[2].x = scale * tri_modA[2].x + x;
                            p[2].y = y;
                            l[i].points = p;
                        }
                    }
                    else
                    {
                        l[i].distance = 0.18f;
                        float a = l[i].h;
                        if (a != 0)//angle
                        {
                            p = RotatePoint2(ref tri_mod, ref l[i].location, a,0.5625f);
                            l[i].points = p;
                        }
                        else
                        {
                            float x = l[i].location.x;
                            float y = l[i].location.y;
                            p[0].x = tri_modA[0].x + x;
                            p[0].y = y;
                            p[1].x = x;
                            p[1].y = tri_modA[1].y + y;
                            p[2].x = tri_modA[2].x + x;
                            p[2].y = y;
                            l[i].points = p;
                        }
                    }
                    unsafe
                    {
                        fixed (VertexPositionTexture* t = &buff_img[imgid].vertex[i * 3])
                        {
                            VertexPositionTexture* vpt = t;
                            vpt->Position.X = p[0].x;
                            vpt->Position.Y = p[0].y;
                            vpt->TextureCoordinate.X = 0.25f;
                            vpt->TextureCoordinate.Y = 1;
                            vpt++;
                            vpt->Position.X = p[1].x;
                            vpt->Position.Y = p[1].y;
                            vpt->TextureCoordinate.X = 0.375f;
                            vpt++;
                            vpt->Position.X = p[2].x;
                            vpt->Position.Y = p[2].y;
                            vpt->TextureCoordinate.X = 0.5f;
                            vpt->TextureCoordinate.Y = 1;
                        }
                    }
                    count = i;
                }
                else
                {
                    int c = i * 3;
                    for(int t=0;t<3;t++)
                    {
                        v[c].Position.X = 0;
                        v[c].Position.Y = 0;
                        c++;
                    }
                }
            }
            buff_img[imgid].count = (count+1);
        }
        #endregion

        protected static void DebugLayout(ref LandDate[] l,ref LandDate[] n)
        {
            buff_land = l;
            buff_needle = n;
        }

        #region recorde
        protected static void LoadRecord(ref LandDate[] lay,int imgid)
        {
            int c = lay.Length;
            if (c > 4)
                c = 4;
            int d = 0;
            for (int i = 0; i < c; i++)
            {
                if (lay[i].active)
                {
                    if (lay[i].points == null)
                        lay[i].points = new Point2[4];
                    SetDefRect(ref lay[i].points, lay[i].location);
                    CaculRect_Pos(ref buff_img[imgid].vertex, lay[i].location, 0.0675f, 0.12f, i * 6);
                    d = i;
                }
            }
            d++;
            d *= 2;
            buff_img[imgid].count = d;
        }
        static int CheckRecord()
        {
            for (int i = 0; i < buff_record.Length; i++)
            {
                if (buff_record[i].active)
                {
                    float w = buff_record[i].location.x - role.location.x;
                    if (w < 0)
                        w = -w;
                    if (w < 0.0675f)
                    {
                        w = buff_record[i].location.y - role.location.y;
                        if (w < 0)
                            w = -w;
                        if (w < 0.12f)
                            return i;
                    }
                }
            }
            return -1;
        }
        #endregion

        #region initial data
        public static void LoadLayOut(LayOut l)
        {
            buff_land = l.land;
            LoadLand(ref buff_land,iid.land);
            buff_needle = l.needle;
            LoadNeedle(ref buff_needle,iid.needle);
            role.origin = l.role.origin;
            role.location = role.origin;
            role.motion.x = 0;
            role.motion.y = 0;
        }
        #endregion
    }
}
