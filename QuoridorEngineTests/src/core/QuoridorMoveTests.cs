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
    }
}