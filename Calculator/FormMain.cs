using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FormMain : Form
    {
        public struct btnStruct
        {
            public char Content;
            public bool IsBold;
            public btnStruct(char c, bool b)
            {
                this.Content = c;
                this.IsBold = b;
            }
        }

        private btnStruct[,] buttons =
        {
            { new btnStruct('%',false), new btnStruct('Œ',false), new btnStruct('C',false), new btnStruct('⌫',false) },
            { new btnStruct('\u215F',false), new btnStruct('\u00B2',false), new btnStruct('\u221A',false), new btnStruct('÷',false) },
            { new btnStruct('7',true), new btnStruct('8',true), new btnStruct('9',true), new btnStruct('×',false) },
            { new btnStruct('4',true), new btnStruct('5',true), new btnStruct('6',true), new btnStruct('-',false) },
            { new btnStruct('1',true), new btnStruct('2',true), new btnStruct('3',true), new btnStruct('+',false) },
            { new btnStruct('±',true), new btnStruct('0',true), new btnStruct(',',true), new btnStruct('=',false) },
        };

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            makeButton(buttons.GetLength(0), buttons.GetLength(1));
        }

        private void makeButton(int rows, int cols)
        {
            const int btnWidth = 80;
            const int btnHeight = 60;
            int posX = 0;
            int posY = 116;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Button myButton = new Button();
                    FontStyle fs = buttons[i, j].IsBold ? FontStyle.Bold : FontStyle.Regular;
                    myButton.Font = new Font("Segoe UI", 16, fs);
                    myButton.BackColor = buttons[i, j].IsBold ? Color.White : Color.Transparent;
                    myButton.Text = buttons[i,j].Content.ToString();
                    myButton.Width = btnWidth;
                    myButton.Height = btnHeight;
                    myButton.Top = posY;
                    myButton.Left += posX;
                    posX += myButton.Width;
                    this.Controls.Add(myButton);
                }
                posX = 0;
                posY += btnHeight;
            }
        }
    }
}
