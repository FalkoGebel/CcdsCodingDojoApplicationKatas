using ComparativeEstimationLibrary;

namespace ComparativeEstimationTests
{
    [TestClass]
    public class AdministrationTests
    {
        [DataTestMethod]
        [DataRow("")]
        [DataRow("invalid email")]
        [DataRow("de.de")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_invalid_email_and_exception(string email)
        {
            Administration sut = new(email);
        }
    }
}