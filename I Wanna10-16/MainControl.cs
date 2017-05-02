using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO.IsolatedStorage;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using Windows.UI.Xaml;
using System.IO;
using Windows.Storage;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices.WindowsRuntime;

namespace I_Wanna
{
    delegate void LoadLayout(LayOut[] l);
    class Main
    {
        #region angle table 精度1
        public static readonly Point2[] angle_table = new Point2[]{new Point2(0f, 1f),
new Point2(-0.01745241f, 0.9998477f),new Point2(-0.0348995f, 0.9993908f),new Point2(-0.05233596f, 0.9986295f),new Point2(-0.06975647f, 0.9975641f),new Point2(-0.08715574f, 0.9961947f),new Point2(-0.1045285f, 0.9945219f),new Point2(-0.1218693f, 0.9925461f),new Point2(-0.1391731f, 0.9902681f),new Point2(-0.1564345f, 0.9876884f),new Point2(-0.1736482f, 0.9848077f),
new Point2(-0.190809f, 0.9816272f),new Point2(-0.2079117f, 0.9781476f),new Point2(-0.224951f, 0.9743701f),new Point2(-0.2419219f, 0.9702957f),new Point2(-0.258819f, 0.9659258f),new Point2(-0.2756374f, 0.9612617f),new Point2(-0.2923717f, 0.9563048f),new Point2(-0.309017f, 0.9510565f),new Point2(-0.3255681f, 0.9455186f),new Point2(-0.3420201f, 0.9396926f),
new Point2(-0.3583679f, 0.9335804f),new Point2(-0.3746066f, 0.9271839f),new Point2(-0.3907311f, 0.9205049f),new Point2(-0.4067366f, 0.9135455f),new Point2(-0.4226183f, 0.9063078f),new Point2(-0.4383712f, 0.8987941f),new Point2(-0.4539905f, 0.8910065f),new Point2(-0.4694716f, 0.8829476f),new Point2(-0.4848096f, 0.8746197f),new Point2(-0.5f, 0.8660254f),
new Point2(-0.5150381f, 0.8571673f),new Point2(-0.5299193f, 0.8480481f),new Point2(-0.5446391f, 0.8386706f),new Point2(-0.5591929f, 0.8290376f),new Point2(-0.5735765f, 0.8191521f),new Point2(-0.5877852f, 0.809017f),new Point2(-0.601815f, 0.7986355f),new Point2(-0.6156615f, 0.7880108f),new Point2(-0.6293204f, 0.7771459f),new Point2(-0.6427876f, 0.7660444f),
new Point2(-0.656059f, 0.7547096f),new Point2(-0.6691306f, 0.7431448f),new Point2(-0.6819984f, 0.7313537f),new Point2(-0.6946584f, 0.7193398f),new Point2(-0.7071068f, 0.7071068f),new Point2(-0.7193398f, 0.6946584f),new Point2(-0.7313537f, 0.6819984f),new Point2(-0.7431448f, 0.6691306f),new Point2(-0.7547095f, 0.656059f),new Point2(-0.7660444f, 0.6427876f),
new Point2(-0.7771459f, 0.6293204f),new Point2(-0.7880107f, 0.6156615f),new Point2(-0.7986355f, 0.601815f),new Point2(-0.809017f, 0.5877853f),new Point2(-0.8191521f, 0.5735765f),new Point2(-0.8290375f, 0.5591929f),new Point2(-0.8386706f, 0.5446391f),new Point2(-0.8480481f, 0.5299193f),new Point2(-0.8571673f, 0.5150381f),new Point2(-0.8660254f, 0.5f),
new Point2(-0.8746197f, 0.4848097f),new Point2(-0.8829476f, 0.4694716f),new Point2(-0.8910065f, 0.4539905f),new Point2(-0.8987941f, 0.4383712f),new Point2(-0.9063078f, 0.4226182f),new Point2(-0.9135455f, 0.4067366f),new Point2(-0.9205048f, 0.3907312f),new Point2(-0.9271839f, 0.3746066f),new Point2(-0.9335804f, 0.358368f),new Point2(-0.9396926f, 0.3420202f),
new Point2(-0.9455186f, 0.3255681f),new Point2(-0.9510565f, 0.309017f),new Point2(-0.9563047f, 0.2923718f),new Point2(-0.9612617f, 0.2756374f),new Point2(-0.9659258f, 0.2588191f),new Point2(-0.9702957f, 0.2419219f),new Point2(-0.9743701f, 0.224951f),new Point2(-0.9781476f, 0.2079117f),new Point2(-0.9816272f, 0.1908091f),new Point2(-0.9848077f, 0.1736482f),
new Point2(-0.9876884f, 0.1564345f),new Point2(-0.9902681f, 0.1391731f),new Point2(-0.9925461f, 0.1218693f),new Point2(-0.9945219f, 0.1045284f),new Point2(-0.9961947f, 0.0871558f),new Point2(-0.9975641f, 0.06975651f),new Point2(-0.9986295f, 0.05233597f),new Point2(-0.9993908f, 0.0348995f),new Point2(-0.9998477f, 0.01745238f),new Point2(-1f, 0),
new Point2(-0.9998477f, -0.01745235f),new Point2(-0.9993908f, -0.03489946f),new Point2(-0.9986295f, -0.05233594f),new Point2(-0.9975641f, -0.06975648f),new Point2(-0.9961947f, -0.08715577f),new Point2(-0.9945219f, -0.1045284f),new Point2(-0.9925461f, -0.1218693f),new Point2(-0.9902681f, -0.1391731f),new Point2(-0.9876884f, -0.1564344f),new Point2(-0.9848077f, -0.1736482f),
new Point2(-0.9816272f, -0.190809f),new Point2(-0.9781476f, -0.2079116f),new Point2(-0.9743701f, -0.224951f),new Point2(-0.9702957f, -0.2419219f),new Point2(-0.9659258f, -0.258819f),new Point2(-0.9612617f, -0.2756374f),new Point2(-0.9563047f, -0.2923717f),new Point2(-0.9510565f, -0.3090169f),new Point2(-0.9455186f, -0.3255681f),new Point2(-0.9396926f, -0.3420201f),
new Point2(-0.9335805f, -0.3583679f),new Point2(-0.9271839f, -0.3746066f),new Point2(-0.9205049f, -0.3907312f),new Point2(-0.9135455f, -0.4067366f),new Point2(-0.9063078f, -0.4226183f),new Point2(-0.8987941f, -0.4383711f),new Point2(-0.8910066f, -0.4539904f),new Point2(-0.8829476f, -0.4694716f),new Point2(-0.8746197f, -0.4848095f),new Point2(-0.8660254f, -0.5000001f),
new Point2(-0.8571673f, -0.515038f),new Point2(-0.8480482f, -0.5299191f),new Point2(-0.8386706f, -0.5446391f),new Point2(-0.8290376f, -0.5591928f),new Point2(-0.819152f, -0.5735765f),new Point2(-0.809017f, -0.5877852f),new Point2(-0.7986355f, -0.6018151f),new Point2(-0.7880108f, -0.6156614f),new Point2(-0.777146f, -0.6293203f),new Point2(-0.7660444f, -0.6427876f),
new Point2(-0.7547096f, -0.656059f),new Point2(-0.7431448f, -0.6691307f),new Point2(-0.7313537f, -0.6819983f),new Point2(-0.7193399f, -0.6946583f),new Point2(-0.7071068f, -0.7071068f),new Point2(-0.6946585f, -0.7193397f),new Point2(-0.6819983f, -0.7313538f),new Point2(-0.6691306f, -0.7431448f),new Point2(-0.656059f, -0.7547097f),new Point2(-0.6427876f, -0.7660444f),
new Point2(-0.6293205f, -0.7771459f),new Point2(-0.6156614f, -0.7880108f),new Point2(-0.6018151f, -0.7986355f),new Point2(-0.5877852f, -0.8090171f),new Point2(-0.5735765f, -0.8191521f),new Point2(-0.559193f, -0.8290375f),new Point2(-0.5446391f, -0.8386706f),new Point2(-0.5299193f, -0.848048f),new Point2(-0.515038f, -0.8571673f),new Point2(-0.5000001f, -0.8660254f),
new Point2(-0.4848098f, -0.8746197f),new Point2(-0.4694716f, -0.8829476f),new Point2(-0.4539906f, -0.8910065f),new Point2(-0.4383711f, -0.8987941f),new Point2(-0.4226183f, -0.9063078f),new Point2(-0.4067366f, -0.9135455f),new Point2(-0.3907312f, -0.9205049f),new Point2(-0.3746067f, -0.9271838f),new Point2(-0.3583679f, -0.9335805f),new Point2(-0.3420202f, -0.9396926f),
new Point2(-0.3255681f, -0.9455186f),new Point2(-0.309017f, -0.9510565f),new Point2(-0.2923718f, -0.9563047f),new Point2(-0.2756374f, -0.9612617f),new Point2(-0.2588191f, -0.9659258f),new Point2(-0.2419219f, -0.9702957f),new Point2(-0.2249511f, -0.9743701f),new Point2(-0.2079116f, -0.9781476f),new Point2(-0.190809f, -0.9816272f),new Point2(-0.1736483f, -0.9848077f),
new Point2(-0.1564344f, -0.9876884f),new Point2(-0.1391732f, -0.9902681f),new Point2(-0.1218693f, -0.9925461f),new Point2(-0.1045285f, -0.9945219f),new Point2(-0.08715588f, -0.9961947f),new Point2(-0.06975647f, -0.9975641f),new Point2(-0.05233605f, -0.9986295f),new Point2(-0.03489945f, -0.9993908f),new Point2(-0.01745246f, -0.9998477f),new Point2(0, -1f),
new Point2(0.01745239f, -0.9998477f),new Point2(0.03489939f, -0.9993908f),new Point2(0.05233599f, -0.9986295f),new Point2(0.0697564f, -0.9975641f),new Point2(0.08715581f, -0.9961947f),new Point2(0.1045284f, -0.9945219f),new Point2(0.1218692f, -0.9925461f),new Point2(0.1391731f, -0.9902681f),new Point2(0.1564344f, -0.9876884f),new Point2(0.1736482f, -0.9848077f),
new Point2(0.190809f, -0.9816272f),new Point2(0.2079116f, -0.9781476f),new Point2(0.224951f, -0.9743701f),new Point2(0.2419218f, -0.9702957f),new Point2(0.2588191f, -0.9659258f),new Point2(0.2756373f, -0.9612617f),new Point2(0.2923718f, -0.9563047f),new Point2(0.309017f, -0.9510565f),new Point2(0.3255681f, -0.9455186f),new Point2(0.3420202f, -0.9396926f),
new Point2(0.3583679f, -0.9335805f),new Point2(0.3746066f, -0.9271838f),new Point2(0.3907311f, -0.9205049f),new Point2(0.4067365f, -0.9135455f),new Point2(0.4226183f, -0.9063078f),new Point2(0.4383711f, -0.8987941f),new Point2(0.4539905f, -0.8910065f),new Point2(0.4694715f, -0.8829476f),new Point2(0.4848095f, -0.8746198f),new Point2(0.5f, -0.8660254f),
new Point2(0.515038f, -0.8571674f),new Point2(0.5299193f, -0.8480481f),new Point2(0.544639f, -0.8386706f),new Point2(0.559193f, -0.8290375f),new Point2(0.5735764f, -0.8191521f),new Point2(0.5877851f, -0.8090171f),new Point2(0.601815f, -0.7986355f),new Point2(0.6156614f, -0.7880108f),new Point2(0.6293204f, -0.7771459f),new Point2(0.6427876f, -0.7660445f),
new Point2(0.6560589f, -0.7547097f),new Point2(0.6691306f, -0.7431448f),new Point2(0.6819983f, -0.7313538f),new Point2(0.6946584f, -0.7193398f),new Point2(0.7071067f, -0.7071068f),new Point2(0.7193398f, -0.6946583f),new Point2(0.7313537f, -0.6819984f),new Point2(0.7431448f, -0.6691307f),new Point2(0.7547096f, -0.656059f),new Point2(0.7660446f, -0.6427875f),
new Point2(0.777146f, -0.6293203f),new Point2(0.7880107f, -0.6156615f),new Point2(0.7986354f, -0.6018152f),new Point2(0.8090168f, -0.5877854f),new Point2(0.8191521f, -0.5735763f),new Point2(0.8290376f, -0.5591929f),new Point2(0.8386706f, -0.5446391f),new Point2(0.848048f, -0.5299194f),new Point2(0.8571672f, -0.5150383f),new Point2(0.8660254f, -0.4999999f),
new Point2(0.8746197f, -0.4848096f),new Point2(0.8829476f, -0.4694716f),new Point2(0.8910065f, -0.4539907f),new Point2(0.8987939f, -0.4383714f),new Point2(0.9063078f, -0.4226182f),new Point2(0.9135454f, -0.4067366f),new Point2(0.9205048f, -0.3907312f),new Point2(0.9271838f, -0.3746068f),new Point2(0.9335805f, -0.3583678f),new Point2(0.9396927f, -0.3420201f),
new Point2(0.9455186f, -0.3255682f),new Point2(0.9510565f, -0.3090171f),new Point2(0.9563047f, -0.2923719f),new Point2(0.9612617f, -0.2756372f),new Point2(0.9659259f, -0.258819f),new Point2(0.9702957f, -0.2419219f),new Point2(0.9743701f, -0.2249512f),new Point2(0.9781476f, -0.2079119f),new Point2(0.9816272f, -0.1908088f),new Point2(0.9848078f, -0.1736481f),
new Point2(0.9876883f, -0.1564345f),new Point2(0.9902681f, -0.1391733f),new Point2(0.9925461f, -0.1218696f),new Point2(0.9945219f, -0.1045283f),new Point2(0.9961947f, -0.08715571f),new Point2(0.997564f, -0.06975655f),new Point2(0.9986295f, -0.05233612f),new Point2(0.9993908f, -0.03489976f),new Point2(0.9998477f, -0.0174523f),new Point2(1f, 0f),
new Point2(0.9998477f, 0.01745232f),new Point2(0.9993908f, 0.03489931f),new Point2(0.9986296f, 0.05233567f),new Point2(0.997564f, 0.06975657f),new Point2(0.9961947f, 0.08715574f),new Point2(0.9945219f, 0.1045284f),new Point2(0.9925462f, 0.1218691f),new Point2(0.9902681f, 0.1391733f),new Point2(0.9876883f, 0.1564345f),new Point2(0.9848077f, 0.1736481f),
new Point2(0.9816272f, 0.1908089f),new Point2(0.9781476f, 0.2079115f),new Point2(0.97437f, 0.2249512f),new Point2(0.9702957f, 0.2419219f),new Point2(0.9659258f, 0.258819f),new Point2(0.9612617f, 0.2756372f),new Point2(0.9563048f, 0.2923715f),new Point2(0.9510565f, 0.3090171f),new Point2(0.9455186f, 0.3255682f),new Point2(0.9396926f, 0.3420201f),
new Point2(0.9335805f, 0.3583678f),new Point2(0.9271839f, 0.3746064f),new Point2(0.9205048f, 0.3907312f),new Point2(0.9135454f, 0.4067367f),new Point2(0.9063078f, 0.4226182f),new Point2(0.8987941f, 0.438371f),new Point2(0.8910066f, 0.4539903f),new Point2(0.8829476f, 0.4694717f),new Point2(0.8746197f, 0.4848096f),new Point2(0.8660254f, 0.4999999f),
new Point2(0.8571674f, 0.5150379f),new Point2(0.8480483f, 0.529919f),new Point2(0.8386705f, 0.5446391f),new Point2(0.8290376f, 0.5591929f),new Point2(0.8191521f, 0.5735763f),new Point2(0.8090171f, 0.5877851f),new Point2(0.7986354f, 0.6018152f),new Point2(0.7880107f, 0.6156615f),new Point2(0.777146f, 0.6293204f),new Point2(0.7660445f, 0.6427875f),
new Point2(0.7547097f, 0.6560588f),new Point2(0.7431448f, 0.6691307f),new Point2(0.7313536f, 0.6819984f),new Point2(0.7193398f, 0.6946583f),new Point2(0.7071069f, 0.7071066f),new Point2(0.6946585f, 0.7193396f),new Point2(0.6819983f, 0.7313538f),new Point2(0.6691306f, 0.7431449f),new Point2(0.6560591f, 0.7547095f),new Point2(0.6427878f, 0.7660443f),
new Point2(0.6293206f, 0.7771458f),new Point2(0.6156614f, 0.7880108f),new Point2(0.601815f, 0.7986355f),new Point2(0.5877853f, 0.8090169f),new Point2(0.5735766f, 0.8191519f),new Point2(0.5591931f, 0.8290374f),new Point2(0.5446389f, 0.8386706f),new Point2(0.5299193f, 0.8480481f),new Point2(0.5150381f, 0.8571672f),new Point2(0.5000002f, 0.8660253f),
new Point2(0.4848099f, 0.8746195f),new Point2(0.4694715f, 0.8829476f),new Point2(0.4539905f, 0.8910065f),new Point2(0.4383712f, 0.898794f),new Point2(0.4226184f, 0.9063077f),new Point2(0.4067365f, 0.9135455f),new Point2(0.3907311f, 0.9205049f),new Point2(0.3746066f, 0.9271839f),new Point2(0.3583681f, 0.9335804f),new Point2(0.3420204f, 0.9396926f),
new Point2(0.325568f, 0.9455186f),new Point2(0.3090169f, 0.9510565f),new Point2(0.2923717f, 0.9563047f),new Point2(0.2756375f, 0.9612616f),new Point2(0.2588193f, 0.9659258f),new Point2(0.2419218f, 0.9702958f),new Point2(0.224951f, 0.9743701f),new Point2(0.2079118f, 0.9781476f),new Point2(0.1908092f, 0.9816272f),new Point2(0.1736484f, 0.9848077f),
new Point2(0.1564344f, 0.9876884f),new Point2(0.1391731f, 0.9902681f),new Point2(0.1218694f, 0.9925461f),new Point2(0.1045287f, 0.9945219f),new Point2(0.08715603f, 0.9961947f),new Point2(0.06975638f, 0.9975641f),new Point2(0.05233596f, 0.9986295f),new Point2(0.0348996f, 0.9993908f),new Point2(0.01745261f, 0.9998477f),new Point2(0f, 1f),};
        #endregion

