using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace Gomoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ChooseMode mode = new ChooseMode();
            mode.ShowDialog();
            InitializeComponent();
            DrawTable();
            update += AIUpdate; // Theard riêng để tính toán nước đi của AI (offline Mode)
            autoupdate += AIAutoUpdate; //Theard riêng để tính toán nước đi của AI(online Mode)
        }

        // Vẽ bàn cờ
        public void DrawTable()
        {
            ChessBroad.Children.Clear();
            for (int i = 0; i < GlobalVariable.MaxRow; i++)
                for (int j = 0; j < GlobalVariable.MaxCol; j++)
                {
                    GlobalVariable.isCheck[i, j] = -1;
                    // -1 là chưa đánh, 0 là người chơi 1, 1 là người chơi 2, 2 là máy
                    Button btn = new Button()
                    {
                        BorderBrush = Brushes.White,
                    };
                    //Set Event Click
                    btn.Click += new RoutedEventHandler(btn_Click);

                    Grid.SetColumn(btn, i);
                    Grid.SetRow(btn, j);
                    if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                        btn.Background = Brushes.WhiteSmoke;
                    else
                        btn.Background = Brushes.DarkGray;
                    ChessBroad.Children.Add(btn);
                }
            GlobalVariable.depth = 0;
            GlobalVariable.fWin = false;
            tbxChat.IsEnabled = true;
            GlobalVariable.index = 0;
        }

        //Event khi chọn ô cờ để đánh
        private void btn_Click(object sender, EventArgs e)
        {
            if (canChange == false)
            {
                MessageBox.Show("Please Enter Your Name!", "Gomoku", MessageBoxButton.OK);
                return;
            }
            if (GlobalVariable.Mode == 0)
            {
                MessageBox.Show("Please Choose Your Mode!", "Gomoku", MessageBoxButton.OK);
                return;
            }
            if (GlobalVariable.Mode == 4)
            {
                MessageBox.Show("This Is AI Online Mode!", "Gomoku", MessageBoxButton.OK);
                return;
            }
            Button btn = sender as Button;
            Button bt = new Button();
            int row = Grid.GetRow(btn);
            int col = Grid.GetColumn(btn);

            if (GlobalVariable.isCheck[row, col] == -1)
            {
                if (GlobalVariable.Mode != 3)
                {
                    if (GlobalVariable.index == 0)
                    {
                        OpacityAnimation(btn, false, false, 1);
                        BackGroundButton(btn, row, col, GlobalVariable.index);
                        GlobalVariable.isCheck[row, col] = GlobalVariable.index;

                        if (checkGame.isWin(row, col, 0) == true)
                        {
                            //Winning Animation
                            WinningAnimation(row, col, 0);
                            MessageBox.Show("Player win!", "Gomoku", MessageBoxButton.OK);
                            DrawTable();
                            return;
                        }

                        if (GlobalVariable.Mode == 2)
                        {
                            //Tham khảo project của Lê Bá Khánh Trình trên http://blog.myclass.vn/game-caro-c-co-xu-ly-ai-nang-cao/
                            Thread AIThread = new Thread(PCMove);
                            AIThread.Start();
                        }
                        else GlobalVariable.index = 1 - GlobalVariable.index;
                    }
                    else
                    {
                        OpacityAnimation(btn, false, false, 1);
                        BackGroundButton(btn, row, col, GlobalVariable.index);
                        GlobalVariable.isCheck[row, col] = GlobalVariable.index;
                        if (checkGame.isWin(row, col, GlobalVariable.index) == true)
                        {
                            //Winning Animation
                            WinningAnimation(row, col, 1);
                            MessageBox.Show("Player 2 win!", "Gomoku", MessageBoxButton.OK);
                            DrawTable();
                            return;
                        }
                        GlobalVariable.index = 1 - GlobalVariable.index;
                    }
                }
                else if (GlobalVariable.Mode == 3) // play Online
                {
                    if (YourTurn == true)
                    {
                        GlobalVariable.isCheck[row, col] = 0; // isMe;
                        GlobalVariable.Socket.Emit("MyStepIs", JObject.FromObject(new { row = row, col = col }));
                        YourTurn = false;
                        this.Dispatcher.Invoke(() =>
                        {
                            OpacityAnimation(btn, false, false, 1);
                            BackGroundButton(btn, row, col, 0);
                        });
                    }
                    else
                    {
                        PushMessage("This is not your turn", "Server");
                    }
                }
                else return; // Đánh AI online
            }

        }


        // Thread riêng cho AI Move (offline Mode);
        #region AITheard(Offline)
        private delegate void UpdateChessBroad(Point PCMove);

        private event UpdateChessBroad update;
        private void PCMove()
        {
            //Thread.Sleep(5000);
            Point AIMove = new Point();
            AI PC = new AI();
            PC.AIStart();
            if (GlobalVariable.fWin)
            {
                PC.Move((int)GlobalVariable.WinMove[0].Y, (int)GlobalVariable.WinMove[0].X);
                AIMove = GlobalVariable.WinMove[0];
            }
            else
            {
                PC.EvalBoard(2);
                AIMove = PC.MaxPos();
                PC.Move((int)AIMove.Y, (int)AIMove.X);
            }
            //tính toán nước đi ở phía trên
            update(AIMove);
        }

        private void AIUpdate(Point PCMove) // invoke cho class chứa event update thực hiện việc update
        {
            this.Dispatcher.Invoke(() =>
            {
                Button bt = new Button();
                UpdateUI((int) PCMove.Y, (int) PCMove.X, bt);
            });
        }

        #endregion

        // Thread riêng cho AI Move (online Mode);
        #region AITheard(Online)
        //event AI tự đánh (online mode)
        private delegate void AutoChessBroad(Point PCMove);

        private event AutoChessBroad autoupdate;

        private void PCAutoPlay()
        {
            if (YourBotTurn == false)
            {
                PushMessage("This is not your turn","Server");
                return;    
            }
            Point AIAutoMove = new Point();
            AI PC = new AI();
            PC.AIStart();
            if (GlobalVariable.fWin)
            {
                PC.Move((int)GlobalVariable.WinMove[0].Y, (int)GlobalVariable.WinMove[0].X);
                AIAutoMove = GlobalVariable.WinMove[0];
            }
            else
            {
                PC.EvalBoard(2);
                AIAutoMove = PC.MaxPos();
                PC.Move((int)AIAutoMove.Y, (int)AIAutoMove.X);
            }
            autoupdate(AIAutoMove); // update UI;
        }
        private Thread aiAutoThread;
        private void AIAutoUpdate(Point PCMove)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (YourBotTurn == false) return;
                Button bt = new Button();
                GlobalVariable.isCheck[(int)PCMove.Y, (int)PCMove.X] = 2; // isMyAI;
                bt = (Button)ChessBroad.Children[(int)PCMove.Y + (int)PCMove.X * GlobalVariable.MaxRow];
                OpacityAnimation(bt, false, false, 1);
                BackGroundButton(bt, (int)PCMove.Y, (int)PCMove.X, 2);
                GlobalVariable.Socket.Emit("MyStepIs", JObject.FromObject(new { row = (int)PCMove.Y, col = (int)PCMove.X }));
                YourBotTurn = false;
            });
        }
