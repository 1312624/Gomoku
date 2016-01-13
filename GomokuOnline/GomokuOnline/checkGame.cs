using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Gomoku
{
    public class checkGame
    {
        public struct WinMove
        {
            public int row;
            public int col;
        }

        public static WinMove[] win = new WinMove[9];

        private static void Refresh(WinMove[] a)
        {
            for (int i = 0; i < 9; i++)
            {
                a[i].row = 0;
                a[i].col = 0;
            }
        }

        //Tham khảo project của Lê Bá Khánh Trình trên http://blog.myclass.vn/game-caro-c-co-xu-ly-ai-nang-cao/
        public static bool isWin(int row, int col, int index)
        {
            int r = 0, c = 0;
            int i, sameIndex = 0, count = 0;
            //Check hàng ngang
            while (c <= GlobalVariable.MaxCol - 5)
            {
                for (i = 0; i < 5; i++)
                {
                    if (GlobalVariable.isCheck[row, c + i] == index)
                    {
                        sameIndex++;
                        win[count].row = row;
                        win[count].col = c + i;
                        count++;
                    }
                    else
                    {
                        sameIndex = 0;
                        count = 0;
                        Refresh(win);
                    }
                }
                if (sameIndex >= 5) return true;
                c++;
            }

            //Check hàng dọc
            while (r <= GlobalVariable.MaxRow - 5)
            {
                for (i = 0; i < 5; i++)
                {
                    if (GlobalVariable.isCheck[r + i, col] == index)
                    {
                        sameIndex++;
                        win[count].row = r + i;
                        win[count].col = col;
                        count++;
                    }
                    else
                    {
                        sameIndex = 0;
                        count = 0;
                        Refresh(win);
                    }
                }
                if (sameIndex >= 5) return true;
                r++;
            }

            //Check duong cheo xuong trai qua phai
            r = row; c = col;
            while (r > 0 && c > 0) { r--; c--; }
            while (r <= GlobalVariable.MaxRow - 5 && c <= GlobalVariable.MaxCol - 5)
            {
                for (i = 0; i < 5; i++)
                {
                    if (GlobalVariable.isCheck[r + i, c + i] == index)
                    {
                        sameIndex++;
                        win[count].row = r + i;
                        win[count].col = c + i;
                        count++;
                    }
                    else
                    {
                        sameIndex = 0;
                        count = 0;
                        Refresh(win);
                    }
                }
                if (sameIndex >= 5) return true;
                c++; r++;
            }

            //Check duong cheo len trai qua phai    
            r = row; c = col;
            while (r < GlobalVariable.MaxRow - 1 && c > 0) { r++; c--; }
            while (r >= 4 && c <= GlobalVariable.MaxCol - 5)
            {
                for (i = 0; i < 5; i++)
                {
                    if (GlobalVariable.isCheck[r - i, c + i] == index)
                    {
                        sameIndex++;
                        win[count].row = r - i;
                        win[count].col = c + i;
                        count++;
                    }
                    else
                    {
                        sameIndex = 0;
                        count = 0;
                        Refresh(win);
                    }
                }
                if (sameIndex >= 5) return true;
                c++; r--;
            }
            return false;
        }
    }
}