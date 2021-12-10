IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_confirm_user')
DROP PROCEDURE pr_confirm_user
GO

CREATE PROCEDURE pr_confirm_user
    @ds_email VARCHAR(100),
    @nr_code_confirm INT
AS
/*

Descrição: altera a flag de confirmação do usuário para true (1) caso os dados estejam corretos
           retorna um bool sobre a confirmação (true ou false)

*/
BEGIN
    IF EXISTS(SELECT 1 FROM tb_user WITH(NOLOCK) WHERE ds_email = @ds_email AND nr_code_confirm = @nr_code_confirm)
    BEGIN
        DECLARE @date_expire SMALLDATETIME = (SELECT dt_expire_code_confirm FROM tb_user WITH(NOLOCK) WHERE ds_email = @ds_email AND nr_code_confirm = @nr_code_confirm)

        IF (@date_expire < DATEADD(HOUR, -3, GETDATE()))
            SELECT 0 AS fg_confirmed
        ELSE
        BEGIN
            UPDATE tb_user SET fg_confirmed = 1 WHERE ds_email = @ds_email AND nr_code_confirm = @nr_code_confirm

            SELECT 1 AS fg_confirmed
        END
    END
    ELSE
        SELECT 0 AS fg_confirmed
END