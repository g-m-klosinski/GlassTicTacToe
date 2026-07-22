using System;
using System.Collections.Generic;
using System.Text;

namespace GlassTicTacToe
{
    internal class Board : List<Label>
    {
        private int side;
        private int winLength;

        private int emptyCellCount;

        private bool xToMove = true;


        private string getCell(int x, int y)
        {
            return this[y * side + x].Text;
        }


        public Board(int side, int winLength)
        {
            this.side = side;
            this.winLength = winLength;

            this.emptyCellCount = side * side;

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

        enum TransformationType
        {
            None,
            AxesSwapping,
            Rotation180,
        }

        private GameState checkGameEnd()
        {
            List<
                (int xStep, int yStep, TransformationType transformation)
                > winConditions =
                [
                (0, 1, TransformationType.None),  // vertical lines
                (0, 1, TransformationType.AxesSwapping),  // horizontal lines
                
                // backslash-like diagonal lines
                (1, 1, TransformationType.None),
                (1, 1, TransformationType.AxesSwapping),
                
                // forward slash-like diagonal lines
                (1, -1, TransformationType.None),
                (1, -1, TransformationType.Rotation180),
            ];

            foreach (var (xStep, yStep, transformation) in winConditions)
            {
                var (found, winner) = checkLine(xStep, yStep, transformation);

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

        private (bool, string) checkLine(
            int xStep,
            int yStep,
            TransformationType transformation
            )
        {
            for (int x = 0; x < side; x++)
            {
                int sameInRowCount = 1;
                string last = "";

                int y0;
                int y1;
                int x0;

                if (xStep == 0)
                {
                    x0 = x;
                    y0 = 0;
                    y1 = side;
                }
                else
                {
                    x0 = yStep == 1 ? x : 0;
                    y0 = yStep == 1 ? 0 : x;
                    y1 = yStep == 1 ? side - x : -1;
                }

                for (int y = y0, xx = x0; y != y1; y += yStep, xx += xStep)
                {
                    string current;

                    switch (transformation)
                    {
                        case TransformationType.None:
                            current = getCell(xx, y);        
                            break;
                        case TransformationType.AxesSwapping:
                            current = getCell(y, xx);
                            break;
                        case TransformationType.Rotation180:
                            current = getCell(side - 1 - xx, side - 1 - y);
                            break;
                        default:
                            current = string.Empty;
                            break;
                    }

                    if (last.Length > 0 && current == last)
                    {
                        sameInRowCount++;

                        if (sameInRowCount == winLength)
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

            emptyCellCount = side * side;

            xToMove = true;
        }
    }
}
