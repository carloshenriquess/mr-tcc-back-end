IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_category_by_id_company')
DROP PROCEDURE pr_select_category_by_id_company
GO

CREATE PROCEDURE pr_select_category_by_id_company
    @id_company INT
AS
/*

Descrição: seleciona as categorias de uma empresa pelo seu id

*/
BEGIN
    SELECT 
        tc.*
    FROM
        tb_category tc WITH(NOLOCK)
    JOIN 
        tb_company_category tcc WITH(NOLOCK) ON tc.id_category = tcc.id_category
    JOIN
        tb_company tcp WITH(NOLOCK) ON tcp.id_company = tcc.id_company
    WHERE
        tcp.id_company = @id_company
END