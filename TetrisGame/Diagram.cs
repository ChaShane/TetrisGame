using System;
/*
 * 도형 이동 및 변경에 대한 정의 클래스
 */
namespace TetrisGame
{

    class Diagram
    {
        private static int BlockNumFromForm1;        

        //Form1 dp
        internal static void SetBlockNumFromForm1(int blockNum)
        {
            BlockNumFromForm1 = blockNum;
        }
        internal int X
        {
            get;
            private set;
        }

        internal int Y
        {
            get;
            private set;
        }
        internal int Turn
        {
            get;
            private set;
        }
        internal int BlockNum
        {
            get;
            private set;
        }
        internal Diagram()
        {
            Reset();
        }
        //블럭 타입을 랜덤으로 불러온다.
        internal int GetRandomValue()
        {
            Random rand = new Random();
            return rand.Next() % 7;
        }

        //1턴이 끝난 경우
        internal void Reset()
        {
            Random rand = new Random();
            X = GameRule.SX;
            Y = GameRule.SY;
            //블럭 랜덤으로 적용
            Turn = rand.Next() % 4; 
            BlockNum = BlockNumFromForm1;//Next에 있는 블럭 가져옴
            GetRandomValue(); //그 다음 블럭은 랜덤으로 가져옴
        }



        internal void MoveLeft()
        {
            X--;
        }
        internal void MoveRight()
        {
            X++;
        }
        internal void MoveDown()
        {
            Y++;
        }
        internal void MoveTurn()
        {
            Turn = (Turn + 1) % 4;
        }

    }
}
