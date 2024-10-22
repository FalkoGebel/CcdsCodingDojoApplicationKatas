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
                    // TODO - (C)ompare

                    /*
                     * 1. choose project
                     * 
                     * 2. show title
                     * 
                     * 3. ask for comparisions
                     * while (true)
                     * {
                     *      comp = administraton.GetNextComparision();
                     *      
                     *      if (comp == null)
                     *          break;
                     *          
                     *      WriteLine(comparision text);
                     *      answer = ReadLine();
                     *      administration.SetComparisionResult(answer);
                     * }
                    */
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