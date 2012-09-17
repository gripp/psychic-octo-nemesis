using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
namespace PicturePuzzle
{
    public partial class PuzzleForm : Form
    {
        // public bool Modal { get { return true; } }
        public bool Solved
        {
            get
            {
                return solved;
            }
        }
        private bool solved = true;

        public PuzzleForm(int puzzleNum)
        {
            InitializeComponent();
            _B[4] = _Pan.Width / 4;
            _B[5] = _Pan.Height / 4;
            for (int i = 0; i < 15; i++)
            {
                if (i < 4) Add_Box(i * _B[4], 0);
                if (i >= 4 & i < 8) Add_Box((i - 4) * _B[4], _B[4]);
                if (i >= 8 & i < 12) Add_Box((i - 8) * _B[4], _B[4] * 2);
                if (i >= 12 & i < 16) Add_Box((i - 12) * _B[4], _B[4] * 3);
            }

            Bitmap picture;
            switch (puzzleNum)
            {
                case 3:
                    picture = new Bitmap(Image.FromFile("c:/puzzle3.jpg"));
                    break;
                case 2:
                    picture = new Bitmap(Image.FromFile("c:/puzzle2.jpg"));
                    break;
                default:
                    picture = new Bitmap(Image.FromFile("c:/puzzle1.jpg"));
                    break;
            }

            Split_Bitmap(picture);
            Shuffle();
        }

        #region Storage
        Point LP;
        string[] Position = { "R1_1", "R1_2", "R1_3", "R1_4", "R2_1", "R2_2", "R2_3", "R2_4", "R3_1", "R3_2", "R3_3", "R3_4", "R4_1", "R4_2", "R4_3", "R4_4" };
        int FSpace = 0, CSpace = 0;
        enum Direction { None, Up, Down, Left, Right };
        Direction Dir = Direction.None;
        int[] _B = { 0, 0, 0, 0, 0, 0 };//bounds- MinX/MaxX MinY/MaxY Width/Height
        Control eSend { get; set; }
        #endregion

        #region Add Picturebox

        public void Add_Box(int x, int y)
        {
            PictureBox Con = new PictureBox();
            Con.Location = new Point(x, y);
            Con.Name = "_P" + (_Pan.Controls.Count + 1);
            Con.BackColor = Color.White;
            Con.Size = new Size(_B[4], _B[5]);
            Con.BackgroundImageLayout = ImageLayout.Stretch;
            Con.MouseMove += new MouseEventHandler(_Move);
            Con.MouseUp += new MouseEventHandler(_Up);
            Con.MouseDown += new MouseEventHandler(_Down);
            _Pan.Controls.Add(Con);
        }

        #endregion

        #region MouseEvents

        void _Move(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) LP = new Point(e.X, e.Y);
            else
            {
                if (Dir != Direction.None)
                {
                    if (Dir == Direction.Left | Dir == Direction.Right)
                    {
                        if (eSend.Left <= _B[1])
                            eSend.Left = Math.Max(_B[0], e.X + eSend.Left - LP.X);
                    }
                    else if (Dir == Direction.Up | Dir == Direction.Down)
                    {
                        if (eSend.Top <= _B[3])
                            eSend.Top = Math.Max(_B[2], e.Y + eSend.Top - LP.Y);
                    }
                }
            }
        }//Moves the box only in the correct direction and bounds

        void _Up(object sender, MouseEventArgs e)
        {
            Point P = new Point(eSend.Location.X + (eSend.Width / 2), eSend.Location.Y + (eSend.Height / 2));
            int X = P.X >= 0 & P.X <= _B[4] ? 0 : P.X >= _B[4] & P.X < _B[4] * 2 ? _B[4] : P.X >= _B[4] * 2 & P.X < _B[4] * 3 ? _B[4] * 2 : _B[4] * 3;
            int Y = P.Y >= 0 & P.Y < _B[5] ? 0  : P.Y >= _B[5] & P.Y < _B[5] * 2 ? _B[5] : P.Y >= _B[5] * 2 & P.Y < _B[5] * 3 ? _B[5] * 2 : _B[5] * 3;
            eSend.Location = new Point(X, Y);
            CheckComplete();
        }//Moves the Picture to the nearest location when released.

        void _Down(object sender, MouseEventArgs e)
        {
            eSend = (Control)(sender);
            _Positions();
            _Direction();
        }//Self-explanatory

        #endregion

        #region Misc

        void _Positions()//Gets the position of the current picturebox and current unused block
        {
            Point _Point = new Point(0, 0);
            for (int i = 16; i-- > 0; )
            {
                if (i < 4) _Point = new Point(i * _B[4], 0);
                if (i >= 4 & i < 8) _Point = new Point((i - 4) * _B[4], _B[5]);
                if (i >= 8 & i < 12) _Point = new Point((i - 8) * _B[4], _B[5] * 2);
                if (i >= 12 & i <= 16) _Point = new Point((i - 12) * _B[4], _B[5] * 3);
                if (_Pan.GetChildAtPoint(_Point) == null)//Gets the free space
                    FSpace = i;
                if (eSend.Location == _Point)//Current picture location
                    CSpace = i;
            }
        }