        #region buff
        public static LayOut[] maps { get; set; }
        public static Game1 game {set;get;}
        public static float screenX { set; get; }
        public static float screenY { set; get; }
        public static float mouseX { get; set; }
        public static float mouseY { get; set; }
        public static bool mousedown { get; set; }
        public static Textproperty[] buff_t = new Textproperty[8];
        public static AssetsPic[] buff_pic = new AssetsPic[24];
        public static ImageProperty[] buff_img = new ImageProperty[32];
        public static Point2[] tri_mod = new Point2[] { new Point2(90, 0.06f), new Point2(0, 0.12f), new Point2(270, 0.06f) };
        public static Point2[] tri_modA = new Point2[] { new Point2(-0.03375f, 0), new Point2(0, 0.12f), new Point2(0.03375f, 0) };
        protected static int RegPicture(string name)
        {
            int i;
            for (i = 0; i < 24; i++)
            {
                if (buff_pic[i].path == name)
                    return i;
                else if (buff_pic[i].path == null)
                {
                    buff_pic[i].path = name;
                    buff_pic[i].reg = true;
                    return i;
                }
            }
            return i;
        }
        protected static int RegImage(int a_id, int maxcount)
        {
            for (int i = 0; i < 32; i++)
            {
                if (!buff_img[i].reg)
                {
                    buff_img[i].reg = true;
                    buff_img[i].a_id = a_id;
                    if(maxcount>0)
                    buff_img[i].vertex = new VertexPositionTexture[maxcount * 6];
                    return i;
                }
            }
            return -1;
        }
        public static void GetPicBuff(GetPicBuff_A a) { a(ref buff_pic); }
        public static void GetImageBuff(GetImageBuff_A a){a(ref buff_img);}
        #endregion

