IF OBJECT_ID ('tb_user') IS NOT NULL
	DROP TABLE tb_user
CREATE TABLE tb_user
(
  id_user INT PRIMARY KEY,
  ds_name VARCHAR(50) NOT NULL,
  ds_email VARCHAR(100) NOT NULL,
  ds_password VARCHAR(255) NOT NULL,
  fg_confirmed BIT NOT NULL,
  nr_code_confirm INT NULL,
  dt_expire_code_confirm SMALLDATETIME NULL,
  dt_include SMALLDATETIME NOT NULL,
  dt_update SMALLDATETIME NULL,
  ds_step_onboarding VARCHAR(50)
)

IF OBJECT_ID ('tb_company') IS NOT NULL
	DROP TABLE tb_company
CREATE TABLE tb_company
(
  id_company INT PRIMARY KEY,
  id_user INT NOT NULL UNIQUE FOREIGN KEY (id_user) REFERENCES tb_user (id_user) ON DELETE CASCADE,
  nr_latitude FLOAT NOT NULL,
  nr_longitude FLOAT NOT NULL,
  ds_trading_name VARCHAR(MAX) NOT NULL,
  ds_name VARCHAR(MAX) NOT NULL,
  nr_cnpj VARCHAR(14) NOT NULL,
  nr_cep VARCHAR(8) NOT NULL,
  ds_street VARCHAR(100) NOT NULL,
  nr_number VARCHAR(20) NOT NULL,
  ds_district VARCHAR(100) NOT NULL,
  ds_city VARCHAR(50) NOT NULL,
  ds_state VARCHAR(20) NOT NULL,
  nr_phone VARCHAR(20) NULL,
  nr_phone_whatsapp VARCHAR(20) NULL,
  ds_email VARCHAR(100) NULL,
  ds_image VARCHAR(MAX) NULL,
  dt_include SMALLDATETIME NOT NULL,
  dt_update SMALLDATETIME NULL
)

IF OBJECT_ID ('tb_category') IS NOT NULL
	DROP TABLE tb_category
CREATE TABLE tb_category
(
  id_category INT IDENTITY(1,1) PRIMARY KEY,
  ds_name VARCHAR(50) NOT NULL
)

IF OBJECT_ID ('tb_company_category') IS NOT NULL
	DROP TABLE tb_company_category
CREATE TABLE tb_company_category
(
  id_company INT NOT NULL FOREIGN KEY (id_company) REFERENCES tb_company (id_company) ON DELETE CASCADE,
  id_category INT NOT NULL FOREIGN KEY (id_category) REFERENCES tb_category (id_category) ON DELETE CASCADE,
  PRIMARY KEY(id_company, id_category)
)

IF OBJECT_ID ('tb_suggestion_category') IS NOT NULL
	DROP TABLE tb_suggestion_category
CREATE TABLE tb_suggestion_category
(
  id_suggestion INT IDENTITY(1,1) PRIMARY KEY,
  ds_suggestion VARCHAR(MAX) NOT NULL,
  id_user_include INT NULL,
  dt_include SMALLDATETIME NOT NULL
)

IF OBJECT_ID ('tb_company_commentary') IS NOT NULL
	DROP TABLE tb_company_commentary
CREATE TABLE tb_company_commentary
(
  id_commentary INT IDENTITY(1,1) PRIMARY KEY,
  id_company INT NOT NULL,
  id_user INT NOT NULL,
  ds_commentary VARCHAR(MAX) NOT NULL,
  nr_like INT NULL,
  nr_deslike INT NULL,
  dt_include SMALLDATETIME NOT NULL
)