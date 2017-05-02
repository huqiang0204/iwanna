using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace I_Wanna
{
    class Editor:GameControl
    {
        #region
        enum Target : int
        {
           None,Land,Needle, CreateMenu,LandMenu,NeedleMenu,Role,Record,RecordMenu
        }
        static LandDate createBar;
        public static LayOut layout;
        static LayOut view;
        static Point2 mousePoint;
        static long time;
        static int presstime=0;
        static bool mousestate;
        static bool test,pause;
        struct Selected
        {
            public int land;
            public int needle;
            public int record;
            public int pressdbutton;
            public Point2 mousepoint;
            public bool drag;
            public Target menu;
            public Target target;//
        }
        static Selected select = new Selected();
        static bool initail;
        public static void Initial()
        {
            if(initail)
            {
                for (int i = 0; i < 64; i++)
                    layout.land[i].active = false;
                for (int i = 0; i < 128; i++)
                    layout.needle[i].active = false;
            }
            else
            {
                int c = RegPicture("other");
                iid.other = RegImage(c, 4);
                c = RegPicture("polygon");
                iid.bar = RegImage(c, 0);
                iid.record = RegImage(c, 8);
                layout.land = new LandDate[64];
                layout.needle = new LandDate[128];
                VertexPositionTexture[]  vpt = new VertexPositionTexture[6];
                vpt[0].TextureCoordinate.Y = 1;
                vpt[2].TextureCoordinate.X = 0.75f;
                vpt[2].TextureCoordinate.Y = 1;
                vpt[3].TextureCoordinate.X = 0.75f;
                vpt[3].TextureCoordinate.Y = 1;
                vpt[5].TextureCoordinate.X = 0.75f;
                buff_img[iid.bar].vertex = vpt;
                createBar = new LandDate();
                layout.tool = new LandDate[4];
                layout.tool[0].points = new Point2[4];
                SetTex_Pos(ref buff_img[iid.other].vertex, 0, 0, 0, 0.25f, 1);
                buff_img[iid.other].count = 2;
                layout.record = new LandDate[8];
            }
            buff_img[iid.land].count = 0;
            role.location.x = -0.88f;
            role.location.y = -0.82f;
            LoadRole();
            initail = true;
        }
        public static void EventUpdate()
        {
            presstime--;
            if(presstime<0)
            if (ks.IsKeyDown(Keys.F2))
               {
                    presstime = 15;
                    if (pause)
                    {
                        pause = false;
                        game.Pause = false;
                        game.execute = () => { GamePage.page.HideMenu(); };
                    }
                    else
                    {
                        pause = true;
                        game.Pause = true;
                        game.execute = () => { GamePage.page.CreateMenu(); };
                        return;
                    }
                }
            if (pause)
                return;
            if (test)
            {
                if (presstime < 0)
                    if (ks.IsKeyDown(Keys.F5))
                    {
                        presstime = 15;
                        test = false;
                        return;
                    }
                UpdateRole();
                return;
            }
            else
            {
                if (presstime < 0)
                    if (ks.IsKeyDown(Keys.F5))
                    {
                        presstime = 15;
                        buff_img[iid.other].count = 0;
                        buff_img[iid.bar].count = 0;
                        DebugLayout(ref layout.land, ref layout.needle);
                        test = true;
                        return;
                    }
            }
            float x = (mouseX / screenX - 0.5f) * 2;
            mousePoint.x = x;
            float y = (0.5f - mouseY / screenY) * 2;
            mousePoint.y = y;
            if(mousedown)
            {
                if(!mousestate)
                {
                    time = DateTime.Now.Ticks;
                    mousestate = true;
                }
                DragEvent();
            }
            else
            {
                select.drag = false;
                if(mousestate)
                {
                    mousestate = false;
                    if (DateTime.Now.Ticks - time < 1500000)
                        ClickEvent();
                }
            }
        }
        static void ClickEvent()
        {
            if (CheckRole())
                return;
            int c;
            if(select.target==Target.Land|select.target==Target.LandMenu)
            {
                c = CheckButton();
                if (c > -1)
                {
                    select.pressdbutton = c;
                    if (c == 0)
                    {
                        DeleteLand(select.land);
                        select.target = Target.None;
                        buff_img[iid.other].count = 0;
                    }
                    return;
                }
            }else if(select.target==Target.Needle|select.target==Target.NeedleMenu)
            {
                c = CheckButton();
                if(c>-1)
                {
                    select.pressdbutton = c;
                    if(c==0)
                    {
                        DeleteNeedle(select.needle);
                        select.target = Target.None;
                        buff_img[iid.other].count = 0;
                    }
                    return;
                }
            }else if(select.target==Target.Record|select.target==Target.RecordMenu)
            {
                c = CheckButton();
                if(c>-1)
                {
                    DeleteRecord();
                    select.target = Target.None;
                    buff_img[iid.other].count = 0;
                }
            }
            c = CheckRecord();
            if(c>-1)
            {
                CreateRecordMenu(c);
                select.menu = Target.RecordMenu;
                return;
            }
            c = CheckLand();
            if (c > -1)
            {
                CreateLandMenu(c);
                select.menu = Target.LandMenu;
                return;
            }
            c = CheckNeedle();
            if (c > -1)
            {
                CreateNeedleMenu(c);
                select.menu = Target.NeedleMenu;
                return;
            }
            if (select.target == Target.CreateMenu)
            {
                c = CheckBar();
                if(c>-1)
                {
                    if (c == 0)
                        AddLand(createBar.location);
                    else if (c == 1)
                        AddNeedle(createBar.location);
                    else AddRecord(createBar.location);
                    buff_img[iid.bar].count = 0;
                    select.target = Target.None;
                    return;
                }    
            }
            select.target = Target.CreateMenu;
            CreateBar();
        }
        static void DragEvent()
        {
            if (select.drag)
                Draging();
            else CheckDrag();
        }
        static void CheckDrag()
        {
            if( CheckRole())
            {
                select.drag = true;
                return;
            }
            int c;
            if(select.target==Target.Land |select.target==Target.LandMenu)
            {
                c = CheckButton();
                if (c > -1)
                {
                    select.pressdbutton = c;
                    select.target = Target.LandMenu;
                    if (c > 0)
                    {
                        select.drag = true;
                        select.mousepoint = mousePoint;
                    }
                    return;
                }
            }
            else if (select.target == Target.Needle|select.target==Target.NeedleMenu)
            {
                c = CheckButton();
                if (c > -1)
                {
                    select.target = Target.NeedleMenu;
                    select.pressdbutton = c;
                    if (c > 0)
                    {
                        select.drag = true;
                        select.mousepoint = mousePoint;
                    }
                    return;
                }
            }
            c = CheckRecord();
            if(c>-1)
            {
                select.drag = true;
                return;
            }
            c = CheckLand();
            if (c > -1)
            {
                select.mousepoint = mousePoint;
                select.drag = true;
                return;
            }
            c = CheckNeedle();
            if (c > -1)
            {
                select.mousepoint = mousePoint;
                select.drag = true;
                return;
            }
        }
        static void Draging()
        {
            float x = mousePoint.x - select.mousepoint.x;
            float y = mousePoint.y - select.mousepoint.y;
            select.mousepoint = mousePoint;
            if (select.target == Target.Role)
                MoveRole();
            else if (select.target == Target.Record)
                MoveRecord();
            else if (select.target == Target.LandMenu)
                ChangeLandSize(x, y);
            else if (select.target == Target.NeedleMenu)
                ChangeNeedleSize();
            else if (select.target == Target.Land)
                MoveLand(x, y);
            else if (select.target == Target.Needle)
                MoveNeedle(x, y);
        }
        static int CheckBar()
        {
            float x = createBar.location.x;
            float x1 = x + 0.2025f;
            if (mousePoint.x > x & mousePoint.x < x1)
            {
                float y = createBar.location.y;
                float y1 = y - 0.12f;
                if (mousePoint.y > y1 & mousePoint.y < y)
                {
                    x1 = mousePoint.x - x;
                    x1 *= 14.8148f;
                    return (int)x1;
                }
            }
            return -1;
        }
        static int CheckButton()
        {
            int e = 3;
            if (select.target == Target.Record | select.target == Target.RecordMenu)
                e = 1;
            for(int i=0;i<e;i++)
            {
                float x = layout.tool[i].location.x-0.016875f;
                float x1 = x + 0.03375f;
                if(mousePoint.x>x&mousePoint.x<x1)
                {
                    x = layout.tool[i].location.y - 0.03f;
                    x1 = x + 0.06f;
                    if(mousePoint.y>x&mousePoint.y<x1)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        static int CheckLand()
        {
            LandDate[] l = layout.land;
            for(int i=0;i<l.Length;i++)
            {
                if(l[i].active)
                {
                    Point2[] p = l[i].points;
                    float x = p[0].x;
                    if (mousePoint.x > p[0].x & mousePoint.x < p[2].x)
                        if (mousePoint.y > p[0].y & mousePoint.y < p[2].y)
                        { select.land = i;select.target = Target.Land; return i; }
                }
            }
            return -1;
        }
        static int CheckNeedle()
        {
            LandDate[] n = layout.needle;
            for(int i=0;i<n.Length;i++)
            {
                if (n[i].active)
                    if (DotToPolygon(ref n[i].points, mousePoint))
                    { select.needle = i; select.target = Target.Needle; return i; }
            }
            return -1;
        }
        static void CreateBar()
        {
            float x = mousePoint.x;
            float y = mousePoint.y;
            float x1 = x + 0.2025f;
            float y1 = y - 0.12f;
            unsafe
            {
                fixed (VertexPositionTexture* t = &buff_img[iid.bar].vertex[0])
                {
                    VertexPositionTexture* vpt = t;
                    vpt->Position.X = x;
                    vpt->Position.Y = y1;
                    vpt++;
                    vpt->Position.X = x;
                    vpt->Position.Y = y;
                    vpt++;
                    vpt->Position.X = x1;
                    vpt->Position.Y = y1;
                    vpt++;
                    vpt->Position.X = x1;
                    vpt->Position.Y = y1;
                    vpt++;
                    vpt->Position.X = x;
                    vpt->Position.Y = y;
                    vpt++;
                    vpt->Position.X = x1;
                    vpt->Position.Y = y;
                }
            }
            buff_img[iid.bar].count = 2;
            createBar.location = mousePoint;
            select.menu = Target.CreateMenu;
        }
        static void CreateLandMenu(int index)
        {
            float px = layout.land[index].location.x;
            float py = layout.land[index].location.y;
            float w = layout.land[index].w*0.5f ;
            float h = layout.land[index].h*0.5f ;
            Point2 p = new Point2(px - w-0.016f, py);
            layout.tool[0].location = p;
            CaculRect_Pos(ref buff_img[iid.other].vertex,p, 0.03375f,0.06f,0);
            p.x = px + w+0.016f;
            layout.tool[1].location = p;
            CaculRect_Pos(ref buff_img[iid.other].vertex, p, 0.03375f, 0.06f, 6);
            p.x = px;
            p.y = py + h+0.03f;
            layout.tool[2].location = p;
            CaculRect_Pos(ref buff_img[iid.other].vertex, p, 0.03375f, 0.06f, 12);
            SetTex_Pos(ref buff_img[iid.other].vertex, 0, 0.375f, 0, 0.5f, 0.5f);
            SetTex_Pos(ref buff_img[iid.other].vertex, 6, 0.5f,0,0.6125f,0.5f);
            SetTex_Pos(ref buff_img[iid.other].vertex, 12, 0.5f, 0, 0.6125f, 0.5f);
            buff_img[iid.other].count = 8;
            select.menu = Target.LandMenu;
        }
        static void CreateNeedleMenu(int index)
        {
            Point2[] t = layout.needle[index].points;
            layout.tool[0].location = t[0];
            CaculRect_Pos(ref buff_img[iid.other].vertex, t[0], 0.03375f, 0.06f, 0);
            layout.tool[1].location = t[1];
            CaculRect_Pos(ref buff_img[iid.other].vertex, t[1], 0.03375f, 0.06f, 6);
            layout.tool[2].location = t[2];
            CaculRect_Pos(ref buff_img[iid.other].vertex, t[2], 0.03375f, 0.06f, 12);
            SetTex_Pos(ref buff_img[iid.other].vertex, 0, 0.375f, 0, 0.5f, 0.5f);
            SetTex_Pos(ref buff_img[iid.other].vertex, 6, 0.25f, 0, 0.375f, 0.5f);
            SetTex_Pos(ref buff_img[iid.other].vertex, 12, 0.5f, 0, 0.6125f, 0.5f);
            buff_img[iid.other].count = 8;
            select.menu = Target.NeedleMenu;
        }
        static void AddLand(Point2 location)
        {
            LandDate[] l = layout.land;
            for(int i=0;i<128;i++)
            {
                if(!l[i].active)
                {
                    l[i].active = true;
                    CaculRect_Pos(ref buff_img[iid.land].vertex,location,0.0675f,0.12f,i*6);
                    int c = (i + 1) * 2;
                    if (c > buff_img[iid.land].count)
                        buff_img[iid.land].count = c;
                    l[i].points = new Point2[4];
                    SetDefRect(ref l[i].points,location);
                    l[i].w = 0.0675f;
                    l[i].h = 0.12f;
                    l[i].location = location;
                    break;
                }
            }
        }
        static void UpdateLand(int s)
        {
            layout.land[s].active = true;
            int c = (s + 1) * 2;
            if (c > buff_img[iid.land].count)
                buff_img[iid.land].count = c;
            Point2 l = layout.land[s].location;
            float w = layout.land[s].w * 0.5f;
            float h = layout.land[s].h * 0.5f;
            float x = l.x - w;
            float x1 = l.x + w;
            float y = l.y - h;
            float y1 = l.y + h;
            Point2[] p = layout.land[s].points;
            p[0].x = x;
            p[0].y = y;
            p[1].x = x;
            p[1].y = y1;
            p[2].x = x1;
            p[2].y = y1;
            p[3].x = x1;
            p[3].y = y;
            layout.land[s].points = p;
            CaculRect_Pos(ref buff_img[iid.land].vertex, l, layout.land[s].w, layout.land[s].h, s * 6);
        }
        static void ChangeLand(int index, Point2 location)
        {

        }
        static void ChangeLandSize(float x, float y)
        {
            float t;
            int s = select.land;
            if(select.pressdbutton==1)//horizontal landscape
            {
                t = layout.land[s].w + x*2;
                if (t < 0.03f)
                    return;
                layout.land[s].w = t;
            }
            else if(select.pressdbutton==2)
            {
                t = layout.land[s].h + y*2;
                if (t < 0.06f)
                    return;
                layout.land[s].h = t;
            }
            UpdateLand(s);
            CreateLandMenu(s);
        }
        static void MoveLand(float x,float y)
        {
            int s = select.land;
            layout.land[s].location.x += x;
            layout.land[s].location.y += y;
            UpdateLand(s);
            CreateLandMenu(s);
        }
        static void DeleteLand(int index)
        {
            layout.land[index].active = false;
            int c = index * 6;
            for(int i=0;i<6;i++)
            {
                buff_img[iid.land].vertex[c].Position.X = 0;
                buff_img[iid.land].vertex[c].Position.Y = 0;
                c++;
            }
        }
        static void AddNeedle(Point2 location)
        {
            LandDate[] n = layout.needle;
            for(int i=0;i<128;i++)
            {
                if(!n[i].active)
                {
                    n[i].active = true;
                    n[i].h = 0;
                    n[i].location = location;
                    n[i].distance = 0.18f;
                    CreateNeedle(ref n[i],ref buff_img[iid.needle].vertex,i*3);
                    int c = i + 1;
                    if (c > buff_img[iid.needle].count)
                        buff_img[iid.needle].count = c;
                    break;
                }
            }
        }
        static void ChangeNeedleSize()
        {
            int s = select.needle;
            if(select.pressdbutton==1)
            {
                Point2 p = new Point2(mousePoint.x,mousePoint.y);
                layout.needle[s].h = Aim(ref layout.needle[s].location, ref p);
            }if (select.pressdbutton==2)
            {
                float x1= layout.needle[s].location.x - mousePoint.x;
                x1 *= x1;
                float y1= layout.needle[s].location.y - mousePoint.y;
                y1 *= y1;
                float d = x1 + y1;
                d=(float) Math.Sqrt(d);
                d *= 29.6f;
                layout.needle[s].w = d;
                layout.needle[s].distance = d * 0.12f + 0.06f;
            }
            CreateNeedle(ref layout.needle[s],ref buff_img[iid.needle].vertex,s*3);
            CreateNeedleMenu(s);
        }
        static void MoveNeedle(float x,float y)
        {
            int s = select.needle;
            layout.needle[s].location.x += x;
            layout.needle[s].location.y += y;
            CreateNeedle(ref layout.needle[s], ref buff_img[iid.needle].vertex, s * 3);
            CreateNeedleMenu(s);
        }
        static void DeleteNeedle(int index)
        {
            layout.needle[index].active = false;
            int c = index * 3;
            for (int i = 0; i < 3; i++)
            {
                buff_img[iid.needle].vertex[c].Position.X = 0;
                buff_img[iid.needle].vertex[c].Position.Y = 0;
                c++;
            }
        }
        static void CreateNeedle(ref LandDate n ,ref VertexPositionTexture[] vpt,int index)
        {
            Point2[] p = new Point2[3];
            float scale = n.w;
            if (scale != 0)//0==def 1
            {
                float a = n.h;
                if (a != 0)//angle
                {
                    p[0].x = tri_mod[0].x;
                    p[0].y = scale * tri_mod[0].y;
                    p[1].x = tri_mod[1].x;
                    p[1].y = scale * tri_mod[1].y;
                    p[2].x = tri_mod[2].x;
                    p[2].y = scale * tri_mod[2].y;
                    p = RotatePoint2(ref p, ref n.location, a,0.5625f);
                    n.points = p;
                }
                else
                {
                    float x = n.location.x;
                    float y = n.location.y;
                    p[0].x = scale * tri_modA[0].x + x;
                    p[0].y = y;
                    p[1].x = x;
                    p[1].y = scale * tri_modA[1].y + y;
                    p[2].x = scale * tri_modA[2].x + x;
                    p[2].y = y;
                    n.points = p;
                }
            }
            else
            {
                float a = n.h;
                if (a != 0)//angle
                {
                    p = RotatePoint2(ref tri_mod, ref n.location, a,0.5625f);
                    n.points = p;
                }
                else
                {
                    float x = n.location.x;
                    float y = n.location.y;
                    p[0].x = tri_modA[0].x + x;
                    p[0].y = y;
                    p[1].x = x;
                    p[1].y = tri_modA[1].y + y;
                    p[2].x = tri_modA[2].x + x;
                    p[2].y = y;
                    n.points = p;
                }
            }
            vpt[index].Position.X = p[0].x;
            vpt[index].Position.Y = p[0].y;
            index++;
            vpt[index].Position.X = p[1].x;
            vpt[index].Position.Y = p[1].y;
            index++;
            vpt[index].Position.X = p[2].x;
            vpt[index].Position.Y = p[2].y;
        }
        static bool CheckRole()
        {
            float x = role.location.x;
            float a = x - 0.03f;
            float b = x + 0.03f;
            if(mousePoint.x>a & mousePoint.x<b)
            {
                x = role.location.y;
                a = x - 0.05f;
                b = x + 0.04f;
                if (mousePoint.y > a & mousePoint.y < b)
                { select.target = Target.Role; return true; }
            }
            return false;
        }
        static void MoveRole()
        {
            role.location = mousePoint;
            role.origin = mousePoint;
            GetPoints(mousePoint,ref role.points);
            CaculRect_PosA(ref buff_img[role.imgid].vertex, mousePoint, 0.05625f, 0.1f, 0);
        }
        static void AddRecord(Point2 location)
        {
            LandDate[] r = layout.record;
            for (int i=0;i<8;i++)
            {
                if(!r[i].active)
                {
                    r[i].active = true;
                    SetTex_Pos(ref buff_img[iid.record].vertex,i*6,0.5f,0,0.75f,1);
                    CaculRect_Pos(ref buff_img[iid.record].vertex,location,0.0675f,0.12f,i*6);
                    r[i].points = new Point2[4];
                    SetDefRect(ref r[i].points,location);
                    r[i].w = 0.0675f;
                    r[i].h = 0.12f;
                    r[i].location = location;
                    int c=(i+1)*2;
                    if (c > buff_img[iid.record].count)
                        buff_img[iid.record].count = c;
                    break;
                }
            }
        }
        static int CheckRecord()
        {
            if(layout.record.Length==0)
            {
                Debug.WriteLine("the record is null");
                return -1;
            }
            LandDate[] r = layout.record;
            for (int i=0;i<8;i++)
            {
                if(r[i].active)
                {
                    float x = r[i].location.x;
                    x -= 0.03375f;
                    float x1 = x + 0.0675f;
                    if(mousePoint.x>x&mousePoint.x<x1)
                    {
                        x = r[i].location.y;
                        x = x - 0.06f;
                        x1 = x + 0.12f;
                        if (mousePoint.y > x & mousePoint.y < x1)
                        {
                            select.record = i;
                            select.target = Target.Record;
                            return i;
                        }
                    }
                }
            }
            return -1;
        }
        static void DeleteRecord()
        {
            int s = select.record;
            layout.record[s].active = false;
            VertexPositionTexture[] vpt = buff_img[iid.record].vertex;
            s *= 6;
            for(int i=0;i<6;i++)
            {
                vpt[s].Position.X = 0;
                vpt[s].Position.Y = 0;
                s++;
            }
        }
        static void MoveRecord()
        {
            int s = select.record;
            layout.record[s].location = mousePoint;
            CaculRect_Pos(ref buff_img[iid.record].vertex, select.mousepoint, 0.0675f, 0.12f, s * 6);
            SetDefRect(ref layout.record[s].points, select.mousepoint);
        }
        static void CreateRecordMenu(int index)
        {
            float px = layout.record[index].location.x;
            float py = layout.record[index].location.y;
            float w = layout.record[index].w * 0.5f;
            float h = layout.record[index].h * 0.5f;
            Point2 p = new Point2(px - w - 0.016f, py);
            layout.tool[1].location = p;
            CaculRect_Pos(ref buff_img[iid.other].vertex, p, 0.03375f, 0.06f, 6);
            CaculRect_Pos(ref buff_img[iid.other].vertex, p, 0.03375f, 0.06f, 18);
            SetTex_Pos(ref buff_img[iid.other].vertex, 6, 0.375f, 0, 0.5f, 0.5f);
            buff_img[iid.other].count = 4;
            select.menu = Target.RecordMenu;
        }
        public static void Dispose()
        {
            buff_img[iid.land].count = 0;
            buff_img[iid.needle].count = 0;
            buff_img[iid.other].count = 0;
            buff_img[iid.record].count = 0;
            buff_img[iid.bar].count = 0;
            if (pause)
            {
                pause = false;
                game.Pause = false;
                game.execute = () => { GamePage.page.HideMenu(); };
            }
        }
        #endregion

        #region
        public static void HideView()
        {
            SetCurrentLayout(ref layout);
        }
        static void SetCurrentLayout(ref LayOut lay)
        {
            if (lay.land != null)
                LoadLand(ref lay.land, iid.land);
            if (lay.needle != null)
                LoadNeedle(ref lay.needle, iid.needle);
            if (lay.record != null)
                LoadRecord(ref lay.record, iid.record);
            GetPoints(role.location, ref role.points);
            CaculRect_PosA(ref buff_img[role.imgid].vertex, role.location, 0.0675f, 0.12f, 0);
        }
        static List<LayOut> level;
        public static int GetCount()
        {
            if (level != null)
                return level.Count;
            else
            {
                level = FileManage.LoadLayout("custom");
                if (level != null)
                    return level.Count;
                return -1;
            }
        }
        public static void Save()
        {
            if (level == null)
                level = new List<LayOut>();
            level.Add(layout);
            layout.role.origin = role.location;
            FileManage.SaveLayout(ref layout, "custom");
        }
        public static void Replace(int index)
        {
            layout.role.origin = role.location;
            FileManage.ReplaceLayout(ref layout, "custom",index);
        }
        public static void Delete(int index)
        {

        }
        public static void View(int index)
        {
            view = level[index];
            role.location = level[index].role.origin;
            role.origin = level[index].role.origin;
            SetCurrentLayout(ref view);
        }
        public static void Edit(int index)
        {
            LandDate[] l = level[index].land;
            int i;
            for (i = 0; i < l.Length;i++)
                layout.land[i]=l[i];
            for (int c = i; c < 64;c++)
                layout.land[c].active = false;
            l = level[index].needle;
            for (i = 0; i < l.Length; i++)
                layout.needle[i] = l[i];
            for (int c = i; c < 128; c++)
                layout.needle[c].active = false;
            role.location = level[index].role.origin;
            role.origin = level[index].role.origin;
            SetCurrentLayout(ref layout);
        }
        #endregion
    }
}
