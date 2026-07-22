using Microsoft.VisualBasic;

namespace GlassTicTacToe
{
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            InitializeComponent();

            Board board = new();

            foreach (var cell in board)
            {
                grid.Controls.Add(cell);
            }
        }
        
    }
}
