using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuoridorEngine.Core.Tests
{
    [TestClass()]
    public class QuoridorMoveTests
    {
        [TestMethod()]
        [DataRow(1, 1, false, Orientation.Vertical)]
        public void QuoridorWallMoveTest(int row, int column, bool isWhitePlayer, Orientation orientation)
        {
            QuoridorMove move = new QuoridorMove(row, column, isWhitePlayer, orientation);
            Assert.AreEqual(row, move.Row);
            Assert.AreEqual(column, move.Column);
            Assert.AreEqual(isWhitePlayer, move.IsWhitePlayer);
            Assert.AreEqual(orientation, move.Orientation);
            Assert.AreEqual(MoveType.WallPlacement, move.Type);
        }

        [TestMethod()]
        [DataRow(1, 1, false)]
        public void QuoridorPlayerMoveTest(int row, int column, bool isWhitePlayer)
        {
            QuoridorMove move = new QuoridorMove(0, 0, row, column, isWhitePlayer);
            Assert.AreEqual(row, move.Row);
            Assert.AreEqual(column, move.Column);
            Assert.AreEqual(isWhitePlayer, move.IsWhitePlayer);
            Assert.AreEqual(MoveType.PlayerMovement, move.Type);
        }

        [TestMethod()]
        [DataRow()]

        public void NullMoveIsEqualTest()
        {
            QuoridorMove move1 = null;
            QuoridorMove move2 = new QuoridorMove(1, 0, 2, 0, true);

            Assert.IsFalse(move2.IsEqual(move1));
        }

        [TestMethod()]
        [DataRow(3, 4, 3, 5, true, 3, 4, 3, 5, true, true)]
        [DataRow(3, 4, 3, 5, true, 3, 4, 3, 5, false, false)]
        [DataRow(6, 5, 7, 5, false, 6, 5, 7, 5, false, true)]
        [DataRow(5, 2, 5, 3, true, 5, 0, 5, 3, true, false)]

        public void PawnMovementIsEqualTest(
            int prevRow1, int prevColumn1, int row1, int column1, bool isWhitePlayer1,
            int prevRow2, int prevColumn2, int row2, int column2, bool isWhitePlayer2,
            bool areEqual)
        {
            QuoridorMove move1 = new QuoridorMove(prevRow1, prevColumn1, row1, column1, isWhitePlayer1);
            QuoridorMove move2 = new QuoridorMove(prevRow2, prevColumn2, row2, column2, isWhitePlayer2);

            if (areEqual)
                Assert.IsTrue(move1.IsEqual(move2));
            else
                Assert.IsFalse(move1.IsEqual(move2));
        }

        [TestMethod()]
        [DataRow(1, 0, true, Orientation.Horizontal, 1, 0, true, Orientation.Horizontal, true)]
        [DataRow(6, 5, false, Orientation.Vertical, 6, 5, false, Orientation.Vertical, true)]
        [DataRow(1, 0, true, Orientation.Horizontal, 1, 0, false, Orientation.Horizontal, false)]
        [DataRow(2, 0, true, Orientation.Vertical, 1, 0, true, Orientation.Vertical, false)]
        [DataRow(5, 4, false, Orientation.Vertical, 5, 4, false, Orientation.Horizontal, false)]

        public void WallPlacementIsEqualTest(
            int row1, int column1, bool isWhitePlayer1, Orientation orientation1,
            int row2, int column2, bool isWhitePlayer2, Orientation orientation2,
            bool areEqual)
        {
            QuoridorMove move1 = new QuoridorMove(row1, column1, isWhitePlayer1, orientation1);
            QuoridorMove move2 = new QuoridorMove(row2, column2, isWhitePlayer2, orientation2);

            if (areEqual)
                Assert.IsTrue(move1.IsEqual(move2));
            else
                Assert.IsFalse(move1.IsEqual(move2));
        }
    }
}