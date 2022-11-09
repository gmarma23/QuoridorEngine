using QuoridorEngine.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuoridorEngine.Utils;

namespace QuoridorEngine.Core.Tests
{
    [TestClass()]
    public class QuoridorGameTests
    {
        [TestMethod()]
        [DataRow(5)]
        [DataRow(7)]
        [DataRow(15)]
        [DataRow(3)]
        public void QuoridorGameConstructorTest(int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            int whiteRow = 0, whiteColumn = 0, blackRow = 0, blackColumn = 0;
            game.GetWhiteCoordinates(ref whiteRow, ref whiteColumn);
            game.GetBlackCoordinates(ref blackRow, ref blackColumn);

            Assert.IsNotNull(game);
            Assert.AreEqual(dimension, game.Dimension);
            Assert.AreEqual(dimension - 1, blackRow);
            Assert.AreEqual(dimension / 2 + 1, blackColumn);
            Assert.AreEqual(0, whiteRow);
            Assert.AreEqual(dimension / 2 + 1, whiteColumn);

            // TODO: assert game history is initialized
        }

        [TestMethod()]
        [DataRow(2)]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(-4)]
        [DataRow(6)]
        [DataRow(50)]
        public void QuoridorGameConstructorFailTest(int dimension)
        {
            Assert.ThrowsException<ArgumentException>(() => new QuoridorGame(dimension));
        }

        [TestMethod()]
        [DataRow(2, 1, 1, 1, 3, true)]
        [DataRow(1, 1, 0, 1, 3, true)]
        [DataRow(4, 1, 0, 1, 5, true)]
        [DataRow(1, 1, 0, 1, 5, true)]
        [DataRow(8, 1, 0, 1, 9, true)]
        [DataRow(6, 1, 0, 1, 9, true)]
        [DataRow(0, 1, 2, 1, 3, false)]
        [DataRow(2, 1, 0, 1, 5, false)]
        [DataRow(2, 1, 0, 1, 9, false)]

        public void IsTerminalStateTest(int whiteRow, int whiteColumn, int blackRow, int blackColumn, int dimension, bool expected)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove whiteMove = new QuoridorMove(whiteRow, whiteColumn, true);
            QuoridorMove blackMove = new QuoridorMove(blackRow, blackColumn, false);
            game.ExecuteMove(whiteMove);
            game.ExecuteMove(blackMove);