        #region collision
        static float atan(float dx, float dy)
        {
            //ax<ay
            float ax = dx < 0 ? -dx : dx, ay = dy < 0 ? -dy : dy;
            float a;
            if (ax < ay) a = ax / ay; else a = ay / ax;
            float s = a * a;
            float r = ((-0.0464964749f * s + 0.15931422f) * s - 0.327622764f) * s * a + a;
            if (ay > ax) r = 1.57079637f - r;
            r *= 57.32484f;
            if (dx < 0)
            {
                if (dy < 0)
                    r += 90;
                else r = 90 - r;
            }
            else
            {
                if (dy < 0)
                    r = 270 - r;
                else r += 270;
            }
            return r;
        }
        public static float Aim(ref Point2 self, ref Point2 v)
        {
            float x = v.x - self.x;
            float y = v.y - self.y;
            return atan(x, y);
        }
        public static Point2 RotatePoint2(ref Point2 p, ref Point2 location, float angle)//a=绝对角度 d=直径
        {
            int a = (int)(p.x + angle);
            if (a < 0)
                a += 360;
            if (a > 360)
                a -= 360;
            float d = p.y;
            Point2 temp = new Point2();
            temp.x = location.x - angle_table[a].x * d;
            temp.y = location.y + angle_table[a].y * d;
            return temp;
        }
        public static Point2[] RotatePoint2(ref Point2[] P, ref Point2 location, float angle)//p[].x=绝对角度 p[].y=直径
        {
            Point2[] temp = new Point2[P.Length];
            for (int i = 0; i < P.Length; i++)
            {
                int a = (int)(P[i].x + angle);//change angle to radin
                if (a < 0)
                    a += 360;
                if (a >= 360)
                    a -= 360;
                float d = P[i].y;
                temp[i].x = location.x + angle_table[a].x * d;
                temp[i].y = location.y + angle_table[a].y * d;
            }
            return temp;
        }
        public static Point2[] RotatePoint2(ref Point2[] P, ref Point2 location, float angle,float ratio)//p[].x=绝对角度 p[].y=直径
        {
            Point2[] temp = new Point2[P.Length];
            for (int i = 0; i < P.Length; i++)
            {
                int a = (int)(P[i].x + angle);//change angle to radin
                if (a < 0)
                    a += 360;
                if (a >= 360)
                    a -= 360;
                float d = P[i].y;
                temp[i].x = location.x + angle_table[a].x * d*ratio;
                temp[i].y = location.y + angle_table[a].y * d;
            }
            return temp;
        }
        public static bool LineToLine(ref Point2[] A, ref Point2[] B, ref Point2 O)//相交线相交点
        {
            float VAx = A[1].x - A[0].x;
            float VAy = A[1].y - A[0].y;
            float VBx = B[1].x - B[0].x;
            float VBy = B[1].y - B[0].y;
            //(V1.y*V2.x-V1.x*V2.y)
            float y = VAy * VBx - VAx * VBy;
            if (y == 0)
                return false;
            //((B.y-A.y)*V2.x+(A.x-B.x)*V2.y)
            float x = (B[0].y - A[0].y) * VBx + (A[0].x - B[0].x) * VBy;
            float d = x / y;
            if (d >= 0 & d <= 1)
            {
                if (VBx == 0)
                {
                    //x2=(A.y+x1*V1.y-B.y)/V2.y
                    y = (A[0].y - B[0].y + d * VAy) / VBy;
                }
                else
                {
                    //x2=(A.x+x1*V1.x-B.x)/V2.x
                    y = (A[0].x - B[0].x + d * VAx) / VBx;
                }
                //location.x=A.x+x1*V1.x
                //location.y=A.x+x1*V1.y
                if (y >= 0 & y <= 1)
                {
                    O.x = A[0].x + d * VAx;
                    O.y = A[0].y + d * VAy;
                    return true;
                }
            }
            return false;
        }
        public static bool PToP2(Point2[] A, Point2[] B)
        {
            //Cos A=(b²+c²-a²)/2bc
            float min1 = 0, max1 = 0, min2 = 0, max2 = 0;
            int second = 0;
            Point2 a, b;
            label1:
            for (int i = 0; i < A.Length; i++)
            {
                int id;
                a = A[i];
                if (i == A.Length - 1)
                {
                    b = A[0];
                    id = 1;
                }
                else
                {
                    b = A[i + 1];
                    id = i + 2;
                }
                float x = a.x - b.x;
                float y = a.y - b.y;//向量
                a.x = y;
                a.y = -x;//法线点a
                b.x = -y;
                b.y = x;//法线点b
                float ac;
                float bc;
                //float d = Mathf.Sqrt(bc) + Mathf.Sqrt(ac) - Mathf.Sqrt(ab);
                float d;
                for (int o = 0; o < A.Length; o++)
                {
                    float x1 = A[o].x - a.x;
                    x1 *= x1;
                    float y1 = A[o].y - a.y;
                    ac = x1 + y1 * y1;//ac
                    float x2 = b.x - A[o].x;
                    x2 *= x2;
                    float y2 = b.y - A[o].y;
                    bc = x2 + y2 * y2;//bc
                    d = ac - bc;//ab+ac-bc
                    if (o == 0)
                    {
                        min1 = max1 = d;
                    }
                    else
                    {
                        if (d < min1)
                            min1 = d;
                        else if (d > max1)
                            max1 = d;
                    }
                }
                for (int o = 0; o < B.Length; o++)
                {
                    float x1 = B[o].x - a.x;
                    x1 *= x1;
                    float y1 = B[o].y - a.y;
                    ac = x1 + y1 * y1;//ac
                    float x2 = b.x - B[o].x;
                    x2 *= x2;
                    float y2 = b.y - B[o].y;
                    bc = x2 + y2 * y2;//bc
                    d = ac - bc;//ab+ac-bc
                    if (o == 0)
                        max2 = min2 = d;
                    else
                    {
                        if (d < min2)
                            min2 = d;
                        else if (d > max2)
                            max2 = d;
                    }
                }
                if (min2 > max1 | min1 > max2)
                    return false;
            }
            second++;
            if (second < 2)
            {
                Point2[] temp = A;
                A = B;
                B = temp;
                goto label1;
            }
            return true;
        }
        public static bool PToP2A(ref Point2[] PA, ref Point2[] PB)
        {
            //Cos A=(b²+c²-a²)/2bc
            Point2[] A = PA;
            Point2[] B = PB;
            float min1 = 0, max1 = 0, min2 = 0, max2 = 0;
            int second = 0;
            Point2 a, b;
            label1:
            for (int i = 0; i < A.Length; i++)
            {
                int id;
                a = A[i];
                if (i == A.Length - 1)
                {
                    b = A[0];
                    id = 1;
                }
                else
                {
                    b = A[i + 1];
                    id = i + 2;
                }
                float x = a.x - b.x;
                float y = a.y - b.y;//向量
                a.x = y;
                a.y = -x;//法线点a
                b.x = -y;
                b.y = x;//法线点b
                float ac;
                float bc;
                //float d = Mathf.Sqrt(bc) + Mathf.Sqrt(ac) - Mathf.Sqrt(ab);
                float d;
                for (int o = 0; o < A.Length; o++)
                {
                    float x1 = A[o].x - a.x;
                    x1 *= x1;
                    float y1 = A[o].y - a.y;
                    ac = x1 + y1 * y1;//ac
                    float x2 = b.x - A[o].x;
                    x2 *= x2;
                    float y2 = b.y - A[o].y;
                    bc = x2 + y2 * y2;//bc
                    d = ac - bc;//ab+ac-bc
                    if (o == 0)
                    {
                        min1 = max1 = d;
                    }
                    else
                    {
                        if (d < min1)
                            min1 = d;
                        else if (d > max1)
                            max1 = d;
                    }
                }
                for (int o = 0; o < B.Length; o++)
                {
                    float x1 = B[o].x - a.x;
                    x1 *= x1;
                    float y1 = B[o].y - a.y;
                    ac = x1 + y1 * y1;//ac
                    float x2 = b.x - B[o].x;
                    x2 *= x2;
                    float y2 = b.y - B[o].y;
                    bc = x2 + y2 * y2;//bc
                    d = ac - bc;//ab+ac-bc
                    if (o == 0)
                        max2 = min2 = d;
                    else
                    {
                        if (d < min2)
                            min2 = d;
                        else if (d > max2)
                            max2 = d;
                    }
                }
                if (min2 > max1 | min1 > max2)
                    return false;
            }
            second++;
            if (second < 2)
            {
                A = PB;
                B = PA;
                goto label1;
            }
            return true;
        }
        public static bool PToP2A(ref Point2[] PA, ref Point2[] PB, ref Point2 la, ref Point2 lb)
        {
            //formule
            //A.x+x1*V1.x=B.x+x2*V2.x
            //x2*V2.x=A.x+x1*V1.x-B.x
            //x2=(A.x+x1*V1.x-B.x)/V2.x
            //A.y+x1*V1.y=B.y+x2*V2.y
            //A.y+x1*V1.y=B.y+(A.x+x1*V1.x-B.x)/V2.x*V2.y
            //x1*V1.y=B.y+(A.x-B.x)/V2.x*V2.y-A.y+x1*V1.x/V2.x*V2.y
            //x1*V1.y-x1*V1.x/V2.x*V2.y=B.y+(A.x-B.x)/V2.x*V2.y-A.y
            //x1*(V1.y-V1.x/V2.x*V2.y)=B.y+(A.x-B.x)/V2.x*V2.y-A.y
            //x1=(B.y-A.y+(A.x-B.x)/V2.x*V2.y)/(V1.y-V1.x/V2.x*V2.y)
            //改除为乘防止除0溢出
            //if((V1.y*V2.x-V1.x*V2.y)==0) 平行
            //x1=((B.y-A.y)*V2.x+(A.x-B.x)*V2.y)/(V1.y*V2.x-V1.x*V2.y)
            //x2=(A.x+x1*V1.x-B.x)/V2.x
            //x2=(A.y+x1*V1.y-B.y)/V2.y
            //if(x1>=0&x1<=1 &x2>=0 &x2<=1) cross =true
            //location.x=A.x+x1*V1.x
            //location.y=A.x+x1*V1.y
            Point2[] A = PA;
            Point2[] B = PB;
            bool re = false;
            Point2[] VB = new Point2[B.Length];
            for (int i = 0; i < B.Length; i++)
            {
                if (i == B.Length - 1)
                {
                    VB[i].x = B[0].x - B[i].x;
                    VB[i].y = B[0].y - B[i].y;
                }
                else
                {
                    VB[i].x = B[i + 1].x - B[i].x;
                    VB[i].y = B[i + 1].y - B[i].y;
                }
            }
            for (int i = 0; i < A.Length; i++)
            {
                Point2 VA = new Point2();
                if (i == A.Length - 1)
                {
                    VA.x = A[0].x - A[i].x;
                    VA.y = A[0].y - A[i].y;
                }
                else
                {
                    VA.x = A[i + 1].x - A[i].x;
                    VA.y = A[i + 1].y - A[i].y;
                }
                for (int c = 0; c < B.Length; c++)
                {
                    //(V1.y*V2.x-V1.x*V2.y)
                    float y = VA.y * VB[c].x - VA.x * VB[c].y;
                    if (y == 0)
                        break;
                    //((B.y-A.y)*V2.x+(A.x-B.x)*V2.y)
                    float x = (B[c].y - A[i].y) * VB[c].x + (A[i].x - B[c].x) * VB[c].y;
                    float d = x / y;
                    if (d >= 0 & d <= 1)
                    {
                        if (VB[c].x == 0)
                        {
                            //x2=(A.y+x1*V1.y-B.y)/V2.y
                            y = (A[i].y - B[c].y + d * VA.y) / VB[c].y;
                        }
                        else
                        {
                            //x2=(A.x+x1*V1.x-B.x)/V2.x
                            y = (A[i].x - B[c].x + d * VA.x) / VB[c].x;
                        }
                        //location.x=A.x+x1*V1.x
                        //location.y=A.x+x1*V1.y
                        if (y >= 0 & y <= 1)
                        {
                            if (re)
                            {
                                lb.x = A[i].x + d * VA.x;
                                lb.y = A[i].y + d * VA.y;
                                return true;
                            }
                            else
                            {
                                la.x = A[i].x + d * VA.x;
                                la.y = A[i].y + d * VA.y;
                                re = true;
                            }
                        }
                    }
                }
            }
            return re;
        }
        public static bool DotToPolygon(ref Point2[] A, Point2 B)//rotate
        {
            int count = 0;
            for (int i = 0; i < A.Length; i++)
            {
                Point2 p1 = A[i];
                Point2 p2 = i == A.Length - 1 ? A[0] : A[i + 1];
                if (B.y >= p1.y & B.y <= p2.y | B.y >= p2.y & B.y <= p1.y)
                {
                    float t = (B.y - p1.y) / (p2.y - p1.y);
                    float xt = p1.x + t * (p2.x - p1.x);
                    if (B.x == xt)
                        return true;
                    if (B.x < xt)
                        count++;
                }
            }
            return count % 2 > 0 ? true : false;
        }
        #endregion

