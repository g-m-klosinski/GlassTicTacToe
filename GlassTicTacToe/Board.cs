using System;
using System.Collections.Generic;
using System.Text;

namespace GlassTicTacToe
{
    internal class Board: List<Label>
    {
        public const int SIDE = 3;
        public const int WIN_LENGTH = 3;
        
        private bool xToMove = true;
        
        private int emptyCellCount = SIDE * SIDE;

        private string getCell(int x, int y)
        {
            return this[y % SIDE + x].Text;
        }

        private void setCell(int x, int y, string value)
        {
            this[y % SIDE + x].Text = value;
        }

        public Board()
        {
            for (int i = 0; i < emptyCellCount; i++)
            {
                Label label = new();

                label.Dock = DockStyle.Fill;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Click += Label_Click;

                Add(label);
            }
        }

        enum GameState
        {
            On,
            XWon,
            OWon,
            Draw,
        }

        private void Label_Click(object? sender, EventArgs e)
        {
            if (sender is Label piece && piece.Text.Length == 0)
            {
                piece.Text = xToMove ? "X" : "O";

                xToMove = !xToMove;

                emptyCellCount--;

                var gameState = checkGameEnd();

                if (gameState != GameState.On)
                {
                    restart(gameState);
                }
            }
        }

        private GameState checkGameEnd()
        {
            List<(int yStep, bool swapAxes)> winConditions = new List<(int yStep, bool swapAxes)>()
            {
                (1, true),  // horizontal lines
            };
            
            
            (bool found, GameState value) = checkLine();
            if (!found)
            {
                return value;
            }

            // vertical lines
            for (int x = 0; x < SIDE; x++)
            {
                int sameInRowCount = 1;
                CellContent lastCell = CellContent.Empty;

                for (int y = 0; y < SIDE; y++)
                {
                    if (lastCell != CellContent.Empty && board[y, x] == lastCell)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return lastCell == CellContent.X ? GameState.XWon : GameState.OWon;
                        }
                    }

                    lastCell = board[y, x];
                }
            }

            // forward slash-like diagonal lines
            for (int x = WIN_LENGTH - 1; x < SIDE; x++)
            {
                int sameInRowCount = 1;
                CellContent lastCell = CellContent.Empty;

                for (int y = 0; y <= x; y++)
                {
                    if (lastCell != CellContent.Empty && board[y, x - y] == lastCell)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return lastCell == CellContent.X ? GameState.XWon : GameState.OWon;
                        }
                    }

                    lastCell = board[y, x - y];
                }
            }

            for (int y = 1; y < SIDE - WIN_LENGTH; y++)
            {
                int sameInRowCount = 1;
                CellContent lastCell = CellContent.Empty;

                for (int x = SIDE - 1; x <= y; x--)
                {
                    if (lastCell != CellContent.Empty && board[y, x] == lastCell)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return lastCell == CellContent.X ? GameState.XWon : GameState.OWon;
                        }
                    }

                    lastCell = board[y, x];
                }
            }

            // backslash-like diagonal lines
            for (int x = SIDE - WIN_LENGTH; x >= 0; x--)
            {
                int sameInRowCount = 1;
                CellContent lastCell = CellContent.Empty;

                for (int y = 0; y < SIDE - x; y++)
                {
                    if (lastCell != CellContent.Empty && board[y, x + y] == lastCell)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return lastCell == CellContent.X ? GameState.XWon : GameState.OWon;
                        }
                    }

                    lastCell = board[y, x + y];
                }
            }

            for (int y = 1; y < SIDE - WIN_LENGTH; y++)
            {
                int sameInRowCount = 1;
                CellContent lastCell = CellContent.Empty;

                for (int x = 0; x <= SIDE - y; x++)
                {
                    if (lastCell != CellContent.Empty && board[y, x] == lastCell)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return lastCell == CellContent.X ? GameState.XWon : GameState.OWon;
                        }
                    }

                    lastCell = board[y, x];
                }
            }

            if (emptyCellCount == 0)
            {
                return GameState.Draw;
            }

            return GameState.On;
        }

        private (bool found, string winner) checkLine(int yStep, bool swapAxes)
        {
            for (int x = 0; x < SIDE; x++)
            {
                int sameInRowCount = 1;
                string lastCell = "";

                int y0 = yStep == 1 ? 0 : x;
                int y1 = yStep == 1 ? SIDE - x : 0;

                for (int y = y0; y != y1 ; y += yStep)
                {
                    if (lastCell.Length > 0 && getCell(x, y) == lastCell)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return (found: true, winner: lastCell);
                        }
                    }

                    lastCell = getCell(x, y);
                }
            }

            return (found: false, value: default);
        }

        private void restart(GameState gameState)
        {
            string message;
            switch (gameState)
            {
                case GameState.XWon:
                    message = "X won";
                    break;
                case GameState.OWon:
                    message = "O won";
                    break;
                default:
                    message = "Draw";
                    break;
            }

            MessageBox.Show(message);

            clean();
        }

        private void clean()
        {
            foreach (var control in grid.Controls)
            {
                if (control is Label label)
                {
                    label.Text = "";
                }
            }

            for (int i = 0; i < SIDE; i++)
            {
                for (int j = 0; j < SIDE; j++)
                {
                    board[i, j] = CellContent.Empty;
                }
            }

            emptyCellCount = SIDE * SIDE;

            xToMove = true;
        }
    }
}
