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
                    administration.CreateNewProject();
                    Console.WriteLine($"New Project");

                    // Ask once for title -> optional
                    Console.Write("Title (optional): ");
                    administration.AddTitleToProject(Console.ReadLine() ?? "");

                    // Ask repeatedly for item until no item id is available
                    do
                    {
                        char itemId = administration.GetNextItemIdForProject();

                        if (itemId == ' ')
                            break;

                        Console.Write($"Item {itemId}: ");
                        string? itemDescription = Console.ReadLine();

                        if (string.IsNullOrEmpty(itemDescription))
                            break;

                        administration.AddItemToProject(itemId, itemDescription);
                    } while (true);

                    // Show number of items and max. number of comparisions or if not valid
                    if (administration.ProjectIsValid())
                    {
                        Console.WriteLine($"{administration.GetNumberOfItemsForProject()} items" +
                            $", max. number of comparisons: {administration.GetMaxNumberOfComparisonsForProject()}");

                        // Ask, if Ok -> save
                        Console.Write("Ok [Y/n]: ");
                        string? answer = Console.ReadLine();

                        if (!string.IsNullOrEmpty(answer) && answer.Equals("y", StringComparison.CurrentCultureIgnoreCase))
                            administration.SaveProject();
                    }
                    else
                    {
                        Console.WriteLine("Project is not valid");
                    }
                }
                else if (input.Equals("c", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Choose project
                    Console.Write("Project number: ");
                    string? projectNumber = Console.ReadLine();

                    try
                    {
                        administration.SetCurrentProject(projectNumber);
                    }
                    catch (ArgumentException ae)
                    {
                        Console.WriteLine($"Invalid project number - {ae.Message}");
                        continue;
                    }

                    // Show title
                    Console.WriteLine($"\n{administration.GetCurrentProjectTitle()}");

                    // Ask for repeatedly for comparisions
                    while (true)
                    {
                        Comparision? comparision = administration.GetNextComparision();

                        if (comparision == null)
                            break;

                        Console.Write($"Compare {comparision.Item1.Output} to {comparision.Item2.Output}: ");
                        input = Console.ReadLine();
                        char choosenItem;

                        if (string.IsNullOrEmpty(input) || input.Length > 1 ||
                            (!input.Equals(comparision.Item1.Id.ToString()) && !input.Equals(comparision.Item2.Id.ToString())))
                        {
                            choosenItem = comparision.Item1.Id;
                            Console.WriteLine($"No or invalid item id given -> {choosenItem} choosen");
                        }
                        else
                        {
                            choosenItem = input[0];
                        }

                        administration.AddItemRanking(comparision, choosenItem);
                    }

                    // TODO - Save the user ranking for the current project

                    // TODO - Show the user ranking for the current project

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