IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_update_company')
DROP PROCEDURE pr_update_company
GO

CREATE PROCEDURE pr_update_company
    @id_company INT,
    @nr_latitude FLOAT,
    @nr_longitude FLOAT,
    @ds_trading_name VARCHAR(MAX),
    @ds_name VARCHAR(MAX),
    @nr_cnpj VARCHAR(14),
    @nr_cep VARCHAR(8),
    @ds_street VARCHAR(100),
    @nr_number VARCHAR(20),
    @ds_district VARCHAR(100),
    @ds_city VARCHAR(50),
    @ds_state VARCHAR(20),
    @ds_image VARCHAR(MAX) = NULL,
    @nr_phone VARCHAR(20) = NULL,
    @nr_phone_whatsapp VARCHAR(20) = NULL,
    @ds_email VARCHAR(100) = NULL
AS
/*

Descrição: altera uma empresa

*/
BEGIN
    UPDATE
        tb_company
    SET
        nr_latitude     = @nr_latitude,
        nr_longitude    = @nr_longitude,
        ds_trading_name = @ds_trading_name,
        ds_name         = @ds_name,
        nr_cnpj         = @nr_cnpj,
        nr_cep          = @nr_cep,
        ds_street       = @ds_street,
        nr_number       = @nr_number,
        ds_district     = @ds_district,
        ds_city         = @ds_city,
        ds_state        = @ds_state,
        ds_image        = @ds_image,
        nr_phone        = @nr_phone,
        nr_phone_whatsapp = @nr_phone_whatsapp,
        ds_email        = @ds_email,
        dt_update       = DATEADD(HOUR, -3, GETDATE())
    WHERE
        id_company = @id_company
END