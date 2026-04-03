using System.Text;

namespace ApiClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            await WorkWithApi.Run();
        }
    }
}
