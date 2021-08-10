using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Actions
{
    public class LogSqlQueryAct :IDisposable
    {
        /*Klasa zawierająca operacje do kontroli Logów z zapytaniami SQL, wysyłanymi przez EntityFramework*/

        private string LogFilePath { get; set; }

        public void Dispose()
        {
            return;
        }

        public void SaveQuery (string sql)
        {
            LogFilePath = "C:/Repozytorium/BikeService/BikeService/wwwroot/BikeServiceLog.txt";
            using(StreamWriter sr = new StreamWriter(LogFilePath, true))
            {
                sr.WriteLine();
                sr.Write(sql);
            }

        }
    }
}
