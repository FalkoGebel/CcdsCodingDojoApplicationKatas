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

            Console.WriteLine("TODO -> next step");

        }
    }
}