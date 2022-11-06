using QuoridorEngine.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuoridorEngine.Utils.Tests
{
    [TestClass()]
    public class Vector2Tests
    {
        [TestMethod()]
        [DataRow(0, 0)]
        [DataRow(-1, 10)]
        [DataRow(27, 1034)]
        public void Vector2Test(int row, int column)
        {
            Vector2 vector = new Vector2(row, column);
            Assert.AreEqual(row, vector.Row);
            Assert.AreEqual(column, vector.Column);
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, true)]
        [DataRow(2, 2, 2, 2, true)]
        [DataRow(6, 45, 6, 45, true)]

        [DataRow(6, 45, 3, 45, false)]
        [DataRow(6, 47, 6, 45, false)]
        [DataRow(6, 47, 6, 45, false)]
        [DataRow(6, 7, 65, 5, false)]
        public void EqualToTest(int row1, int column1, int row2, int column2, bool expected)
        {
            Vector2 vector = new Vector2(row1, column1);
            Vector2 vector2 = new Vector2(row2, column2);

            bool actualResult = vector.EqualTo(vector2);
            Assert.AreEqual(expected, actualResult);
            Assert.AreEqual(vector.EqualTo(vector2), vector2.EqualTo(vector));
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, true)]
        [DataRow(2, 2, 2, 2, true)]
        [DataRow(6, 45, 6, 45, true)]

        [DataRow(6, 45, 3, 45, false)]
        [DataRow(6, 47, 6, 45, false)]
        [DataRow(6, 47, 6, 45, false)]
        [DataRow(6, 7, 65, 5, false)]
        public void EqualTest(int row1, int column1, int row2, int column2, bool expected)
        {
            Vector2 vector = new Vector2(row1, column1);
            Vector2 vector2 = new Vector2(row2, column2);

            bool actualResult = Vector2.Equal(vector, vector2);
            Assert.AreEqual(expected, actualResult);
            Assert.AreEqual(actualResult, Vector2.Equal(vector2, vector));
        }

        [TestMethod()]
        [DataRow(0, 0, 0, 0, 0)]
        [DataRow(6, 45, 6, 45, 0)]
        [DataRow(0, 1, 1, 0, 2)]
        [DataRow(7, 1, 4, 1, 3)]
        [DataRow(14, 3, 34, 9, 26)]

        public void ManhattanDistaceTest(int row1, int column1, int row2, int column2, int expected)
        {
            Vector2 vector = new Vector2(row1, column1);
            Vector2 vector2 = new Vector2(row2, column2);

            int manhattanDistance = Vector2.ManhattanDistace(vector, vector2);

            Assert.AreEqual(expected, manhattanDistance);
        }
    }
}