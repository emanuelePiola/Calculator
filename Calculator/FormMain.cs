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
    public enum symbolType
    {
        Number,
        Operator,
        DecimalPoint,
        PlusMinusSign,
        Backspace,
        Undefined
    }

    public partial class FormMain : Form
    {
        public struct btnStruct
        {
            public char Content;
            public bool IsBold;
            public symbolType Type;
            public btnStruct(char c, symbolType t = symbolType.Undefined, bool b = false)
            {
                this.Content = c;
                this.Type = t;
                this.IsBold = b;
            }
        }

        private btnStruct[,] buttons =
        {
            { new btnStruct('%'), new btnStruct('Œ'), new btnStruct('C'), new btnStruct('⌫',symbolType.Backspace) },
            { new btnStruct('\u215F'), new btnStruct('\u00B2'), new btnStruct('\u221A'), new btnStruct('÷',symbolType.Operator) },
            { new btnStruct('7',symbolType.Number,true), new btnStruct('8',symbolType.Number,true), new btnStruct('9',symbolType.Number,true), new btnStruct('×',symbolType.Operator) },
            { new btnStruct('4',symbolType.Number,true), new btnStruct('5',symbolType.Number,true), new btnStruct('6',symbolType.Number,true), new btnStruct('-',symbolType.Operator) },
            { new btnStruct('1',symbolType.Number,true), new btnStruct('2',symbolType.Number,true), new btnStruct('3',symbolType.Number,true), new btnStruct('+',symbolType.Operator) },
            { new btnStruct('±',symbolType.PlusMinusSign), new btnStruct('0',symbolType.Number,true), new btnStruct(',',symbolType.DecimalPoint), new btnStruct('=',symbolType.Operator) },
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
                    myButton.BackColor = buttons[i, j].IsBold ? Color.White : Color.LightGray;
                    myButton.Text = buttons[i,j].Content.ToString();
                    myButton.Width = btnWidth;
                    myButton.Height = btnHeight;
                    myButton.Top = posY;
                    myButton.Left += posX;
                    myButton.Tag = buttons[i, j];
                    myButton.Click += MyButton_Click;
                    this.Controls.Add(myButton);
                    posX += myButton.Width;
                }
                posX = 0;
                posY += btnHeight;
            }
        }

        private void MyButton_Click(object sender, EventArgs e)
        {
            Button clikedButton = (Button)sender;
            btnStruct cbStruct = (btnStruct)clikedButton.Tag;

            switch (cbStruct.Type)
            {
                case symbolType.Number:
                    if (lblResult.Text == "0")
                    {
                        lblResult.Text = "";
                    }
                    lblResult.Text += clikedButton.Text;
                    break;
                case symbolType.Operator:
                    break;
                case symbolType.DecimalPoint:
                    if (lblResult.Text.IndexOf(",") == -1)
                    {
                        lblResult.Text += clikedButton.Text;
                    }
                    break;
                case symbolType.PlusMinusSign:
                    if (lblResult.Text != "0")
                    {
                        if (lblResult.Text.IndexOf("-") == -1)
                        {
                            lblResult.Text = "-" + lblResult.Text;
                        }
                        else
                        {
                            lblResult.Text = lblResult.Text.Substring(1);
                        }
                    }
                    break;
                case symbolType.Backspace:
                    lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1);
                    if (lblResult.Text == "" || lblResult.Text == "-0") 
                    {
                        lblResult.Text = "0";
                    }
                    break;
                case symbolType.Undefined:
                    break;
            }
        }
    }
}
