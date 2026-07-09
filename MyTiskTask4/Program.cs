using Microsoft.EntityFrameworkCore;
using MyTiskTask4;
using MyTiskTask4.DataBase;
namespace MyTiskTask4
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