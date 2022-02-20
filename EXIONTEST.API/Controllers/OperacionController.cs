using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using EXIONTEST.BUSINESS.Order;
using EXIONTEST.BUSINESS.Account;

using EXIONTEST.ENTITIES.Models;

namespace EXIONTEST.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperacionController : Controller
    {
        [HttpPost]
        [Route("/Account")]
        public MCResponse GeneraCuenta(string cash)
        {
            AccountProcess ap = new AccountProcess();
            MCResponse obj = ap.CreaCuenta(cash);

            return obj;
        }

        [HttpPost]
        [Route("/Orders")]
        public MOResponse ProcesarOrden(int accountid, [FromBody]List<MORequest> param)
        {
            OrderProcess op = new OrderProcess();
            MOResponse obj = op.ProcesaOrden(accountid, param);

            return obj;
        }
    }
}