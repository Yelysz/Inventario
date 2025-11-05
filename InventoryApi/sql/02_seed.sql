USE InventarioDB;
SET IDENTITY_INSERT dbo.TipoMovimiento ON;
IF NOT EXISTS (SELECT 1 FROM dbo.TipoMovimiento WHERE IdTipoMovimiento = 1)
  INSERT INTO dbo.TipoMovimiento(IdTipoMovimiento, Nombre) VALUES (1,'Entrada');
IF NOT EXISTS (SELECT 1 FROM dbo.TipoMovimiento WHERE IdTipoMovimiento = 2)
  INSERT INTO dbo.TipoMovimiento(IdTipoMovimiento, Nombre) VALUES (2,'Salida');
SET IDENTITY_INSERT dbo.TipoMovimiento OFF;
