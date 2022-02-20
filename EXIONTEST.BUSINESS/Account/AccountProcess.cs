using System;

using EXIONTEST.BUSINESS.Common;

using EXIONTEST.DATAACCESS.Core;

using EXIONTEST.ENTITIES.Models;
using EXIONTEST.ENTITIES.Objects;

namespace EXIONTEST.BUSINESS.Account
{
    public class AccountProcess : BaseExecution<MCResponse, cuentaDTO>
    {
        public MCResponse CreaCuenta(string cash)
        {
            MCResponse result = new MCResponse();
            try
            {
                if (decimal.TryParse(cash, out decimal saldo) && saldo > 0)
                {
                    DAProcess<BaseItem, BaseItem> da = new DAProcess<BaseItem, BaseItem>();

                    DExcecute del = da.ObtieneItem;
                    result = LocalExcecute(del, "spi_cuenta", new cuentaDTO() { saldo = saldo });
                }
                else
                {
                    result.errors.Add("El monto proporcionado no es válido.");
                }
            }
            catch (Exception ex)
            {
                result.errors.Add(ex.Message);
            }

            return result;
        }
    }
}