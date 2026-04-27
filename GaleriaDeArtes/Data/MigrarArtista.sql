-- ============================================================
-- Script: MigrarArtista.sql
-- Base de datos: GaleriaArte
-- Agrega las nuevas columnas al módulo de Artistas.
-- Idempotente: puede ejecutarse varias veces sin error.
-- ============================================================

USE GaleriaArte;
GO

-- Datos personales separados
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'nombre')
    ALTER TABLE dbo.ARTISTA ADD nombre NVARCHAR(100) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'apellido_paterno')
    ALTER TABLE dbo.ARTISTA ADD apellido_paterno NVARCHAR(100) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'apellido_materno')
    ALTER TABLE dbo.ARTISTA ADD apellido_materno NVARCHAR(100) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'correo')
    ALTER TABLE dbo.ARTISTA ADD correo NVARCHAR(150) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'telefono')
    ALTER TABLE dbo.ARTISTA ADD telefono NVARCHAR(20) NULL;
GO

-- Dirección
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'calle')
    ALTER TABLE dbo.ARTISTA ADD calle NVARCHAR(200) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'colonia')
    ALTER TABLE dbo.ARTISTA ADD colonia NVARCHAR(100) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'ciudad')
    ALTER TABLE dbo.ARTISTA ADD ciudad NVARCHAR(100) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.ARTISTA') AND name = 'codigo_postal')
    ALTER TABLE dbo.ARTISTA ADD codigo_postal NVARCHAR(10) NULL;
GO

PRINT 'Migración de ARTISTA completada.';
GO
