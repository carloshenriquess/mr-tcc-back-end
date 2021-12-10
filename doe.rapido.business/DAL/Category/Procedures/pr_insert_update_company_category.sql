IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_insert_update_company_category')
DROP PROCEDURE pr_insert_update_company_category
GO

CREATE PROCEDURE pr_insert_update_company_category
    @categories VARCHAR(8000),
    @id_company INT
AS
/*

Descrição: insere e altera as categorias de uma empresa | recebe uma string com a lista das categorias separadas por tabulação ("/t" no .NET)

*/
BEGIN
	IF ISNULL(OBJECT_ID('tempdb..#category'), 0) <> 0
		DROP TABLE #category

	CREATE TABLE #category (id_category VARCHAR(8000))

    SET @categories = (SELECT REPLACE(@categories, ';', CHAR(10)))
    Exec pr_string_table_temp @categories, '#category', 'id_category'

	DELETE FROM tb_company_category WHERE id_company = @id_company

    INSERT INTO
        tb_company_category
    SELECT 
		@id_company, id_category
    FROM
		#category
END