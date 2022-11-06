using QuoridorEngine.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuoridorEngine.Core.Tests
{
    [TestClass()]
    public class QuoridorBoardTests
    {
        [TestMethod()]
        [DataRow(9)]
        [DataRow(3)]
        [DataRow(201)]
        [DataRow(35)]
        [DataRow(7)]

        public void QuoridorBoardTest(int dimension)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsNotNull(board);
        }

        [TestMethod()]
        [DataRow(3, 1, 0)]
        [DataRow(3, 1, 1)]
        [DataRow(3, 1, 2)]
        [DataRow(3, 2, 0)]
        [DataRow(3, 2, 1)]
        [DataRow(3, 2, 2)]

        [DataRow(5, 1, 0)]
        [DataRow(5, 1, 1)]
        [DataRow(5, 1, 3)]
        [DataRow(5, 3, 0)]
        [DataRow(5, 4, 1)]
        [DataRow(5, 4, 4)]

        [DataRow(6, 1, 0)]
        [DataRow(6, 1, 1)]
        [DataRow(6, 1, 2)]
        [DataRow(6, 4, 0)]
        [DataRow(6, 5, 4)]
        [DataRow(6, 5, 5)]

        [DataRow(9, 1, 0)]
        [DataRow(9, 1, 1)]
        [DataRow(9, 1, 3)]
        [DataRow(9, 3, 0)]
        [DataRow(9, 5, 2)]
        [DataRow(9, 7, 6)]
        [DataRow(9, 8, 0)]
        [DataRow(9, 8, 1)]
        [DataRow(9, 8, 2)]
        [DataRow(9, 8, 4)]
        [DataRow(9, 8, 7)]
        [DataRow(9, 8, 8)]

        public void CheckWallPartHorizontalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsFalse(board.CheckWallPartHorizontal(row, col));
        }

        [TestMethod()]
        [DataRow(9, 1, 0)]
        [DataRow(9, 1, 1)]
        [DataRow(9, 2, 3)]
        [DataRow(9, 5, 6)]
        [DataRow(9, 7, 8)]
        [DataRow(9, 7, 8)]

        [DataRow(3, 1, 0)]
        [DataRow(3, 2, 2)]
        [DataRow(3, 2, 3)]
        [DataRow(3, 1, 1)]
        [DataRow(3, 3, 2)]

        [DataRow(4, 4, 4)]
        public void AddWallPartHorizontalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddWallPartHorizontal(row, col);

            Assert.IsTrue(board.CheckWallPartHorizontal(row, col));
        }

        
    }
}