IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_delete_user_company')
DROP PROCEDURE pr_delete_user_company
GO

CREATE PROCEDURE pr_delete_user_company
    @id_user INT
AS
/*

Descri��o: deleta um usu�rio, a empresa ao qual ele est� vinculado e demais informa��es relacionadas

*/
BEGIN
    DELETE FROM tb_user WHERE id_user = @id_user
END