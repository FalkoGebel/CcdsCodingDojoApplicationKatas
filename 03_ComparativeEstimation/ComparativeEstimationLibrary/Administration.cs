using Newtonsoft.Json;
using System.Net.Mail;

namespace ComparativeEstimationLibrary
{
    public class Administration
    {
        private readonly string _fileName = "compest.data";
        private readonly List<Project> _projects = [];
        private string _currentUserAsEmail = string.Empty;
        private Project _currentProject = new();
        private char CurrentItem1ToRank { get; set; } = ' ';
        private char CurrentItem2ToRank { get; set; } = ' ';
        private int CurrentItem1Index { get; set; }
        private List<List<char>> RankedItemIds { get; set; } = [];

        public string CurrentUserAsEmail
        {
            get
            {
                return _currentUserAsEmail;
            }

            private set
            {
                ValidateEmail(value);
                _currentUserAsEmail = value;
            }
        }

        public Administration(string currentUserAsEmail)
        {
            CurrentUserAsEmail = currentUserAsEmail;
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is missing");

            try
            {
                MailAddress m = new(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email");
            }
        }

        public void CreateNewProject() => _currentProject = new Project();

        public void AddTitleToProject(string title) => _currentProject.Title = title;

        public string GetCurrentProjectTitle() => _currentProject.Title;

        public char GetNextItemIdForProject() => _currentProject.GetNextItemId();

        public void AddItemToProject(char itemId, string itemDescription)
            => _currentProject.AddItem(itemId, itemDescription);

        public int GetNumberOfItemsForProject() => _currentProject.Items.Count;

        public bool ProjectIsValid() => _currentProject.Items.Count >= 2;

        public void SaveProject()
        {
            LoadProjectsFromFile();

            _currentProject.Id = _projects.Count + 1;
            _projects.Add(_currentProject);

            SaveProjectsToFile();
        }

        private void SaveProjectsToFile()
        {
            List<ProjectModel> projectModels = [];

            foreach (Project project in _projects)
            {
                ProjectModel projectModel = new()
                {
                    Id = project.Id,
                    Title = project.Title,
                };

                foreach (Item item in project.Items)
                {
                    projectModel.Items.Add(new()
                    {
                        Id = item.Id,
                        Description = item.Description
                    });
                }

                foreach (string userAsEmail in project.UsersRankedItemIds.Keys)
                    projectModel.UsersRankedItemIds[userAsEmail] = project.UsersRankedItemIds[userAsEmail];

                projectModels.Add(projectModel);
            }

            WriteToJsonFile(_fileName, projectModels);
        }

        private void LoadProjectsFromFile()
        {
            List<ProjectModel> projectModels = ReadFromJsonFile<List<ProjectModel>>(_fileName);

            _projects.Clear();

            if (projectModels != null)
            {
                foreach (ProjectModel projectModel in projectModels)
                {
                    Project project = new()
                    {
                        Id = projectModel.Id,
                        Title = projectModel.Title,
                    };

                    foreach (ItemModel item in projectModel.Items)
                    {
                        project.Items.Add(new(item.Id)
                        {
                            Description = item.Description
                        });
                    }

                    foreach (string userAsEmail in projectModel.UsersRankedItemIds.Keys)
                        project.UsersRankedItemIds[userAsEmail] = projectModel.UsersRankedItemIds[userAsEmail];

                    _projects.Add(project);
                }
            }
        }

        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite) where T : new()
        {
            TextWriter? writer = null;

            try
            {
                FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(fs);
                writer.Write(contentsToWriteToFile);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"File error - {e.Message}");
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader? reader = null;

            try
            {
                FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                reader = new StreamReader(fs);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents) ?? new();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"File error - {e.Message}");
            }
            finally
            {
                reader?.Close();
            }
        }

        public int GetMaxNumberOfComparisonsForProject()
            => Enumerable.Range(1, _currentProject.Items.Count - 1).Sum();

        public IEnumerable<string> GetProjects()
        {
            LoadProjectsFromFile();
            return _projects.Select(p => $"{p.Id}. {(p.Title == string.Empty ? "<UNTITLED>" : p.Title)}");
        }

        public Comparision? GetNextComparision()
        {
            if (_currentProject.Items.Count == 0)
                return null;

            if (RankedItemIds.Count == 0)
            {
                CurrentItem1ToRank = _currentProject.Items[0].Id;
                CurrentItem2ToRank = (char)(CurrentItem1ToRank + 1);
                RankedItemIds.Add([]);
                RankedItemIds.Add(_currentProject.Items.Select(i => i.Id).ToList());
                RankedItemIds.Add([]);
                CurrentItem1Index = 1;
            }

            while (true)
            {
                if (CurrentItem2ToRank > _currentProject.Items[^1].Id)
                {
                    CurrentItem1ToRank++;

                    if (CurrentItem1ToRank >= _currentProject.Items[^1].Id)
                        break;

                    CurrentItem1Index = RankedItemIds.Select((l, i) => l.Contains(CurrentItem1ToRank) ? i : -1)
                                                                                     .Where(idx => idx >= 0)
                                                                                     .First();
                    RankedItemIds.Insert(CurrentItem1Index, []);
                    CurrentItem1Index++;
                    RankedItemIds.Insert(CurrentItem1Index + 1, []);
                }

                if (RankedItemIds[CurrentItem1Index].Count <= 1)
                {
                    CurrentItem2ToRank = (char)(_currentProject.Items[^1].Id + 1);
                    continue;
                }

                CurrentItem2ToRank = RankedItemIds[CurrentItem1Index][1];
                RankedItemIds[CurrentItem1Index].RemoveAt(1);

                Item item1 = _currentProject.Items.Where(i => i.Id == CurrentItem1ToRank).First(),
                     item2 = _currentProject.Items.Where(i => i.Id == CurrentItem2ToRank).First();

                return new(item1, item2);
            }

            return null;
        }

        public void AddItemRanking(Comparision comparision, char preferedItemId)
        {
            if (preferedItemId == comparision.Item1.Id)
                RankedItemIds[CurrentItem1Index + 1].Add(comparision.Item2.Id);
            else
                RankedItemIds[CurrentItem1Index - 1].Add(comparision.Item2.Id);
        }

        public void SetCurrentProject(string? projectNumberInput)
        {
            if (string.IsNullOrEmpty(projectNumberInput))
                throw new ArgumentException("No project number given");

            int projectNumber;
            try
            {
                projectNumber = int.Parse(projectNumberInput);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Project number is not a valid integer");
            }

            LoadProjectsFromFile();
            _currentProject = _projects.Where(p => p.Id == projectNumber).FirstOrDefault() ??
                throw new ArgumentException($"There is no project with number {projectNumber}");
        }

        public void SaveCurrentItemRanking()
        {
            LoadProjectsFromFile();

            Project project = _projects.First(p => p.Id == _currentProject.Id);
            project.AddComparation(CurrentUserAsEmail, RankedItemIds.SelectMany(id => id).ToList());
            RankedItemIds = [];

            SaveProjectsToFile();
        }

        public IEnumerable<Item> GetRankedItemsForCurrentProject()
        {
            Project project = _projects.First(p => p.Id == _currentProject.Id);
            return project.UsersRankedItemIds[_currentUserAsEmail].Select(id => project.Items.First(i => i.Id == id));
        }

        public IEnumerable<Item> GetTotalRankedItemsForCurrentProject()
        {
            Project project = _projects.First(p => p.Id == _currentProject.Id);
            return project.Items.OrderBy(i => project.UsersRankedItemIds.Values.Select(ids => ids.Select((id, idx) => id == i.Id ? idx : -1)
                                                                                                 .Where(idx => idx >= 0)
                                                                                                 .First())
                                                                               .Sum());
        }
    }
}