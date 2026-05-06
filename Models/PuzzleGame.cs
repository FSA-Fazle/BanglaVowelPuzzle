using System.Collections.Generic;
using System.Linq;

namespace BanglaVowelPuzzle.Models
{
    public class PuzzleGame
    {
        public static readonly string[] BanglaVowels =
            { "অ", "আ", "ই", "ঈ", "উ", "ঊ", "ঋ", "এ", "ঐ", "ও", "ঔ" };

        public const int Rows = 3;
        public const int Cols = 4;
        public const string Empty = "";

        // Flat board: 12 cells (0–11), Empty string = blank tile
        public List<string> Board { get; set; } = new();
        public bool IsSolved { get; set; }

        public static List<string> GetSolvedBoard()
        {
            var b = new List<string>(BanglaVowels) { Empty };
            return b;
        }

        public static List<string> GetShuffledBoard()
        {
            var b = GetSolvedBoard();
            var rng = new System.Random();
            // Fisher-Yates shuffle, then verify solvability
            do
            {
                for (int i = b.Count - 1; i > 0; i--)
                {
                    int j = rng.Next(i + 1);
                    (b[i], b[j]) = (b[j], b[i]);
                }
            } while (!IsSolvable(b) || CheckSolved(b));
            return b;
        }

        // 15-puzzle solvability: count inversions + blank row from bottom
        public static bool IsSolvable(List<string> board)
        {
            var tiles = board.Where(x => x != Empty).ToList();
            int inversions = 0;
            for (int i = 0; i < tiles.Count; i++)
                for (int j = i + 1; j < tiles.Count; j++)
                    if (Array.IndexOf(BanglaVowels, tiles[i]) > Array.IndexOf(BanglaVowels, tiles[j]))
                        inversions++;

            int blankIndex = board.IndexOf(Empty);
            int blankRowFromBottom = Rows - (blankIndex / Cols);

            // For even-width grid: (inversions + blank row from bottom) must be odd
            return (inversions + blankRowFromBottom) % 2 == 1;
        }

        public static bool CheckSolved(List<string> board)
        {
            var solved = GetSolvedBoard();
            return board.SequenceEqual(solved);
        }
    }
}
