IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_select_user_by_login')
DROP PROCEDURE pr_select_user_by_login
GO

CREATE PROCEDURE pr_select_user_by_login
    @ds_email VARCHAR(100),
    @ds_password VARCHAR(255)
AS
/*

Descrição: seleciona um usuário pelo seu login (caso haja)

*/
BEGIN
    SELECT * FROM tb_user WITH(NOLOCK) WHERE ds_email = @ds_email AND ds_password = @ds_password
END