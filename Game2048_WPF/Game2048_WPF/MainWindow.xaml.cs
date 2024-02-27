using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game2048_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private int[,] arr = new int[4, 4];             //当前数字矩阵，用以显示WPF
        private int[,] arrbackup = new int[4, 4];       //矩阵备份，用于判断输赢和状态转换
        private Random rd = new Random();               //随机数种子，用于生成随机的2
        private List<CoordinateTools> listOfCoo = new List<CoordinateTools>();  //当前矩阵所有格子，往里面塞 2 用
        private List<Label> labelList = new List<Label>();  //WPF窗口中所有Label的集合

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown; //监听键盘事件
            FindLabels(); //保存所有label
        }

        private void FindLabels()
        {
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    string labName = "lab" + i.ToString() + j.ToString(); //WPF中label的命名规则是lab00 lab01 到 lab33
                    Label label = (Label)FindName(labName);
                    labelList.Add(label);
                }
            }
        }

        //游戏开始
        public void GameStart()
        {
            //往矩阵随机塞两个2 然后绘制窗口
            Add2();
            Add2();
            Repaint();
        }

        //按钮点击事件
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            GameStart();
            textBlock.Text = "游戏开始....\n\n";
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "游戏已重置，请继续游玩!\n";
            //清空矩阵
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    arr[i, j] = 0;
                }
            }

            GameStart();
        }

        //重新绘制窗口显示内容
        public void Repaint()
        {
            string str = "";
            int i = 0, j = 0;
            //foreach列表，列表保存的时候一行为基准，因此i j控制矩阵的位置也以行为基准，保证一一对应
            foreach (Label label in labelList)
            {
                Image image = new Image();
                if (j == 4)
                {
                    i++;
                    j = 0;
                }
                //一个作用域，为了方便区分
                {   //负责将矩阵数字转换给窗口对应图片
                    if (arr[i, j] == 0)
                    {
                        label.Content = str;
                    }
                    if (arr[i, j] == 2)
                    {
                        image.Source = new BitmapImage(new Uri("images/2.png", UriKind.Relative)); //UriKind.Relative代表本地相对路径
                        label.Content = image;
                    }
                    if (arr[i, j] == 4)
                    {
                        image.Source = new BitmapImage(new Uri("images/4.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 8)
                    {
                        image.Source = new BitmapImage(new Uri("images/8.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 16)
                    {
                        image.Source = new BitmapImage(new Uri("images/16.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 32)
                    {
                        image.Source = new BitmapImage(new Uri("images/32.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 64)
                    {
                        image.Source = new BitmapImage(new Uri("images/64.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 128)
                    {
                        image.Source = new BitmapImage(new Uri("images/128.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 256)
                    {
                        image.Source = new BitmapImage(new Uri("images/256.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 512)
                    {
                        image.Source = new BitmapImage(new Uri("images/512.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 1024)
                    {
                        image.Source = new BitmapImage(new Uri("images/1024.png", UriKind.Relative));
                        label.Content = image;
                    }
                    if (arr[i, j] == 2048)
                    {
                        image.Source = new BitmapImage(new Uri("images/2048.png", UriKind.Relative));
                        label.Content = image;
                    }
                }
                j++;
            }
        }

        //键盘响应事件
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //↑↓←→ 和 WASD
            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    MoveUp();
                    break;
                case Key.Down:
                case Key.S:
                    MoveDown();
                    break;
                case Key.Left:
                case Key.A:
                    MoveLeft();
                    break;
                case Key.Right:
                case Key.D:
                    MoveRight();
                    break;
                default:
                    textBlock.Text += "\n请按下正确按键!\n";
                    break;
            }
        }

        //保存上一步数组状态
        private void saveStat()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    arrbackup[i, j] = arr[i, j];
                }
            }
        }

        //更新数组状态
        private void updateStat()
        {
            foreach (int item in arr)
            {
                if (item == 2048)
                {
                    textBlock.Text += "\n(ﾉ´▽｀)ﾉ♪  游戏胜利 (ﾉ´▽｀)ﾉ♪\n";
                    Last();
                }
            }

            bool flag = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (arrbackup[i, j] != arr[i, j])
                    {
                        // 一旦有任意一个元素在之前之后不一样  那么falg改为true
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                // 如果falg是true 说明变了, 如果变了 就刷一个2出来,  
                // 反之就什么也不干
                Add2();
            }

            // 输出到控制台
            Repaint();

            // 检测按下方向键之后死没死
            if (!End())
            {
                textBlock.Text += "\n(；´д｀)ゞ  游戏失败  (；´д｀)ゞ\n";
                Last();
            }
        }

        //遍历非零元素 随机把一个赋为2 
        public void Add2()
        {
            listOfCoo.Clear();
            // 遍历所有零元素的坐标
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (arr[i, j] == 0)
                    {
                        // 把遍历到的坐标 当成参数 实例化
                        CoordinateTools coo = new CoordinateTools(i, j);
                        // 把实例化的结果add到list里
                        listOfCoo.Add(coo);
                    }
                }
            }
            // 如果列表里一个元素都没存进来 说明表里没有空格了 直接退出
            if (listOfCoo.Count == 0)
            {
                return;
            }
            // 从表里随机取一个位置 ​
            int cooPlus = rd.Next(0, listOfCoo.Count);
            // 把这个位置赋值改写为2
            arr[listOfCoo[cooPlus].x, listOfCoo[cooPlus].y] = 2;
        }
        
        
        //键盘得上下左右移动  每一步移动都有保存状态和更新状态，用以判断输赢等
        public void MoveDown()
        {
            saveStat(); //一个对二维矩阵的操作，不做赘述
            for (int j = 0; j < 4; j++)
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (arr[i, j] == 0) continue;
                    for (int k = i + 1; k < 4; k++)
                    {
                        if (arr[k, j] != 0)
                        {
                            if (arr[i, j] == arr[k, j])
                            {
                                arr[k, j] += arr[i, j];
                                arr[i, j] = 0;
                                break;
                            }
                            else if (arr[i, j] != arr[k, j] && k - 1 != i)
                            {
                                arr[k - 1, j] = arr[i, j];
                                arr[i, j] = 0;
                                break;
                            }
                            else if (arr[i, j] != arr[k, j] && k - 1 == i)
                            {
                                break;
                            }
                        }
                        if (k == 3)
                        {
                            arr[k, j] = arr[i, j];
                            arr[i, j] = 0;
                            break;
                        }
                    }
                }
            }
            updateStat();
        }

        public void MoveUp()
        {   //把矩阵倒转，然后调用Down，然后再倒回来，就是Up
            saveStat();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int tmp = 0;
                    tmp = arr[i, j];
                    arr[i, j] = arr[3 - i, j];
                    arr[3 - i, j] = tmp;
                }
            }
            MoveDown();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int tmp = 0;
                    tmp = arr[i, j];
                    arr[i, j] = arr[3 - i, j];
                    arr[3 - i, j] = tmp;
                }
            }
            updateStat();
        }

        public void MoveLeft()
        {
            saveStat();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (arr[i, j] == 0) continue;
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (arr[i, k] != 0)
                        {
                            if (arr[i, j] == arr[i, k])
                            {
                                arr[i, k] += arr[i, j];
                                arr[i, j] = 0;
                                break;
                            }
                            else if (arr[i, j] != arr[i, k] && k + 1 != j)
                            {
                                arr[i, k + 1] = arr[i, j];
                                arr[i, j] = 0;
                                break;
                            }
                            else if (arr[i, j] != arr[i, k] && k + 1 == j)
                            {
                                break;
                            }
                        }
                        if (k == 0)
                        {
                            arr[i, k] = arr[i, j];
                            arr[i, j] = 0;
                            break;
                        }
                    }
                }
            }
            updateStat();
        }

        public void MoveRight()
        {   //倒转，调用Left，再倒回来 就是Right
            saveStat();
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    int tmp = 0;
                    tmp = arr[i, j];
                    arr[i, j] = arr[i, 3 - j];
                    arr[i, 3 - j] = tmp;
                }
            }
            MoveLeft();
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    int tmp = 0;
                    tmp = arr[i, j];
                    arr[i, j] = arr[i, 3 - j];
                    arr[i, 3 - j] = tmp;
                }
            }
            updateStat();
        }

        //判断是否失败
        public bool End()
        {
            // 遍历数组 有任何一个空元素都说明不可能死
            foreach (int item in arr)
            {
                if (item == 0)
                    return true;
            }
            // 从2开始到2048进行遍历   
            // 目的是检测 每一个数字 他上下左右相邻有没有和他一样的数字 
            for (int num = 2; num <= 2048; num *= 2)
            {
                List<CoordinateTools> listOfget2 = new List<CoordinateTools>();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (arr[i, j] == num)
                        {
                            // 先把所有值为NUM的元素的下标 存到list里
                            CoordinateTools coo = new CoordinateTools(i, j);
                            listOfget2.Add(coo);
                        }
                    }
                }
                // 如果这个list 是空的  就说明当前表里没有num 回到FOR继续
                if (listOfget2 == null)
                {
                    continue;
                }

                // 从列表里的第一个元素开始 (每一个元素存的都是一组下标x,y)
                foreach (CoordinateTools item in listOfget2)
                {
                    foreach (CoordinateTools item2 in listOfget2)
                    {
                        // 判断 同一行的是不是列坐标差的绝对值是1  同一列的是不是行坐标差的绝对值是1
                        if ((item.y == item2.y && Math.Abs(item.x - item2.x) == 1) ||
                            (item.x == item2.x && Math.Abs(item.y - item2.y) == 1))
                        {
                            // 如果有一个 就不用再循环了 肯定没死
                            return true;
                        }
                    }
                }
            }
            // 全遍历完了 就说明已经死了 返回false
            return false;
        }

        //游戏结束提示语
        public void Last()
        {
            textBlock.Text += "\n游戏已结束\n继续游玩请点击重置按钮\n";
        }

        //工具类 用于存储搜索到的数组的下标
        public class CoordinateTools
        {
            public int x { set; get; }
            public int y { set; get; }
            public CoordinateTools(int i, int j)
            {
                this.x = i;
                this.y = j;
            }
        }
    }
}
