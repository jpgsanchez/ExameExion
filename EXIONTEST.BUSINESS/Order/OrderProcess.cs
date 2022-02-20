using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using EXIONTEST.BUSINESS.Common;

using EXIONTEST.DATAACCESS.Core;

using EXIONTEST.ENTITIES.Models;
using EXIONTEST.ENTITIES.Objects;

namespace EXIONTEST.BUSINESS.Order
{
    public class OrderProcess : BaseExecution<balance, ordenDTO>
    {
        public MOResponse ProcesaOrden(int accountid, List<MORequest> lst)
        {
            List<ordenDTO> lstorden = lst.Select(s => new ordenDTO()
            {
                cid = accountid,
                toid = s.operation,
                eid = s.issuer_id,
                acciones = s.total_shares,
                fecharegistro = s.timestamp
            }).ToList();

            MOResponse result = OrdenCompraVenta(lstorden);

            return result;
        }

        public MOResponse OrdenCompraVenta(List<ordenDTO> lstorden)
        {
            MOResponse result = new MOResponse();
            DAProcess<BaseItem, BaseItem> da = new DAProcess<BaseItem, BaseItem>();

            DExcecute del = da.ObtieneItem;
            result.current_balance = LocalExcecute(del, "sp_procesaorden", lstorden);

            return result;
        }
    }
}