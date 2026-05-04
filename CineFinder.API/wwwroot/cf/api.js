// ============================================================
// API SERVICE — chama C# API e TMDB
// ============================================================

const Api = {
  async _req(url, opts = {}) {
    const res = await fetch(url, {
      headers: { 'Content-Type': 'application/json' },
      ...opts
    });
    if (!res.ok) {
      const msg = await res.text().catch(() => res.statusText);
      throw new Error(msg || `HTTP ${res.status}`);
    }
    if (res.status === 204) return null;
    return res.json();
  },

  get:    url       => Api._req(url),
  post:  (url, d)  => Api._req(url, { method: 'POST',   body: JSON.stringify(d) }),
  put:   (url, d)  => Api._req(url, { method: 'PUT',    body: JSON.stringify(d) }),
  del:    url       => Api._req(url, { method: 'DELETE' }),

  // --- Filmes ---
  getFilmes:          ()     => Api.get(apiUrl('/filmes')),
  getFilme:           id     => Api.get(apiUrl(`/filmes/${id}`)),
  buscarFilmes:       q      => Api.get(apiUrl(`/filmes/buscar?titulo=${encodeURIComponent(q)}`)),
  getFilmesPorGenero: gid    => Api.get(apiUrl(`/filmes/genero/${gid}`)),
  criarFilme:         data   => Api.post(apiUrl('/filmes'), data),
  atualizarFilme:    (id, d) => Api.put(apiUrl(`/filmes/${id}`), d),
  deletarFilme:       id     => Api.del(apiUrl(`/filmes/${id}`)),

  // --- Séries ---
  getSeries:          ()     => Api.get(apiUrl('/series')),
  getSerie:           id     => Api.get(apiUrl(`/series/${id}`)),
  buscarSeries:       q      => Api.get(apiUrl(`/series/buscar?titulo=${encodeURIComponent(q)}`)),
  getSeriesPorGenero: gid    => Api.get(apiUrl(`/series/genero/${gid}`)),

  // --- Gêneros ---
  getGeneros: () => Api.get(apiUrl('/generos')),

  // --- Usuários (Social — Java) ---
  login:    data => Api._req(socialUrl('/usuarios/login'), { method: 'POST', body: JSON.stringify(data) }),
  cadastrar: data => Api._req(socialUrl('/usuarios'), { method: 'POST', body: JSON.stringify(data) }),

  // --- Avaliações (Social — Java) ---
  getAvaliacoesFilme: id   => Api.get(socialUrl(`/avaliacoes/filme/${id}`)),
  getAvaliacoesSerie: id   => Api.get(socialUrl(`/avaliacoes/serie/${id}`)),
  criarAvaliacao:     data => Api._req(socialUrl('/avaliacoes'), { method: 'POST', body: JSON.stringify(data) }),

  // --- Favoritos (Social — Java) ---
  getFavoritosUsuario: uid  => Api.get(socialUrl(`/favoritos/usuario/${uid}`)),
  adicionarFavorito:   data => Api._req(socialUrl('/favoritos'), { method: 'POST', body: JSON.stringify(data) }),
  removerFavorito:     id   => Api._req(socialUrl(`/favoritos/${id}`), { method: 'DELETE' }),

  // --- Posts (Social — Java) ---
  getPosts:   ()   => Api.get(socialUrl('/posts')),
  criarPost: data  => Api._req(socialUrl('/posts'), { method: 'POST', body: JSON.stringify(data) }),

  // --- TMDB ---
  tmdb: {
    _cache: {},
    async _tmdbGet(url) {
      const res = await fetch(url);
      if (!res.ok) return null;
      return res.json();
    },
    async buscarPoster(titulo, tipo = 'movie') {
      if (!CONFIG.TMDB_KEY) return null;
      const key = `${tipo}:${titulo}`;
      if (key in Api.tmdb._cache) return Api.tmdb._cache[key];
      try {
        const ep = tipo === 'movie' ? 'movie' : 'tv';
        const url = `${CONFIG.TMDB_SEARCH}/${ep}?api_key=${CONFIG.TMDB_KEY}&query=${encodeURIComponent(titulo)}&language=pt-BR`;
        const data = await Api.tmdb._tmdbGet(url);
        const path = data.results?.[0]?.poster_path;
        const poster = path ? `${CONFIG.TMDB_IMG}${path}` : null;
        Api.tmdb._cache[key] = poster;
        return poster;
      } catch { return null; }
    },
    async buscarInfo(titulo, tipo = 'movie') {
      if (!CONFIG.TMDB_KEY) return null;
      try {
        const ep = tipo === 'movie' ? 'movie' : 'tv';
        const url = `${CONFIG.TMDB_SEARCH}/${ep}?api_key=${CONFIG.TMDB_KEY}&query=${encodeURIComponent(titulo)}&language=pt-BR`;
        const data = await Api.tmdb._tmdbGet(url);
        return data.results?.[0] || null;
      } catch { return null; }
    },
    async catalogo(tipo = 'movie', page = 1) {
      if (!CONFIG.TMDB_KEY) return [];
      try {
        const ep = tipo === 'movie' ? 'movie' : 'tv';
        const url = `https://api.themoviedb.org/3/discover/${ep}?api_key=${CONFIG.TMDB_KEY}&language=pt-BR&sort_by=popularity.desc&page=${page}`;
        const data = await Api.tmdb._tmdbGet(url);
        return data?.results || [];
      } catch { return []; }
    },
    async buscarLista(q, tipo = 'movie') {
      if (!CONFIG.TMDB_KEY || !q?.trim()) return [];
      try {
        const ep = tipo === 'movie' ? 'movie' : 'tv';
        const url = `${CONFIG.TMDB_SEARCH}/${ep}?api_key=${CONFIG.TMDB_KEY}&query=${encodeURIComponent(q)}&language=pt-BR`;
        const data = await Api.tmdb._tmdbGet(url);
        return data?.results || [];
      } catch { return []; }
    }
  }
};
