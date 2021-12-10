IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_list_company_by_categories')
DROP PROCEDURE pr_select_list_company_by_categories
GO

CREATE PROCEDURE pr_select_list_company_by_categories
    @categories VARCHAR(8000)
AS
/*

Descrição: seleciona uma lista com as empresas que possuem as categorias filtradas
           recebe uma string com a lista das categorias separadas por ";"

*/
BEGIN
    IF ISNULL(OBJECT_ID('tempdb..#category'), 0) <> 0
		DROP TABLE #category

	CREATE TABLE #category (id_category VARCHAR(8000))

    SET @categories = (SELECT REPLACE(@categories, ';', CHAR(10)))
    Exec pr_string_table_temp @categories, '#category', 'id_category'

	IF ISNULL(OBJECT_ID('tempdb..#companies'), 0) <> 0
		DROP TABLE #companies

    SELECT tc.* INTO #companies FROM 
        tb_company tc WITH(NOLOCK)
    JOIN 
        tb_company_category tcc WITH(NOLOCK) ON tcc.id_company = tc.id_company
	JOIN
		#category c ON c.id_category = tcc.id_category
	JOIN
		tb_user tu WITH(NOLOCK) ON tu.id_user = tc.id_user
	WHERE
		tu.ds_step_onboarding = 'finished'
    GROUP BY
		tc.id_company
		,tc.id_user
		,tc.nr_latitude
		,tc.nr_longitude
		,tc.ds_trading_name
		,tc.ds_name
		,tc.nr_cnpj
		,tc.nr_cep
		,tc.ds_street
		,tc.nr_number
		,tc.ds_district
		,tc.ds_city
		,tc.ds_state
		,tc.nr_phone
		,tc.nr_phone_whatsapp
		,tc.ds_email
		,tc.ds_image
		,tc.dt_include
		,tc.dt_update

	SELECT * FROM #companies

	SELECT tcc.* FROM 
		tb_company_category tcc WITH(NOLOCK)
	JOIN
		#companies c WITH(NOLOCK) ON c.id_company = tcc.id_company
END