using Onti2022_V2.Models;
using Onti2022_V2.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onti2022_V2.DatabaseHelper
{
    public static class DatabaseHelper
    {
        public static List<UserModel> utilizatori = new List<UserModel>();
        public static List<HartaModel> obiecte = new List<HartaModel>();
        public static int hedy = 0, hedx = 0;
        public static void GetUseri()
        {
            using(StreamReader rdr = new StreamReader(Resources.userString))
            {
                while (rdr.Peek() >= 1)
                {
                    var line = rdr.ReadLine().Split(' ');

                    UserModel model = new UserModel
                    {
                        username = line[0],
                        password = line[1]
                    };
                    utilizatori.Add(model);
                }
            }
        }
        public static void LoadObjects(string File)
        {
            using(StreamReader rdr = new StreamReader(File))
            {
                while(rdr.Peek()>=1)
                {
                    var line = rdr.ReadLine().Split(' ');
                    obiecte.Add(new HartaModel
                    {
                        type = line[0],
                        casx = Int32.Parse(line[1]),
                        casy = Int32.Parse(line[2])
                    });
                }
                rdr.Close();
            }
        }
    }
}
