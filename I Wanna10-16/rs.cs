using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace I_Wanna
{
    delegate void GetPicBuff_A(ref AssetsPic[] a);
    delegate void GetImageBuff_A(ref ImageProperty[] a);
    struct Textproperty
    {
        public bool reg;
        public Vector2 position;
        public Vector2 screen_pos;
        public string text;
        public Color col;
    }
    struct AssetsPic
    {
        public bool reg;
        public bool done;
        public string path;
        public Texture2D t2d;
        public object content;
    }
    struct ImageProperty
    {
        public bool reg;
        public int a_id;
        public VertexPositionTexture[] vertex;
        public int count;
    }
    struct Imgid
    {
        public int role;
        public int land;
        public int needle;
        public int other;
        public int bar;
        public int record;
    }
    struct Role
    {
        public bool flooer;
        public bool backword;
        public int jump;
        public int time;
        public int pid;
        public int imgid;
        public int index;
        public Point2[] points;
        public Point2 location;
        public Point2 motion;
        public Point2 origin;
        public Point2 loc_back;
    }
    struct LandDate
    {
        public bool active;
        public Point2 location;
        public Point2[] points;
        public float w;//tri scale
        public float h;//tri angle
        public float distance;
        public float property;//remain
    }
    struct LayOut
    {
        public int level;
        public LandDate[] land;
        public LandDate[] needle;
        public LandDate[] tool;
        public LandDate[] record;
        public Role role;
    }
    struct Point2
    {
        public float x;
        public float y;
        public Point2(float x1, float y1) { x = x1; y = y1; }
    }
    struct Archive
    {
        public int level;
        public int die_count;
        public int achi_count;
        public Point2 loaction;
    }
}
