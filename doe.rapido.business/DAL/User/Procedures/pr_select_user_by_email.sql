IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_user_by_email')
DROP PROCEDURE pr_select_user_by_email
GO

CREATE PROCEDURE pr_select_user_by_email
    @ds_email VARCHAR(100)
AS
/*
Descrição: seleciona o usuário pelo e-mail
*/
BEGIN
    SELECT * FROM tb_user WITH(NOLOCK) WHERE ds_email = @ds_email
END