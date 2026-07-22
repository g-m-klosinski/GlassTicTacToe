using Microsoft.VisualBasic;

namespace GlassTicTacToe
{
    public partial class MainForm : Form
    {
        const int SIDE = 4;
        const int WIN_LENGTH = 3;

        public MainForm()
        {
            InitializeComponent();

            Board board = new(SIDE, WIN_LENGTH);

            TableLayoutPanel grid = new()
            {
                Dock = DockStyle.Fill,
                Size = ClientSize,
                BackColor = Color.Wheat,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
                ColumnCount = SIDE,
                RowCount = SIDE,
            };

            for (int i = 0; i < SIDE; i++)
            {
                grid.ColumnStyles.Add(new ColumnStyle(
                    SizeType.Percent,
                    100f / SIDE)
                    );
                grid.RowStyles.Add(new RowStyle(
                    SizeType.Percent,
                    100f / SIDE)
                    );
            }

            foreach (var cell in board)
            {
                grid.Controls.Add(cell);
            }

            Controls.Add(grid);
        }

    }
}
