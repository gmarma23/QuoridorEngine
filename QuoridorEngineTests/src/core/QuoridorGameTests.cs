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
            Assert.AreEqual(dimension / 2, blackColumn);
            Assert.AreEqual(0, whiteRow);
            Assert.AreEqual(dimension / 2, whiteColumn);

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
        [DataRow(0, 1, 2, 1, 5, false)]
        [DataRow(0, 1, 2, 1, 9, false)]

        public void IsTerminalStateTest(int whiteRow, int whiteColumn, int blackRow, int blackColumn, int dimension, bool expected)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            game.ForcePlayerMovement(whiteRow, whiteColumn, true);
            game.ForcePlayerMovement(blackRow, blackColumn, false);

            bool actual = game.IsTerminalState();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DataRow(1, 0, 0, 0, 3, true)]
        [DataRow(2, 1, 2, 2, 3, true)]
        [DataRow(1, 0, 2, 0, 3, true)]
        [DataRow(2, 1, 2, 0, 3, true)]
        [DataRow(0, 1, 1, 1, 3, true)]
        [DataRow(2, 19, 2, 20, 21, true)]
        [DataRow(1, 0, 0, 0, 9, true)]
        [DataRow(7, 1, 8, 1, 9, true)]
        [DataRow(0, 7, 0, 8, 9, true)]
        [DataRow(7, 8, 8, 8, 9, true)]
        [DataRow(5, 7, 5, 6, 9, true)]
        [DataRow(6, 3, 5, 3, 7, true)]
        [DataRow(6, 5, 6, 4, 7, true)]
        [DataRow(1, 4, 0, 4, 7, true)]
        [DataRow(1, 5, 1, 4, 7, true)]

        [DataRow(1, 0, 0, 0, 3, false)]
        [DataRow(2, 1, 2, 2, 3, false)]
        [DataRow(1, 0, 1, 1, 3, false)]
        [DataRow(2, 1, 2, 0, 3, false)]
        [DataRow(0, 1, 1, 1, 3, false)]
        [DataRow(2, 19, 2, 20, 21, false)]
        [DataRow(1, 0, 0, 0, 9, false)]
        [DataRow(7, 1, 8, 1, 9, false)]
        [DataRow(0, 7, 0, 8, 9, false)]
        [DataRow(7, 8, 8, 8, 9, false)]
        [DataRow(5, 7, 5, 6, 9, false)]
        [DataRow(6, 3, 5, 3, 7, false)]
        [DataRow(6, 4, 6, 3, 7, false)]
        [DataRow(1, 4, 0, 4, 7, false)]
        [DataRow(1, 5, 1, 4, 7, false)]
        public void ExecutePlayerMoveNoWallsTest(int startR, int startCol, int row, int col, int dimension, bool white)
        {
            QuoridorGame game = new QuoridorGame(dimension);

            int prevRow = 0, prevCol = 0;
            if (white)
                game.GetWhiteCoordinates(ref prevRow, ref prevCol);
            else
                game.GetBlackCoordinates(ref prevRow, ref prevCol);

            QuoridorMove move = new QuoridorMove(prevRow, prevCol, row, col, white);
            game.ForcePlayerMovement(startR, startCol, white);

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
        [DataRow(-1, 9, 9, false)]
        public void ExecutePlayerMoveNoWallsFailTest(int row, int col, int dimension, bool white)
        {
            QuoridorGame game = new QuoridorGame(dimension);

            int prevRow = 0, prevCol = 0;
            if (white)
                game.GetWhiteCoordinates(ref prevRow, ref prevCol);
            else
                game.GetBlackCoordinates(ref prevRow, ref prevCol);

            QuoridorMove move = new QuoridorMove(prevRow, prevCol, row, col, white);

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

        [TestMethod]
        [DataRow(1, 0, true, Orientation.Vertical, 9)]
        [DataRow(1, 7, false, Orientation.Horizontal, 9)]
        [DataRow(4, 6, true, Orientation.Horizontal, 9)]
        [DataRow(6, 0, false, Orientation.Vertical, 9)]

        public void ExecuteWallMoveSimplePlayerHasNoWalls(int row, int col, bool isWhite, Orientation orientation, int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, col, isWhite, orientation);

            game.SetPlayerWalls(isWhite, 0);
            int initialWalls = game.GetPlayerWalls(isWhite);

            Assert.ThrowsException<InvalidMoveException>(() => game.ExecuteMove(move));
            Assert.IsFalse(game.HasWall(row, col, orientation));
            Assert.AreEqual(initialWalls, 0);
        }

        [TestMethod]
        [DataRow(1, 0, Orientation.Horizontal, false, 9)]
        [DataRow(3, 5, Orientation.Vertical, true, 9)]
        [DataRow(6, 6, Orientation.Horizontal, false, 9)]
        [DataRow(8, 7, Orientation.Vertical, true, 9)]

        public void ExecuteWallMoveSimpleFromsIllegalCross(int row, int col, Orientation orientation, bool isWhite, int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move1 = new QuoridorMove(row, col, isWhite, orientation);

            Orientation oppositeOrientation = orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            QuoridorMove move2 = new QuoridorMove(row, col, !isWhite, oppositeOrientation);

            game.ExecuteMove(move1);
            Assert.IsTrue(game.HasWall(row, col, orientation));

            Assert.ThrowsException<InvalidMoveException>(() => game.ExecuteMove(move2));
            Assert.IsFalse(game.HasWall(row, col, oppositeOrientation));

        }

        [TestMethod]
        [DataRow(2, 0, Orientation.Horizontal, 3, 0, Orientation.Vertical, 1, 0, 9)]
        [DataRow(7, 7, Orientation.Horizontal, 8, 7, Orientation.Vertical, 6, 7, 9)]
        [DataRow(1, 1, Orientation.Vertical, 1, 0, Orientation.Horizontal, 1, 2, 9)]
        [DataRow(8, 6, Orientation.Vertical, 8, 5, Orientation.Horizontal, 8, 7, 9)]

        public void ExecuteWallMoveSimpleFromsLegalCross(
            int row1, int col1, Orientation orientation1,
            int row2, int col2, Orientation orientation2,
            int row3, int col3, int dimension
            )
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move1 = new QuoridorMove(row1, col1, true, orientation1);
            QuoridorMove move2 = new QuoridorMove(row2, col2, false, orientation2);
            QuoridorMove move3 = new QuoridorMove(row3, col3, true, orientation2);

            game.ExecuteMove(move1);
            Assert.IsTrue(game.HasWall(row1, col1, orientation1));

            game.ExecuteMove(move2);
            Assert.IsTrue(game.HasWall(row2, col2, orientation2));

            game.ExecuteMove(move3);
            Assert.IsTrue(game.HasWall(row3, col3, orientation2));
        }

        [TestMethod]
        [DataRow(1, 0, Orientation.Horizontal, 9)]
        [DataRow(3, 2, Orientation.Horizontal, 9)]
        [DataRow(5, 6, Orientation.Vertical, 9)]
        [DataRow(8, 7, Orientation.Vertical, 9)]

        public void ExecuteWallMoveSimpleWallExists(int row, int col, Orientation orientation, int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, col, true, orientation);

            game.ExecuteMove(move);
            Assert.IsTrue(game.HasWall(row, col, orientation));

            Assert.ThrowsException<InvalidMoveException>(() => game.ExecuteMove(move));
            Assert.IsTrue(game.HasWall(row, col, orientation));
        }

        [TestMethod()]
        [DataRow(9, 1, 0, true, Orientation.Horizontal)]
        [DataRow(9, 1, 7, false, Orientation.Horizontal)]
        [DataRow(9, 8, 0, true, Orientation.Horizontal)]
        [DataRow(9, 8, 7, false, Orientation.Horizontal)]
        [DataRow(9, 5, 4, true, Orientation.Horizontal)]

        [DataRow(9, 1, 0, false, Orientation.Vertical)]
        [DataRow(9, 1, 7, true, Orientation.Vertical)]
        [DataRow(9, 8, 0, false, Orientation.Vertical)]
        [DataRow(9, 8, 7, true, Orientation.Vertical)]
        [DataRow(9, 5, 4, false, Orientation.Vertical)]

        public void UndoWallPlacementTest(int dimension, int row, int column, bool isWhitePlayer, Orientation orientation)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            QuoridorMove move = new QuoridorMove(row, column, isWhitePlayer, orientation);

            int initialWalls = game.GetPlayerWalls(isWhitePlayer);

            game.ExecuteMove(move);
            Assert.IsTrue(game.HasWall(row, column, orientation));

            int wallsAfterMove = game.GetPlayerWalls(isWhitePlayer);
            Assert.IsTrue(wallsAfterMove == initialWalls - 1);

            game.UndoMove(move);
            Assert.IsFalse(game.HasWall(row, column, orientation));

            int wallsAfterUndoMove = game.GetPlayerWalls(isWhitePlayer);
            Assert.IsTrue(wallsAfterUndoMove == initialWalls);
        }

        [TestMethod()]
        [DataRow(9, 0, 0, 0, 1, true)]
        [DataRow(9, 3, 2, 3, 3, false)]
        [DataRow(9, 5, 4, 6, 4, true)]
        [DataRow(9, 6, 7, 7, 7, false)]
        [DataRow(9, 7, 8, 6, 8, true)]
        [DataRow(9, 8, 5, 8, 4, false)]

        public void UndoPlayerMoveTest(int dimension, int initRow, int initColumn, int newRow, int newColumn, bool isWhitePlayer)
        {
            QuoridorGame game = new QuoridorGame(dimension);

            game.ForcePlayerMovement(initRow, initColumn, isWhitePlayer);
            QuoridorMove move = new QuoridorMove(initRow, initColumn, newRow, newColumn, isWhitePlayer);

            int currentRow = 0, currentColumn = 0;

            game.ExecuteMove(move);
            if (isWhitePlayer)
                game.GetWhiteCoordinates(ref currentRow, ref currentColumn);
            else
                game.GetBlackCoordinates(ref currentRow, ref currentColumn);
            Assert.IsTrue(currentRow == newRow);
            Assert.IsTrue(currentColumn == newColumn);

            game.UndoMove(move);
            if (isWhitePlayer)
                game.GetWhiteCoordinates(ref currentRow, ref currentColumn);
            else
                game.GetBlackCoordinates(ref currentRow, ref currentColumn);
            Assert.IsTrue(currentRow == initRow);
            Assert.IsTrue(currentColumn == initColumn);
        }

        [TestMethod()]
        [DataRow(9)]
        [DataRow(7)]
        [DataRow(5)]

        public void GetIDInitStateTest(int dimension)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            long initIDWhite = game.GetID(true);
            long initIDBlack = game.GetID(false);

            Assert.AreNotEqual(initIDWhite, initIDBlack);
        }
        
        [TestMethod()]
        [DataRow(9, 0, 0, 0, 1, true)]
        [DataRow(9, 8, 0, 7, 0, false)]
        [DataRow(9, 5, 4, 5, 5, true)]
        [DataRow(9, 6, 3, 6, 2, false)]

        public void GetIDPawnMoveTest(int dimension, int initRow, int initColumn, int newRow, int newColumn, bool isWhitePlayer)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            long initID1 = game.GetID(isWhitePlayer);

            game.ForcePlayerMovement(initRow, initColumn, isWhitePlayer);
            QuoridorMove move = new QuoridorMove(initRow, initColumn, newRow, newColumn, isWhitePlayer);

            game.ExecuteMove(move);
            long newID = game.GetID(!isWhitePlayer);
            long samePlayerNewID = game.GetID(isWhitePlayer);
            game.UndoMove(move);

            long initID2 = game.GetID(isWhitePlayer);

            Assert.AreEqual(initID1, initID2);
            Assert.AreNotEqual(samePlayerNewID, newID);
            Assert.AreNotEqual(initID1, newID);
        }

        [TestMethod()]
        [DataRow(9, 1, 0, Orientation.Horizontal, true)]
        [DataRow(9, 8, 7, Orientation.Horizontal, false)]
        [DataRow(9, 5, 4, Orientation.Vertical, true)]
        [DataRow(9, 6, 3, Orientation.Vertical, false)]

        public void GetIDWallPlacementTest(int dimension, int row, int column, Orientation orientation, bool isWhitePlayer)
        {
            QuoridorGame game = new QuoridorGame(dimension);
            long initID1 = game.GetID(isWhitePlayer);

            QuoridorMove move = new QuoridorMove(row, column, isWhitePlayer, orientation);

            game.ExecuteMove(move);
            long newID = game.GetID(!isWhitePlayer);
            long samePlayerNewID = game.GetID(isWhitePlayer);
            game.UndoMove(move);

            long initID2 = game.GetID(isWhitePlayer);

            Assert.AreEqual(initID1, initID2);
            Assert.AreNotEqual(samePlayerNewID, newID);
            Assert.AreNotEqual(initID1, newID);
        }
    }
}