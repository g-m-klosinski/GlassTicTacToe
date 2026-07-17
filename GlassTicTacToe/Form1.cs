using Microsoft.VisualBasic;

namespace GlassTicTacToe
{
    public partial class Form1 : Form
    {
        private bool xToMove = true;
        
        public Form1()
        {
            InitializeComponent();

            for (var i = 0; i < grid.RowCount; i++)
            {
                for (int j = 0; j < grid.ColumnCount; j++)
                {
                    Label label = new();

                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Click += Label_Click;

                    grid.Controls.Add(label, j, i);
                }
            }
        }

        private void Label_Click(object? sender, EventArgs e)
        {
            if (sender is Label piece && piece.Text.Length == 0)
            {
                piece.Text = xToMove ? "X" : "O";

                xToMove = !xToMove;
            }
        }
    }
}
