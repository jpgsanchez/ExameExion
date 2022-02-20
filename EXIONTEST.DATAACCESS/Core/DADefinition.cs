using System.Data;
using System.Collections;

namespace EXIONTEST.DATAACCESS.Core
{
    /// <summary>
    /// Clase base de acceso a datos.
    /// </summary>
    /// <typeparam name="TEntity">Tipo de dato que se espera tras la ejecución del proceso.</typeparam>
    /// <typeparam name="UEntity">Tipo de dato que define los parametros de la ejecución.</typeparam>
    public abstract class DADefinition<TEntity, UEntity> where TEntity : class where UEntity : class
    {
        public abstract IDbConnection creaconexion();

        public abstract TEntity ObtieneItem(string strprocedure);

        public abstract TEntity ObtieneItem(string strprocedure, UEntity obj);

        public abstract IEnumerable ObtieneLista(string strprocedure);

        public abstract IEnumerable ObtieneLista(string strprocedure, UEntity obj);

        public abstract bool EjecutaProceso(string strprocedure, UEntity obj);
    }
}