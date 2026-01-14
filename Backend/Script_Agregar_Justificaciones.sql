-- Script para agregar columnas de justificación a la tabla resultados
-- Ejecutar este script en la base de datos demo_web

USE demo_web;
GO

-- Verificar si las columnas ya existen antes de agregarlas
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[resultados]') AND name = 'justificacion_jefe')
BEGIN
    ALTER TABLE resultados
    ADD justificacion_jefe NVARCHAR(500) NULL;
    PRINT 'Columna justificacion_jefe agregada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La columna justificacion_jefe ya existe.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[resultados]') AND name = 'justificacion_super_jefe')
BEGIN
    ALTER TABLE resultados
    ADD justificacion_super_jefe NVARCHAR(500) NULL;
    PRINT 'Columna justificacion_super_jefe agregada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La columna justificacion_super_jefe ya existe.';
END
GO

PRINT 'Script ejecutado correctamente.';
GO
