using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StepFlow.Core;

namespace StepFlow.Tests;

[TestClass]
public class PropertyAssignerTest
{
    [TestMethod]
    public void AssignPropertyFromSourceToTarget()
    {
        DataA dataA = new("Some value", 1);
        DataB dataB = new("Other value", 42);

        Expression<Func<DataA, string>> sourceFunc1 = x => x.PropertyA1;
        Expression<Func<DataB, string>> targetFunc1 = x => x.PropertyB1;
        PropertyAssigner propertyAssigner1 = new(sourceFunc1, targetFunc1);

        Expression<Func<DataB, int>> sourceFunc2 = x => x.PropertyB2;
        Expression<Func<DataA, int>> targetFunc2 = x => x.PropertyA2;
        PropertyAssigner propertyAssigner2 = new(sourceFunc2, targetFunc2);

        propertyAssigner1.Assign(dataA, dataB);
        propertyAssigner2.Assign(dataB, dataA);

        Assert.AreEqual("Some value", dataA.PropertyA1);
        Assert.AreEqual("Some value", dataB.PropertyB1);
        Assert.AreEqual(42, dataA.PropertyA2);
        Assert.AreEqual(42, dataB.PropertyB2);
    }

    private class DataA
    {
        public DataA(string propertyA1, int propertyA2)
        {
            PropertyA1 = propertyA1;
            PropertyA2 = propertyA2;
        }

        public string PropertyA1 { get; set; }

        public int PropertyA2 { get; set; }
    }

    private class DataB
    {
        public DataB(string propertyB1, int propertyB2)
        {
            PropertyB1 = propertyB1;
            PropertyB2 = propertyB2;
        }

        public string PropertyB1 { get; set; }

        public int PropertyB2 { get; set; }
    }
}