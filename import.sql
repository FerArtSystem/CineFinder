-- ============================================================
-- CineFinder - import.sql
-- Dados de teste para o banco SQL Server (CineFinderDb)
-- Execute APÓS rodar as migrations: dotnet ef database update
-- ============================================================

-- -------------------------
-- GENEROS
-- -------------------------
INSERT INTO Generos (Nome) VALUES
    ('Ação'),
    ('Drama'),
    ('Comédia'),
    ('Terror'),
    ('Ficção Científica'),
    ('Animação'),
    ('Thriller');

-- -------------------------
-- FILMES
-- -------------------------
INSERT INTO Filmes (Titulo, Sinopse, DataLancamento, DuracaoMinutos, NotaMedia, PosterUrl, GeneroId) VALUES
    ('O Poderoso Chefão',
     'A saga da família mafiosa Corleone, liderada por Vito Corleone, e a ascensão de seu filho Michael.',
     '1972-03-24', 175, 9.2,
     'https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsLeBHka3nHkS.jpg', 2),

    ('Vingadores: Ultimato',
     'Após o devastador ataque de Thanos, os Vingadores se reúnem para reverter o caos.',
     '2019-04-26', 181, 8.4,
     'https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg', 1),

    ('Interestelar',
     'Um grupo de astronautas viaja por um buraco de minhoca em busca de um novo lar para a humanidade.',
     '2014-11-07', 169, 8.6,
     'https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg', 5),

    ('Parasita',
     'Uma família pobre se infiltra na vida de uma família rica de formas cada vez mais obscuras.',
     '2019-05-30', 132, 8.5,
     'https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg', 7),

    ('Coco',
     'Um jovem aspirante a músico entra acidentalmente no Reino dos Mortos e descobre sua história familiar.',
     '2017-10-27', 105, 8.4,
     'https://image.tmdb.org/t/p/w500/gGEsBPAijhVUFoiNpgZXqRVWJt2.jpg', 6),

    ('Get Out',
     'Um jovem negro visita a família da namorada branca e descobre algo perturbador.',
     '2017-02-24', 104, 7.7,
     'https://image.tmdb.org/t/p/w500/tFXcEccSQMf3lfhfXKSU9iRBpa3.jpg', 4),

    ('Superbad',
     'Dois amigos do ensino médio tentam aproveitar ao máximo sua última festa antes da formatura.',
     '2007-08-17', 113, 7.6,
     'https://image.tmdb.org/t/p/w500/ek8e8txUyUwd2BNqj6lFEerJfms.jpg', 3);

-- -------------------------
-- SERIES
-- -------------------------
INSERT INTO Series (Titulo, Sinopse, NumTemporadas, DataEstreia, NotaMedia, PosterUrl, GeneroId) VALUES
    ('Breaking Bad',
     'Um professor de química do ensino médio com câncer terminal torna-se produtor de metanfetamina.',
     5, '2008-01-20', 9.5,
     'https://image.tmdb.org/t/p/w500/ggFHVNu6YYI5L9pCfOacjizRGt.jpg', 7),

    ('Dark',
     'Um thriller de ficção científica alemão que entrelaça quatro famílias em ciclos temporais.',
     3, '2017-12-01', 8.8,
     'https://image.tmdb.org/t/p/w500/apbrbWs8M9lyOpJYU5WXrpFbk1Z.jpg', 5),

    ('The Bear',
     'Um chef renomado retorna a Chicago para administrar o restaurante de sanduíches de sua família.',
     3, '2022-06-23', 8.7,
     'https://image.tmdb.org/t/p/w500/sHFlbKS3WLqMnp9t2ghADIJFnuQ.jpg', 2),

    ('Stranger Things',
     'Um grupo de crianças se depara com forças sobrenaturais ao investigar o desaparecimento de um amigo.',
     4, '2016-07-15', 8.7,
     'https://image.tmdb.org/t/p/w500/49WJfeN0moxb9IPfGn8AIqMGskD.jpg', 5),

    ('Chernobyl',
     'A minissérie narra os eventos do desastre nuclear de Chernobyl em 1986.',
     1, '2019-05-06', 9.3,
     'https://image.tmdb.org/t/p/w500/hlLXt2tOPT6RRnjiUmoxyG1LTFi.jpg', 2);

