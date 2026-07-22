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
            return this[y * SIDE + x].Text;
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
            List<(int yStep, bool swapAxes)> winConditions = new List<(int, bool)>()
            {
                (1, false),  // vertical lines
                (1, true),  // horizontal lines
                
                // backslash-like diagonal lines
                (1, false),
                (1, true),
                
                // forward slash-like diagonal lines
                (-1, false),
                (-1, true),
            };

            foreach (var (yStep, swapAxes) in winConditions)
            {
                var (found, winner) = checkLine(yStep, swapAxes);

                if (found)
                {
                    return winner == "X" ? GameState.XWon : GameState.OWon;
                }
            }
            
            if (emptyCellCount == 0)
            {
                return GameState.Draw;
            }

            return GameState.On;
        }

        private (bool, string) checkLine(int yStep, bool swapAxes)
        {// (-1, true) raises error for getCell
            for (int x = 0; x < SIDE; x++)
            {
                int sameInRowCount = 1;
                string last = "";

                int y0 = yStep == -1 ? x : 0;

                // For orthogonal lines, make y1 = SIDE
                int y1 = yStep == -1 ? 0 : SIDE - yStep * x;

                for (int y = y0, xx = x; y != y1 ; y += yStep, xx++)
                {
                    string current = swapAxes ? getCell(y, xx) :
                        getCell(xx, y);

                    if (last.Length > 0 && current == last)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == WIN_LENGTH)
                        {
                            return (true, last);
                        }
                    }
                    else
                    {
                        sameInRowCount = 1;
                    }

                    last = current;
                }
            }

            return (false, string.Empty);
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
            foreach (var label in this)
            {
                label.Text = "";
            }

            emptyCellCount = SIDE * SIDE;

            xToMove = true;
        }
    }
}
