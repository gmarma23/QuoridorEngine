using System.Diagnostics;

namespace QuoridorEngine.Utils
{
    /// <summary>
    /// Defines a utility class and related functionality for a Vector
    /// item containing 2 attributes: a row and a column. This provides
    /// neat packaging for those 2 values used throughout the whole project
    /// and also makes operation on them easier and more centralized.
    /// </summary>
    public class Vector2
    {
        private int row, column;

        /// <summary>
        /// Creates a new Vector2 with the given attributes
        /// </summary>
        /// <param name="row">The row parameter of the vector</param>
        /// <param name="column">The column parameter of the vector</param>
        public Vector2(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// Returns whether this vector is equal to another vector
        /// </summary>
        /// <param name="a">The vector to make the comparison with</param>
        /// <returns>Whether this vector and vector a are equal</returns>
        public bool Equals(Vector2 a)
        {
            Debug.Assert(a != null);
            return (row == a.row && column == a.column);
        }

        /// <summary>
        /// Returns whether two vectors a and b are equal
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Whether a and b are equal</returns>
        public static bool Equal(Vector2 a, Vector2 b)
        {
            return a.row == b.row && a.column == b.column;
        }

        /// <summary>
        /// Computes the manhattan distance between two Vectors a and b.
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The manhattan distance between a and b</returns>
        public static int ManhattanDistace(Vector2 a, Vector2 b)
        {
            Debug.Assert(a != null);
            Debug.Assert(b != null);

            return Math.Abs(a.row - b.row) + Math.Abs(a.column - b.column);
        }

        public int Row { get { return row; } }
        public int Column { get { return column; } }
    }
}
