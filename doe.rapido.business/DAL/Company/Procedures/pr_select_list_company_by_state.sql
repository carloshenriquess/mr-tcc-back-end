IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_list_company_by_state')
DROP PROCEDURE pr_select_list_company_by_state
GO

CREATE PROCEDURE pr_select_list_company_by_state
    @ds_state VARCHAR(20)
AS
/*

Descrição: seleciona uma lista com as empresas de um determinado estado

*/
BEGIN
    SELECT tc.* FROM 
		tb_company tc WITH(NOLOCK) 
	JOIN
		tb_user tu WITH(NOLOCK) ON tu.id_user = tc.id_user 
	WHERE 
		ds_state = @ds_state AND tu.ds_step_onboarding = 'finished'

    SELECT tcc.* FROM 
        tb_company tc WITH(NOLOCK)
    JOIN 
        tb_company_category tcc WITH(NOLOCK) ON tcc.id_company = tc.id_company
    JOIN
		tb_user tu WITH(NOLOCK) ON tu.id_user = tc.id_user
    WHERE
        tc.ds_state = @ds_state AND tu.ds_step_onboarding = 'finished'
END