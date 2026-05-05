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
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (1, 1, 9, 'Uma obra-prima absoluta!', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (1, 1, 8, 'Muito bom, mas longo demais.', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, serie_id, nota, comentario, data_criacao) VALUES (1, 1, 10, 'A melhor serie de todos os tempos.', CURRENT_TIMESTAMP());
INSERT INTO avaliacoes(usuario_id, filme_id, nota, comentario, data_criacao) VALUES (1, 5, 9, 'Nolan e genial.', CURRENT_TIMESTAMP());

-- Favoritos (filmeId/serieId referenciam IDs da API C#)
INSERT INTO favoritos(usuario_id, filme_id, data_adicionado) VALUES (1, 1, CURRENT_TIMESTAMP());
INSERT INTO favoritos(usuario_id, filme_id, data_adicionado) VALUES (1, 5, CURRENT_TIMESTAMP());
INSERT INTO favoritos(usuario_id, serie_id, data_adicionado) VALUES (1, 1, CURRENT_TIMESTAMP());

-- Posts
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (1, 'Oppenheimer e o filme do ano sem duvida alguma! A cena da detonacao e inesquecivel.', CURRENT_TIMESTAMP());
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (1, 'Acabei de maratonar Breaking Bad pela terceira vez. Ainda e perfeito.', CURRENT_TIMESTAMP());
INSERT INTO posts(usuario_id, conteudo, data_criacao) VALUES (1, 'Alguem mais ficou confuso com Dark nas primeiras temporadas? Vale muito a pena!', CURRENT_TIMESTAMP());