        protected static void CaculRect_Pos(ref VertexPositionTexture[] vpt, Point2 p, float w, float h, int index)
        {
            float x1, y1, x2, y2;
            w *= 0.5f;
            h *= 0.5f;
            x1 = p.x - w;
            x2 = p.x + w;
            y1 = p.y - h;
            y2 = p.y + h;

            vpt[index].Position.X = x1;
            vpt[index].Position.Y = y1;
            index++;
            vpt[index].Position.X = x1;
            vpt[index].Position.Y = y2;
            index++;
            vpt[index].Position.X = x2;
            vpt[index].Position.Y = y1;
            index++;
            vpt[index].Position.X = x2;
            vpt[index].Position.Y = y1;
            index++;
            vpt[index].Position.X = x1;
            vpt[index].Position.Y = y2;
            index++;
            vpt[index].Position.X = x2;
            vpt[index].Position.Y = y2;
        }
        protected static void CaculRect_PosA(ref VertexPositionTexture[] vpt, Point2 p, float w, float h, int index)
        {
            float x1, y1, x2, y2;
            w *= 0.5f;
            h *= 0.5f;
            x1 = p.x - w;
            x2 = p.x + w;
            y1 = p.y - h;
            y2 = p.y + h;
            //(x2,y1)(x1,y1)(x2,y2) (x2,y2)(x1,y1)(x1,y2)
            vpt[index].Position.X = x2;
            vpt[index].Position.Y = y1;
            index++;
            vpt[index].Position.X = x1;
            vpt[index].Position.Y = y1;
            index++;
            vpt[index].Position.X = x2;
            vpt[index].Position.Y = y2;
            index++;
            vpt[index].Position.X = x2;
            vpt[index].Position.Y = y2;
            index++;
            vpt[index].Position.X = x1;
            vpt[index].Position.Y = y1;
            index++;
            vpt[index].Position.X = x1;
            vpt[index].Position.Y = y2;
        }
    }

