using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        ClearAll,
        ClearEntry,
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
            { new btnStruct('%'), new btnStruct('Œ',symbolType.ClearEntry), new btnStruct('C',symbolType.ClearAll), new btnStruct('⌫',symbolType.Backspace) },
            { new btnStruct('\u215F'), new btnStruct('\u00B2'), new btnStruct('\u221A'), new btnStruct('÷',symbolType.Operator) },
            { new btnStruct('7',symbolType.Number,true), new btnStruct('8',symbolType.Number,true), new btnStruct('9',symbolType.Number,true), new btnStruct('×',symbolType.Operator) },
            { new btnStruct('4',symbolType.Number,true), new btnStruct('5',symbolType.Number,true), new btnStruct('6',symbolType.Number,true), new btnStruct('-',symbolType.Operator) },
            { new btnStruct('1',symbolType.Number,true), new btnStruct('2',symbolType.Number,true), new btnStruct('3',symbolType.Number,true), new btnStruct('+',symbolType.Operator) },
            { new btnStruct('±',symbolType.PlusMinusSign), new btnStruct('0',symbolType.Number,true), new btnStruct(',',symbolType.DecimalPoint), new btnStruct('=',symbolType.Operator) },
        };

        float lblResultBaseFontSize;
        const int lblResultWidthMargin = 25;
        const int lblResultMaxDigit = 25;

        char lastOperator = ' ';
        decimal operand1, operand2, result;
        btnStruct lastButtonClicked;

        public FormMain()
        {
            InitializeComponent();
            lblResultBaseFontSize = lblResult.Font.Size;
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
                    if (lblResult.Text == "0" || lastButtonClicked.Type==symbolType.Operator)
                    {
                        lblResult.Text = "";
                    }
                    lblResult.Text += clikedButton.Text;
                    break;
                case symbolType.Operator:
                    if(lastButtonClicked.Type==symbolType.Operator && lastButtonClicked.Content!='=')
                    {
                        lastOperator = lastButtonClicked.Content;
                    }
                    else
                    {
                        manageOperator(cbStruct);
                    }
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
                    if (lastButtonClicked.Type!=symbolType.Operator)
                    {
                        lblResult.Text = lblResult.Text.Substring(0, lblResult.Text.Length - 1);
                        if (lblResult.Text == "" || lblResult.Text == "-0")
                        {
                            lblResult.Text = "0";
                        }
                    }
                    break;
                case symbolType.ClearAll:
                    lblResult.Text = "0";
                    break;
                case symbolType.Undefined:
                    break;
            }
            if(cbStruct.Type!=symbolType.Backspace)
            {
                lastButtonClicked = cbStruct;
            }
        }

        private void manageOperator(btnStruct cbStruct)
        {
            if (lastOperator==' ')
            {
                operand1=decimal.Parse(lblResult.Text);
                if (cbStruct.Content != '=')
                {
                    lastOperator = cbStruct.Content;
                }
            }
            else
            {
                if(lastButtonClicked.Content!='=')
                {
                    operand2 = decimal.Parse(lblResult.Text);
                }
                switch (lastOperator)
                {
                    case '+':
                        result = operand1 + operand2;
                        break;
                    case '-':
                        result = operand1 - operand2;
                        break;
                    case '×':
                        result = operand1 * operand2;
                        break;
                    case '÷':
                        result = operand1 / operand2;
                        break;
                    default:
                        break;
                }
                lblResult.Text = result.ToString();
                if (cbStruct.Content != '=')
                {
                    lastOperator = cbStruct.Content;
                    if (lastButtonClicked.Content=='=')
                    {
                        operand2 = 0;
                    }
                }
                operand1 =result;
            }
        }

        private void lblResult_TextChanged(object sender, EventArgs e)
        {
            if (lblResult.Text == "-")
            {
                lblResult.Text = "0";
                return;
            }
            if (lblResult.Text.Length>0)
            {
                decimal num = decimal.Parse(lblResult.Text);
                NumberFormatInfo nfi = new CultureInfo("it-IT", false).NumberFormat;
                int decimalSeparatorPosition = lblResult.Text.IndexOf(",");
                nfi.NumberDecimalDigits = decimalSeparatorPosition == -1 ? 0 : lblResult.Text.Length - decimalSeparatorPosition - 1;
                string stOut = num.ToString("N", nfi);
                if(lblResult.Text.IndexOf(",") == lblResult.Text.Length-1)
                {
                    stOut += ",";
                }
                lblResult.Text = stOut;
            }
            if (lblResult.Text.Length > lblResultMaxDigit)
            {
                lblResult.Text = lblResult.Text.Substring(0, lblResultMaxDigit);
            }
            int textWidth = TextRenderer.MeasureText(lblResult.Text, lblResult.Font).Width;
            float newSize = lblResult.Font.Size * (((float)lblResult.Size.Width - lblResultWidthMargin) / textWidth);
            if(newSize>lblResultBaseFontSize)
            {
                newSize = lblResultBaseFontSize;
            }
            lblResult.Font = new Font("Segoe UI", newSize, FontStyle.Regular);
        }
    }
}