#endregion
        // Update giao diện
        private void UpdateUI(int row, int col, Button bt)
        {
            GlobalVariable.isCheck[row, col] = 2;
            bt = (Button) ChessBroad.Children[row + col*GlobalVariable.MaxRow];
            //Animation khi đánh
            OpacityAnimation(bt, false, false, 1.5);
            BackGroundButton(bt, row, col, 2);
            if (checkGame.isWin(row, col, 2) == true)
            {
                WinningAnimation(row, col, 2);
                MessageBox.Show("Computer win!", "Gomoku", MessageBoxButton.OK);
                DrawTable();
                return;
            }
        }

        //Update Nút
        private void BackGroundButton(Button bt, int row, int col, int index)
        {
            Uri resourceUri;
            if (index == 0)
            {
                if ((row%2 == 0 && col%2 != 0) || (row%2 != 0 && col%2 == 0))
                    resourceUri = new Uri("Icon/index0a.png", UriKind.Relative);
                else resourceUri = new Uri("Icon/index0b.png", UriKind.Relative);
            }
            else
            {
                if ((row%2 == 0 && col%2 != 0) || (row%2 != 0 && col%2 == 0))
                    resourceUri = new Uri("Icon/index1a.png", UriKind.Relative);
                else resourceUri = new Uri("Icon/index1b.png", UriKind.Relative);
            }
            ImageBrush brush = new ImageBrush();
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            brush.ImageSource = temp;
            bt.Background = brush;
        }

        #region StartGame
        bool canChange = false;

        private void btnName_Click(object sender, RoutedEventArgs e)
        {
            if (tbxName.Text == "")
            {
                MessageBox.Show("Please Enter Your Name!", "Gomoku", MessageBoxButton.OK);
                return;
            }
            if (canChange == false) // Chưa đặt tên
            {
                GlobalVariable.myName = tbxName.Text;
                canChange = true; // đã connect vào rồi
                btnName.Content = "Change!";
                tbxChat.IsEnabled = true;
                TextBlock tb = new TextBlock()
                {
                    FontWeight = FontWeights.Bold,
                    Text = GlobalVariable.myName + " started a new game \n",
                };
                OpacityAnimation(tb, false, false, 1.5);
                ViewPanel.Children.Add(tb);
                if (GlobalVariable.Mode == 3 || GlobalVariable.Mode == 4)
                {
                    InitConnect();
                    ChatConnect();
                    GamePlayConnect();
                }
            }
            else //Thay đổi tên
            {
                tbxName.IsEnabled = true;
                GlobalVariable.myName = tbxName.Text;
                if (GlobalVariable.Mode == 3 || GlobalVariable.Mode == 4)
                {
                    GlobalVariable.Socket.Emit("MyNameIs", GlobalVariable.myName);
                }
                MessageBox.Show("Name Changed Successfully!!!", "Gomoku", MessageBoxButton.OK);
            }
        }


        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalVariable.Mode == 1 || GlobalVariable.Mode == 2)
            {
                if (canChange == false)
                {
                    MessageBox.Show("Please Enter Your Name!", "Gomoku", MessageBoxButton.OK);
                    return;
                }
                PushMessage(tbxChat.Text,GlobalVariable.myName);
            }
            else GlobalVariable.Socket.Emit("ChatMessage", tbxChat.Text);
            tbxChat.Text = "";
        }
