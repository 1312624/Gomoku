using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gomoku
{
    //Tham khảo project của Lê Bá Khánh Trình trên http://blog.myclass.vn/game-caro-c-co-xu-ly-ai-nang-cao/
    class AI
    {
        public AI()
        {
        }

        public int[,] AIBoard = new int[GlobalVariable.MaxRow + 2, GlobalVariable.MaxCol + 2];
        int row, col; // toạ độ mà máy đi
        bool isLose = false;

        public void Move(int r, int c)
        {
            row = r;
            col = c;
        }

        public int GetRow()
        {
            return row;
        }

        public int GetCol()
        {
            return col;
        }

        public void ResetBoard()
        {
            for (int r = 0; r < GlobalVariable.MaxRow + 2; r++)
                for (int c = 0; c < GlobalVariable.MaxCol + 2; c++)
                    AIBoard[c, r] = 0;
        }

        public Point MaxPos()
        {
            int Max = 0;
            Point p = new Point();
            for (int i = 0; i < GlobalVariable.MaxRow; i++)
            {
                for (int j = 0; j < GlobalVariable.MaxCol; j++)
                {
                    if (AIBoard[j, i] > Max)
                    {
                        p.X = i; p.Y = j;
                        Max = AIBoard[j, i];
                    }

                }
            }
            return p;
        }

        public void EvalBoard(int index)
        {
            int rw, cl, ePC, eHuman;
            ResetBoard();
            //Danh gia theo cot
            for (cl = 0; cl < GlobalVariable.MaxCol; cl++)
                for (rw = 0; rw < GlobalVariable.MaxRow - 4; rw++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GlobalVariable.isCheck[rw + i, cl] == 0) eHuman++;
                        if (GlobalVariable.isCheck[rw + i, cl] == 2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (GlobalVariable.isCheck[rw + i, cl] == -1)
                            {
                                if (eHuman == 0)
                                    if (index == 0)
                                        AIBoard[rw + i, cl] += GlobalVariable.DScore[ePC];
                                    else AIBoard[rw + i, cl] += GlobalVariable.AScore[ePC];
                                if (ePC == 0)
                                    if (index == 2)
                                        AIBoard[rw + i, cl] += GlobalVariable.DScore[eHuman];
                                    else AIBoard[rw + i, cl] += GlobalVariable.AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    AIBoard[rw + i, cl] *= 2;
                            }
                        }
                    }
                }

            //Danh gia theo hang
            for (rw = 0; rw < GlobalVariable.MaxRow; rw++)
                for (cl = 0; cl < GlobalVariable.MaxCol - 4; cl++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GlobalVariable.isCheck[rw, cl + i] == 0) eHuman++;
                        if (GlobalVariable.isCheck[rw, cl + i] == 2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (GlobalVariable.isCheck[rw, cl + i] == -1) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (index == 0)
                                        AIBoard[rw, cl + i] += GlobalVariable.DScore[ePC];
                                    else AIBoard[rw, cl + i] += GlobalVariable.AScore[ePC];
                                if (ePC == 0)
                                    if (index == 2)
                                        AIBoard[rw, cl + i] += GlobalVariable.DScore[eHuman];
                                    else AIBoard[rw, cl + i] += GlobalVariable.AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    AIBoard[rw, cl + i] *= 2;
                            }
                        }
                    }
                }

            //Danh gia duong cheo xuong
            for (rw = 0; rw < GlobalVariable.MaxRow - 4; rw++)
                for (cl = 0; cl < GlobalVariable.MaxCol - 4; cl++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GlobalVariable.isCheck[rw + i, cl + i] == 0) eHuman++;
                        if (GlobalVariable.isCheck[rw + i, cl + i] == 2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (GlobalVariable.isCheck[rw + i, cl + i] == -1) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (index == 0)
                                        AIBoard[rw + i, cl + i] += GlobalVariable.DScore[ePC];
                                    else AIBoard[rw + i, cl + i] += GlobalVariable.AScore[ePC];
                                if (ePC == 0)
                                    if (index == 2)
                                        AIBoard[rw + i, cl + i] += GlobalVariable.DScore[eHuman];
                                    else AIBoard[rw + i, cl + i] += GlobalVariable.AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    AIBoard[rw + i, cl + i] *= 2;
                            }
                        }

                    }
                }

            //Danh gia duong cheo len
            for (cl = 4; cl < GlobalVariable.MaxCol; cl++)
                for (rw = 0; rw < GlobalVariable.MaxRow - 4; rw++)
                {
                    ePC = 0; eHuman = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GlobalVariable.isCheck[rw + i, cl - i] == 0) eHuman++;
                        if (GlobalVariable.isCheck[rw + i, cl - i] == 2) ePC++;
                    }

                    if (eHuman * ePC == 0 && eHuman != ePC)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if (GlobalVariable.isCheck[rw + i, cl - i] == -1) // Neu o chua duoc danh
                            {
                                if (eHuman == 0)
                                    if (index == 0)
                                        AIBoard[rw + i, cl - i] += GlobalVariable.DScore[ePC];
                                    else AIBoard[rw + i, cl - i] += GlobalVariable.AScore[ePC];
                                if (ePC == 0)
                                    if (index == 2)
                                        AIBoard[rw + i, cl - i] += GlobalVariable.DScore[eHuman];
                                    else AIBoard[rw + i, cl - i] += GlobalVariable.AScore[eHuman];
                                if (eHuman == 4 || ePC == 4)
                                    AIBoard[rw + i, cl - i] *= 2;
                            }
                        }
                    }
                }
        }


        //Ham tim nuoc di cho may
        public void FindMove()
        {
            if (GlobalVariable.depth > GlobalVariable.maxDepth) return;
            GlobalVariable.depth++;
            //fWin = false;
            GlobalVariable.fWin = false;
            //bool fLose = false;
            Point pcMove = new Point();
            Point humanMove = new Point();
            int countMove = 0;
            EvalBoard(2);

            //Lay ra MaxMove buoc di co diem cao nhat
            Point temp = new Point();
            for (int i = 0; i < GlobalVariable.maxMove; i++)
            {
                temp = MaxPos();
                GlobalVariable.PCMove[i] = temp;
                AIBoard[(int)temp.Y, (int)temp.X] = 0;
            }

            //Lay nuoc di trong PCMove[] ra danh thu
            countMove = 0;
            while (countMove < GlobalVariable.maxMove)
            {

                pcMove = GlobalVariable.PCMove[countMove++];
                GlobalVariable.isCheck[(int)pcMove.Y, (int)pcMove.X] = 2;
                GlobalVariable.WinMove.SetValue(pcMove, GlobalVariable.depth - 1);

                //Tim cac nuoc di toi uu cua nguoi
                ResetBoard();
                EvalBoard(0);
                //Lay ra maxMove nuoc di co diem cao nhat cua nguoi
                for (int i = 0; i < GlobalVariable.maxMove; i++)
                {
                    temp = MaxPos();
                    GlobalVariable.HumanMove[i] = temp;
                    AIBoard[(int)temp.Y, (int)temp.X] = 0;
                }
                //Danh thu cac nuoc di
                for (int i = 0; i < GlobalVariable.maxMove; i++)
                {
                    humanMove = GlobalVariable.HumanMove[i];
                    GlobalVariable.isCheck[(int)humanMove.Y, (int)humanMove.X] = 0;
                    if (checkGame.isWin((int)humanMove.Y, (int)humanMove.X, 2) == true)
                    {
                        GlobalVariable.fWin = true;
                    }
                    if (checkGame.isWin((int)humanMove.Y, (int)humanMove.X, 0) == true)
                    {
                        isLose = true;
                    }
                    if (isLose)
                    {
                        GlobalVariable.isCheck[(int)pcMove.Y, (int)pcMove.X] = -1;
                        GlobalVariable.isCheck[(int)humanMove.Y, (int)humanMove.X] = -1;
                        break;
                    }
                    if (GlobalVariable.fWin)
                    {
                        GlobalVariable.isCheck[(int)pcMove.Y, (int)pcMove.X] = -1;
                        GlobalVariable.isCheck[(int)humanMove.Y, (int)humanMove.X] = -1;
                        return;
                    }
                    FindMove();
                    GlobalVariable.isCheck[(int)humanMove.Y, (int)humanMove.X] = -1;
                }
                GlobalVariable.isCheck[(int)pcMove.Y, (int)pcMove.X] = -1;
            }
        }

        public void AIStart()
        {
            for (int i = 0; i < GlobalVariable.maxMove; i++)
            {
                GlobalVariable.WinMove[i] = new Point();
                GlobalVariable.PCMove[i] = new Point();
                GlobalVariable.HumanMove[i] = new Point();
            }

            GlobalVariable.depth = 0;
            FindMove();
        }
    }
}