            bool actual = game.IsTerminalState();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DataRow(0, 0, 3, true)]
        [DataRow(2, 2, 3, true)]
        [DataRow(0, 2, 3, true)]
        [DataRow(2, 0, 3, true)]
        [DataRow(1, 1, 3, true)]
        [DataRow(2, 20, 3, true)]
        [DataRow(0, 0, 9, true)]
        [DataRow(8, 1, 9, true)]
        [DataRow(0, 8, 9, true)]
        [DataRow(8, 8, 9, true)]
        [DataRow(5, 6, 9, true)]

        [DataRow(0, 0, 3, false)]
        [DataRow(2, 2, 3, false)]
        [DataRow(0, 2, 3, false)]
        [DataRow(2, 0, 3, false)]
        [DataRow(1, 1, 3, false)]
        [DataRow(2, 20, 3, false)]
        [DataRow(0, 0, 9, false)]
        [DataRow(8, 1, 9, false)]
        [DataRow(0, 8, 9, false)]
        [DataRow(8, 8, 9, false)]
        [DataRow(5, 6, 9, false)]
        public void ExecutePlayerMoveNoWallsTest(int row, int col, int dimension, bool white)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, col, white);

            game.ExecuteMove(move);
            int newRow = 0, newCol = 0;
            if (white)
                game.GetWhiteCoordinates(ref newRow, ref newCol);
            else
                game.GetBlackCoordinates(ref newRow, ref newCol);

            Assert.AreEqual(row, newRow);
            Assert.AreEqual(col, newCol);
        }

        [TestMethod]
        [DataRow(-1, 0, 3, true)]
        [DataRow(-1, -1, 3, true)]
        [DataRow(0, -1, 3, true)]
        [DataRow(3, -1, 3, true)]
        [DataRow(3, 3, 3, true)]
        [DataRow(0, -1, 9, true)]
        [DataRow(9, 9, 9, true)]
        [DataRow(-1, -1, 9, true)]
        [DataRow(8, 9, 9, true)]

        [DataRow(-1, 0, 3, false)]
        [DataRow(-1, -1, 3, false)]
        [DataRow(0, -1, 3, false)]
        [DataRow(3, -1, 3, false)]
        [DataRow(3, 3, 3, false)]
        [DataRow(0, -1, 9, false)]
        [DataRow(9, 9, 9, false)]
        [DataRow(-1, -1, 9, false)]
        [DataRow(8, 9, 9, false)]
        public void ExecutePlayerMoveNoWallsFailTest(int row, int col, int dimension, bool white)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, col, white);

            Assert.ThrowsException<InvalidMoveException>(() => game.ExecuteMove(move));
        }

        // TODO: Test player move with walls
        // TODO: Test player move with jumping
        // TODO: Test player move with jumping and walls special case

        [TestMethod]
        [DataRow(1, 0, true, Orientation.Horizontal, 9)]
        [DataRow(1, 7, true, Orientation.Horizontal, 9)]
        [DataRow(4, 6, true, Orientation.Horizontal, 9)]
        [DataRow(6, 0, true, Orientation.Horizontal, 9)]
        [DataRow(7, 7, true, Orientation.Horizontal, 9)]
        [DataRow(8, 0, true, Orientation.Horizontal, 9)]
        [DataRow(8, 7, true, Orientation.Horizontal, 9)]

        [DataRow(1, 0, false, Orientation.Horizontal, 9)]
        [DataRow(1, 7, false, Orientation.Horizontal, 9)]
        [DataRow(4, 6, false, Orientation.Horizontal, 9)]
        [DataRow(6, 0, false, Orientation.Horizontal, 9)]
        [DataRow(7, 7, false, Orientation.Horizontal, 9)]
        [DataRow(8, 0, false, Orientation.Horizontal, 9)]
        [DataRow(8, 7, false, Orientation.Horizontal, 9)]

        [DataRow(1, 0, true, Orientation.Vertical, 9)]
        [DataRow(1, 7, true, Orientation.Vertical, 9)]
        [DataRow(4, 6, true, Orientation.Vertical, 9)]
        [DataRow(6, 0, true, Orientation.Vertical, 9)]
        [DataRow(7, 7, true, Orientation.Vertical, 9)]
        [DataRow(8, 0, true, Orientation.Vertical, 9)]
        [DataRow(8, 7, true, Orientation.Vertical, 9)]

        [DataRow(1, 0, false, Orientation.Vertical, 9)]
        [DataRow(1, 7, false, Orientation.Vertical, 9)]
        [DataRow(4, 6, false, Orientation.Vertical, 9)]
        [DataRow(6, 0, false, Orientation.Vertical, 9)]
        [DataRow(7, 7, false, Orientation.Vertical, 9)]
        [DataRow(8, 0, false, Orientation.Vertical, 9)]
        [DataRow(8, 7, false, Orientation.Vertical, 9)]


        public void ExecuteWallMoveSimpleTest(int row, int col, bool isWhite, Orientation orientation, int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, col, isWhite, orientation);
            int initialWalls = game.GetPlayerWalls(isWhite);

            game.ExecuteMove(move);

            Assert.IsTrue(game.HasWall(row, col, orientation));
            Assert.AreEqual(initialWalls - 1, game.GetPlayerWalls(isWhite));
        }

        [TestMethod]
        [DataRow(-5, -7, Orientation.Horizontal, 9)]
        [DataRow(-2, 0, Orientation.Horizontal, 9)]
        [DataRow(3, -4, Orientation.Horizontal, 9)]
        [DataRow(0, 0, Orientation.Horizontal, 9)]
        [DataRow(8, 8, Orientation.Horizontal, 9)]
        [DataRow(9, 0, Orientation.Horizontal, 9)]
        [DataRow(9, 0, Orientation.Horizontal, 9)]
        [DataRow(9, 7, Orientation.Horizontal, 9)]
        [DataRow(9, 9, Orientation.Horizontal, 9)]
        [DataRow(10, 0, Orientation.Horizontal, 9)]
        [DataRow(10, 10, Orientation.Horizontal, 9)]
        [DataRow(17, 14, Orientation.Horizontal, 9)]

        [DataRow(0, 0, Orientation.Vertical, 9)]
        [DataRow(0, 7, Orientation.Vertical, 9)]
        [DataRow(1, 8, Orientation.Vertical, 9)]
        [DataRow(8, 8, Orientation.Vertical, 9)]
        [DataRow(9, 0, Orientation.Vertical, 9)]
        [DataRow(9, 9, Orientation.Vertical, 9)]
        [DataRow(9, 10, Orientation.Vertical, 9)]
        [DataRow(10, 9, Orientation.Vertical, 9)]
        [DataRow(16, 19, Orientation.Vertical, 9)]


        public void ExecuteWallMoveSimpleFailTest(int row, int col, Orientation orientation, int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, col, true, orientation);

            Assert.ThrowsException<InvalidMoveException>(() => game.ExecuteMove(move));
        }

        // TODO: test when wall forms illegal cross or is placed on an occupied position
        // TODO: test when wall blocks player path to goal
        // TODO: test when walls form a cross that is legal
    }
}