USE master;
IF DB_ID('InventarioDB') IS NULL CREATE DATABASE InventarioDB;
GO
USE InventarioDB;
GO

-- Productos
CREATE TABLE dbo.Productos(
  IdProducto INT IDENTITY(1,1) PRIMARY KEY,
  Nombre NVARCHAR(120) NOT NULL,
  Descripcion NVARCHAR(500),
  PrecioVenta DECIMAL(18,2) NOT NULL CHECK (PrecioVenta >= 0),

  FechaCreacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  UltimaFechaActualizacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Inventario
CREATE TABLE dbo.Inventario(
  IdProducto INT PRIMARY KEY FOREIGN KEY REFERENCES dbo.Productos(IdProducto) ON DELETE CASCADE,
  Existencia INT NOT NULL DEFAULT 0 CHECK (Existencia >= 0),
  UltimaFechaActualizacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- TipoMovimiento
CREATE TABLE dbo.TipoMovimiento(
  IdTipoMovimiento INT IDENTITY(1,1) PRIMARY KEY,
  Nombre NVARCHAR(50) NOT NULL,
  UltimaFechaActualizacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- MovimientosInventario
CREATE TABLE dbo.MovimientosInventario(
  IdMovimiento BIGINT IDENTITY(1,1) PRIMARY KEY,
  IdProducto INT NOT NULL FOREIGN KEY REFERENCES dbo.Productos(IdProducto),
  Fecha DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  Cantidad INT NOT NULL,
  IdTipoMovimiento INT NOT NULL FOREIGN KEY REFERENCES dbo.TipoMovimiento(IdTipoMovimiento),
  UltimaFechaActualizacion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- TRIGGERS de actualización de fecha
GO
CREATE OR ALTER TRIGGER TRG_UpdateFechaProducto ON dbo.Productos
AFTER UPDATE AS
BEGIN
  UPDATE p SET UltimaFechaActualizacion = SYSUTCDATETIME()
  FROM dbo.Productos p INNER JOIN inserted i ON i.IdProducto = p.IdProducto;
END;
GO

CREATE OR ALTER TRIGGER TRG_UpdateFechaInventario ON dbo.Inventario
AFTER UPDATE AS
BEGIN
  UPDATE t SET UltimaFechaActualizacion = SYSUTCDATETIME()
  FROM dbo.Inventario t INNER JOIN inserted i ON i.IdProducto = t.IdProducto;
END;
GO

CREATE OR ALTER TRIGGER TRG_UpdateFechaMovimiento ON dbo.MovimientosInventario
AFTER UPDATE AS
BEGIN
  UPDATE m SET UltimaFechaActualizacion = SYSUTCDATETIME()
  FROM dbo.MovimientosInventario m INNER JOIN inserted i ON i.IdMovimiento = m.IdMovimiento;
END;
GO

CREATE OR ALTER TRIGGER TRG_UpdateFechaTipoMovimiento ON dbo.TipoMovimiento
AFTER UPDATE AS
BEGIN
  UPDATE t SET UltimaFechaActualizacion = SYSUTCDATETIME()
  FROM dbo.TipoMovimiento t INNER JOIN inserted i ON i.IdTipoMovimiento = t.IdTipoMovimiento;
END;
GO

USE InventarioDB;
GO
IF COL_LENGTH('dbo.Productos','MinimoExistencia') IS NULL
    ALTER TABLE dbo.Productos ADD MinimoExistencia INT NOT NULL CONSTRAINT DF_Productos_Minimo DEFAULT(0);
GO


SET IDENTITY_INSERT dbo.TipoMovimiento ON;
IF NOT EXISTS (SELECT 1 FROM dbo.TipoMovimiento WHERE IdTipoMovimiento=1)
    INSERT INTO dbo.TipoMovimiento(IdTipoMovimiento,Nombre) VALUES (1,'Entrada');
IF NOT EXISTS (SELECT 1 FROM dbo.TipoMovimiento WHERE IdTipoMovimiento=2)
    INSERT INTO dbo.TipoMovimiento(IdTipoMovimiento,Nombre) VALUES (2,'Salida');
SET IDENTITY_INSERT dbo.TipoMovimiento OFF;
GO