-- -------------------------
-- USUARIOS
-- Senhas em BCrypt (todas as senhas originais são: senha123)
-- Hash gerado com cost=11 para demonstração
-- -------------------------
INSERT INTO Usuarios (Nome, Email, SenhaHash, Nickname, DataCadastro) VALUES
    ('João Victor',
     'joaovictordevsm@gmail.com',
     '$2a$11$wALjHyIE3gLzChX/T1OwXOjlkWJpCBZ2X0dBHZGKN6zw5gLzXaO3i',
     'joao_dev',
     '2024-01-10 08:00:00'),

    ('Ivan Junior',
     '13ivanvitorinojunior@gmail.com',
     '$2a$11$wALjHyIE3gLzChX/T1OwXOjlkWJpCBZ2X0dBHZGKN6zw5gLzXaO3i',
     'ivan_jr',
     '2024-01-12 09:30:00'),

    ('Maria Clara',
     'mariaclara@email.com',
     '$2a$11$wALjHyIE3gLzChX/T1OwXOjlkWJpCBZ2X0dBHZGKN6zw5gLzXaO3i',
     'mariclara_cinefan',
     '2024-02-05 14:00:00'),

    ('Pedro Alves',
     'pedro.alves@email.com',
     '$2a$11$wALjHyIE3gLzChX/T1OwXOjlkWJpCBZ2X0dBHZGKN6zw5gLzXaO3i',
     'pedrofilm',
     '2024-03-01 11:00:00');

-- -------------------------
-- AVALIACOES
-- -------------------------
INSERT INTO Avaliacoes (UsuarioId, FilmeId, SerieId, Nota, Comentario, DataCriacao) VALUES
    (1, 1, NULL, 10, 'Obra-prima absoluta. Roteiro perfeito, atuações inesquecíveis.', '2024-02-01 10:00:00'),
    (1, 3, NULL, 9,  'Nolan no seu melhor. A física misturada com emoção é de arrepiar.', '2024-02-03 15:00:00'),
    (2, NULL, 1, 10, 'Melhor série de todos os tempos. Heisenberg é um personagem épico.', '2024-02-10 20:00:00'),
    (3, 4, NULL, 9,  'Bong Joon-ho é gênio. Crítica social embalada em thriller perfeito.', '2024-02-15 18:30:00'),
    (3, NULL, 2, 8,  'Dark me fez questionar o espaço-tempo. Complexo mas valeu cada minuto.', '2024-02-20 21:00:00'),
    (4, 2, NULL, 8,  'Épico e emocionante. O melhor final de saga.', '2024-03-05 17:00:00'),
    (4, NULL, 4, 9,  'Stranger Things é nostalgia e tensão combinados. Temporada 4 foi incrível.', '2024-03-10 19:00:00');

-- -------------------------
-- FAVORITOS
-- -------------------------
INSERT INTO Favoritos (UsuarioId, FilmeId, SerieId, DataAdicionado) VALUES
    (1, 1,    NULL, '2024-02-01 10:05:00'),
    (1, 3,    NULL, '2024-02-03 15:05:00'),
    (1, NULL, 1,    '2024-02-10 20:05:00'),
    (2, NULL, 1,    '2024-02-11 08:00:00'),
    (2, NULL, 2,    '2024-02-22 09:00:00'),
    (3, 4,    NULL, '2024-02-15 18:35:00'),
    (4, 2,    NULL, '2024-03-05 17:05:00'),
    (4, NULL, 4,    '2024-03-10 19:05:00');

-- -------------------------
-- POSTS (Comunidade)
-- -------------------------
INSERT INTO Posts (UsuarioId, Conteudo, DataCriacao, DataEdicao) VALUES
    (1,
     'Acabei de reassistir O Poderoso Chefão pela quinta vez. Cada detalhe que o Coppola colocou é incrível. Alguém mais tem esse vício?',
     '2024-02-02 11:00:00', NULL),

    (2,
     'Breaking Bad é, sem dúvida, a melhor série já produzida. A evolução do Walter White é uma aula de escrita de personagem. Quem ainda não assistiu, por favor, se resolve!',
     '2024-02-11 21:00:00', NULL),

    (3,
     'Pessoal, alguém conseguiu entender completamente a linha do tempo de Dark? Fiz um mapa mas ainda perco o fio... kkk. Aceito ajuda!',
     '2024-02-23 14:00:00', NULL),

    (4,
     'Finalmente assisti Interestelar. Chorei no final. Aquela cena da conta de e-mails do Cooper é cruel e linda ao mesmo tempo.',
     '2024-03-06 10:00:00', NULL),

    (1,
     'Recomendação da semana: Chernobyl na HBO. É uma minissérie curta mas absolutamente devastadora. Nota 10 sem pestanejar.',
     '2024-03-15 09:00:00', NULL);
