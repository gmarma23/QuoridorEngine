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

        public void QuoridorBoardConstructorTest(int dimension)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsNotNull(board);
            Assert.AreEqual(dimension, board.Dimension);

        }

        [TestMethod()]
        [DataRow(9, 0, 0)]
        [DataRow(9, 0, 1)]
        [DataRow(9, 1, 0)]
        [DataRow(9, 5, 6)]
        [DataRow(9, 6, 8)]
        [DataRow(9, 7, 7)]
        [DataRow(9, 7, 8)]
        [DataRow(9, 8, 7)]
        [DataRow(9, 8, 8)]

        public void IsValidPlayerSquareTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsTrue(board.IsValidPlayerSquare(row, col));
        }

        [TestMethod()]
        [DataRow(9, -23, -15)]
        [DataRow(9, -7, -8)]
        [DataRow(9, -1, -1)]
        [DataRow(9, -1, 0)]
        [DataRow(9, 0, -1)]
        [DataRow(9, 8, 9)]
        [DataRow(9, 9, 8)]
        [DataRow(9, 9, 9)]
        [DataRow(9, 10, 10)]
        [DataRow(9, 34, 46)]

        public void IsValidPlayerSquareTestInvalid(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsFalse(board.IsValidPlayerSquare(row, col));
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

        public void AddWallPartHorizontalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddWallPartHorizontal(row, col);

            Assert.IsTrue(board.CheckWallPartHorizontal(row, col));
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

        public void RemoveWallPartHorizontalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddWallPartHorizontal(row, col);
            board.RemoveWallPartHorizontal(row, col);

            Assert.IsFalse(board.CheckWallPartHorizontal(row, col));
        }

        [TestMethod()]
        [DataRow(3, 0, 0)]
        [DataRow(3, 0, 1)]
        [DataRow(3, 1, 0)]
        [DataRow(3, 1, 1)]
        [DataRow(3, 2, 0)]
        [DataRow(3, 2, 1)]

        [DataRow(5, 0, 0)]
        [DataRow(5, 0, 1)]
        [DataRow(5, 0, 3)]
        [DataRow(5, 1, 0)]
        [DataRow(5, 1, 3)]
        [DataRow(5, 3, 0)]
        [DataRow(5, 4, 3)]

        [DataRow(6, 0, 0)]
        [DataRow(6, 0, 1)]
        [DataRow(6, 0, 4)]
        [DataRow(6, 1, 1)]
        [DataRow(6, 2, 1)]
        [DataRow(6, 4, 3)]
        [DataRow(6, 5, 4)]

        [DataRow(9, 0, 0)]
        [DataRow(9, 0, 1)]
        [DataRow(9, 0, 3)]
        [DataRow(9, 0, 7)]
        [DataRow(9, 1, 1)]
        [DataRow(9, 1, 7)]
        [DataRow(9, 2, 5)]
        [DataRow(9, 2, 6)]
        [DataRow(9, 3, 1)]
        [DataRow(9, 4, 7)]
        [DataRow(9, 6, 7)]
        [DataRow(9, 7, 7)]
        [DataRow(9, 8, 7)]

        public void CheckWallPartVerticalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsFalse(board.CheckWallPartVertical(row, col));
        }

        [TestMethod()]
        [DataRow(3, 0, 0)]
        [DataRow(3, 0, 1)]
        [DataRow(3, 1, 0)]
        [DataRow(3, 1, 1)]
        [DataRow(3, 2, 0)]
        [DataRow(3, 2, 1)]

        [DataRow(5, 0, 0)]
        [DataRow(5, 0, 1)]
        [DataRow(5, 0, 3)]
        [DataRow(5, 1, 0)]
        [DataRow(5, 1, 3)]
        [DataRow(5, 3, 0)]
        [DataRow(5, 4, 3)]

        [DataRow(6, 0, 0)]
        [DataRow(6, 0, 1)]
        [DataRow(6, 0, 4)]
        [DataRow(6, 1, 1)]
        [DataRow(6, 2, 1)]
        [DataRow(6, 4, 3)]
        [DataRow(6, 5, 4)]

        [DataRow(9, 0, 0)]
        [DataRow(9, 0, 1)]
        [DataRow(9, 0, 3)]
        [DataRow(9, 0, 7)]
        [DataRow(9, 1, 1)]
        [DataRow(9, 1, 7)]
        [DataRow(9, 2, 5)]
        [DataRow(9, 2, 6)]
        [DataRow(9, 3, 1)]
        [DataRow(9, 4, 7)]
        [DataRow(9, 6, 7)]        
        [DataRow(9, 7, 7)]
        [DataRow(9, 8, 7)]

        public void AddWallPartVerticalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddWallPartVertical(row, col);

            Assert.IsTrue(board.CheckWallPartVertical(row, col));
        }

        [TestMethod()]
        [DataRow(3, 0, 0)]
        [DataRow(3, 0, 1)]
        [DataRow(3, 1, 0)]
        [DataRow(3, 1, 1)]
        [DataRow(3, 2, 0)]
        [DataRow(3, 2, 1)]

        [DataRow(5, 0, 0)]
        [DataRow(5, 0, 1)]
        [DataRow(5, 0, 3)]
        [DataRow(5, 1, 0)]
        [DataRow(5, 1, 3)]
        [DataRow(5, 3, 0)]
        [DataRow(5, 4, 3)]

        [DataRow(6, 0, 0)]
        [DataRow(6, 0, 1)]
        [DataRow(6, 0, 4)]
        [DataRow(6, 1, 1)]
        [DataRow(6, 2, 1)]
        [DataRow(6, 4, 3)]
        [DataRow(6, 5, 4)]

        [DataRow(9, 0, 0)]
        [DataRow(9, 0, 1)]
        [DataRow(9, 0, 3)]
        [DataRow(9, 0, 7)]
        [DataRow(9, 1, 1)]
        [DataRow(9, 1, 7)]
        [DataRow(9, 2, 5)]
        [DataRow(9, 2, 6)]
        [DataRow(9, 3, 1)]
        [DataRow(9, 4, 7)]
        [DataRow(9, 6, 7)]
        [DataRow(9, 7, 7)]
        [DataRow(9, 8, 7)]

        public void RemoveWallPartVerticalTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddWallPartVertical(row, col);
            board.RemoveWallPartVertical(row, col);

            Assert.IsFalse(board.CheckWallPartVertical(row, col));
        }

        [TestMethod()]
        [DataRow(3, 1, 1)]
        [DataRow(3, 1, 2)]
        [DataRow(3, 2, 1)]
        [DataRow(3, 2, 2)]

        [DataRow(5, 1, 1)]
        [DataRow(5, 1, 2)]
        [DataRow(5, 2, 1)]
        [DataRow(5, 3, 2)]
        [DataRow(5, 3, 3)]
        [DataRow(5, 4, 3)]

        [DataRow(6, 1, 1)]
        [DataRow(6, 3, 4)]
        [DataRow(6, 4, 4)]
        [DataRow(6, 5, 1)]
        [DataRow(6, 5, 3)]
        [DataRow(6, 5, 5)]

        [DataRow(9, 1, 1)]
        [DataRow(9, 3, 1)]
        [DataRow(9, 4, 3)]
        [DataRow(9, 5, 5)]
        [DataRow(9, 5, 7)]
        [DataRow(9, 5, 8)]
        [DataRow(9, 6, 1)]
        [DataRow(9, 7, 6)]
        [DataRow(9, 7, 7)]
        [DataRow(9, 7, 8)]
        [DataRow(9, 8, 8)]


        public void CheckCornerTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            Assert.IsFalse(board.CheckCorner(row, col));
        }

        [TestMethod()]
        [DataRow(3, 1, 1)]
        [DataRow(3, 1, 2)]
        [DataRow(3, 2, 1)]
        [DataRow(3, 2, 2)]

        [DataRow(5, 1, 1)]
        [DataRow(5, 1, 2)]
        [DataRow(5, 2, 1)]
        [DataRow(5, 3, 2)]
        [DataRow(5, 3, 3)]
        [DataRow(5, 4, 4)]

        [DataRow(6, 1, 1)]
        [DataRow(6, 3, 4)]
        [DataRow(6, 4, 4)]
        [DataRow(6, 5, 1)]
        [DataRow(6, 5, 3)]
        [DataRow(6, 5, 5)]

        [DataRow(9, 1, 1)]
        [DataRow(9, 3, 1)]
        [DataRow(9, 4, 3)]
        [DataRow(9, 5, 5)]
        [DataRow(9, 5, 7)]
        [DataRow(9, 5, 8)]
        [DataRow(9, 6, 1)]
        [DataRow(9, 7, 6)]
        [DataRow(9, 7, 7)]
        [DataRow(9, 7, 8)]
        [DataRow(9, 8, 8)]

        public void AddCornerTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddCorner(row, col);

            Assert.IsTrue(board.CheckCorner(row, col));
        }

        [TestMethod()]
        [DataRow(3, 1, 1)]
        [DataRow(3, 1, 2)]
        [DataRow(3, 2, 1)]
        [DataRow(3, 2, 2)]

        [DataRow(5, 1, 1)]
        [DataRow(5, 1, 2)]
        [DataRow(5, 2, 1)]
        [DataRow(5, 3, 2)]
        [DataRow(5, 3, 3)]
        [DataRow(5, 4, 4)]

        [DataRow(6, 1, 1)]
        [DataRow(6, 3, 4)]
        [DataRow(6, 4, 4)]
        [DataRow(6, 5, 1)]
        [DataRow(6, 5, 3)]
        [DataRow(6, 5, 5)]

        [DataRow(9, 1, 1)]
        [DataRow(9, 3, 1)]
        [DataRow(9, 4, 3)]
        [DataRow(9, 5, 5)]
        [DataRow(9, 5, 7)]
        [DataRow(9, 5, 8)]
        [DataRow(9, 6, 1)]
        [DataRow(9, 7, 6)]
        [DataRow(9, 7, 7)]
        [DataRow(9, 7, 8)]
        [DataRow(9, 8, 8)]

        public void RemoveCornerTest(int dimension, int row, int col)
        {
            QuoridorBoard board = new QuoridorBoard(dimension);
            board.AddCorner(row, col);
            board.RemoveCorner(row, col);

            Assert.IsFalse(board.CheckCorner(row, col));
        }

        
    }
}