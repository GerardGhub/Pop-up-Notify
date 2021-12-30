using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MsgBoxConsole
{
    class Program
    {
       

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          string systemName = "";

            string uix = File.ReadLines("C:\\type.txt").First();

            if (uix.Trim() == "hr")
            {
                systemName = "HR-System";
            }
            else if (uix.Trim() == "mftg")
            {
                systemName = "MFTG-System";
            }

                string logs = "MessageBox Notification Popped Up " + systemName;

                try
                {
                    const string location = @"ponLogs";

                    if (!File.Exists(location))
                    {
                        var createText = "New Activities Logs" + Environment.NewLine;
                        File.WriteAllText(location, createText);
                    }
                    var appendLogs = "Activities Logs: " + logs + " " + DateTime.Now + Environment.NewLine;
                    File.AppendAllText(location, appendLogs);
                }
                catch (Exception ex)
                {
                    const string location = @"ponLogs";
                    if (!File.Exists(location))
                    {
                        TextWriter file = File.CreateText(@"C:\ponLogs");
                        var createText = "New Activities Logs" + Environment.NewLine;

                        File.WriteAllText(location, createText);

                    }
                    var appendLogs = ex.Message + logs + DateTime.Now + Environment.NewLine;
                    File.AppendAllText(location, appendLogs);

                }



            MessageBox.Show(systemName + " was succesfully updated on " + DateTime.Now.ToString("MMM/dd/yyyy hh:mm ttt") + "", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