    class AsyncManage
    {
        struct ThreadInfo
        {
            public AutoResetEvent are;
            public bool run;
        }
        static ThreadInfo[] pool = new ThreadInfo[3];

        static bool stop;
        static Action[] buff_async = new Action[9];

        static int waittask = 0;
        public static void Inital()//pc
        {
            stop = false;
            for (int i = 0; i < 3; i++)
            {
                pool[i].are = new AutoResetEvent(false);
            }
            Task.Run(() => { AsyncExcute(0); });
            Task.Run(() => { AsyncExcute(1); });
            Task.Run(() => { AsyncExcute(2); });
        }

        public static void AsyncDelegate(Action a)
        {
            for (int i = 0; i < 9; i++)
            {
                if (a == buff_async[i])
                    return;
            }
            if (waittask >= 9)//pc
                waittask = 0;
            buff_async[waittask] = a;
            int id = waittask % 3;
            if (!pool[id].run)
                pool[id].are.Set();
            waittask++;
            //Task.Run(a);
        }
        static void AsyncExcute(int s)
        {
            try
            {
                while (true)
                {
                    pool[s].are.WaitOne();
                    if(stop)
            return;
                    pool[s].run = true;
                    for (int i=s;i<buff_async.Length;i+=3)
                    {
                        if (buff_async[i] != null)
                        {
                            buff_async[i]();
                            buff_async[i] = null;
                        }
                    }
                    pool[s].run = false;
                }
                // ...
                throw null;    // 异常会在下面被捕获
                               // ...
            }
            catch (Exception ex)
            {
                // 一般会记录异常， 和/或通知其它线程我们遇到问题了
                // ...
                Debug.WriteLine(ex);
                
            }
        }

