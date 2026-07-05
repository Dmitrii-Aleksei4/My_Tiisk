using Microsoft.EntityFrameworkCore;
using MyTiskTask3;
using MyTiskTask3.DataBase;
namespace MyTiskTask3
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