#endregion
        #region Animation

        private void OpacityAnimation(UIElement UI, bool isReverse, bool isRepeat, double second)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(second));
            if (isRepeat == true && isReverse == true)
            {
                da.RepeatBehavior = RepeatBehavior.Forever;
                da.AutoReverse = true;
            }
            UI.BeginAnimation(OpacityProperty, da);
        }

        private void WinningAnimation(int row, int col, int index)
        {
            for (int i = 0; i < checkGame.win.Length; i++)
            {
                Button button = new Button();
                button = (Button) ChessBroad.Children[checkGame.win[i].row + checkGame.win[i].col*12];
                OpacityAnimation(button, true, true, 1);
            }
        }

        void PushMessage(string a, string from)
        {
            a = a.Replace("<br />", "\r\n"); // chuyển đổi kí tự xuống dòng
            TextBlock tb = new TextBlock()
            {
                FontWeight = FontWeights.Bold,
                Text = from + "\t\t\t\t" + DateTime.Now.ToLongTimeString() + "\n",
            };

            TextBlock tb1 = new TextBlock()
            {
                FontWeight = FontWeights.Normal,
                Text = a,
            };
                
            TextBlock tb2 = new TextBlock()
            {
                FontWeight = FontWeights.Thin,
                Text = "...................................................................................................................\n",
            };

            OpacityAnimation(tb,false,false,1.5);
            OpacityAnimation(tb1, false, false, 1.5);
            OpacityAnimation(tb2, false, false, 1.5);

            ViewPanel.Children.Add(tb);
            ViewPanel.Children.Add(tb1);
            ViewPanel.Children.Add(tb2);
            ScrollChat.ScrollToEnd();
        }