        public static void Stop()
        {
            stop = true;
            if(pool[0].are!=null)
            {
                pool[0].are.Set();
                pool[1].are.Set();
                pool[2].are.Set();
            }
        }
    }

    class FileManage
    {
        #region file 
        readonly static byte[] cypher = Encoding.UTF8.GetBytes("huqiang@1990outlook.com");
        public static byte[] AES_Encrypt(byte[] input, byte[] pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            //string encrypted = "";
            //try
            //{
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(pass));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);//key1
                Array.Copy(temp, 0, hash, 15, 16);//key2

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));

                //IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.UTF8.GetBytes(input));
                //encrypted = CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(AES, Buffer, null));
                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(input);
                byte[] Encrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Encrypt(AES, Buffer, null), out Encrypted);
                return Encrypted;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
        public static byte[] AES_Decrypt(byte[] input, byte[] pass)
        {
            SymmetricKeyAlgorithmProvider SAP = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            CryptographicKey AES;
            HashAlgorithmProvider HAP = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash Hash_AES = HAP.CreateHash();

            //string decrypted = "";
            //try
            //{
                byte[] hash = new byte[32];
                Hash_AES.Append(CryptographicBuffer.CreateFromByteArray(pass));
                byte[] temp;
                CryptographicBuffer.CopyToByteArray(Hash_AES.GetValueAndReset(), out temp);

                Array.Copy(temp, 0, hash, 0, 16);//key1
                Array.Copy(temp, 0, hash, 15, 16);//key2

                AES = SAP.CreateSymmetricKey(CryptographicBuffer.CreateFromByteArray(hash));
                //IBuffer Buffer = CryptographicBuffer.DecodeFromBase64String(input);
                IBuffer Buffer = CryptographicBuffer.CreateFromByteArray(input);
                byte[] Decrypted;
                CryptographicBuffer.CopyToByteArray(CryptographicEngine.Decrypt(AES, Buffer, null), out Decrypted);
                //decrypted = System.Text.Encoding.UTF8.GetString(Decrypted, 0, Decrypted.Length);

                return Decrypted;
            //}
            //catch (Exception ex)
            //{                
            //    return null;
            //}
        }
        unsafe static byte[] SaveLayout(ref LayOut data)
        {
            byte[] buff = new byte[4096];
            fixed (byte* pb = &buff[0])
            {
                float* pf = (float*)pb;
                *pf = data.role.origin.x;
                pf++;
                *pf = data.role.origin.y;
                pf++;
                int* head = (int*)pf;
                pf += 22;//offset=96
                int p = (int)pf;
                int c = 0;
                c = SaveData(ref data.land, p);
                *head = c;
                head++;
                p += c * 20;
                c = SaveData(ref data.needle, p);
                *head = c;
                head++;
                p += c * 20;
                c = SaveData(ref data.record, p);
                *head = c;
                head++;
                p += c * 20;
            }
            return buff;
        }
        unsafe static int SaveData(ref LandDate[] l,int p)
        {
            int c = 0;
            float* f =(float*) p;
            for (int i = 0; i < l.Length; i++)
            {
                if (l[i].active)
                {
                    *f = l[i].location.x;
                    f++;
                    *f = l[i].location.y;
                    f++;
                    *f = l[i].w;
                    f++;
                    *f = l[i].h;
                    f++;
                    *f = l[i].property;
                    f++;
                    c++;
                }
            }
            return c;
        }
        unsafe static LayOut LoadLayout(ref byte[] data,int offset)
        {
            LayOut l = new LayOut();
            fixed (byte* pb = &data[offset])
            {
                float* pf = (float*)pb;
                l.role.origin.x= *pf;
                pf++;
                l.role.origin.y= *pf;
                pf++;
                int* head = (int*)pf;
                pf += 22;//offset=96
                int p = (int)pf;
                int c = *head;
                LandDate[] t = new LandDate[c];
                LoadData(ref data,ref t, p);
                l.land = t;
                head++;
                p += c * 20;
                c = *head;
                t = new LandDate[c];
                LoadData(ref data, ref t, p);
                l.needle= t;
                head++;
                p += c * 20;
                c = *head;
                t = new LandDate[c];
                LoadData(ref data, ref t, p);
                l.record = t;
            }
            return l;
        }
        unsafe static void LoadData(ref byte[] data, ref LandDate[] l,int p)
        {
            float* f = (float*)p;
            for (int i = 0; i < l.Length; i++)
            {
                l[i].location.x=*f;
                f++;
                l[i].location.y=*f;
                f++;
                l[i].w=*f;
                f++;
                l[i].h=*f;
                f++;
                l[i].property=*f;
                f++;
                l[i].active = true;
            }
        }
        public static void SaveLayout(ref LayOut l,string name)
        {
            string path= ApplicationData.Current.LocalFolder.Path;
            path += "\\" + name + ".map";
            byte[] t = SaveLayout(ref l);
            FileStream fs;
            if(File.Exists(path))
            {
                fs = File.OpenWrite(path);
                fs.Seek(fs.Length,SeekOrigin.Begin);
                fs.Write(t, 0, 4096);
            }
            else
            {
                fs = File.Create(path, 4096);
                fs.Write(t, 0, 4096);
            }
            fs.Dispose();
        }
        public static void ReplaceLayout(ref LayOut l, string name,int index)
        {
            string path = ApplicationData.Current.LocalFolder.Path;
            path += "\\" + name + ".map";
            byte[] t = SaveLayout(ref l);
            FileStream fs;
            if (File.Exists(path))
            {
                fs = File.OpenWrite(path);
                fs.Seek(index*4096, SeekOrigin.Current);
                fs.Write(t, 0, 4096);
            }
            else
            {
                fs = File.Create(path, 4096);
                fs.Write(t, 0, 4096);
            }
            fs.Dispose();
        }
        public static List<LayOut> LoadLayout(string name)
        {
            string path = ApplicationData.Current.LocalFolder.Path;
            path += "\\" + name + ".map";
            FileStream fs;
            if (File.Exists(path))
            {
                fs= File.OpenRead(path);
                byte[] buff = new byte[fs.Length];
                fs.Read(buff,0,buff.Length);
                int c = buff.Length / 4096;
                List<LayOut> l = new List<LayOut>();
                for(int i=0;i< c;i++)
                    l.Add(LoadLayout(ref buff,i*4096));
                fs.Dispose();
                return l;
            }
            return null;
        }
        public static async void LoadAppLayout(string name,LoadLayout t)
        {
            Uri u = new Uri("ms-appx:///Maps/" + name);
            RandomAccessStreamReference rass = RandomAccessStreamReference.CreateFromUri(u);
            IRandomAccessStream ir= await rass.OpenReadAsync();
            Stream s = ir.AsStream();
            byte[] buff = new byte[s.Length];
            s.Read(buff,0,buff.Length);
            int c = buff.Length / 4096;
            LayOut[] l = new LayOut[c];
            for (int i = 0; i < c; i++)
                l[i] = LoadLayout(ref buff, i * 4096);
            t(l);
        }
        public static Archive LoadArichive()
        {
            string path = ApplicationData.Current.LocalFolder.Path;
            path += "\\vs";
            FileStream fs;
            Archive a=new Archive();
            if (File.Exists(path))
            {
                fs = File.Open(path, FileMode.Open);
                byte[] buff = new byte[fs.Length];
                fs.Read(buff,0,buff.Length);
                buff = AES_Decrypt(buff,cypher);
                unsafe
                {
                    fixed(byte* b=&buff[0])
                    {
                        int* ip = (int*)b;
                        a.level = *ip;
                        ip++;
                        a.die_count = *ip;
                        ip++;
                        a.achi_count = *ip;
                        ip++;
                        float* fp = (float*)ip;
                        a.loaction.x = *fp;
                        fp++;
                        a.loaction.y = *fp;
                    }
                }
            }
            else
            {
                fs= File.Create(path);
                byte[] c = new byte[20];
                c = AES_Encrypt(c,cypher);
                fs.Write(c,0,c.Length);
            }
            fs.Dispose();
            return a;
        }
        public static void SaveArichive(Archive a)
        {
            string path = ApplicationData.Current.LocalFolder.Path;
            path += "\\vs";
            FileStream fs;
            if (File.Exists(path))
                fs = File.Open(path, FileMode.Open);
            else
            {
                fs = File.Create(path);
                byte[] c = new byte[20];
                fs.Write(c, 0, c.Length);
            }
            byte[] buff = new byte[20];
            unsafe
            {
                fixed (byte* b = &buff[0])
                {
                    int* ip = (int*)b;
                    *ip= a.level;
                    ip++;
                    *ip= a.die_count;
                    ip++;
                    *ip=a.achi_count;
                    ip++;
                    float* fp = (float*)ip;
                    *fp= a.loaction.x;
                    fp++;
                    *fp= a.loaction.y;
                }
            }
            buff = AES_Encrypt(buff,cypher);
            fs.Seek(0,SeekOrigin.Current);
            fs.Write(buff,0,buff.Length);
            fs.Dispose();
        }
        #endregion

    }

}
