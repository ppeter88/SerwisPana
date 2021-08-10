using BikeService.Data;
using BikeService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Actions
{
    public class RequestAct : IDisposable
    {
        private readonly ApplicationDbContext _db;

        public RequestAct(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Dispose()
        {
            return;
        }

        /*SYMBOL_TYM - generuje tymczasowy symbol zgłoszenia (przed zapisem zgłoszenia do bazy danych)*/
        public string GenerateTmpSymbol()
        {
            var tmpSymbol = _db.Requests.OrderByDescending(s => s.Id).Select(s => s.Id).FirstOrDefault();
            return "TYM/" + tmpSymbol.ToString() + "/" + DateTime.Now.ToString("yyyyMM");
        }

        /*SYMBOL - generuje symbol zgłoszenia*/
        public string GenerateSymbol(DateTime timeStamp)
        {
            var tmpSymb = new RequestSymbol();
            string period = timeStamp.ToString("yyyyMM");
            var lastNumb =  _db.RequestSymbol.OrderByDescending(s => s.Number).FirstOrDefault(s => s.Period == period);

            if (lastNumb != null)
                tmpSymb.Number = lastNumb.Number + 1;
            else
                tmpSymb.Number = 1;
            tmpSymb.Period = period;
            tmpSymb.Symbol = "ZG/" + tmpSymb.Number + "/" + period;

            _db.Add(tmpSymb);
            _db.SaveChanges();

            return tmpSymb.Symbol;
        }

        
    }
}
