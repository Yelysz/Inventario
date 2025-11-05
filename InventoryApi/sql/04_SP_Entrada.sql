GO
CREATE OR ALTER PROCEDURE dbo.spInventario_Entrada
  @IdProducto INT,
  @Cantidad   INT
AS
BEGIN
  SET NOCOUNT ON; SET XACT_ABORT ON;
  IF (@Cantidad <= 0) THROW 50001, 'Cantidad debe ser positiva', 1;

  BEGIN TRAN;
    -- Asegura fila en Inventario
    IF NOT EXISTS (SELECT 1 FROM dbo.Inventario WHERE IdProducto=@IdProducto)
      INSERT INTO dbo.Inventario(IdProducto, Existencia) VALUES (@IdProducto, 0);

    UPDATE dbo.Inventario
      SET Existencia = Existencia + @Cantidad
      WHERE IdProducto = @IdProducto;

    INSERT INTO dbo.MovimientosInventario (IdProducto, Cantidad, IdTipoMovimiento)
      VALUES (@IdProducto, @Cantidad, 1); -- 1 = Entrada
  COMMIT;

  SELECT i.IdProducto, i.Existencia FROM dbo.Inventario i WHERE i.IdProducto=@IdProducto;
END
GO
