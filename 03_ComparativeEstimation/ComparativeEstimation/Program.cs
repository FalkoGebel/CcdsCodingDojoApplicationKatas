using ComparativeEstimationLibrary;

namespace ComparativeEstimation
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No email given. You need to start the program with the email of the working user as the only parameter.");
                return;
            }

            Administration administration;

            try
            {
                administration = new(args[0]);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
                return;
            }

            do
            {
                Console.Write("(N)ew estimation project, (C)ompare, (E)valuate project, (L)ist projects, e(X)it: ");
                string? input = Console.ReadLine();

                if (input == null)
                    continue;

                if (input.Equals("x", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
                else if (input.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Create new project and get project Id to show
                    int projectId = administration.CreateNewProject();
                    Console.WriteLine($"Project #{projectId}");

                    // Ask once for title -> optional
                    Console.Write("Title (optional): ");
                    administration.AddTitleToProject(projectId, Console.ReadLine() ?? "");

                    // Ask repeatedly for item until no item id is available
                    do
                    {
                        char itemId = administration.GetNextProjectItemId(projectId);

                        if (itemId == ' ')
                            break;

                        Console.Write($"Item {itemId}: ");
                        string? itemDescription = Console.ReadLine();

                        if (string.IsNullOrEmpty(itemDescription))
                            break;

                        administration.AddItemToProject(projectId, itemId, itemDescription);
                    } while (true);

                    // Show number of items and max. number of comparisions or if not valid
                    if (administration.ProjectIsValid(projectId))
                    {
                        Console.WriteLine($"{administration.GetNumberOfItemsForProject(projectId)} items" +
                            $", max. number of comparisons: {administration.GetMaxNumberOfComparisonsForProject(projectId)}");

                        // Ask, if Ok -> save
                        Console.Write("Ok [Y/n]: ");
                        string? answer = Console.ReadLine();

                        if (!string.IsNullOrEmpty(answer) && answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            administration.SaveProject(projectId);
                    }
                    else
                    {
                        Console.WriteLine("project is not valid");
                    }
                }
                else if (input.Equals("c", StringComparison.CurrentCultureIgnoreCase))
                {
                    // TODO - (C)ompare

                }
                else if (input.Equals("e", StringComparison.CurrentCultureIgnoreCase))
                {
                    // TODO - (E)valuate

                }
                else if (input.Equals("l", StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (string project in administration.GetProjects())
                        Console.WriteLine(project);
                }
            } while (true);

        }
    }
}