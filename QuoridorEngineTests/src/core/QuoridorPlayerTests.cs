using QuoridorEngine.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuoridorEngine.Core.Tests
{
    [TestClass()]
    public class QuoridorPlayerTests
    {
        [TestMethod()]
        [DataRow(false, 0, 5, 10, 8)]
        [DataRow(true, 2, 6, 0, 10)]
        public void QuoridorPlayerTest(bool isWhite, int row, int column, int startingWalls, int targetBaseline)
        {
            QuoridorPlayer player = new QuoridorPlayer(isWhite, row, column, startingWalls, targetBaseline);

            Assert.AreEqual(player.IsWhite, isWhite);
            Assert.AreEqual(player.Row, row);
            Assert.AreEqual(player.Column, column);
            Assert.AreEqual(player.AvailableWalls, startingWalls);
            Assert.AreNotEqual(player.IsInTargetBaseline(), targetBaseline);
        }

        [TestMethod()]
        [DataRow(0, 1)]
        [DataRow(2, 3)]
        [DataRow(10, 11)]
        [DataRow(43, 44)]
        public void IncreaseAvailableWallsTest(int startingWalls, int expectedWalls)
        {
            QuoridorPlayer player = new QuoridorPlayer(false, 0, 0, startingWalls, 10);

            player.IncreaseAvailableWalls();

            Assert.AreEqual(player.AvailableWalls, expectedWalls);
        }

        [TestMethod()]
        [DataRow(1, 0)]
        [DataRow(5, 4)]
        [DataRow(10, 9)]
        public void DecreaseAvailableWallsTest(int startingWalls, int expectedWalls)
        {
            QuoridorPlayer player = new QuoridorPlayer(false, 0, 0, startingWalls, 10);

            player.DecreaseAvailableWalls();

            Assert.AreEqual(player.AvailableWalls, expectedWalls);
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(15)]
        public void IsInTargetBaselineTest(int targetBaseline)
        {
            QuoridorPlayer player = new QuoridorPlayer(false, 0, 0, 10, targetBaseline);

            player.Row = targetBaseline;

            Assert.IsTrue(player.IsInTargetBaseline());
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(10)]
        [DataRow(15)]
        public void IsInTargetBaselineTestFail(int targetBaseline)
        {
            QuoridorPlayer player = new QuoridorPlayer(false, 0, 0, 10, targetBaseline);

            player.Row = targetBaseline-1;

            Assert.IsFalse(player.IsInTargetBaseline());
        }
    }
}