using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace I_Wanna
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        #region language
        static string[] s_level = { "关卡","level"};
        static string[] s_add = {"添加","Add new" };
        static string[] s_view = {"查看","View" };
        static string[] s_edit = {"编辑","Edit" };
        static string[] s_replace = {"替换","Replace" };
        static string[] s_delete = {"删除","Delete" };
        static string[][] s_menu = { s_view,s_edit,s_replace,s_delete};
        static string[] declare = { "Esc=返回\r\n F12=地图编辑(F2=菜单 F5=调试)\r\n F1=存档\r\n R=回到原点\r\n A=left=左\r\n D=right=右\r\n J=大跳\r\n K=小跳",
        "Esc=Back\r\nF12=Map eidt(F2=Menu F5=Debug)\r\n F1 = Archive\r\n R = origin\r\n A = left\r\n D = Right\r\n J = Large jump\r\n K = Small jump"};
        #endregion

        readonly Game1 _game;
        public static GamePage page{ get; set; }
		public GamePage()
        {
            this.InitializeComponent();
#if phone
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
#endif
            Window.Current.CoreWindow.SizeChanged += (o, e) =>
            {
                Main.screenX = (float)e.Size.Width;
                Main.screenY = (float)e.Size.Height;
                //AsyncManage.AsyncDelegate(Controls.SizeChanged);
            };
            // Create the game.
            var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<Game1>.Create(launchArguments, Window.Current.CoreWindow, swapChainPanel);
            page = this;
        }
        Canvas can;
        ListBox lb;
        public void CreateMenu()
        {
            float x = (float)Window.Current.Bounds.Width - 80;
            if (can != null)
            {
                can.Margin= new Thickness(x, 0, 0, 0);
                can.Visibility = Visibility.Visible;
                if(lb.SelectedIndex!=-1 & lb.SelectedIndex!=lb.Items.Count-1)
                    if (sp != null)
                        sp.Visibility = Visibility.Visible;
                        return;
            }
            can = new Canvas();
            swapChainPanel.Children.Add(can);
            lb = new ListBox();
            can.Margin = new Thickness(x,0,0,0);
            lb.Width = 100;
            lb.Height = 720;
            lb.Background = new SolidColorBrush(Color.FromArgb(64, 128, 128, 128));
            can.Children.Add(lb);
            lb.SelectionChanged += (o,e)=>{
                if (lb.SelectedIndex < lb.Items.Count - 1)
                    ShowMenuA();
                else Add();
            };
            int c = Editor.GetCount();
            for (int i = 0; i < c; i++)
                lb.Items.Add(s_level[language]+i.ToString());
            lb.Items.Add(s_add[language]);
        }
        public void HideMenu()
        {
            can.Visibility = Visibility.Collapsed;
            if (sp != null)
                sp.Visibility = Visibility.Collapsed;
        }
        StackPanel sp;
        Button[] act;
        public int language { set; get; }
        void ShowMenuA()
        {
            float y = Main.mouseY;
            if (y > Main.screenY - 136)
                y = Main.screenY - 136;
            if(sp!=null)
            {
                sp.Visibility = Visibility.Visible;
                sp.Margin = new Thickness(Main.screenX - 150, y, 0, 0);
                return;
            }
            sp = new StackPanel();
            swapChainPanel.Children.Add(sp);
            sp.Margin = new Thickness(Main.screenX-150,y,0,0);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(64, 128, 128, 128));
            act = new Button[3];
            for (int i=0;i<3;i++)
            {
                act[i] = new Button();
                act[i].Width = 70;
                act[i].Background = scb;
                act[i].Content = s_menu[i][language];
                sp.Children.Add(act[i]);
            }
            act[0].Click += (o, e) => { View(); };
            act[1].Click += (o, e) => { Edit(); };
            act[2].Click += (o, e) => { Replace(); };
            //act[3].Click += (o, e) => { Delete(); };
        }
        void Add()
        {
            int c = lb.Items.Count - 1;
            lb.Items.Insert(c, s_level[language]+c.ToString());
            if (sp != null)
                sp.Visibility = Visibility.Collapsed;
            Editor.Save();
        }
        void View()
        {
            Editor.View(lb.SelectedIndex);
            Main.game.delay = 2;
        }
        void Edit()
        {
            Editor.Edit(lb.SelectedIndex);
            Main.game.delay = 2;
        }
        void Replace()
        {
            Editor.Replace(lb.SelectedIndex);
        }
        void Delete()
        {
            Editor.Delete(lb.SelectedIndex);
            lb.Items.RemoveAt(lb.SelectedIndex);
        }
        TextBlock tb;
        public void ShowDeclare()
        {
            if (tb == null)
            {
                tb = new TextBlock();
                tb.Foreground = new SolidColorBrush(Colors.Black);
                tb.Text = declare[language];
                swapChainPanel.Children.Add(tb);
            }
            else tb.Visibility = Visibility.Visible;
        }
        public void HideDeclare()
        {
            tb.Visibility = Visibility.Collapsed;
        }
        public void ShowText(string str, float x,float y)
        {
            tb.Margin = new Thickness(x,y,0,0);
            if(str!=null)
            tb.Text = str;
        }
    }
}
