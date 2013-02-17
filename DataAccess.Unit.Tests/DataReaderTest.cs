using NUnit.Framework;

namespace DataAccess.Unit.Tests
{
    [TestFixture]
    public class DataReaderTest
    {
        [Test]
        public void ShouldRollbackATransactionWhenAnExceptionOccurs()
        {
            //Given
            var dataReader = new DataReader();

            //When
            dataReader.Read();

            //Then

        }
    }

    public class DataReader
    {

        public void Read()
        {
            
        }
    }
}