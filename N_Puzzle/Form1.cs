using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace N_Puzzle
{
    public partial class Form1 : Form
    {
        GameSolver game;
        int[] array;
        int index = 0;
        public Form1()
        {
            InitializeComponent();

            game = new GameSolver();

            EnableButtons(false);
            trackBar1.Value = 8;
            comboBox1.SelectedIndex = 0;
        }
        private void InitButtons()
        {
            EnableInput(true);
            int n = int.Parse(comboBox1.Text);
            game.Node = new Node(n);
            game.Size = n;
            array = new int[n * n];
            board1.MatrixSize = n;
            board1.BlankValue = n * n;
            int width = panel1.Width / n;
            panel1.Controls.Clear();
            int m = n * n;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {

                    Button btn = new Button();

                    int a = i * n + j;
                    array[a] = 0;
                    a++;
                    if (a != m)
                        btn.Text = a.ToString();
                    else
                        //btn.Text = "(   )";
                        btn.BackColor = Color.Gray;
                    btn.Tag = a;
                    btn.Left = width * j;
                    btn.Top = width * i;
                    btn.Width = width;
                    btn.Height = width;
                    btn.TabStop = false;
                    btn.Click += button_Click;
                    panel1.Controls.Add(btn);
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData.ToString())
            {
                case "Up":
                    btnUp_Click(null, null); break;
                case "Down":
                    btnDown_Click(null, null); break;
                case "Right":
                    btnRight_Click(null, null); break;
                case "Left":
                    btnLeft_Click(null, null); break;
                default:
                    break;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int t = (int)btn.Tag;
            array[index] = t;
            board1.Array = array;
            
            if (index == 0)
            {
                txtInput.Clear();
            }
            

            if (t == game.Node.BlankValue)
                game.Node.BlankPosition = index;
            txtInput.Text += " " + t;
            index++;

            // Nút cuối cùng được nhấn
            if (index == array.Length)
            {
                game.Node.Value = array;
                index = 0;
                EnableButtons(true);
                EnableInput(false);
            }

            btn.Enabled = false;

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitButtons();
            board1.Array = null;
            index = 0;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(null, null);
            EnableButtons(false);
            EnableInput(true);
            btnShowSolution.Enabled = false;
        }
        private void btnShuffle_Click(object sender, EventArgs e)
        {
            game.Shuffle();
            btnShowSolution.Enabled = false;
            board1.Array = game.Node.Value;

            EnableInput(false);

            txtInput.Text = "";
            for (int i = 0; i < game.Node.Size; i++)
            {
                for (int j = 0; j < game.Node.Size; j++)
                {
                    txtInput.Text += " " + game.Node[i, j];
                }
            }
            EnableButtons(true);
        }
        private void EnableInput(bool value)
        {
            foreach (Control ctl in panel1.Controls)
                ctl.Enabled = value;            
            txtInput.Enabled = value;
            btnInput.Enabled = value;
        }
        private void EnableButtons(bool value)
        {
            btnLeft.Enabled = value;
            btnRight.Enabled = value;
            btnUp.Enabled = value;
            btnDown.Enabled = value;
            btnSolve.Enabled = value;
        }
        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (game.Node.CanMoveLeft)
            {
                NodeHelper.MakeMove(game.Node, MoveDirection.LEFT);
                board1.Invalidate();
            }

        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (game.Node.CanMoveRight)
            {
                NodeHelper.MakeMove(game.Node, MoveDirection.RIGHT);
                board1.Invalidate();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {

            if (game.Node.CanMoveUp)
            {
                NodeHelper.MakeMove(game.Node, MoveDirection.UP);
                board1.Invalidate();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (game.Node.CanMoveDown)
            {
                NodeHelper.MakeMove(game.Node, MoveDirection.DOWN);
                board1.Invalidate();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (game.Solution.Count > 0)
            {
                NodeHelper.MakeMove(game.Node, game.Solution.Pop());

                board1.Array = game.Node.Value;

            }
            else
            {
                timer1.Enabled = false;
                EnableInput(true);
            }
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (game.CanSolve())
            {
                System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
                st.Start();
                game.Solve();
                st.Stop();
                if (game.Solution.Count == 0)
                    MessageBox.Show("Please create new game.");
                else
                {
                    MessageBox.Show(st.ElapsedMilliseconds + "ms. Found a solution in " + game.Solution.Count + " moves. Click 'View Solution' to continue.");
                    btnShowSolution.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Cannot find any solution.");
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = 1001 - trackBar1.Value * 100;
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            txtInput.Text = Regex.Replace(txtInput.Text.Trim(), @"\s+", " ");

            string[] str = txtInput.Text.Split(' ');
            int n = int.Parse(comboBox1.Text);

            int[] arr = new int[n * n];

            if (str.Length < arr.Length)
            {
                MessageBox.Show("Nhập thiếu phần tử");
                return;
            }

            try
            {
                int index = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    int t = int.Parse(str[i]);
                    if (i > arr.Length)
                    {
                        MessageBox.Show("Giá trị phần tử " + t + " nằm ngoài phạm vi");
                        return;
                    }
                    if (Array.Exists(arr, x => x == t))
                    {
                        MessageBox.Show("Nhập trùng phần tử " + t);
                        return;
                    }
                    arr[i] = t;
                    if (arr[i] == arr.Length)
                        index = i;
                }
                game.Node.Value = arr;
                game.Node.BlankPosition = index;
                board1.Array = arr;
                EnableButtons(true);
                EnableInput(false);
            }
            catch
            {
                MessageBox.Show("Nhập dữ liệu sai");
            }

        }

        private void btnShowSolution_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            btnShowSolution.Enabled = false;
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://yinyangit.wordpress.com");
        }
    }
}