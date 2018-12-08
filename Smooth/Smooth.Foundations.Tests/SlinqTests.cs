using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Smooth.Algebraics;
using Smooth.Algebraics.Extensions;
using Smooth.Slinq;
using Smooth.Tests.Utils;

namespace Smooth.Tests
{
    [TestFixture]
    [TestFixtureSource(nameof(TestData))]
    public class Tests
    {
        private const int REPEATS = 100;
        private const int MIN_COUNT = 50;
        private const int MAX_COUNT = 100;
        private const int MIN_VALUE = 1;
        private const int MAX_VALUE = 100;

        private static readonly Func<ValueTuple<int, int>, (int, int), bool> Eq = (a, b) => a.ItemsEquals(b);
        private static readonly Func<(int, int, int), (int, int, int), bool> EqT3 = (a, b) => a.ItemsEquals(b);
        private static readonly Func<(int, int), int> To1 = t => t.Item1;
        private static readonly Func<(int, int), int> To_1F = t => t.Item1;
        private static readonly IEqualityComparer<(int, int)> Eq1 = new Equals_1<int, int>();

        private readonly List<(int, int)> _tuples1 = new List<(int, int)>();
        private readonly List<(int, int)> _tuples2 = new List<(int, int)>();
        private readonly (int, int) _testTuple;
        private readonly int _testInt;
        private readonly int _testInt2;
        private readonly int _midSkip;

        public Tests() : this(1)
        {
        }

        public Tests(int counter)
        {
            var random = new Random(counter);
            var count = random.Next(MIN_COUNT, MAX_COUNT + 1);
            for (var i = 0; i < count; ++i)
            {
                _tuples1.Add(new ValueTuple<int, int>(random.Next(MIN_VALUE, MAX_VALUE + 1), i));
                _tuples2.Add(new ValueTuple<int, int>(random.Next(MIN_VALUE, MAX_VALUE + 1), i));
            }

            _testTuple = _tuples2.Slinq().FirstOrDefault();
            _testInt = _testTuple.Item1;
            _testInt2 = _testInt * (MAX_VALUE - MIN_VALUE + 1) / 25;
            _midSkip = random.Next() < 0.5f ? _testInt : 0;
        }

        public static IEnumerable<int> TestData => Enumerable.Range(0, REPEATS);

