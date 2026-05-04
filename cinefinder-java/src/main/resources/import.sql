-- ============================================================
-- IMPORT.SQL — Java API (apenas dados sociais)
-- Conteudo (filmes/series/generos) gerenciado pela API C#
-- ============================================================

-- Usuarios (senha = "senha123" com BCrypt)
INSERT INTO usuarios(nome, email, senha_hash, nickname, data_cadastro)
VALUES ('Admin CineFinder', 'admin@cinefinder.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lh7y', 'admin', CURRENT_TIMESTAMP());

INSERT INTO usuarios(nome, email, senha_hash, nickname, data_cadastro)
VALUES ('Joao Silva', 'joao@email.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lh7y', 'joaoS', CURRENT_TIMESTAMP());

INSERT INTO usuarios(nome, email, senha_hash, nickname, data_cadastro)
VALUES ('Maria Souza', 'maria@email.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lh7y', 'mariaS', CURRENT_TIMESTAMP());

-- Avaliacoes (filmeId/serieId referenciam IDs da API C#)
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (2, 1, 9, 'Uma obra-prima absoluta!', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (3, 1, 8, 'Muito bom, mas longo demais.', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, serie_id, nota, comentario, data_criacao) VALUES (2, 1, 10, 'A melhor serie de todos os tempos.', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (3, 5, 9, 'Nolan e genial.', CURRENT_TIMESTAMP());

-- Favoritos (filmeId/serieId referenciam IDs da API C#)
INSERT INTO favoritos(usuario_id, filme_id, data_adicionado) VALUES (2, 1, CURRENT_TIMESTAMP());
INSERT INTO favoritos(usuario_id, filme_id, data_adicionado) VALUES (2, 5, CURRENT_TIMESTAMP());
INSERT INTO favoritos(usuario_id, serie_id, data_adicionado) VALUES (3, 1, CURRENT_TIMESTAMP());

-- Posts
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (2, 'Oppenheimer e o filme do ano sem duvida alguma! A cena da detonacao e inesquecivel.', CURRENT_TIMESTAMP());
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (3, 'Acabei de maratonar Breaking Bad pela terceira vez. Ainda e perfeito.', CURRENT_TIMESTAMP());
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (2, 'Alguem mais ficou confuso com Dark nas primeiras temporadas? Vale muito a pena!', CURRENT_TIMESTAMP());


-- Filmes
INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('Oppenheimer', 'A história do pai da bomba atômica e o dilema moral que definiu o mundo moderno.', '2023-07-21', 180, 8.9, NULL, 1);

INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('Duna: Parte Dois', 'Paul Atreides une forças com os Fremen em sua vingança contra os que destruíram sua família.', '2024-03-01', 166, 8.5, NULL, 5);

INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('Barbie', 'Barbie e Ken viajam para o mundo real e descobrem as complexidades da vida humana.', '2023-07-21', 114, 7.1, NULL, 3);

INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('Parasita', 'Uma família pobre se infiltra na vida de uma família rica com consequências inesperadas.', '2019-05-30', 132, 8.5, NULL, 7);

INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('Interestelar', 'Astronautas viajam por um buraco de minhoca em busca de um novo lar para a humanidade.', '2014-11-07', 169, 8.6, NULL, 5);

INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('O Poço', 'Um sistema carcerário vertical onde a comida desce mas nunca é suficiente para todos.', '2019-11-08', 94, 7.0, NULL, 7);

INSERT INTO filmes(titulo, sinopse, data_lancamento, duracao_minutos, nota_media, poster_url, genero_id)
VALUES ('A Origem', 'Um ladrão que rouba segredos do subconsciente recebe a missão de implantar uma ideia.', '2010-07-16', 148, 8.8, NULL, 1);

-- Séries
INSERT INTO series(titulo, sinopse, num_temporadas, data_estreia, nota_media, poster_url, genero_id)
VALUES ('Breaking Bad', 'Um professor de química se torna um temido fabricante de metanfetamina.', 5, '2008-01-20', 9.5, NULL, 7);

INSERT INTO series(titulo, sinopse, num_temporadas, data_estreia, nota_media, poster_url, genero_id)
VALUES ('The Last of Us', 'Joel deve escoltear Ellie através de um mundo pós-apocalíptico dominado por infectados.', 2, '2023-01-15', 8.8, NULL, 7);

INSERT INTO series(titulo, sinopse, num_temporadas, data_estreia, nota_media, poster_url, genero_id)
VALUES ('Stranger Things', 'Crianças de Hawkins enfrentam forças sobrenaturais vindas do Mundo Invertido.', 4, '2016-07-15', 8.7, NULL, 5);

INSERT INTO series(titulo, sinopse, num_temporadas, data_estreia, nota_media, poster_url, genero_id)
VALUES ('Dark', 'Quatro famílias alemãs desvendam um mistério de viagem no tempo que conecta gerações.', 3, '2017-12-01', 8.8, NULL, 5);

INSERT INTO series(titulo, sinopse, num_temporadas, data_estreia, nota_media, poster_url, genero_id)
VALUES ('Chernobyl', 'A história real do desastre nuclear soviético de 1986 e seus heróis anônimos.', 1, '2019-05-06', 9.4, NULL, 2);

-- Usuários (senha = "senha123" com BCrypt)
INSERT INTO usuarios(nome, email, senha_hash, nickname, data_cadastro)
VALUES ('Admin CineFinder', 'admin@cinefinder.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lh7y', 'admin', CURRENT_TIMESTAMP());

INSERT INTO usuarios(nome, email, senha_hash, nickname, data_cadastro)
VALUES ('João Silva', 'joao@email.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lh7y', 'joaoS', CURRENT_TIMESTAMP());

INSERT INTO usuarios(nome, email, senha_hash, nickname, data_cadastro)
VALUES ('Maria Souza', 'maria@email.com', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lh7y', 'mariaS', CURRENT_TIMESTAMP());

-- Avaliações
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (2, 1, 9, 'Uma obra-prima absoluta!', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (3, 1, 8, 'Muito bom, mas longo demais.', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, serie_id, nota, comentario, data_criacao) VALUES (2, 1, 10, 'A melhor série de todos os tempos.', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (3, 5, 9, 'Nolan é genial.', CURRENT_TIMESTAMP());

-- Favoritos
INSERT INTO favoritos(usuario_id, filme_id, data_adicionado) VALUES (2, 1, CURRENT_TIMESTAMP());
INSERT INTO favoritos(usuario_id, filme_id, data_adicionado) VALUES (2, 5, CURRENT_TIMESTAMP());
INSERT INTO favoritos(usuario_id, serie_id, data_adicionado) VALUES (3, 1, CURRENT_TIMESTAMP());

-- Posts
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (2, 'Oppenheimer é o filme do ano sem dúvida alguma! A cena da detonação é inesquecível.', CURRENT_TIMESTAMP());
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (3, 'Acabei de maratonar Breaking Bad pela terceira vez. Ainda é perfeito.', CURRENT_TIMESTAMP());
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (2, 'Alguém mais ficou confuso com Dark nas primeiras temporadas? Vale muito a pena!', CURRENT_TIMESTAMP());
