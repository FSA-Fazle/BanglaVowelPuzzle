using BanglaVowelPuzzle.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace BanglaVowelPuzzle.Controllers
{
    public class GameController : Controller
    {
        private const string SessionKey = "PuzzleBoard";

        public IActionResult Index()
        {
            var board = LoadOrCreate();
            var vm = new GameViewModel
            {
                Board = board,
                IsSolved = PuzzleGame.CheckSolved(board),
                SolvedBoard = PuzzleGame.GetSolvedBoard()
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult Move([FromBody] MoveRequest req)
        {
            var board = LoadOrCreate();
            int emptyIdx = board.IndexOf(PuzzleGame.Empty);
            int tileIdx = req.TileIndex;

            int emptyRow = emptyIdx / PuzzleGame.Cols;
            int emptyCol = emptyIdx % PuzzleGame.Cols;
            int tileRow = tileIdx / PuzzleGame.Cols;
            int tileCol = tileIdx % PuzzleGame.Cols;

            bool adjacent = (emptyRow == tileRow && System.Math.Abs(emptyCol - tileCol) == 1)
                         || (emptyCol == tileCol && System.Math.Abs(emptyRow - tileRow) == 1);

            if (adjacent)
            {
                (board[emptyIdx], board[tileIdx]) = (board[tileIdx], board[emptyIdx]);
                SaveBoard(board);
            }

            bool solved = PuzzleGame.CheckSolved(board);
            return Json(new { board, solved });
        }

        [HttpPost]
        public IActionResult NewGame()
        {
            var board = PuzzleGame.GetShuffledBoard();
            SaveBoard(board);
            return Json(new { board, solved = false });
        }

        private List<string> LoadOrCreate()
        {
            var json = HttpContext.Session.GetString(SessionKey);
            if (json != null)
                return JsonSerializer.Deserialize<List<string>>(json)!;
            var board = PuzzleGame.GetShuffledBoard();
            SaveBoard(board);
            return board;
        }

        private void SaveBoard(List<string> board)
        {
            HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(board));
        }
    }

    public class GameViewModel
    {
        public List<string> Board { get; set; } = new();
        public bool IsSolved { get; set; }
        public List<string> SolvedBoard { get; set; } = new();
    }

    public class MoveRequest
    {
        public int TileIndex { get; set; }
    }
}
