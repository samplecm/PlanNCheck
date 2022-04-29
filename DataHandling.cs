using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace Plan_n_Check
{
    class DataHandling
    {
        public static void SaveList<T>(string file_name, List<T> list)
        {
            try
            {
                using (var stream = new FileStream(file_name, FileMode.Create, FileAccess.Write))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, list);
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Could not save serialized list: " + file_name);
                System.Windows.Forms.Application.Exit();
            }
        }
        public static List<T> Load<T>(string file_name)
        {
            var list = new List<T>();
            if (File.Exists(file_name))
            {
                try
                {
                    using (var stream = new FileStream(file_name, FileMode.Open, FileAccess.Read))
                    {
                        var formatter = new BinaryFormatter();
                        list = (List<T>)formatter.Deserialize(stream);
                    }
                }
                catch
                {
                    System.Windows.MessageBox.Show("Could not load serialized list: " + file_name);
                    System.Windows.Forms.Application.Exit();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Could not find list at: " + file_name);
                System.Windows.Forms.Application.Exit();
            }
            return list;
        }
        public static void List_To_CSV(string fileName, List<List<double>> list)
        {
            var csv = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {

                csv.AppendLine(string.Format("{0},{1},{2}", list[i][0], list[i][1], list[i][2]));

            }

            File.WriteAllText(fileName, csv.ToString());
            
             
        }

        
    }
}
