using Microsoft.EntityFrameworkCore;
using MyTiskTask2.DataBase;
namespace MyTiskTask2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            using (var context = new AppDbContext())
            {
                context.Database.EnsureCreated();  // Теперь работает
            }

            ApplicationConfiguration.Initialize();
            
            Application.Run(new Form1());
        }
    }
}