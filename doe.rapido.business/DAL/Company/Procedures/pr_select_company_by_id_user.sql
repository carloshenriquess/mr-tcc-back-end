IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_company_by_id_user')
DROP PROCEDURE pr_select_company_by_id_user
GO

CREATE PROCEDURE pr_select_company_by_id_user
    @id_user INT
AS
/*

Descrição: seleciona uma empresa pelo id do usuário vinculado

*/
BEGIN
    SELECT * FROM tb_company WITH(NOLOCK) WHERE id_user = @id_user
END