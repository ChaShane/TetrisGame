/*
    도형 쌓는 이벤트 클래스
 */
namespace TetrisGame
{

    class Board
    {
        //한줄이 되었을때 개수 카운트를 위한 변수
        int cnt = 0;

        private static Form1 formInstance;

        internal static Board GameBoard
        {
            get;
            private set;
        }
        static Board()
        {
            GameBoard = new Board();
        }

        int[,] board = new int[GameRule.BX, GameRule.BY];
        internal int this[int x, int y]
        {
            get
            {
                return board[x, y];
            }
        }
        /*
         * 블럭을 이동할때 새로운 위치에 이동이 가능한지 여부
         */
        internal bool MoveEnable(int bn, int tn, int x, int y)
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        if (board[x + xx, y + yy] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /*
         * 블록을 이동한 위치에 저장하는 메소드
         */
        internal void Store(int bn, int turn, int x, int y)
        {
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (((x + xx) >= 0) && (x + xx < GameRule.BX) && (y + yy >= 0) && (y + yy < GameRule.BY))
                    {
                        board[x + xx, y + yy] += BlockValue.bvals[bn, turn, xx, yy];

                    }
                }
            }
            CheckLines(y + 3);
        }


        //Form1 에서 Board.cs Form1에 대한 인스턴스를 전달한다
        internal void SetFormInstance(Form1 form)
        {
            formInstance = form; // Form1 인스턴스 저장
        }


        public void UpdateFormLabel(int value)
        {
            //인스턴스가 있다면 해당 Form1에 있는 레이블 호출 값을 전달한다
            if (formInstance != null)
            {
                formInstance.UpdateLabel(value); 
            }
        }

        /*
         * 블록을 놓은 후에 라인이 가득 찼는지 체크
         */
        private void CheckLines(int y)
        {
            int yy = 0;
            for (yy = 0; (yy < 4); yy++)
            {
                if (y - yy < GameRule.BY)
                {
                    if (CheckLine(y - yy))
                    {
                        ClearLine(y - yy);
                        y++;
                        cnt++;
                    }
                }
            }
            //줄을 삭제 할 카운터 개수 (1줄당 10점)
            if (cnt != 0)
            {
                UpdateFormLabel(cnt * 10);
                cnt = 0;
            }

        }
        /*
         * 가득 찬 라인을 지우고 위에 블록을 아래로 내려준다 
         */
        private void ClearLine(int y)
        {
            for (; y > 0; y--)
            {
                for (int xx = 0; xx < GameRule.BX; xx++)
                {
                    board[xx, y] = board[xx, y - 1];
                }
            }

        }
        /*
         * 한줄이 가득 찼는지 체크
         */
        private bool CheckLine(int y)
        {
            for (int xx = 0; xx < GameRule.BX; xx++)
            {
                if (board[xx, y] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /*
         * 테트리스 블럭 초기화 (Restart)
         */
        internal void ClearBoard()
        {
            for (int xx = 0; xx < GameRule.BX; xx++)
            {
                for (int yy = 0; yy < GameRule.BY; yy++)
                {
                    board[xx, yy] = 0;
                }
            }

        }
    }
}
