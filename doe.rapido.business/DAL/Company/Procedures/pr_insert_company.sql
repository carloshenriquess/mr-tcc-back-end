IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_insert_company')
DROP PROCEDURE pr_insert_company
GO

CREATE PROCEDURE pr_insert_company
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
    @nr_phone VARCHAR(20) = NULL,
    @nr_phone_whatsapp VARCHAR(20) = NULL,
    @ds_email VARCHAR(100) = NULL,
    @ds_image VARCHAR(MAX) = NULL,
    @id_user INT
AS
/*

Descrição: insere uma empresa

*/
BEGIN
    DECLARE @id_company INT, @cont INT = 0

    WHILE (@cont = 0)
    BEGIN
        SET @id_company = (SELECT CAST(RAND()*(99999-10000+1) AS INT)+10000)
        IF NOT EXISTS(SELECT 1 FROM tb_company WITH(NOLOCK) WHERE id_company = @id_company)
            SET @cont = 1
    END

    INSERT INTO 
        tb_company (id_company, nr_latitude, nr_longitude, ds_trading_name, ds_name, nr_cnpj, nr_cep, ds_street, nr_number, ds_district,
                    ds_city, ds_state, nr_phone, nr_phone_whatsapp, ds_email, ds_image, dt_include, id_user)
    VALUES
        (@id_company, @nr_latitude, @nr_longitude, @ds_trading_name, @ds_name, @nr_cnpj, @nr_cep, @ds_street, @nr_number, @ds_district,
                    @ds_city, @ds_state, @nr_phone, @nr_phone_whatsapp, @ds_email, @ds_image, DATEADD(HOUR, -3, GETDATE()), @id_user)

    SELECT @id_company AS id_company
END