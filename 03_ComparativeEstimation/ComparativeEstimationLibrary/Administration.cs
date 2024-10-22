using Newtonsoft.Json;
using System.Net.Mail;

namespace ComparativeEstimationLibrary
{
    public class Administration
    {
        private readonly string _testFileName = "test.txt";
        private readonly string _fileName = "compest.bat";
        private string _currentUserAsEmail = string.Empty;
        private List<Project> _projects = [];
        private Project _currentProject = new();

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

        public string GetProjectString(int projectId) => GetProjectForId(projectId).ToString();

        private Project GetProjectForId(int projectId)
        {
            Project? project = _projects.Where(p => p.Id == projectId).FirstOrDefault();

            return project == null
                ? throw new ArgumentException($"Invalid project id - there is no project with id \"{projectId}\"")
                : project;
        }

        public char GetNextItemIdForProject() => _currentProject.GetNextItemId();

        public void AddItemToProject(char itemId, string itemDescription)
            => _currentProject.AddItem(itemId, itemDescription);

        public int GetNumberOfItemsForProject() => _currentProject.Items.Count;

        public bool ProjectIsValid() => _currentProject.Items.Count >= 2;

        public void SaveProject()
        {
            FileStream fs = OpenAndGetFileStream();

            LoadProjectsFromFile(fs);

            _currentProject.Id = _projects.Count + 1;
            _projects.Add(_currentProject);

            SaveProjectsToFile(fs);

            fs.Close();
        }

        private FileStream OpenAndGetFileStream()
        {
            FileStream fs = new(_testFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            return fs;
        }

        private void SaveProjectsToFile(FileStream fs)
        {
            // TODO - save projects to correct file name -> binary
            // TODO - complete model(s) during saving
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

                projectModels.Add(projectModel);
            }

            WriteToJsonFile(_testFileName, projectModels);
        }

        private void LoadProjectsFromFile(FileStream fs)
        {
            // TODO - load projects from correct file name -> binary
            // TODO - complete model(s) during loading
            List<ProjectModel> projectModels = ReadFromJsonFile<List<ProjectModel>>(fs);

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
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);

                FileStream fs = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                writer = new StreamWriter(fs);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(FileStream fs) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(fs);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
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
            FileStream fs = OpenAndGetFileStream();
            LoadProjectsFromFile(fs);
            fs.Close();

            return _projects.Select(p => $"{p.Id}. {(p.Title == string.Empty ? "<UNTITLED>" : p.Title)}");
        }

        public Comparision? GetNextComparision()
        {
            if (_currentProject.Items.Count == 0)
                return null;

            if (_currentProject.RankedItemIds.Count == 0)
            {
                _currentProject.CurrentItem1ToRank = _currentProject.Items[0].Id;
                _currentProject.CurrentItem2ToRank = (char)(_currentProject.CurrentItem1ToRank + 1);
                _currentProject.RankedItemIds.Add([_currentProject.CurrentItem1ToRank]);
            }

            while (true)
            {
                if (_currentProject.CurrentItem1ToRank >= _currentProject.Items[^1].Id)
                    break;

                if (_currentProject.CurrentItem2ToRank >= _currentProject.Items[^1].Id)
                {
                    _currentProject.CurrentItem1ToRank++;
                    _currentProject.CurrentItem2ToRank = (char)(_currentProject.CurrentItem1ToRank + 1);
                    continue;
                }

                // TODO - consider the list _currentProject.RankedItemIds

                Item item1 = _currentProject.Items.Where(i => i.Id == _currentProject.CurrentItem1ToRank).First(),
                     item2 = _currentProject.Items.Where(i => i.Id == _currentProject.CurrentItem2ToRank).First();

                _currentProject.CurrentItem2ToRank++;

                return new(item1, item2);
            }

            return null;
        }
    }
}