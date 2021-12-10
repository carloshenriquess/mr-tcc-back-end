/* SCRIPT DE CHAMADAS DAS PROCEDURES */

/*Inserir categorias (script fixo e interno)*/
--INSERT INTO tb_category VALUES ('Alimentos')
--INSERT INTO tb_category VALUES ('Higiene')
--INSERT INTO tb_category VALUES ('Roupas')
SELECT * FROM tb_category

/*Inserir um usuário*/
--EXEC pr_insert_user 'Usuário 1', 'usuario@email.com.br', 'senha123'
SELECT * FROM tb_user

/*Fazer login de um usuário*/
--EXEC pr_select_user_by_login 'usuario@email.com.br', 'senha123'

/*Alterar um usuário*/
--EXEC pr_update_user 1, 'Usuário Novo', 'usuarionovo@email.com.br', 'senhaNova123', 54321, DATEADD(HOUR, -3, GETDATE()), NULL
SELECT * FROM tb_user

/*Inserir uma empresa*/
--EXEC pr_insert_company 23.23, 52.52, 'Empresa 1', 'Empresa 1 LTDA', '12345678998745', '02574120', 'Rua Empresa 1', '1', 'Bairro Empresa 1',
--					   'Cidade Empresa 1', 'Estado Empresa 1', '112587410', '1192587410', 'email@empresa.com.br', 'abc123', 1
SELECT * FROM tb_company

/*Alterar uma empresa*/
--EXEC pr_update_company 1, 23.23, 52.52, '1 Empresa', 'LTDA 1 Empresa', '98745632101236', '36985201', '1 Empresa Rua', '2', '1 Empresa Bairro',
--					   '1 Empresa Cidade', '1 Empresa Estado', '321bca', '112587410', 'email@empresa.com.br'
SELECT * FROM tb_company

/*Inserir/Alterar categorias de uma empresa*/
--DECLARE @STRING_CATEGORIES VARCHAR(8000) = CONVERT(VARCHAR(8000), (SELECT '1' + CHAR(10) + '2'))
--EXEC pr_insert_update_company_category @STRING_CATEGORIES, 1
SELECT * FROM tb_company_category

/*Selecionar um usuário por email*/
EXEC pr_select_user_by_email 'email_usuario@email.com.br'

/*Confirmar usuário (flag de confirmação)*/
--EXEC pr_confirm_user 'email_usuario@email.com.br', 12345
SELECT * FROM tb_user

/*Selecionar um usuário pelo seu id*/
EXEC pr_select_user_by_id 1

/*Selecionar as categorias pelo id de uma empresa*/
EXEC pr_select_category_by_id_company 1

/*Selecionar uma empresa pelo seu id*/
EXEC pr_select_company_by_id 1

/*Selecionar a lista das empresas que possuem as categorias filtradas*/
DECLARE @STRING_CATEGORIES VARCHAR(8000) = CONVERT(VARCHAR(8000), (SELECT '1;2'))
EXEC pr_select_list_company_by_categories @STRING_CATEGORIES

/*Selecionar a lista das empresas de um determinado estado*/
EXEC pr_select_list_company_by_state 'SP'

/*Excluir dados de um usuário/empresa*/
--EXEC pr_delete_user_company 1
SELECT * FROM tb_user
SELECT * FROM tb_company
SELECT * FROM tb_company_category