        void _Direction()
        {
            int CRow = CSpace / 4, FRow = FSpace / 4;
            bool UD = (CSpace - 4) == FSpace | (CSpace + 4) == FSpace, LR = (CSpace - 1) == FSpace | (CSpace + 1) == FSpace;
            int RowIndex = CSpace - (CSpace < 4 ? 0 : CSpace >= 4 & CSpace < 8 ? 4 : CSpace >= 8 & CSpace < 12 ? 8 : CSpace >= 12 & CSpace <= 16 ? 12 : 0);
            if (!UD & !LR)//Empty square not near
            {
                Dir = Direction.None;
                return;
            }
            else//Empty square is near but we need to determine the direction and set the bounds
            {
                Dir = UD ? CRow < FRow ? Direction.Down : Direction.Up : CSpace < FSpace ? Direction.Right : Direction.Left;
                _B[0] = (Dir == Direction.Right ? RowIndex : RowIndex - 1) * _B[4];
                _B[1] = (Dir == Direction.Right ? RowIndex + 1 : RowIndex) * _B[4];
                _B[2] = (Dir == Direction.Down ? CRow : CRow - 1) * _B[5];
                _B[3] = (Dir == Direction.Down ? CRow + 1 : CRow) * _B[5];
            }
        }

        void Split_Bitmap(Bitmap Bmp)
        {
            _Ref.BackgroundImage = Bmp;
            int _W = Bmp.Width / 4, _H = Bmp.Height / 4;
            Rectangle Rect = new Rectangle();
            for (int i = 0; i < 15; i++)
            {
                if (i < 4) Rect = new Rectangle(i * _W, 0, _W, _H);
                if (i >= 4 & i < 8) Rect = new Rectangle((i - 4) * _W, _H, _W, _H);
                if (i >= 8 & i < 12) Rect = new Rectangle((i - 8) * _W, _H * 2, _W, _H);
                if (i >= 12 & i < 16) Rect = new Rectangle((i - 12) * _W, _H * 3, _W, _H);
                _Pan.Controls[i].BackgroundImage = Bmp.Clone(Rect, PixelFormat.Format24bppRgb);
            }
        }//Splits the image into equal images and loads them on the box's

        void Shuffle()
        {
            if (_Ref.BackgroundImage == null) return;
            #region Fisher–Yates shuffle http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
            int[] tmp = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
            Random _Ran = new Random();
            int i = tmp.Length;
            while (i > 1)
            {
                int j = _Ran.Next(i);
                i--;
                int k = tmp[i];
                tmp[i] = tmp[j];
                tmp[j] = k;
            }
            #endregion
            for (int s = tmp.Length; s-- > 0; )
            {
                if (tmp[s] < 4) _Pan.Controls[s].Location = new Point(tmp[s] * _B[4], 0);
                if (tmp[s] >= 4 & tmp[s] < 8) _Pan.Controls[s].Location = new Point((tmp[s] - 4) * _B[4], _B[5]);
                if (tmp[s] >= 8 & tmp[s] < 12) _Pan.Controls[s].Location = new Point((tmp[s] - 8) * _B[4], _B[5] * 2);
                if (tmp[s] >= 12 & tmp[s] < 16) _Pan.Controls[s].Location = new Point((tmp[s] - 12) * _B[4], _B[5] * 3);
            }
        }//Generates a random number between 0-14 and sets the new locations

        void CheckComplete()//Checks the location of all the pictures to see the they're in order
        {
            if (_Ref.BackgroundImage == null) return;
            for (int i = _Pan.Controls.Count; i-- > 0; )
            {
                Point P = _Pan.Controls[i].Location;
                if (i < 4 & P != new Point(i * _B[4], 0)) return;
                if (i >= 4 & i < 8 & P != new Point((i - 4) * _B[4], _B[5])) return;
                if (i >= 8 & i < 12 & P != new Point((i - 8) * _B[4], _B[5] * 2)) return;
                if (i >= 12 & i < 16 & P != new Point((i - 12) * _B[4], _B[5] * 3)) return;
            }
            MessageBox.Show("SIMA: I understand completely!");
            solved = true;
            this.Close();
            // Shuffle();
        }

        private void Load_Pix_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open_Pix = new OpenFileDialog();
            Open_Pix.Filter = "Images (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            if (Open_Pix.ShowDialog() == DialogResult.OK)
            {
                Split_Bitmap(new Bitmap(Image.FromFile(Open_Pix.FileName)));
                Shuffle();
            }
        }//Self-explanatory

        private void Form1_Load(object sender, EventArgs e)
        {
            if (_Pan.Width != _Pan.Height)
            {
                MessageBox.Show("Panel width and height must be the same.", "App Closing");
                this.Close();
            }
        }//Just a safeguard. The size of the 15 Box'x are calculated by the size of the panel. So the panel width & height should be equal. You can resize it but make sure both sides are equal.

        #endregion

    }
}
