IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_user_by_id')
DROP PROCEDURE pr_select_user_by_id
GO

CREATE PROCEDURE pr_select_user_by_id
    @id_user INT
AS
/*

Descrição: seleciona um usuário pelo seu id

*/
BEGIN
    SELECT * FROM tb_user WITH(NOLOCK) WHERE id_user = @id_user
END