        [Test]
        public void TestAggregate()
        {
            var slinqResult = _tuples1.Slinq().Aggregate(0, (acc, next) => acc + next.Item1);
            var linqResult = _tuples1.Aggregate(0, (acc, next) => acc + next.Item1);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAggregateWithResultSelector()
        {
            var slinqResult = _tuples1.Slinq().Aggregate(0, (acc, next) => acc + next.Item1, acc => -acc);
            var linqResult = _tuples1.Aggregate(0, (acc, next) => acc + next.Item1, acc => -acc);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAggregateWhile()
        {
            var slinqResult = _tuples1.Slinq()
                .AggregateWhile(0, (acc, next) => acc < _testInt2 ? new Option<int>(acc + next.Item1) : new Option<int>());
            var linqResult = _tuples1.Slinq().AggregateRunning(0, (acc, next) => acc + next.Item1).Where(acc => acc >= _testInt2)
                .FirstOrDefault();
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAll1()
        {
            var slinqResult = _tuples1.Slinq().All(x => x.Item1 < _testInt2);
            var linqResult = _tuples1.All(x => x.Item1 < _testInt2);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAll2()
        {
            var slinqResult = _tuples1.Slinq().All((x, c) => x.Item1 < c, _testInt2);
            var linqResult = _tuples1.All(x => x.Item1 < _testInt2);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAll3()
        {
            var slinqResult = _tuples1.Slinq().Any(x => x.Item1 > _testInt2);
            var linqResult = _tuples1.Any(x => x.Item1 > _testInt2);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAll4()
        {
            var slinqResult = _tuples1.Slinq().Any((x, c) => x.Item1 > c, _testInt2);
            var linqResult = _tuples1.Any(x => x.Item1 > _testInt2);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestAverageOrNone()
        {
            var slinqResult = _tuples1.Slinq().Select(To1).AverageOrNone();
            var linqResult = _tuples1.Count == 0 ? Option<double>.None : _tuples1.Select(To_1F).Average().ToSome();
            Assert.AreEqual(slinqResult.value, linqResult.value);
        }

        [Test]
        public void TestSequenceEqual_Valid()
        {
            var slinqResult = _tuples1.Slinq().SequenceEqual(_tuples1.Slinq());
            Assert.IsTrue(slinqResult);
        }

        [Test]
        public void TestSequenceEqual_NotValid()
        {
            var slinqResult = _tuples1.Slinq().SequenceEqual(_tuples2.Slinq());
            Assert.IsFalse(slinqResult);
        }

        [Test]
        public void TestContains()
        {
            var slinqResult = _tuples1.Slinq().Contains(_testTuple, Eq1);
            var linqResult = _tuples1.Contains(_testTuple, Eq1);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestConcat()
        {
            var slinqResult = _tuples1.Slinq().Concat(_tuples2.Slinq()).ToList();
            var linqResult = _tuples1.Concat(_tuples2);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestCount()
        {
            var slinqResult = _tuples1.Slinq().Where(x => x.Item1 < _testInt).Count();
            var linqResult = _tuples1.Where(x => x.Item1 < _testInt).Count();
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestDistinct()
        {
            var slinqResult = _tuples1.Slinq().Distinct(Eq1).ToList();
            var linqResult = _tuples1.Distinct(Eq1);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestExcept()
        {
            var slinqResult = _tuples1.Slinq().Except(_tuples2.Slinq(), Eq1).Distinct(Eq1).ToList();
            var linqResult = _tuples1.Except(_tuples2, Eq1); // TODO: Except in LINQ returns only unique elements
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestFirstOrNone()
        {
            var slinqResult = _tuples1.Slinq().FirstOrNone();
            var linqResult = !_tuples1.Any() ? Option<(int, int)>.None : _tuples1.First().ToOption();
            Assert.AreEqual(slinqResult.value, linqResult.value);
        }

        [Test]
        public void TestFirstOrNone_Predicate()
        {
            var slinqResult = _tuples1.Slinq().FirstOrNone(x => x.Item1 < _testInt);
            var linqResult = _tuples1.Any(z => z.Item1 < _testInt)
                ? _tuples1.First(z => z.Item1 < _testInt).ToOption()
                : Option<(int, int)>.None;
            Assert.AreEqual(slinqResult.value, linqResult.value);
        }

        [Test]
        public void TestFirstOrNone_Predicate_Parameter()
        {
            var slinqResult = _tuples1.Slinq().FirstOrNone((x, t) => x.Item1 < t, _testInt);
            var linqResult = _tuples1.Any(z => z.Item1 < _testInt)
                ? _tuples1.First(z => z.Item1 < _testInt).ToOption()
                : Option<(int, int)>.None;
            Assert.AreEqual(slinqResult.value, linqResult.value);
        }

        [Test]
        public void TestFlatten()
        {
            var slinqResult = new[] {_tuples1, _tuples2}.Slinq().Select(x => x.Slinq()).Flatten().ToList();
            var linqResult = _tuples1.Concat(_tuples2);
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestForEach()
        {
            var slinqResult = 0;
            _tuples1.Slinq().ForEach(x => slinqResult += x.Item1);
            var linqResult = _tuples1.Select(To_1F).Sum();
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestIntersect()
        {
            var slinqResult = _tuples1.Slinq().Intersect(_tuples2.Slinq(), Eq1).ToList();
            var linqResult = _tuples1.Intersect(_tuples2, Eq1);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestGroupBy()
        {
            var slinqResult = _tuples1.Slinq().GroupBy(To1).Select(g => (g.key, g.values.Count())).ToList();
            var linqResult = _tuples1.GroupBy(To_1F).Select(g => (g.Key, g.Count()));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestGroupJoin()
        {
            var slinqResult = _tuples1.Slinq().GroupJoin(_tuples2.Slinq(), To1, To1, (a, bs) => (a.Item1, a.Item2, bs.Count())).ToList();
            var linqResult = _tuples1.GroupJoin(_tuples2, To_1F, To_1F, (a, bs) => (a.Item1, a.Item2, bs.Count()));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestJoin()
        {
            var slinqResult = _tuples1.Slinq().Join(_tuples2.Slinq(), To1, To1, (a, b) => (a.Item1, a.Item2, b.Item2)).ToList();
            var linqResult = _tuples1.Join(_tuples2, To_1F, To_1F, (a, b) => (a.Item1, a.Item2, b.Item2));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestLastOrNone()
        {
            var slinqResult = _tuples1.Slinq().LastOrNone();
            var linqResult = _tuples1.Any() ? _tuples1.Last().ToSome() : Option<(int, int)>.None;
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestLastOrNone_Predicate()
        {
            var slinqResult = _tuples1.Slinq().LastOrNone(x => x.Item1 < _testInt);
            var linqResult = _tuples1.Any(x => x.Item1 < _testInt)
                ? _tuples1.Last(x => x.Item1 < _testInt).ToSome()
                : Option<(int, int)>.None;
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestMax()
        {
            if (_tuples1.Count == 0)
                return;

            var slinqResult = _tuples1.Slinq().Max();
            var linqResult = _tuples1.Max();
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestMin()
        {
            if (_tuples1.Count == 0)
                return;

            var slinqResult = _tuples1.Slinq().Min();
            var linqResult = _tuples1.Min();
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestMaxOrNone()
        {
            var slinqResult = _tuples1.Slinq().MaxOrNone();
            var linqResult = _tuples1.Any() ? _tuples1.Max().ToSome() : Option<(int, int)>.None;
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestMinOrNone()
        {
            var slinqResult = _tuples1.Slinq().MinOrNone();
            var linqResult = _tuples1.Any() ? _tuples1.Min().ToSome() : Option<(int, int)>.None;
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestOrderBy()
        {
            var slinqResult = _tuples1.Slinq().OrderBy(To1).ToList();
            var linqResult = _tuples1.OrderBy(To_1F);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestOrderByDescending()
        {
            var slinqResult = _tuples1.Slinq().OrderByDescending(To1).ToList();
            var linqResult = _tuples1.OrderByDescending(To_1F);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestOrderBy_Keyless()
        {
            var slinqResult = _tuples1.Slinq().OrderBy().ToList();
            var linqResult = _tuples1.OrderBy(x => x);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestOrderByDescending_Keyless()
        {
            var slinqResult = _tuples1.Slinq().OrderByDescending().ToList();
            var linqResult = _tuples1.OrderByDescending(x => x);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestOrderByGroup()
        {
            var slinqResult = _tuples1.Slinq().OrderByGroup(To1).ToList();
            var linqResult = _tuples1.OrderBy(To_1F);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestOrderByGroupDescending()
        {
            var slinqResult = _tuples1.Slinq().OrderByDescending(To1).ToList();
            var linqResult = _tuples1.OrderByDescending(To_1F);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemove()
        {
            var list = RemovableList();
            var slinq = list.Slinq().Skip(_midSkip);
            for (int i = 0; i < _testInt; ++i)
            {
                slinq.Remove();
            }

            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = _tuples1.Skip(_midSkip).Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemove_Descending()
        {
            var list = RemovableList();
            var slinq = list.SlinqDescending().Skip(_midSkip);
            for (int i = 0; i < _testInt; ++i)
            {
                slinq.Remove();
            }

            var tuples1 = new List<(int, int)>(_tuples1);
            tuples1.Reverse();
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemoveCount()
        {
            var list = RemovableList();
            list.Slinq().Skip(_midSkip).Remove(_testInt).ToList();
            var tuples1 = new List<(int, int)>(_tuples1);
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemoveWhile()
        {
            var list = RemovableList();
            list.Slinq().Skip(_midSkip).RemoveWhile(x => x.Item1 < _testInt2);
            var tuples1 = new List<(int, int)>(_tuples1);
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).SkipWhile(x => x.Item1 < _testInt2);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemoveLinkedList()
        {
            var list = RemovableLinkedList();
            var slinq = list.Slinq().Skip(_midSkip);
            for (int i = 0; i < _testInt; ++i)
            {
                slinq.Remove();
            }

            var tuples1 = new List<(int, int)>(_tuples1);
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemoveLinkedList_Descending()
        {
            var list = RemovableLinkedList();
            var slinq = list.SlinqDescending().Skip(_midSkip);
            for (int i = 0; i < _testInt; ++i)
            {
                slinq.Remove();
            }

            var tuples1 = new List<(int, int)>(_tuples1);
            tuples1.Reverse();
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemoveLinkedListCount()
        {
            var list = RemovableLinkedList();
            list.Slinq().Skip(_midSkip).Remove(_testInt).ToList();
            var tuples1 = new List<(int, int)>(_tuples1);
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestRemoveLinkedListWhile()
        {
            var list = RemovableLinkedList();
            list.Slinq().Skip(_midSkip).RemoveWhile(x => x.Item1 < _testInt2);
            var tuples1 = new List<(int, int)>(_tuples1);
            var slinqResult = list.Slinq().Skip(_midSkip).ToList();
            var linqResult = tuples1.Skip(_midSkip).SkipWhile(x => x.Item1 < _testInt2);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestReverse()
        {
            var slinqResult = _tuples1.Slinq().Reverse().ToList();
            var linqResult = Enumerable.Reverse(_tuples1);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSelect()
        {
            var slinqResult = _tuples1.Slinq().Select(To1).ToList();
            var linqResult = _tuples1.Select(To_1F);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSelectMany()
        {
            var list = new[] {_tuples1, _tuples2};
            var slinqResult = list.Slinq().SelectMany(x => x.Slinq()).ToList();
            var linqResult = list.SelectMany(x => x);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSelectMany_Option()
        {
            var slinqResult = _tuples1.Slinq().SelectMany(x => x.Item1 < _testInt ? new Option<int>(x.Item1) : new Option<int>()).ToList();
            var linqResult = _tuples1.Where(x => x.Item1 < _testInt).Select(x => x.Item1);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSequenceEqual1()
        {
            var isEqual = _tuples1.Slinq().SequenceEqual(_tuples1.Slinq(), Eq);
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void TestSequenceEqual2()
        {
            var slinqResult = _tuples1.Slinq().Take(5).SequenceEqual(_tuples2.Slinq().Take(5), Eq);
            var linqResult = _tuples1.Take(5).SequenceEqual(_tuples2.Take(5));
            Assert.AreEqual(slinqResult, linqResult);
        }

        [Test]
        public void TestSingleOrNone()
        {
            var a = new int[3];
            a[2] = _testInt;
            Assert.IsTrue(a.Slinq().SingleOrNone().isNone);
            Assert.IsTrue(a.Slinq().Skip(1).SingleOrNone().isNone);
            Assert.IsTrue(a.Slinq().Skip(2).SingleOrNone().Contains(_testInt));
            Assert.IsTrue(a.Slinq().Skip(3).SingleOrNone().isNone);
        }

        [Test]
        public void TestSkip()
        {
            var slinq = _tuples1.Slinq();
            for (int i = 0; i < _testInt; ++i)
            {
                slinq.Skip();
            }

            var slinqResult = slinq.ToList();
            var linqResult = _tuples1.Skip(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSkipWhile()
        {
            var slinqResult = _tuples1.Slinq().SkipWhile(x => x.Item1 < _testInt2).ToList();
            var linqResult = _tuples1.SkipWhile(x => x.Item1 < _testInt2);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSlinqDescending_List()
        {
            var tuples1 = new List<(int, int)>(_tuples1);
            tuples1.Reverse();
            var slinqResult = _tuples1.SlinqDescending().ToList();
            var linqResult = tuples1;
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSlinqDescending_LinkedList()
        {
            var tuples1 = new List<(int, int)>(_tuples1);
            tuples1.Reverse();
            var slinqResult = RemovableLinkedList().SlinqDescending().ToList();
            var linqResult = tuples1;
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSlinq_LinkedListNodes()
        {
            var slinqResult = RemovableLinkedList().SlinqNodes().Select(x => x.Value).ToList();
            var linqResult = _tuples1;
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSlinqDescending_LinkedListNodes()
        {
            var tuples1 = new List<(int, int)>(_tuples1);
            tuples1.Reverse();
            var slinqResult = RemovableLinkedList().SlinqNodesDescending().Select(x => x.Value).ToList();
            var linqResult = tuples1;
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSlinqWithIndex()
        {
            var slinqResult = _tuples1.SlinqWithIndex().Select(x => x.Item1).ToList();
            var linqResult = _tuples1;
            Assert.IsTrue(_tuples1.SlinqWithIndex().All(x => x.Item1.Item2 == x.Item2));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSlinqWithIndexDescending()
        {
            var tuples1 = new List<(int, int)>(_tuples1);
            tuples1.Reverse();
            var slinqResult = _tuples1.SlinqWithIndexDescending().Select(x => x.Item1).ToList();
            var linqResult = tuples1;
            Assert.IsTrue(_tuples1.SlinqWithIndexDescending().All(x => x.Item1.Item2 == x.Item2));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestSum()
        {
            var slinqResult = _tuples1.Slinq().Select(To1).Sum();
            var linqResult = _tuples1.Select(To_1F).Sum();
            Assert.AreEqual(linqResult, slinqResult);
        }

        [Test]
        public void TestTake()
        {
            var slinqResult = _tuples1.Slinq().Take(_testInt).ToList();
            var linqResult = _tuples1.Take(_testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestTakeWhile()
        {
            var slinqResult = _tuples1.Slinq().TakeWhile(x => x.Item1 < _testInt2).ToList();
            var linqResult = _tuples1.TakeWhile(x => x.Item1 < _testInt2);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestTakeWhile_WithParam()
        {
            var slinqResult = _tuples1.Slinq().TakeWhile((x, c) => x.Item1 < c, _testInt2).ToList();
            var linqResult = _tuples1.TakeWhile(x => x.Item1 < _testInt2);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestUnion()
        {
            var slinqResult = _tuples1.Slinq().Union(_tuples2.Slinq(), Eq1).ToList();
            var linqResult = _tuples1.Union(_tuples2, Eq1);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestWhere()
        {
            var slinqResult = _tuples1.Slinq().Where(x => x.Item1 < _testInt).ToList();
            var linqResult = _tuples1.Where(x => x.Item1 < _testInt);
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipTuples()
        {
            var slinqResult = _tuples1.Slinq().Zip(_tuples2.Slinq()).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count, _tuples2.Count)).Select(x => (_tuples1[x], _tuples2[x]));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZip()
        {
            var slinqResult = _tuples1.Slinq().Zip(_tuples2.Slinq(), (a, b) => (a.Item1, b.Item1)).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count, _tuples2.Count))
                .Select(x => (_tuples1[x].Item1, _tuples2[x].Item1));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipAll()
        {
            var slinqResult = _tuples1.Slinq().ZipAll(_tuples2.Slinq()).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count, _tuples2.Count))
                .Select(x => (
                    x < _tuples1.Count ? new Option<(int, int)>(_tuples1[x]) : Option<(int, int)>.None,
                    x < _tuples2.Count ? new Option<(int, int)>(_tuples2[x]) : Option<(int, int)>.None));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipAll_Left()
        {
            var slinqResult = _tuples1.Slinq().Skip(_midSkip).ZipAll(_tuples2.Slinq()).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count + _midSkip, _tuples2.Count))
                .Select(x => (
                    x + _midSkip < _tuples1.Count ? new Option<(int, int)>(_tuples1[x + _midSkip]) : Option<(int, int)>.None,
                    x < _tuples2.Count ? new Option<(int, int)>(_tuples2[x]) : Option<(int, int)>.None));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipAll_Right()
        {
            var slinqResult = _tuples1.Slinq().ZipAll(_tuples2.Slinq().Skip(_midSkip)).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count, _tuples2.Count + _midSkip))
                .Select(x => (
                    x < _tuples1.Count ? new Option<(int, int)>(_tuples1[x]) : Option<(int, int)>.None,
                    x + _midSkip < _tuples2.Count ? new Option<(int, int)>(_tuples2[x + _midSkip]) : Option<(int, int)>.None));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipAll_WithSelector()
        {
            var slinqResult = _tuples1.Slinq().ZipAll(_tuples2.Slinq(), (a, b) => (a, b)).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count, _tuples2.Count))
                .Select(x => (
                    x < _tuples1.Count ? new Option<(int, int)>(_tuples1[x]) : Option<(int, int)>.None,
                    x < _tuples2.Count ? new Option<(int, int)>(_tuples2[x]) : Option<(int, int)>.None));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipAll_LeftWithSelector()
        {
            var slinqResult = _tuples1.Slinq().Skip(_midSkip).ZipAll(_tuples2.Slinq(), (a, b) => (a, b)).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count + _midSkip, _tuples2.Count))
                .Select(x => (
                    x + _midSkip < _tuples1.Count ? new Option<(int, int)>(_tuples1[x + _midSkip]) : Option<(int, int)>.None,
                    x < _tuples2.Count ? new Option<(int, int)>(_tuples2[x]) : Option<(int, int)>.None));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipAll_RightWithSelector()
        {
            var slinqResult = _tuples1.Slinq().ZipAll(_tuples2.Slinq().Skip(_midSkip), (a, b) => (a, b)).ToList();
            var linqResult = Enumerable.Range(0, Math.Min(_tuples1.Count, _tuples2.Count + _midSkip))
                .Select(x => (
                    x < _tuples1.Count ? new Option<(int, int)>(_tuples1[x]) : Option<(int, int)>.None,
                    x + _midSkip < _tuples2.Count ? new Option<(int, int)>(_tuples2[x + _midSkip]) : Option<(int, int)>.None));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        [Test]
        public void TestZipWithIndex()
        {
            var slinqResult = _tuples1.Slinq().ZipWithIndex().Select(x => x.Item1).ToList();
            var linqResult = _tuples1;
            Assert.IsTrue(_tuples1.Slinq().ZipWithIndex().All(x => x.Item1.Item2 == x.Item2));
            Assert.That(slinqResult, Is.EquivalentTo(linqResult));
        }

        private void T()
        {
            
        }


        private List<(int, int)> RemovableList()
        {
            return new List<(int, int)>(_tuples1);
        }

        private LinkedList<(int, int)> RemovableLinkedList()
        {
            return new LinkedList<(int, int)>(_tuples1);
        }
    }
}