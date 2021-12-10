IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_company_by_id')
DROP PROCEDURE pr_select_company_by_id
GO

CREATE PROCEDURE pr_select_company_by_id
    @id_company INT
AS
/*

Descrição: seleciona uma empresa pelo seu id

*/
BEGIN
    SELECT * FROM tb_company WITH(NOLOCK) WHERE id_company = @id_company
END