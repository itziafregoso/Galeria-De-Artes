-- ============================================================
-- Script: CrearTablas.sql
-- Base de datos: GaleriaArte
-- Crea las tablas necesarias para el módulo de ventas/compras.
-- Ejecutar una vez; las sentencias son idempotentes.
-- ============================================================

USE GaleriaArte;
GO

-- 1. CLIENTE
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.CLIENTE') AND type = 'U')
CREATE TABLE dbo.CLIENTE (
    id_cliente      INT           IDENTITY(1,1) PRIMARY KEY,
    nombre_completo NVARCHAR(150) NOT NULL,
    email           NVARCHAR(150) NULL,
    telefono        NVARCHAR(30)  NULL
);
GO

-- 2. PROVEEDOR
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.PROVEEDOR') AND type = 'U')
CREATE TABLE dbo.PROVEEDOR (
    id_proveedor     INT           IDENTITY(1,1) PRIMARY KEY,
    nombre_proveedor NVARCHAR(150) NOT NULL,
    contacto         NVARCHAR(150) NULL,
    telefono         NVARCHAR(30)  NULL
);
GO

-- 3. VENTA  (una pintura → un cliente → una venta)
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.VENTA') AND type = 'U')
CREATE TABLE dbo.VENTA (
    id_venta     INT           IDENTITY(1,1) PRIMARY KEY,
    id_pintura   INT           NOT NULL REFERENCES dbo.PINTURA(id_pintura),
    id_cliente   INT           NULL     REFERENCES dbo.CLIENTE(id_cliente),
    fecha_venta  DATETIME      NOT NULL DEFAULT GETDATE(),
    precio_venta DECIMAL(18,2) NOT NULL
);
GO

-- 4. COMPRA  (una pintura adquirida de un proveedor)
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.COMPRA') AND type = 'U')
CREATE TABLE dbo.COMPRA (
    id_compra     INT           IDENTITY(1,1) PRIMARY KEY,
    id_proveedor  INT           NOT NULL REFERENCES dbo.PROVEEDOR(id_proveedor),
    id_pintura    INT           NULL     REFERENCES dbo.PINTURA(id_pintura),
    fecha_compra  DATETIME      NOT NULL DEFAULT GETDATE(),
    precio_compra DECIMAL(18,2) NOT NULL
);
GO

-- 5. PIEZA (unidad física de cada obra — base del inventario dinámico)
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID('dbo.PIEZA') AND type = 'U')
CREATE TABLE dbo.PIEZA (
    id_pieza      INT           IDENTITY(1,1) PRIMARY KEY,
    id_pintura    INT           NOT NULL REFERENCES dbo.PINTURA(id_pintura),
    descripcion   NVARCHAR(200) NULL,
    estado_fisico NVARCHAR(50)  NOT NULL DEFAULT 'Disponible'
        CONSTRAINT CK_PIEZA_estado CHECK (
            estado_fisico IN ('Disponible', 'Vendida', 'En Exhibición', 'Dañada', 'En Restauración')
        )
);
GO

-- ============================================================
-- Datos de muestra (comentar si ya existen registros)
-- ============================================================
/*
INSERT INTO dbo.CLIENTE (nombre_completo, email, telefono) VALUES
    ('Ana Torres',     'ana@mail.com',     '555-1001'),
    ('Luis Ramírez',   'luis@mail.com',    '555-1002'),
    ('Sofía Mendez',   'sofia@mail.com',   '555-1003');

INSERT INTO dbo.PROVEEDOR (nombre_proveedor, contacto, telefono) VALUES
    ('Arte & Más S.A.',       'contacto@artemasas.com',  '555-2001'),
    ('Galería Importadora',   'ventas@galimp.com',       '555-2002'),
    ('Colecciones del Norte', 'info@colnorte.com',       '555-2003');
*/
