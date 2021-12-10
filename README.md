# DOE.RAPIDO

![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge)

## Índice

- [Índice](#índice)
- [Descrição do Projeto](#descrição-do-projeto)
- [Funcionalidades do Projeto](#Funcionalidades-do-Projeto)
- [Abrir e rodar o projeto](#Abrir-e-rodar-o-projeto)
- [Tecnologias utilizadas](#tecnologias-utilizadas)
- [Autores](#pessoas-desenvolvedoras)

## Descrição do projeto

- A ideia dessa solução, é buscar, cadastrar e editar dados de instituições, usuários, cadastradas no banco de dados, e tratá-los para retornar ao front.

- Também nessa API existe a conexão com a API do Google maps, para buscar a distância e outros dados úteis para o usuário e exibir no front.

## :hammer: Funcionalidades do projeto

- `Funcionalidade 1`: É possível pesquisar instituições próximas a sua residência para que possa doar itens.
- `Funcionalidade 1a`: É possível obter informações de contato da instituição selecionada na busca
- `Funcionalidade 2`: É possível cadastrar sua instituição para que possa receber itens afim de ajudar outras pessoas.
- `Funcionalidade 3`: É possível se conectar com a API do Google Maps e tratar diversos resultados das coordenadas utilizadas como parâmetro

## 🛠️ Abrir e rodar o projeto

**Após baixar a solução, é necessário configurar o arquivo 'Settings' dentro do projeto doe.rapido.api com os acessos de uma conta do azure dev ops server,**
**editando a string de 'ConnectionStringStorageAzure' com a sua própria string de conexao do azure, e o nome do seu container, para salvar os arquivos.**
**Também é necessário uma conta de email do google, para envio de email, junto a uma chave da API do Google Maps para acesso as APIS de busca do mesmo.**
**Por fim, editar a string de conexão do arquivo DataBase.cs que está dentro do projeto doe.rapido.data com o seu acesso.**

## Tecnologias utilizadas

** Nesse projeto foi utilizado a linguagem c#, .net 6.0 para desenvolvimento da API, e conceitos de banco de dados, como procedures e afins. **

## Autores

| [<img src="https://avatars.githubusercontent.com/u/35262475?v=4" width=115><br><sub>Carlos Henrique</sub>](https://github.com/carloshenriquess) | [<img src="https://avatars.githubusercontent.com/u/40438354?v=4" width=115><br><sub>Guilherme Nunes</sub>](https://github.com/gnunesinf) | [<img src="https://avatars.githubusercontent.com/u/90986499?v=4" width=115><br><sub>Lucas Ferraz</sub>](https://github.com/Ferraz2000) | [<img src="https://avatars.githubusercontent.com/u/48872445?v=4" width=115><br><sub>José Vitor</sub>](https://github.com/j-vitor-silva) |
| :---------------------------------------------------------------------------------------------------------------------------------------------: | :--------------------------------------------------------------------------------------------------------------------------------------: | :------------------------------------------------------------------------------------------------------------------------------------: | --------------------------------------------------------------------------------------------------------------------------------------- |
