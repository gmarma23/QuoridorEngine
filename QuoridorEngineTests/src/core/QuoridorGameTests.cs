﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuoridorEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void QuoridorGameConstructorTest(int size)
        {
            QuoridorGame game = new QuoridorGame(size);
            Assert.IsNotNull(game);
        }

        [TestMethod()]
        [DataRow(2)]
        [DataRow(1)]
        [DataRow(4)]
        [DataRow(-4)]
        [DataRow(6)]
        [DataRow(50)]
        public void QuoridorGameConstructorFailTest(int size)
        {
            QuoridorGame game = new QuoridorGame(size);
            String? testStr = null;
            Assert.ThrowsException<ArgumentException>(() => testStr.ToUpper());
        }
    }
}