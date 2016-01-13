using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Quobject.EngineIoClientDotNet.Client;
using Socket = Quobject.SocketIoClientDotNet.Client.Socket;

namespace Gomoku
{
    class GlobalVariable
    {
        public static int MaxRow = 12;
        public static int MaxCol = 12;
        public static int[,] isCheck = new int[MaxRow, MaxCol]; // mảng kiểm tra ô đã được đánh chưa, nếu đánh rồi là của người chơi nào
        public static int index = 0; // player #1;
        public static string myName = "";

        //Tham khảo project của Lê Bá Khánh Trình trên http://blog.myclass.vn/game-caro-c-co-xu-ly-ai-nang-cao/
        public static int maxDepth = 11; // độ sâu của máy
        public static int maxMove = 3; // số lược đi thử của máy
        public static int depth = 0;
        public static bool fWin = false;
        
        public static int[] DScore = new int[5] { 0, 1, 9, 81, 729 }; // điểm phòng thủ
        public static int[] AScore = new int[5] { 0, 2, 18, 162, 1458 }; // điểm tấn công
        //public static int[] AScore = new int[5] { 0, 1, 9, 81, 729 };

        public static Point[] PCMove = new Point[maxMove + 2];
        public static Point[] HumanMove = new Point[maxMove + 2];
        public static Point[] WinMove = new Point[maxDepth + 2];
        public static Point[] LoseMove = new Point[maxDepth + 2];

        public static int Mode = 0; // 1: player vs player, 2 : player vs com
        public static Socket Socket;
    }
}
