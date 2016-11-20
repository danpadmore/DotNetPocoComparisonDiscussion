using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetPocoComparisonDiscussion
{
    [TestClass]
    public class IsEqualSpecifications
    {
        protected PlainOldClrOjbect Original { get; private set; }
        protected PlainOldClrOjbect New { get; private set; }

        [TestInitialize]
        public void Arrange()
        {
            Original = new PlainOldClrOjbect { Id = Guid.NewGuid(), Name = "test-name" };
            New = new PlainOldClrOjbect { Id = Original.Id, Name = Original.Name, NewlyAddedProperty = "new-property" };
        }

        [TestMethod]
        public void GivenIsEqualByManualComparison_ThenResultShouldBeFalse()
        {
            var result = Original.IsEqualByManualComparison(New);

            Assert.IsFalse(result, "Fails as expected because developer 'forgot' to take newly added property into account");
        }

        [TestMethod]
        public void GivenIsEqualBySerializedStringComparison_ThenResultShouldBeFalse()
        {
            var result = Original.IsEqualBySerializedStringComparison(New);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GivenIsEqualByReflection_ThenResultShouldBeFalse()
        {
            var result = Original.IsEqualByReflection(New);

            Assert.IsFalse(result);
        }
    }
}
