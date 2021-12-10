IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_insert_user')
DROP PROCEDURE pr_insert_user
GO

CREATE PROCEDURE pr_insert_user
    @ds_name VARCHAR(50),
    @ds_email VARCHAR(100),
    @ds_password VARCHAR(255),
    @nr_code_confirm INT = NULL, 
    @dt_expire_code_confirm SMALLDATETIME = NULL,
    @ds_step_onboarding VARCHAR(50)
AS
/*

Descrição: insere usuário

*/
BEGIN
    DECLARE @id_user INT, @cont INT = 0

    WHILE (@cont = 0)
    BEGIN
        SET @id_user = (SELECT CAST(RAND()*(99999-10000+1) AS INT)+10000)
        IF NOT EXISTS(SELECT 1 FROM tb_user WITH(NOLOCK) WHERE id_user = @id_user)
            SET @cont = 1
    END

    INSERT INTO 
        tb_user (id_user, ds_name, ds_email, ds_password, fg_confirmed, nr_code_confirm, dt_expire_code_confirm, dt_include, ds_step_onboarding)
    VALUES
        (@id_user, @ds_name, @ds_email, @ds_password, 0, @nr_code_confirm, @dt_expire_code_confirm, DATEADD(HOUR, -3, GETDATE()), @ds_step_onboarding)

	SELECT @id_user AS id_user
END