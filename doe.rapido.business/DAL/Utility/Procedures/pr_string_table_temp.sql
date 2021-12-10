IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'pr_string_table_temp')
DROP PROCEDURE pr_string_table_temp
GO

CREATE Procedure pr_string_table_temp
    @VarSeparar Varchar(8000),
    @TabTemp Varchar(50),
    @Campos Varchar(500),
    @Delimitador Char(1) = NULL,
    @DelCampo Char(1) = NULL
AS
/*

Descrição: procedure auxiliar que serializa a lista de categorias em uma tabela temporária

*/
Begin   
    Declare @Aux Varchar(8000), @Sql Varchar(8000), @ValorCampo Varchar(8000), @Parar Char(1), @PararCampo Char(1)
    Select @Parar = 'N'
    Select @PararCampo = 'N'
    --Caso os  delimitadores não tenham sido especificados, utiliza os delimitadores padrões.
    If (@Delimitador IS NULL) SET @Delimitador = CHAR(10)
    If (@DelCampo IS NULL) SET @DelCampo = CHAR(9)
    
    --Insere na tabela temporária
    If(@VarSeparar Is Not Null)
    Begin
        While DATALENGTH(@VarSeparar) > 0
        Begin
            Select @Sql = 'Insert Into ' + @TabTemp + '(' + @Campos + ')' + ' Values ('
            --Se não existe separador, indica que o laço deve ser interrompido ao final 
            If CHARINDEX(@Delimitador, @VarSeparar) > 1 
            Begin
                 Select @Aux = LTRIM(SUBSTRING(@VarSeparar, 1, CHARINDEX(@Delimitador, @VarSeparar) - 1))
            End
            Else
            Begin
                 Set @Parar = 'S'
                 Select @Aux = @VarSeparar
            End
            While DATALENGTH(@Aux) > 0
            Begin
                 If CHARINDEX(@DelCampo, @Aux) > 1 
                 Begin
                      Select @ValorCampo = LTRIM(SUBSTRING(@Aux, 1, CHARINDEX(@DelCampo, @Aux) - 1))
                 End
                 Else
                 Begin
                      Select @ValorCampo = @Aux
                      Select @PararCampo = 'S'
                 End
                 --Se o valor do campo não for nulo, adiciona o valor do campo.
                 IF(@ValorCampo <> 'NULO')
                    Select @Sql = @Sql + CHAR(39) +  @ValorCampo + CHAR(39) 
                 ELSE
                    Select @Sql = @Sql + 'NULL'
                 If(@PararCampo = 'N')
                 Begin
                      Select @Sql = @Sql + ','
                      Select @Aux = SUBSTRING(@Aux, CHARINDEX(@DelCampo, @Aux) + 1, DATALENGTH(@Aux))
                 End
                 Else
                 Begin
                      Select @PararCampo = 'N'
                      Break
                 End
            End
              
            Select @Sql = @Sql + ')'
            
            exec(@Sql)
            
            Select @VarSeparar = SUBSTRING(@VarSeparar, CHARINDEX(@Delimitador, @VarSeparar) + 1, DATALENGTH(@VarSeparar))
            
            If @Parar = 'S'
                Break
        End
    End
    Return 0
End