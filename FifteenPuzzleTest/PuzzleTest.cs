using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FifteenPuzzleLib;
using System.Collections.Generic;

namespace FifteenPuzzleTest
{
    public abstract class PuzzleTest
    {
        private IPuzzle mPuzzle;
        private const int WIDTH = 5, HEIGHT = 4;
        private List<Tuple<int, int, int>> mUpdates;

        protected abstract IPuzzle CreatePuzzle();

        private bool UpdateContains(int row, int col, int value)
        {
            return mUpdates.Contains(Tuple.Create(row, col, value));
        }

        [TestInitialize]
        public void SetUp()
        {
            mUpdates = new List<Tuple<int, int, int>>();
            mPuzzle = CreatePuzzle();
            mPuzzle.Init(WIDTH, HEIGHT);
            mPuzzle.CellChanged += mPuzzle_CellChanged;
        }

        [TestCleanup]
        public void CleanUp()
        {
            mPuzzle.CellChanged -= mPuzzle_CellChanged;
        }

        private void mPuzzle_CellChanged(IPuzzle puzzle, int row, int col, int value)
        {
            mUpdates.Add(Tuple.Create(row, col, value));
        }

        [TestMethod]
        public void TestBasicAttributes()
        {
            Assert.AreEqual(WIDTH, mPuzzle.Width);
            Assert.AreEqual(HEIGHT, mPuzzle.Height);
            Assert.AreEqual(WIDTH * HEIGHT, mPuzzle.Cells.Count());
            Assert.IsTrue(mPuzzle.IsSolved);
        }

        [TestMethod]
        public void TestMoveDown()
        {
            int[] vals = Enumerable.Range(0, HEIGHT).Select(i => mPuzzle[i, 0]).ToArray();

            mPuzzle.Move(Direction.Down);
            Assert.AreEqual(vals[1], mPuzzle[0, 0]);
            Assert.AreEqual(vals[0], mPuzzle[1, 0]);
            Assert.IsFalse(mPuzzle.IsSolved);

            mPuzzle.Move(Direction.Down);
            Assert.AreEqual(vals[1], mPuzzle[0, 0]);
            Assert.AreEqual(vals[2], mPuzzle[1, 0]);
            Assert.AreEqual(vals[0], mPuzzle[2, 0]);
            Assert.IsFalse(mPuzzle.IsSolved);

            mPuzzle.Move(Direction.Down);
            Assert.AreEqual(vals[3], mPuzzle[2, 0]);
            Assert.AreEqual(vals[0], mPuzzle[3, 0]);
            Assert.IsFalse(mPuzzle.IsSolved);

            mPuzzle.Move(Direction.Down);
            Assert.AreEqual(vals[3], mPuzzle[2, 0]);
            Assert.AreEqual(vals[0], mPuzzle[3, 0]);
            Assert.IsFalse(mPuzzle.IsSolved);
        }

        [TestMethod]
        public void TestEvent()
        {
            // Cannot move up anymore
            mPuzzle.Move(Direction.Up);
            Assert.AreEqual(0, mUpdates.Count);

            // Move down
            // 5 1 2 3 4
            // 0 6 7 8 9
            mPuzzle.Move(Direction.Down);
            Assert.AreEqual(2, mUpdates.Count);
            Assert.IsTrue(UpdateContains(0, 0, WIDTH));
            Assert.IsTrue(UpdateContains(1, 0, 0));

            // Move right
            // 5 1 2 3 4
            // 6 0 7 8 9
            mUpdates.Clear();
            mPuzzle.Move(Direction.Right);
            Assert.AreEqual(2, mUpdates.Count);
            Assert.IsTrue(UpdateContains(1, 1, 0));
            Assert.IsTrue(UpdateContains(1, 0, WIDTH + 1));

            // Move up
            // 5 0 2 3 4
            // 6 1 7 8 9
            mUpdates.Clear();
            mPuzzle.Move(Direction.Up);
            Assert.AreEqual(2, mUpdates.Count);
            Assert.IsTrue(UpdateContains(0, 1, 0));
            Assert.IsTrue(UpdateContains(1, 1, 1));

            // Move left
            // 0 5 2 3 4
            // 6 1 7 8 9
            mUpdates.Clear();
            mPuzzle.Move(Direction.Left);
            Assert.AreEqual(2, mUpdates.Count);
            Assert.IsTrue(UpdateContains(0, 0, 0));
            Assert.IsTrue(UpdateContains(0, 1, WIDTH));
        }
    }

    [TestClass]
    public class DefaultPuzzleTest : PuzzleTest
    {
        protected override IPuzzle CreatePuzzle()
        {
            return new DefaultPuzzle();
        }
    }

}
