using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncrementosController : ControllerBase
    {
        private readonly DBC_incrementos _context;
        private readonly DBC_Empleados _contextEmpleados;
        private readonly DBC_resultados _contextResultado;
        public IncrementosController(DBC_incrementos context, DBC_Empleados contextEmpleados, DBC_resultados contextResultado)
        {
            _contextEmpleados = contextEmpleados;
            _context = context;
            _contextResultado = contextResultado;
        }

        // GET: api/<IncrementosController>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Incremento>>> Get()
        {
            var Data = await _context.Incrementos.ToListAsync();
            return Ok(Data);
        }
        // GET: api/incrementos/rol
        [HttpGet("user/{nominaID}")]
        public async Task<ActionResult<IEnumerable<Resultado>>> GetUser(int nominaID)
        {            
            string sql = @"  
            SELECt 
                a.*,
                b.jefe,
                b.SupJefe,
                b.Sem12023,
                b.Sem22023,

                -- Desempeño (promedio)
                (b.Sem12023 + b.Sem22023) / 2.0 AS Desempenio,

                -- % Tabulador
                t.porc_incremento AS PorcTabulador,

                -- Posición real en el tabulador
                CASE
                    WHEN a.sueldomensual <= t.inicial   THEN 80
                    WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                    WHEN a.sueldomensual <= t.media     THEN 100
                    WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                    ELSE 120
                END AS PosicionReal,

                -- % Incremento sugerido
                ROUND(
                    t.porc_incremento +
                    (
                        (
                            ((b.Sem12023 + b.Sem22023) / 2.0)
                            -
                            CASE
                                WHEN a.sueldomensual <= t.inicial   THEN 80
                                WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                                WHEN a.sueldomensual <= t.media     THEN 100
                                WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                                ELSE 120
                            END
                        ) / 6.0
                    ),
                    2
                ) AS PorcIncrementoSugerido,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min')   )) as porcentaje_minimo,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo_jefe FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min') )) as porcentaje_minimo_jefe


            FROM admon_sueldos a
            INNER JOIN Empleados b
            on a.nomina = b.nomina
            INNER JOIN tabulador_niveles t
                ON t.nivel = a.nivel
            WHERE b.nomina = @nominaID;";
            

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var param = cmd.CreateParameter();
                    param.ParameterName = "@nominaID";
                    param.Value = nominaID;
                    cmd.Parameters.Add(param);

                    var resultado = new List<Resultado>();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new Resultado
                            {
                                Cia = reader.GetInt32(reader.GetOrdinal("Cia")),
                                TipoTrab = reader.GetString(reader.GetOrdinal("TipoTrab")),
                                Nomina = reader.GetInt32(reader.GetOrdinal("Nomina")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                                Departamento = reader.GetString(reader.GetOrdinal("Departamento")),
                                Segmento = reader.GetString(reader.GetOrdinal("Segmento")),
                                FechaIngreso = reader.IsDBNull(reader.GetOrdinal("FechaIngreso"))
                                              ? null
                                              : reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                SueldoDiario = reader.GetDecimal(reader.GetOrdinal("SueldoDiario")),
                                SueldoMensual = reader.GetDecimal(reader.GetOrdinal("SueldoMensual")),
                                SueldoNuevo = reader.GetDecimal(reader.GetOrdinal("SueldoNuevo")),
                                NivelNum = reader.GetInt32(reader.GetOrdinal("NivelNum")),
                                Nivel = reader.GetString(reader.GetOrdinal("Nivel")),
                                Tipotab = reader.GetString(reader.GetOrdinal("Tipotab")),
                                Antiguedad = reader.GetInt32(reader.GetOrdinal("Antiguedad")),
                                Jefe = reader.IsDBNull(reader.GetOrdinal("Jefe"))
                                       ? null
                                       : reader.GetInt32(reader.GetOrdinal("Jefe")),
                                SupJefe = reader.IsDBNull(reader.GetOrdinal("SupJefe"))
                                          ? null
                                          : reader.GetInt32(reader.GetOrdinal("SupJefe")),
                                Sem12023 = reader.IsDBNull(reader.GetOrdinal("Sem12023"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Sem12023")),
                                Sem22023 = reader.IsDBNull(reader.GetOrdinal("Sem22023"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Sem22023")),
                                Desempenio = reader.IsDBNull(reader.GetOrdinal("Desempenio"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Desempenio")),
                                PorcTabulador = reader.IsDBNull(reader.GetOrdinal("PorcTabulador"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("PorcTabulador")),
                                PosicionReal = reader.IsDBNull(reader.GetOrdinal("PosicionReal"))
                                          ? null
                                          : reader.GetInt32(reader.GetOrdinal("PosicionReal")),
                                PorcIncrementoSugerido = reader.IsDBNull(reader.GetOrdinal("PorcIncrementoSugerido"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("PorcIncrementoSugerido"))
                                ,
                                porcentaje_minimo = reader.IsDBNull(reader.GetOrdinal("porcentaje_minimo"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("porcentaje_minimo")),
                                porcentaje_minimo_jefe = reader.IsDBNull(reader.GetOrdinal("porcentaje_minimo_jefe"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("porcentaje_minimo_jefe"))
                            });
                        }
                    }

                    return Ok(resultado);
                }
            }
        }

        // GET: api/incrementos/rol
        [HttpGet("jefes/{nominaID}")]
        public async Task<ActionResult<IEnumerable<Resultado>>> GetRol1(int nominaID)
        {
            // Primero revisamos si el usuario tiene SupJefe
            var tieneSupJefe = await _contextEmpleados.Empleados
                                             .Where(e => e.Nomina == nominaID)
                                             .AnyAsync(e => e.SupJefe == 1 || e.SupJefe != null);

            string sql;

            if (tieneSupJefe)
            {
                // Si tiene SupJefe, traemos todo
                sql = @"
                SELECT
                    a.*,
                    b.jefe,
                    b.SupJefe,
                    b.Sem12023,
                    b.Sem22023,

                    -- Desempeño (promedio)
                    (b.Sem12023 + b.Sem22023) / 2.0 AS Desempenio,

                    -- % Tabulador
                    t.porc_incremento AS PorcTabulador,

                    -- Posición real en el tabulador
                    CASE
                        WHEN a.sueldomensual <= t.inicial   THEN 80
                        WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                        WHEN a.sueldomensual <= t.media     THEN 100
                        WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                        ELSE 120
                    END AS PosicionReal,

                    -- % Incremento sugerido
                    ROUND(
                        t.porc_incremento +
                        (
                            (
                                ((b.Sem12023 + b.Sem22023) / 2.0)
                                -
                                CASE
                                    WHEN a.sueldomensual <= t.inicial   THEN 80
                                    WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                                    WHEN a.sueldomensual <= t.media     THEN 100
                                    WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                                    ELSE 120
                                END
                            ) / 6.0
                        ),
                        2
                    ) AS PorcIncrementoSugerido,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min')   )) as porcentaje_minimo,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo_jefe FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min') )) as porcentaje_minimo_jefe

                FROM admon_sueldos a
                INNER JOIN Empleados b
                on a.nomina = b.nomina
                INNER JOIN tabulador_niveles t
                    ON t.nivel = a.nivel
                WHERE b.puesto = lower(trim('Jefe'))
                AND SupJefe IS NULL;";
            }
            else
            {
                // Si no tiene SupJefe, excluimos a quienes tengan SupJefe
                sql = @"  
                SELECt 
                    a.*,
                    b.jefe,
                    b.SupJefe,
                    b.Sem12023,
                    b.Sem22023,

                    -- Desempeño (promedio)
                    (b.Sem12023 + b.Sem22023) / 2.0 AS Desempenio,

                    -- % Tabulador
                    t.porc_incremento AS PorcTabulador,

                    -- Posición real en el tabulador
                    CASE
                        WHEN a.sueldomensual <= t.inicial   THEN 80
                        WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                        WHEN a.sueldomensual <= t.media     THEN 100
                        WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                        ELSE 120
                    END AS PosicionReal,

                    -- % Incremento sugerido
                    ROUND(
                        t.porc_incremento +
                        (
                            (
                                ((b.Sem12023 + b.Sem22023) / 2.0)
                                -
                                CASE
                                    WHEN a.sueldomensual <= t.inicial   THEN 80
                                    WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                                    WHEN a.sueldomensual <= t.media     THEN 100
                                    WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                                    ELSE 120
                                END
                            ) / 6.0
                        ),
                        2
                    ) AS PorcIncrementoSugerido,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min')   )) as porcentaje_minimo,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo_jefe FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min') )) as porcentaje_minimo_jefe


                FROM admon_sueldos a
                INNER JOIN Empleados b
                on a.nomina = b.nomina
                INNER JOIN tabulador_niveles t
                    ON t.nivel = a.nivel
                WHERE b.nomina = @nominaID
                AND SupJefe IS NULL;";
            }

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var param = cmd.CreateParameter();
                    param.ParameterName = "@nominaID";
                    param.Value = nominaID;
                    cmd.Parameters.Add(param);

                    var resultado = new List<Resultado>();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new Resultado
                            {
                                Cia = reader.GetInt32(reader.GetOrdinal("Cia")),
                                TipoTrab = reader.GetString(reader.GetOrdinal("TipoTrab")),
                                Nomina = reader.GetInt32(reader.GetOrdinal("Nomina")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                                Departamento = reader.GetString(reader.GetOrdinal("Departamento")),
                                Segmento = reader.GetString(reader.GetOrdinal("Segmento")),
                                FechaIngreso = reader.IsDBNull(reader.GetOrdinal("FechaIngreso"))
                                              ? null
                                              : reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                SueldoDiario = reader.GetDecimal(reader.GetOrdinal("SueldoDiario")),
                                SueldoMensual = reader.GetDecimal(reader.GetOrdinal("SueldoMensual")),
                                SueldoNuevo = reader.GetDecimal(reader.GetOrdinal("SueldoNuevo")),
                                NivelNum = reader.GetInt32(reader.GetOrdinal("NivelNum")),
                                Nivel = reader.GetString(reader.GetOrdinal("Nivel")),
                                Tipotab = reader.GetString(reader.GetOrdinal("Tipotab")),
                                Antiguedad = reader.GetInt32(reader.GetOrdinal("Antiguedad")),
                                Jefe = reader.IsDBNull(reader.GetOrdinal("Jefe"))
                                       ? null
                                       : reader.GetInt32(reader.GetOrdinal("Jefe")),
                                SupJefe = reader.IsDBNull(reader.GetOrdinal("SupJefe"))
                                          ? null
                                          : reader.GetInt32(reader.GetOrdinal("SupJefe")),
                                Sem12023 = reader.IsDBNull(reader.GetOrdinal("Sem12023"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Sem12023")),
                                Sem22023 = reader.IsDBNull(reader.GetOrdinal("Sem22023"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Sem22023")),
                                Desempenio = reader.IsDBNull(reader.GetOrdinal("Desempenio"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Desempenio")),
                                PorcTabulador = reader.IsDBNull(reader.GetOrdinal("PorcTabulador"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("PorcTabulador")),
                                PosicionReal = reader.IsDBNull(reader.GetOrdinal("PosicionReal"))
                                          ? null
                                          : reader.GetInt32(reader.GetOrdinal("PosicionReal")),
                                PorcIncrementoSugerido = reader.IsDBNull(reader.GetOrdinal("PorcIncrementoSugerido"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("PorcIncrementoSugerido"))
                                ,
                                porcentaje_minimo = reader.IsDBNull(reader.GetOrdinal("porcentaje_minimo"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("porcentaje_minimo")),
                                porcentaje_minimo_jefe = reader.IsDBNull(reader.GetOrdinal("porcentaje_minimo_jefe"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("porcentaje_minimo_jefe"))
                            });
                        }
                    }

                    return Ok(resultado);
                }
            }
        }

        [HttpGet("empleados/{nominaID}")]
        public async Task<ActionResult<IEnumerable<Resultado>>> GetRol2y3(int nominaID)
        {
            // Primero revisamos si el usuario tiene SupJefe
            var tieneSupJefe = await _contextEmpleados.Empleados
                                             .Where(e => e.Nomina == nominaID)
                                             .AnyAsync(e => e.SupJefe == 1 || e.SupJefe != null);

            string sql;

            if (tieneSupJefe)
            {
                // Si tiene SupJefe, traemos todo el universo de empleados excepto a los SuperJefes
                sql = @"
                SELECT 
                    a.*,
                    b.jefe,
                    b.SupJefe,
                    b.Sem12023,
                    b.Sem22023,

                    -- Desempeño (promedio)
                    (b.Sem12023 + b.Sem22023) / 2.0 AS Desempenio,

                    -- % Tabulador
                    t.porc_incremento AS PorcTabulador,

                    -- Posición real en el tabulador
                    CASE
                        WHEN a.sueldomensual <= t.inicial   THEN 80
                        WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                        WHEN a.sueldomensual <= t.media     THEN 100
                        WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                        ELSE 120
                    END AS PosicionReal,

                    -- % Incremento sugerido
                    ROUND(
                        t.porc_incremento +
                        (
                            (
                                ((b.Sem12023 + b.Sem22023) / 2.0)
                                -
                                CASE
                                    WHEN a.sueldomensual <= t.inicial   THEN 80
                                    WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                                    WHEN a.sueldomensual <= t.media     THEN 100
                                    WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                                    ELSE 120
                                END
                            ) / 6.0
                        ),
                        2
                    ) AS PorcIncrementoSugerido,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min')   )) as porcentaje_minimo,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo_jefe FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min') )) as porcentaje_minimo_jefe


                FROM admon_sueldos a
                INNER JOIN Empleados b
                    ON a.nomina = b.nomina
                INNER JOIN tabulador_niveles t
                    ON t.nivel = a.nivel
                WHERE b.SupJefe is null and b.puesto != trim(lower('Jefe'));";
            }
            else
            {
                // Si no tiene SupJefe, traemos a los empleados del jefe a excepcion de el mismo.
                sql = @"
                SELECT 
                    a.*,
                    b.jefe,
                    b.SupJefe,
                    b.Sem12023,
                    b.Sem22023,

                    -- Desempeño (promedio)
                    (b.Sem12023 + b.Sem22023) / 2.0 AS Desempenio,

                    -- % Tabulador
                    t.porc_incremento AS PorcTabulador,

                    -- Posición real en el tabulador
                    CASE
                        WHEN a.sueldomensual <= t.inicial   THEN 80
                        WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                        WHEN a.sueldomensual <= t.media     THEN 100
                        WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                        ELSE 120
                    END AS PosicionReal,

                    -- % Incremento sugerido
                    ROUND(
                        t.porc_incremento +
                        (
                            (
                                ((b.Sem12023 + b.Sem22023) / 2.0)
                                -
                                CASE
                                    WHEN a.sueldomensual <= t.inicial   THEN 80
                                    WHEN a.sueldomensual <= t.cuartil_1 THEN 90
                                    WHEN a.sueldomensual <= t.media     THEN 100
                                    WHEN a.sueldomensual <= t.cuartil_3 THEN 110
                                    ELSE 120
                                END
                            ) / 6.0
                        ),
                        2
                    ) AS PorcIncrementoSugerido,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min')   )) as porcentaje_minimo,
            (COALESCE( (SELECT TOP 1 porcentaje_minimo_jefe FROM resultados ACT WHERE  ACT.nomina = a.nomina ), (SELECT top 1 valor FROM porcentajes_estandar WHERE variable = 'min') )) as porcentaje_minimo_jefe

                FROM admon_sueldos a
                INNER JOIN Empleados b
                    ON a.nomina = b.nomina
                INNER JOIN tabulador_niveles t
                    ON t.nivel = a.nivel
                WHERE (b.jefe = @nominaID AND b.nomina != @nominaID)";
            }

            using (var conn = _context.Database.GetDbConnection())
            {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var param = cmd.CreateParameter();
                    param.ParameterName = "@nominaID";
                    param.Value = nominaID;
                    cmd.Parameters.Add(param);

                    var resultado = new List<Resultado>();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            resultado.Add(new Resultado
                            {
                                Cia = reader.GetInt32(reader.GetOrdinal("Cia")),
                                TipoTrab = reader.GetString(reader.GetOrdinal("TipoTrab")),
                                Nomina = reader.GetInt32(reader.GetOrdinal("Nomina")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                                Departamento = reader.GetString(reader.GetOrdinal("Departamento")),
                                Segmento = reader.GetString(reader.GetOrdinal("Segmento")),
                                FechaIngreso = reader.IsDBNull(reader.GetOrdinal("FechaIngreso"))
                                              ? null
                                              : reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                                SueldoDiario = reader.GetDecimal(reader.GetOrdinal("SueldoDiario")),
                                SueldoMensual = reader.GetDecimal(reader.GetOrdinal("SueldoMensual")),
                                SueldoNuevo = reader.GetDecimal(reader.GetOrdinal("SueldoNuevo")),
                                NivelNum = reader.GetInt32(reader.GetOrdinal("NivelNum")),
                                Nivel = reader.GetString(reader.GetOrdinal("Nivel")),
                                Tipotab = reader.GetString(reader.GetOrdinal("Tipotab")),
                                Antiguedad = reader.GetInt32(reader.GetOrdinal("Antiguedad")),
                                Jefe = reader.IsDBNull(reader.GetOrdinal("Jefe"))
                                       ? null
                                       : reader.GetInt32(reader.GetOrdinal("Jefe")),
                                SupJefe = reader.IsDBNull(reader.GetOrdinal("SupJefe"))
                                          ? null
                                          : reader.GetInt32(reader.GetOrdinal("SupJefe")),
                                Sem12023 = reader.IsDBNull(reader.GetOrdinal("Sem12023"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Sem12023")),
                                Sem22023 = reader.IsDBNull(reader.GetOrdinal("Sem22023"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Sem22023")),

                                Desempenio = reader.IsDBNull(reader.GetOrdinal("Desempenio"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("Desempenio")),
                                PorcTabulador = reader.IsDBNull(reader.GetOrdinal("PorcTabulador"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("PorcTabulador")),
                                PosicionReal = reader.IsDBNull(reader.GetOrdinal("PosicionReal"))
                                          ? null
                                          : reader.GetInt32(reader.GetOrdinal("PosicionReal")),
                                PorcIncrementoSugerido = reader.IsDBNull(reader.GetOrdinal("PorcIncrementoSugerido"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("PorcIncrementoSugerido")),
                                porcentaje_minimo = reader.IsDBNull(reader.GetOrdinal("porcentaje_minimo"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("porcentaje_minimo")),
                                porcentaje_minimo_jefe = reader.IsDBNull(reader.GetOrdinal("porcentaje_minimo_jefe"))
                                          ? null
                                          : reader.GetDecimal(reader.GetOrdinal("porcentaje_minimo_jefe"))




                            });
                        }
                    }

                    return Ok(resultado);
                }
            }
        }


        [HttpPut("updateIncremento/{nomina}")]
        public async Task<IActionResult> Put(int nomina, Resultado resultado)
        {
            var resultadoDb = await _contextResultado.Resultados
            .FirstOrDefaultAsync(r => r.Nomina == nomina);

            if (resultadoDb == null)
                return NotFound();

            // Solo actualiza lo necesario
            resultadoDb.PorcentajeMinimo = resultado.porcentaje_minimo;
            resultadoDb.PorcentajeMinimoJefe = resultado.porcentaje_minimo_jefe;
            resultadoDb.Sueldodiario = resultado.SueldoDiario;
            resultadoDb.Sueldonuevo = resultado.SueldoNuevo;
            resultadoDb.Sueldomensual= resultado.SueldoMensual;

            await _contextResultado.SaveChangesAsync();

            return NoContent();
        }



        // GET api/<IncrementosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
            return "value";
        }

        // POST api/<IncrementosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<IncrementosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IncrementosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class Resultado
    {
        public int Cia { get; set; }
        public string TipoTrab { get; set; }
        public int Nomina { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public string Segmento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public decimal SueldoDiario { get; set; }
        public decimal SueldoMensual { get; set; }

        public decimal SueldoNuevo { get; set; }
        public int NivelNum { get; set; }
        public string Nivel { get; set; }
        public string Tipotab { get; set; }
        public int Antiguedad { get; set; }
        public int? Jefe { get; set; }
        public int? SupJefe { get; set; }

        public decimal? Sem12023 { get; set; }

        public decimal? Sem22023 { get; set; }
        public decimal? Desempenio { get; set; }
        public decimal? PorcTabulador { get; set; }
        public int? PosicionReal { get; set; }
        public decimal? PorcIncrementoSugerido { get; set; }

        public decimal? porcentaje_minimo { get; set; }
        public decimal? porcentaje_minimo_jefe { get; set; }
        // agrega otras columnas que necesites
    }
}
