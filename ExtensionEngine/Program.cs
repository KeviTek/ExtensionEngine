using ExtensionEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ExtensionEngine
{
    class Program
    {
        static void Main(string[] args)
        {

            using (Timer timer = new Timer(300000))  //  (1000 * 120 ) for 2 minutes
            {
                //Add event
                timer.Elapsed += (sender, e) => RunExtension(sender, e);

                timer.Start();

                Console.WriteLine("--------------------------------------Timer is started--------------------------------------");

                // Type <<ENTER>> if you want to stop the process
                Console.ReadLine();
            }

            

           
        }

        public static void RunExtension(Object source, ElapsedEventArgs e)
        {
                       
            Dbservice db = new Dbservice();
            List<ExtensionEntry> cipEntries = db.FetchData("APPROVED");

            if (cipEntries.Capacity == 0)
            {
                Console.WriteLine(@"Aucune extension trouvé" + " " + DateTime.Now.ToString("dd-MM-yyyy , HH:mm"));
                //db.mail();
            }
            else
            {
                db.SendEmail(cipEntries);
                db.UpdateStatus("TREATED", "APPROVED");
                Console.WriteLine(@"Extension trouvé" + " " + DateTime.Now.ToString("dd-MM-yyyy , HH:mm"));

            }

            String hourMinute = DateTime.Now.ToString("HH:mm");

            if (hourMinute == "07:00")
            {
                db.DropPending("PENDING");
                Console.WriteLine(@"Extension en attente effacé" + " " + DateTime.Now.ToString("dd-MM-yyyy , HH:mm"));
            }
           
        }
    }
}
