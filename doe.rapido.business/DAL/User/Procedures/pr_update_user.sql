IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_update_user')
DROP PROCEDURE pr_update_user
GO

CREATE PROCEDURE pr_update_user
    @id_user                INT,
    @ds_name                VARCHAR(50),
    @ds_email               VARCHAR(100),
    @ds_password            VARCHAR(255),
    @nr_code_confirm        INT = NULL,  
    @dt_expire_code_confirm SMALLDATETIME = NULL,
    @fg_confirmed           BIT = NULL,
    @ds_step_onboarding     VARCHAR(50)
AS
/*

Descrição: altera usuário

*/
BEGIN
    UPDATE
        tb_user
    SET
        ds_name                = @ds_name, 
        ds_email               = @ds_email, 
        ds_password            = @ds_password, 
        fg_confirmed           = @fg_confirmed, 
        nr_code_confirm        = @nr_code_confirm, 
        dt_expire_code_confirm = @dt_expire_code_confirm, 
        ds_step_onboarding     = @ds_step_onboarding,
        dt_update              = DATEADD(HOUR, -3, GETDATE())
    WHERE
        id_user = @id_user
END