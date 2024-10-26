using ComparativeEstimationLibrary;
using FluentAssertions;

namespace ComparativeEstimationTests
{
    [TestClass]
    public class AdministrationTests
    {
        private readonly string _email = "peter@peter.com";

        [DataTestMethod]
        [DataRow("")]
        [DataRow("invalid email")]
        [DataRow("de.de")]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_invalid_email_and_exception(string email)
        {
            Administration sut = new(email);
        }

        [TestMethod]
        public void Test_get_next_project_item_id_and_get_A_as_returned_id()
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();

            // Act
            char itemId = sut.GetNextItemIdForProject();

            // Assert
            itemId.Should().Be('A');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_add_empty_item_and_get_exception_and_zero_number_of_items()
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();
            char itemId = sut.GetNextItemIdForProject();

            // Act
            sut.AddItemToProject(itemId, "");

            // Assert
            sut.GetNumberOfItemsForProject().Should().Be(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_add_item_with_invalid_item_id_and_get_exception_and_zero_number_of_items()
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();
            char itemId = sut.GetNextItemIdForProject();

            // Act
            sut.AddItemToProject(' ', "description");

            // Assert
            sut.GetNumberOfItemsForProject().Should().Be(0);
        }

        [TestMethod]
        public void Test_add_item_and_get_1_as_number_of_items()
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();
            char itemId = sut.GetNextItemIdForProject();

            // Act
            sut.AddItemToProject(itemId, "Item Description");

            // Assert
            sut.GetNumberOfItemsForProject().Should().Be(1);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void Test_project_is_valid_with_invalid_project(int numberOfItems)
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();

            for (int i = 0; i < numberOfItems; i++)
            {
                char itemId = sut.GetNextItemIdForProject();
                sut.AddItemToProject(itemId, "Item Description");
            }

            // Act
            bool isValid = sut.ProjectIsValid();

            // Assert
            isValid.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(2)]
        [DataRow(8)]
        [DataRow(10)]
        public void Test_project_is_valid_with_valid_project(int numberOfItems)
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();

            for (int i = 0; i < numberOfItems; i++)
            {
                char itemId = sut.GetNextItemIdForProject();
                sut.AddItemToProject(itemId, "Item Description");
            }

            // Act
            bool isValid = sut.ProjectIsValid();

            // Assert
            isValid.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(2, 1)]
        [DataRow(8, 28)]
        [DataRow(10, 45)]
        public void Test_max_number_of_comparisions_for_given_number_of_items(int numberOfItems, int expected)
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();

            for (int i = 0; i < numberOfItems; i++)
            {
                char itemId = sut.GetNextItemIdForProject();
                sut.AddItemToProject(itemId, "Item Description");
            }

            // Act
            int maxNumberOfComparisons = sut.GetMaxNumberOfComparisonsForProject();

            // Assert
            maxNumberOfComparisons.Should().Be(expected);
        }

        [TestMethod]
        public void Test_get_next_comparision()
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();

            for (int i = 0; i < 5; i++)
            {
                char itemId = sut.GetNextItemIdForProject();
                sut.AddItemToProject(itemId, $"Item {itemId}");
            }

            // Act
            Comparision? comparision = sut.GetNextComparision();

            // Assert
            comparision.Should().NotBeNull();
            if (comparision != null)
            {
                comparision.Item1.Output.Should().Be($"A: \"Item A\"");
                comparision.Item2.Output.Should().Be($"B: \"Item B\"");
            }

            // Act
            comparision = sut.GetNextComparision();

            // Assert
            comparision.Should().NotBeNull();
            if (comparision != null)
            {
                comparision.Item1.Output.Should().Be($"A: \"Item A\"");
                comparision.Item2.Output.Should().Be($"C: \"Item C\"");
            }
        }
    }
}