#endregion
        
        #region SOCKET
        void InitConnect()
        {
            // get IP Server from App.config
            string uri = System.Configuration.ConfigurationManager.AppSettings["IPServer"];
            GlobalVariable.Socket = IO.Socket(uri); 
            GlobalVariable.Socket.On(Socket.EVENT_CONNECT, () =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    PushMessage("Connected!", "Server");
                });
            });

            GlobalVariable.Socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    PushMessage(((JObject)data)["message"].ToString(), ((JObject)data)["from"].ToString());
                });

            });
            GlobalVariable.Socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                this.Dispatcher.Invoke(() =>
                {

                    PushMessage(((JObject)data)["message"].ToString(), "Server");
                });
            });

            GlobalVariable.Socket.On(Socket.EVENT_ERROR, (data) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    PushMessage(((JObject)data)["message"].ToString(), "Server");
                });
            });
        }

        private bool YourTurn = false;
        private bool YourBotTurn = false;

        public object ConfigurationManager { get; private set; }

        void ChatConnect()
        {
            GlobalVariable.Socket.On("ChatMessage", (data) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    string From = "";
                    try
                    {
                        From = ((JObject) data)["from"].ToString();
                    }
                    catch
                    {
                        From = "Server";
                    }
                    PushMessage(((JObject) data)["message"].ToString(), From);
                });
                if (((JObject) data)["message"].ToString() == "Welcome!")
                {
                    GlobalVariable.Socket.Emit("MyNameIs", GlobalVariable.myName);
                    GlobalVariable.Socket.Emit("ConnectToOtherPlayer");
                }
                if (((JObject) data)["message"].ToString().EndsWith("first player!") == true)
                {
                    if (GlobalVariable.Mode == 3)
                    {
                        YourTurn = true;
                        return;
                    }
                    Random r = new Random(); // nếu mode 4 mới random
                    int row = r.Next(4, 6);
                    int col = r.Next(4, 6);
                    this.Dispatcher.Invoke(() =>
                    {
                        GlobalVariable.isCheck[row, col] = 2;
                        Button but = (Button) ChessBroad.Children[row + col*GlobalVariable.MaxRow];
                        BackGroundButton(but, row, col, 2);
                        OpacityAnimation(but, false, false, 1);
                        GlobalVariable.Socket.Emit("MyStepIs", JObject.FromObject(new { row = row, col = col }));
                    });
                }
            });
        }

        private void GamePlayConnect()
        {
            if (YourTurn) return;
            if (YourBotTurn) return;
            GlobalVariable.Socket.On("NextStepIs", (data) =>
            {
                int Row = (int)((JObject)data)["row"];
                int Col = (int)((JObject)data)["col"];
                this.Dispatcher.Invoke(() =>
                {
                    if (GlobalVariable.isCheck[Row, Col] == 0 || GlobalVariable.isCheck[Row, Col] == 2) return; // tránh lỗi hiển thị sai icon X hoặc O
                    if (GlobalVariable.Mode == 3)
                    {
                        GlobalVariable.isCheck[Row, Col] = 1; // đối phương đánh;
                        Button btn = (Button)ChessBroad.Children[Row + Col * GlobalVariable.MaxRow];
                        OpacityAnimation(btn, false, false, 1);
                        BackGroundButton(btn, Row, Col, 1);
                        YourTurn = true; // đến lượt mình đánh
                    }
                    if (GlobalVariable.Mode == 4)
                    {
                        GlobalVariable.isCheck[Row, Col] = 0; // đối phương đánh;
                        Button btn = (Button)ChessBroad.Children[Row + Col * GlobalVariable.MaxRow];
                        OpacityAnimation(btn, false, false, 1);
                        BackGroundButton(btn, Row, Col, 0);

                        YourBotTurn = true; // đến lượt Bot mình;
                        aiAutoThread = new Thread(PCAutoPlay);
                        aiAutoThread.Start();
                    }
                });
            });

            GlobalVariable.Socket.On("EndGame", (data) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(((JObject)data)["message"].ToString(), "The game is now stopped!");
                    InitConnect();
                    ChatConnect();
                    GamePlayConnect();
                    DrawTable();
                });
            });
        }
#endregion
    }
}
