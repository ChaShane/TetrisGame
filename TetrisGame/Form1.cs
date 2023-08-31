using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TetrisGame
{
    public partial class Form1 : Form
    {

        Board gameBoard;
        Game game;
        int bx;
        int by;
        int bwidth;
        int bheight;
        int no = 1; //PLAY LIST 정렬

        //스톱워치
        Stopwatch sw = new Stopwatch();

        internal int BlockNum_Temp
        {
            get;
            set;
        }
        //다음 블럭 전달을 위한 메소드
        private void SetBlockNumInDiagram()
        {
            Diagram.SetBlockNumFromForm1(int.Parse(label1.Text));
        }

        public Form1()
        {
            InitializeComponent();
            //도형 이벤트 시 값 호출을 위해 해당 인스턴트 Board 에 전달
            gameBoard = new Board();
            gameBoard.SetFormInstance(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            KeyPreview = true;

            //첫실행시 랜덤 함수 통하여 블럭 출력
            Random rand = new Random();
            Diagram.SetBlockNumFromForm1(rand.Next() % 7);

            //게임 상태 관리
            game = Game.Singleton;

            /*게임 설정한 옵션대로 사이즈 적용 */
            bx = GameRule.BX;
            by = GameRule.BY;
            bwidth = GameRule.B_WIDTH;
            bheight = GameRule.B_HEIGHT;
            this.SetClientSizeCore(GameRule.BX * (GameRule.B_WIDTH + 25), GameRule.BY * (GameRule.B_HEIGHT + 1));



        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            /*도형및 게임공간 출력 */
            DoubleBuffered = true;
            DrawGraduation(e.Graphics);
            DrawDiagram(e.Graphics);
            DrawBoard(e.Graphics);
        }

        /*
          맨 마지막 라인에 도형이 놓였을때 디자인 이벤트
        */
        private void DrawBoard(Graphics graphics)
        {
            for (int xx = 0; xx < bx; xx++)
            {
                for (int yy = 0; yy < by; yy++)
                {
                    if (game[xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle(xx * bwidth + 2, yy * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(Pens.Black, now_rt);
                        graphics.FillRectangle(Brushes.Gray, now_rt);
                    }
                }
            }
        }

        /*
         다음 블럭에 대한 디자인 이벤트
        */
        private void NextDrawDiagram(Graphics graphics, Rectangle groupBoxRect, Label lb)
        {

            Point now = game.NowPosition;
            int bn = int.Parse(lb.Text);
            int tn = game.Turn;
            Pen dpen = new Pen(Color.Black, 2);

            // 블록 크기 및 간격 설정
            int blockSize = 20; // 예시로 블록 크기를 20으로 설정
            int blockGap = 2;

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        // 블록의 위치 계산
                        int x = groupBoxRect.Left + (now.X + xx) * (blockSize + blockGap);
                        int y = groupBoxRect.Top + (now.Y + yy) * (blockSize + blockGap);

                        Rectangle now_rt = new Rectangle(x - 70, y + 5, blockSize, blockSize);
                        graphics.DrawRectangle(dpen, now_rt);
                        graphics.FillRectangle(ColorApply(bn), now_rt);
                    }
                }
            }
        }

        /*
         도형이 내려올때 디자인 이벤트
        */
        private void DrawDiagram(Graphics graphics)
        {
            //Pen dpen = new Pen(Color.Red, 4);
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            Pen dpen = new Pen(Color.Black, 2);
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        graphics.DrawRectangle(dpen, now_rt);
                        graphics.FillRectangle(ColorApply(bn), now_rt);
                    }
                }
            }
        }
        //도형 종류에 따라 색상 적용
        private Brush ColorApply(int btn)
        {
            Brush clr;
            switch (btn)
            {
                case 0:
                    Color color = Color.FromArgb(90, 255, 255);
                    Brush br = new SolidBrush(color);
                    clr = br;
                    break;
                case 1: clr = Brushes.Yellow; break;
                case 2:
                    Color color2 = Color.FromArgb(255, 187, 0);
                    Brush br2 = new SolidBrush(color2);
                    clr = br2;
                    break;
                case 3: clr = Brushes.Blue; break;
                case 4:
                    Color color4 = Color.FromArgb(165, 102, 255);
                    Brush br4 = new SolidBrush(color4);
                    clr = br4;
                    break;
                case 5: clr = Brushes.Red; break;
                case 6:
                    Color color6 = Color.FromArgb(65, 255, 58);
                    Brush br6 = new SolidBrush(color6);
                    clr = br6;
                    break;
                default: clr = Brushes.White; break;
            }

            return clr;
        }



        private void DrawGraduation(Graphics graphics)
        {
            /*
             * 게임 공간라인 그리기
             */
            DrawHorizons(graphics);
            DrawVerticals(graphics);
        }

        private void DrawVerticals(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();

            for (int cx = 0; cx <= bx; cx++)
            {
                st.X = cx * bwidth;
                st.Y = 0;
                et.X = st.X;
                et.Y = by * bheight;

                if (cx == 0)
                    graphics.DrawLine(Pens.Black, st, et);
            }
            graphics.DrawLine(Pens.Black, st, et);
        }

        private void DrawHorizons(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();

            for (int cy = 0; cy <= by; cy++)
            {
                st.X = 0;
                st.Y = cy * bheight;
                et.X = bx * bwidth;
                et.Y = cy * bheight;

                if (cy == 0)
                    graphics.DrawLine(Pens.Black, st, et);
            }
            graphics.DrawLine(Pens.Black, st, et);
        }


        //키조작
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right: MoveRight(); return; //오른쪽
                case Keys.Left: MoveLeft(); return; //왼쪽
                case Keys.Space: MoveSSDown(); return; //도형 놓기
                case Keys.Up: MoveTurn(); return; //도형 변경
                case Keys.Down: MoveDown(); return; // 도형 가속
            }
        }

        // 도형 놓기
        private void MoveSSDown()
        {
            while (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            SetBlockNumInDiagram();
            Diagram diagram = new Diagram();
            Board brd = new Board();
            label1.Text = diagram.GetRandomValue().ToString();
            groupBox1.Invalidate();
            EndingCheck();
        }

        //도형 변경
        private void MoveTurn()
        {
            if (game.MoveTurn())
            {
                Region rg = MakeRegion();
                Invalidate(rg);
            }
        }

        //Board.cs 에서 라인 제거 시, 해당 메소드 실행, (점수 계산)
        internal void UpdateLabel(int value)
        {
            label3.Text = (int.Parse(label3.Text) + value).ToString(); // 값을 Form1의 Label에 표시
        }

        //아래로 움직이기
        private void MoveDown()
        {

            if (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            else
            {
                /* 블럭 이벤트가 끝날때... */
                SetBlockNumInDiagram(); //Next 있는 블럭 값을 Diagram 에있는 BlockNum 변수로 값 전달
                Diagram diagram = new Diagram();
                Board brd = new Board();
                label1.Text = diagram.GetRandomValue().ToString(); //다음 블럭 적용
                groupBox1.Invalidate(); // 다음블럭 이미지 변경 적용
                EndingCheck(); //게임오버 여부 체크

            }
        }

        //게임오버 여부 체크
        private void EndingCheck()
        {
            if (game.Next())
            {
                Invalidate();
            }
            else
            {
                //블럭, 스톱워치 기능 중지
                timer_down.Enabled = false;
                timer.Enabled = false;
                sw.Stop();

                //점수판 등록
                dataGridView1.Rows.Add(no, label3.Text, label5.Text);
                no++;
                sw.Reset();

                //게임오버 안내 및 진행 여부
                if (DialogResult.Yes == MessageBox.Show("Game Over\n\n계속 하실건가요?", "계속 진행 확인 창", MessageBoxButtons.YesNo))
                {

                    //게임 재시작 및 스톱워치, 블럭 작동 ON
                    game.ReStart();
                    timer_down.Enabled = true;
                    Invalidate();
                    sw.Start();
                    timer.Enabled = true;
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void MoveLeft()
        {
            if (game.MoveLeft())
            {
                Region rg = MakeRegion(1, 0);
                Invalidate(rg);
            }
        }
        /*
         * MakeRegion 함수 - 도형이 이동하거나 회전할떄 변경된 영역을 계산 함.
         */
        private Region MakeRegion(int cx, int cy)
        {
            Point now = game.NowPosition;

            int bn = game.BlockNum;
            int tn = game.Turn;
            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        Rectangle rect2 = new Rectangle((now.X + cx + xx) * bwidth, (now.Y + cy + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        Region rg2 = new Region(rect2);
                        region.Union(rg1);
                        region.Union(rg2);
                    }
                }
            }
            return region;
        }

        private Region MakeRegion()
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            int oldtn = (tn + 3) % 4;
            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                    if (BlockValue.bvals[bn, oldtn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 2, (now.Y + yy) * bheight + 2, bwidth - 4, bheight - 4);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                }
            }
            return region;
        }
        private void MoveRight()
        {
            if (game.MoveRight())
            {
                Region rg = MakeRegion(-1, 0);
                Invalidate(rg);
            }
        }

        //게임 시작시, 블럭 아래로 이동
        private void timer_down_Tick_1(object sender, EventArgs e)
        {
            MoveDown();
        }

        //다음 블럭 확인을 위한 이미지 이벤트 처리
        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            Size tSize = TextRenderer.MeasureText("Next", this.Font);
            Rectangle borderRect = e.ClipRectangle;
            borderRect.Y += tSize.Height / 2;
            borderRect.Height -= tSize.Height / 2;
            ControlPaint.DrawBorder(e.Graphics, borderRect, Color.Black, ButtonBorderStyle.Solid);

            Rectangle textRect = e.ClipRectangle;
            textRect.X += 6;
            textRect.Width = tSize.Width;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString("Next", this.Font, new SolidBrush(this.ForeColor), textRect);

            NextDrawDiagram(e.Graphics, groupBox1.ClientRectangle, label1);
        }

        //스톱워치 시작!
        private void timer1_Tick(object sender, EventArgs e)
        {
            sw.Start();
            label5.Text = sw.Elapsed.ToString();
        }

        //Form1 종료
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        // 방향키 조작시 DataGridView Grid 도 움직이므로 못움직이기 위해 DataGridView 키 사용 X
        private void dataGridView1_KeyDown_1(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
