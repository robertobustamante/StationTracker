using System;
using System.IO;
using System.Timers;
using System.Data;
using MeridianEngine.Tools;
using MeridianEngine.DataConnection;

namespace StationTracker
{
    public class ServiceTask
    {
        private readonly Timer _timer;
        Conexion conLocal = new Conexion();
        Conexion conSrv = new Conexion();
        ConfigReader _jsonReader = new ConfigReader();
        Logger log = new Logger();
        Sesion sesionLocal, sesionSrv;
        string query = "", serialNumber = "";
        double tiempoEspera = 5000; //1 segundo
        DataTable dtDatos = new DataTable();
        
        public ServiceTask()
        {
            try
            {
                serialNumber = _jsonReader.Leer("SerialNumber");
                tiempoEspera = Convert.ToDouble(_jsonReader.Leer("SegundosActualizacion"));
                tiempoEspera = tiempoEspera * 1000;
                sesionLocal = conLocal.getSesion(Sesion.TipoBD.Local);
                sesionSrv = conSrv.getSesion(Sesion.TipoBD.Servidor);

                _timer = new Timer(tiempoEspera) { AutoReset = true };
                _timer.Elapsed += _timer_Elapsed;

                log.Historial("Se inicio el servicio correctamente", "Usuario generico");
            }
            catch(Exception ex)
            {
                log.Historial(ex.Message, "Usuario generico");
            }
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            query = "SELECT LaneID, ErrorMessage FROM EventLog Where EventDateTime in (Select max(EventDateTime) from EventLog Group By LaneID)";            
            dtDatos = sesionLocal.Datos.TraerDataTableSql(query);
            

            if(dtDatos.Rows.Count > 0)
            {
                foreach (DataRow row in dtDatos.Rows)
                {
                    query = "Update StationTBL Set StatusDef = '" + row[1] + "' Where StationID = '" + serialNumber + row[0] + "' and Lane = '" + row[0] + "' and NumSerie = '" + serialNumber + "'";
                    sesionSrv.Datos.EjecutarSql(query);
                }
                Console.WriteLine(dtDatos.Rows.Count + " campos actualizados");
            }
            
        }
        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
    }
}
