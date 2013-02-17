using System.Data;
using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    public class DataQueryBuilderTest
    {
        [Test]
        public void ShouldBuildTextQuery()
        {
            //Given
            var dataQueryBuilder = new DataQueryBuilder();
            
            //When
            var dataQuery = dataQueryBuilder.WithCommandText("command text").WithParam("Title", "Mr").WithParam("Name", "David").BuildTextQuery();

            //Then
            Assert.That(dataQuery.CommandText, Is.EqualTo("command text"));
            Assert.That(dataQuery.CommandType, Is.EqualTo(CommandType.Text));
            Assert.That(dataQuery.Parameters["Title"], Is.EqualTo("Mr"));
            Assert.That(dataQuery.Parameters["Name"], Is.EqualTo("David"));
        }

        [Test]
        public void ShouldBuildStoredProcedureQuery()
        {
            //Given
            var dataQueryBuilder = new DataQueryBuilder();

            //When
            var dataQuery = dataQueryBuilder.WithCommandText("command text").WithParam("Title", "Mr").WithParam("Name", "David").BuildStoredQuery();

            //Then
            Assert.That(dataQuery.CommandText, Is.EqualTo("command text"));
            Assert.That(dataQuery.CommandType, Is.EqualTo(CommandType.StoredProcedure));
            Assert.That(dataQuery.Parameters["Title"], Is.EqualTo("Mr"));
            Assert.That(dataQuery.Parameters["Name"], Is.EqualTo("David"));
        }
    }
}