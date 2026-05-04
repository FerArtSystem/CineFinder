# CineFinder Java API — Spring Boot

## Pré-requisitos

Antes de rodar, instale:

1. **Java 21** — https://adoptium.net/temurin/releases/?version=21
   - Baixe o instalador `.msi` (Windows x64)
   - Marque "Set JAVA_HOME" durante a instalação

2. **Maven 3.9+** — https://maven.apache.org/download.cgi
   - Baixe o `apache-maven-3.9.x-bin.zip`
   - Extraia para `C:\maven`
   - Adicione `C:\maven\bin` ao PATH do sistema

## Como rodar

```powershell
cd cinefinder-java
mvn spring-boot:run
```

A API estará disponível em **http://localhost:8080/api**

## Banco de dados

Usa H2 in-memory (não precisa instalar nada).
Console H2 disponível em: http://localhost:8080/h2-console
- JDBC URL: `jdbc:h2:mem:cineFinderDb`
- User: `sa` / Senha: (vazia)

## Endpoints (mesmos da API C#)

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/filmes | Listar filmes |
| GET | /api/filmes/{id} | Buscar filme |
| GET | /api/filmes/buscar?titulo= | Buscar por título |
| GET | /api/filmes/genero/{id} | Filtrar por gênero |
| POST | /api/filmes | Criar filme |
| PUT | /api/filmes/{id} | Atualizar filme |
| DELETE | /api/filmes/{id} | Deletar filme |
| GET | /api/series | Listar séries |
| GET | /api/series/{id} | Buscar série |
| GET | /api/series/buscar?titulo= | Buscar por título |
| GET | /api/series/genero/{id} | Filtrar por gênero |
| GET | /api/generos | Listar gêneros |
| POST | /api/usuarios | Cadastrar usuário |
| POST | /api/usuarios/login | Login |
| GET | /api/avaliacoes/filme/{id} | Avaliações do filme |
| GET | /api/avaliacoes/serie/{id} | Avaliações da série |
| POST | /api/avaliacoes | Criar avaliação |
| GET | /api/favoritos/usuario/{id} | Favoritos do usuário |
| POST | /api/favoritos | Adicionar favorito |
| DELETE | /api/favoritos/{id} | Remover favorito |
| GET | /api/posts | Listar posts |
| POST | /api/posts | Criar post |

## Trocar API no frontend

Abra `CineFinder.API/wwwroot/cf/config.js` e mude:
```js
API_ATIVA: 'java',  // 'csharp' ou 'java'
```

## Credenciais de teste

Email: `admin@cinefinder.com` / Senha: `senha123`
Email: `joao@email.com` / Senha: `senha123`
