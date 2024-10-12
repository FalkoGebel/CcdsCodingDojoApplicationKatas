using Newtonsoft.Json;
using System.Net.Mail;

namespace ComparativeEstimationLibrary
{
    public class Administration
    {
        private readonly string _fileName = "compest.bat";
        private string _currentUserAsEmail = string.Empty;
        private List<Project> _projects = [];

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
            LoadProjectsFromFile();
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

        public int CreateNewProject()
        {
            int newId = _projects.Count == 0 ? 1 : _projects.OrderBy(p => p.Id).Last().Id + 1;

            _projects.Add(new(newId));
            SaveProjectsToFile();

            return _projects.Last().Id;
        }

        public void AddTitleToProject(int projectId, string title)
        {
            Project project = GetProjectForId(projectId);
            project.Title = title;
        }

        public string GetProjectString(int projectId) => GetProjectForId(projectId).ToString();

        private Project GetProjectForId(int projectId)
        {
            Project? project = _projects.Where(p => p.Id == projectId).FirstOrDefault();

            return project == null
                ? throw new ArgumentException($"Invalid project id - there is no project with id \"{projectId}\"")
                : project;
        }

        public char GetNextProjectItemId(int projectId) => GetProjectForId(projectId).GetNextItemId();

        public void AddItemToProject(int projectId, char itemId, string itemDescription)
        {
            Project project = GetProjectForId(projectId);
            project.AddItem(itemId, itemDescription);
        }

        public int GetNumberOfItemsForProject(int projectId) => GetProjectForId(projectId).Items.Count;

        public bool ProjectIsValid(int projectId) => GetProjectForId(projectId).Items.Count >= 2;

        public void SaveProject(int projectId)
        {
            GetProjectForId(projectId).ReadyToUse = true;
            SaveProjectsToFile();
        }

        private void SaveProjectsToFile()
        {
            // TODO - save projects to correct file name -> binary
            // TODO - complete model(s) during saving
            // TODO - only save the current project, because other users can work on the same file
            List<ProjectModel> projectModels = [];

            foreach (Project project in _projects)
            {
                ProjectModel projectModel = new()
                {
                    Id = project.Id,
                    Title = project.Title,
                    ReadyToUse = project.ReadyToUse
                };

                projectModels.Add(projectModel);
            }

            WriteToJsonFile("test.txt", projectModels);
        }
        private void LoadProjectsFromFile()
        {
            // TODO - load projects from correct file name -> binary
            // TODO - complete model(s) during loading
            List<ProjectModel> projectModels = ReadFromJsonFile<List<ProjectModel>>("test.txt");

            _projects.Clear();
            foreach (ProjectModel projectModel in projectModels.Where(pm => pm.ReadyToUse))
            {
                Project project = new(projectModel.Id)
                {
                    Title = projectModel.Title,
                    ReadyToUse = projectModel.ReadyToUse
                };

                _projects.Add(project);
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
                writer = new StreamWriter(filePath, append);
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
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public int GetMaxNumberOfComparisonsForProject(int projectId)
            => Enumerable.Range(1, GetProjectForId(projectId).Items.Count - 1).Sum();

        public IEnumerable<string> GetProjects()
        {
            return _projects.Select((p, i) => $"{i + 1}. {(p.Title == string.Empty ? "<UNTITLED>" : p.Title)}");
        }
    }
}