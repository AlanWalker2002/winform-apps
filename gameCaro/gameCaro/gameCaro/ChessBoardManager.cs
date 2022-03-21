using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameCaro
{
    public class ChessBoardManager
    {
        #region properties
        private Panel chessBoard;
        public Panel ChessBoard
        {
            get => chessBoard;
            set => chessBoard = value;
        }
        #endregion

        private List<Player> player;

        public List<Player> Player 
        { 
            get => player; 
            set => player = value; 
        }       

        private int currentPlayer;
        public int CurrentPlayer 
        { 
            get => currentPlayer; 
            set => currentPlayer = value;
        }

        private TextBox PlayerName;
        public TextBox PlayerName1
        {
            get => PlayerName;
            set => PlayerName = value;
        }

        private PictureBox PlayerMark;
        public PictureBox PlayerMark1 
        {
            get => PlayerMark; 
            set => PlayerMark = value;
        }

        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get => matrix;
            set => matrix = value;
        }
        

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox PlayerName, PictureBox mark)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = PlayerName;
            this.PlayerMark = mark;
            this.player = new List<Player>()
            {
                new Player("PhucLoc",Image.FromFile(Application.StartupPath+"\\Resources\\x.png")),
                new Player("DucAnh",Image.FromFile(Application.StartupPath+"\\Resources\\o.png"))
            };

            currentPlayer = 0;

            ChangePlayer();

            
        }

      
        #endregion

        #region Methods
        public void DrawChessBoard()
        {
            Matrix = new List<List<Button>>();

            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>() { });
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button btn = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Tag = i.ToString()
                    };
                    
                    btn.Click += Btn_Click;

                    ChessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);
                    oldButton = btn;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;

            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            //ép kiểu button
           Button btn = sender as Button;
            //Đổi hinh của button
            /* btn.BackgroundImage = Image.FromFile(Application.StartupPath+"\\Resources\\x.png");*/
            //FromFile là cách để lấy image

            if (btn.BackgroundImage != null)
                return;
            // thay doi nguoi choi

            Mark(btn);

            ChangePlayer();

            if (isEndGame(btn))
            {
                EndGame();
            }
        }
        private void EndGame()
        {
            MessageBox.Show("Kết thúc game!!! ");
        }
        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }      

        private Point GetChessPoint(Button btn)
        {
           
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);
            Point point = new Point(horizontal, vertical);


            return point;
        }

        //kết thúc ở hàng ngang
        private bool isEndHorizontal(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countLeft = 0;
                for( int i = point.X; i >=0; i--)
                {
                     if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                       {
                        countLeft++;
                        }
                     else
                        break;
                }

            int countRight = 0;
                for (int i = point.X+1; i < Cons.CHESS_BOARD_WIDTH; i++)
                {
                    if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
                    {
                        countRight++;
                    }
                    else
                        break;
                }

            return countLeft + countRight == 5;
        }

        //kết thúc ở hàng dọc
        private bool isEndVertical(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        //kết thúc ở hàng cheo chinh
        private bool isEndPrimary(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if(point.X - i < 0 || point.Y - i < 0)
                     break; 

                if (Matrix[point.Y - i ][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.X + i >= Cons.CHESS_BOARD_WIDTH || point.Y + i >= Cons.CHESS_BOARD_HEIGHT)
                    break;

                if (Matrix[point.Y + i][point.X + i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }       

            return countTop + countBottom == 5;
        }

        //kết thúc ở hàng cheo phu 
        private bool isEndSub(Button btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X - i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.X - i <0 || point.Y + i >= Cons.CHESS_BOARD_HEIGHT)
                    break;

                if (Matrix[point.Y + i][point.X = i].BackgroundImage == btn.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        private void Mark(Button btn)
        {
            btn.BackgroundImage = Player[currentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }
        private void ChangePlayer()
        {
            PlayerName.Text = Player[currentPlayer].Name;
            PlayerMark.Image = Player[currentPlayer].Mark;
        }
        
        #endregion


    }
}
