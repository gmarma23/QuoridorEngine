﻿namespace QuoridorEngine.Solver
{
    /// <summary>
    /// Common class to represent a move in any given game.
    /// Any implementation of a particular game needs to extend
    /// (inherit from) this class to provide its particular implementation
    /// details related to the given game.
    /// </summary>
    public abstract class Move 
    {
        public abstract bool IsEqual(Move move);
    }
}