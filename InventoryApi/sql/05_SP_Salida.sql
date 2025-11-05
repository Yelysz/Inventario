GO
CREATE OR ALTER PROCEDURE dbo.spInventario_Salida
  @IdProducto INT,
  @Cantidad   INT
AS
BEGIN
  SET NOCOUNT ON; SET XACT_ABORT ON;
  IF (@Cantidad <= 0) THROW 50002, 'Cantidad debe ser positiva', 1;

  BEGIN TRAN;
    DECLARE @Existencia INT;
    SELECT @Existencia = Existencia FROM dbo.Inventario WITH (UPDLOCK, ROWLOCK)
      WHERE IdProducto=@IdProducto;

    IF (@Existencia IS NULL) THROW 50003, 'No hay inventario para este producto', 1;

    IF (@Existencia < @Cantidad)
      THROW 50004, 'Stock insuficiente', 1;

    UPDATE dbo.Inventario
      SET Existencia = Existencia - @Cantidad
      WHERE IdProducto = @IdProducto;

    INSERT INTO dbo.MovimientosInventario (IdProducto, Cantidad, IdTipoMovimiento)
      VALUES (@IdProducto, @Cantidad, 2); -- 2 = Salida
  COMMIT;

  SELECT i.IdProducto, i.Existencia FROM dbo.Inventario i WHERE i.IdProducto=@IdProducto;
END
GO