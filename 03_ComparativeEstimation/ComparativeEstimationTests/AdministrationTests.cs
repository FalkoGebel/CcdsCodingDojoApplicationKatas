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
        public void Test_new_project_and_get_1_as_returned_id()
        {
            // Arrange
            Administration sut = new(_email);

            // Act
            int newProjectId = sut.CreateNewProject();

            // Assert
            newProjectId.Should().Be(1);
        }

        [TestMethod]
        public void Test_new_project_two_times_and_get_correct_ids()
        {
            // Arrange
            Administration sut = new(_email);
            sut.CreateNewProject();

            // Act
            int newProjectId = sut.CreateNewProject();

            // Assert
            newProjectId.Should().Be(2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_add_title_with_invalid_project_id_and_get_argument_exception()
        {
            // Arrange
            Administration sut = new(_email);

            // Act
            sut.AddTitleToProject(1, "");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow("Sprint 11")]
        [DataRow("Refactoring")]
        [DataRow("just ANOTHER title")]
        public void Test_add_title_and_get_correct_project_string(string title)
        {
            // Arrange
            Administration sut = new(_email);
            int newProjectId = sut.CreateNewProject();

            // Act
            sut.AddTitleToProject(newProjectId, title);

            // Assert
            sut.GetProjectString(newProjectId).Should().Be($"{newProjectId}. {(title == string.Empty ? "<UNTITLED>" : title)}");
        }

        [TestMethod]
        public void Test_get_next_project_item_id_and_get_A_as_returned_id()
        {
            // Arrange
            Administration sut = new(_email);
            int newProjectId = sut.CreateNewProject();

            // Act
            char itemId = sut.GetNextProjectItemId(newProjectId);

            // Assert
            itemId.Should().Be('A');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_add_empty_item_and_get_exception_and_zero_number_of_items()
        {
            // Arrange
            Administration sut = new(_email);
            int newProjectId = sut.CreateNewProject();
            char itemId = sut.GetNextProjectItemId(newProjectId);

            // Act
            sut.AddItemToProject(newProjectId, itemId, "");

            // Assert
            sut.GetNumberOfItemsForProject(newProjectId).Should().Be(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_add_item_with_invalid_item_id_and_get_exception_and_zero_number_of_items()
        {
            // Arrange
            Administration sut = new(_email);
            int newProjectId = sut.CreateNewProject();
            char itemId = sut.GetNextProjectItemId(newProjectId);

            // Act
            sut.AddItemToProject(newProjectId, ' ', "description");

            // Assert
            sut.GetNumberOfItemsForProject(newProjectId).Should().Be(0);
        }

        [TestMethod]
        public void Test_add_item_and_get_1_as_number_of_items()
        {
            // Arrange
            Administration sut = new(_email);
            int newProjectId = sut.CreateNewProject();
            char itemId = sut.GetNextProjectItemId(newProjectId);

            // Act
            sut.AddItemToProject(newProjectId, itemId, "Item Description");

            // Assert
            sut.GetNumberOfItemsForProject(newProjectId).Should().Be(1);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void Test_project_is_valid_with_invalid_project(int numberOfItems)
        {
            // Arrange
            Administration sut = new(_email);
            int newProjectId = sut.CreateNewProject();

            for (int i = 0; i < numberOfItems; i++)
            {
                char itemId = sut.GetNextProjectItemId(newProjectId);
                sut.AddItemToProject(newProjectId, itemId, "Item Description");
            }

            // Act
            bool isValid = sut.ProjectIsValid(newProjectId);

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
            int newProjectId = sut.CreateNewProject();

            for (int i = 0; i < numberOfItems; i++)
            {
                char itemId = sut.GetNextProjectItemId(newProjectId);
                sut.AddItemToProject(newProjectId, itemId, "Item Description");
            }

            // Act
            bool isValid = sut.ProjectIsValid(newProjectId);

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
            int newProjectId = sut.CreateNewProject();

            for (int i = 0; i < numberOfItems; i++)
            {
                char itemId = sut.GetNextProjectItemId(newProjectId);
                sut.AddItemToProject(newProjectId, itemId, "Item Description");
            }

            // Act
            int maxNumberOfComparisons = sut.GetMaxNumberOfComparisonsForProject(newProjectId);

            // Assert
            maxNumberOfComparisons.Should().Be(expected);
        }
    }
}