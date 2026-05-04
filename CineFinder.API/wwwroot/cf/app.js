// ============================================================
// APP.JS — SPA principal do CineFinder
// ============================================================

// ── Utilitários ────────────────────────────────────────────
let _searchTimer;
function debounce(fn, ms = 350) {
  clearTimeout(_searchTimer);
  _searchTimer = setTimeout(fn, ms);
}

function fixMojibake(str) {
  const s = String(str ?? '');
  if (!/[ÃÂ�]/.test(s)) return s;
  try {
    return decodeURIComponent(escape(s));
  } catch {
    return s;
  }
}

function normalizeTitle(str) {
  return fixMojibake(str)
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/[^a-zA-Z0-9\s]/g, ' ')
    .replace(/\s+/g, ' ')
    .trim()
    .toLowerCase();
}

function mergeByTitle(primaryList, extraList, max = 120) {
  const out = [];
  const seen = new Set();
  const push = item => {
    const k = normalizeTitle(item.titulo || item.name || item.original_title || item.original_name || '');
    if (!k || seen.has(k)) return;
    seen.add(k);
    out.push(item);
  };
  primaryList.forEach(push);
  extraList.forEach(push);
  return out.slice(0, max);
}

function tmdbToCard(item, tipo = 'movie') {
  const isMovie = tipo === 'movie';
  const titulo = fixMojibake(item.title || item.name || 'Sem título');
  const ano = (item.release_date || item.first_air_date || '').slice(0, 4) || 'TMDB';
  const meta2 = isMovie ? (ano !== 'TMDB' ? ano : '?') : `Série ${ano !== 'TMDB' ? ano : ''}`.trim();
  return {
    id: `tmdb-${item.id}`,
    titulo,
    generoNome: 'TMDB',
    notaMedia: item.vote_average || 0,
    posterUrl: item.poster_path ? `${CONFIG.TMDB_IMG}${item.poster_path}` : null,
    _meta2: meta2,
    _externoLink: `https://www.themoviedb.org/${isMovie ? 'movie' : 'tv'}/${item.id}`,
    _isTmdb: true,
  };
}

function esc(str) {
  const s = fixMojibake(str);
  return s.replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');
}

function showToast(msg, type = 'success') {
  const t = document.createElement('div');
  t.className = `toast toast-${type}`;
  t.textContent = msg;
  document.body.appendChild(t);
  requestAnimationFrame(() => { requestAnimationFrame(() => t.classList.add('show')); });
  setTimeout(() => { t.classList.remove('show'); setTimeout(() => t.remove(), 320); }, 3200);
}

function closeModal() { document.getElementById('modal-overlay').style.display = 'none'; }

// ── Roteamento ─────────────────────────────────────────────
function getRoute() {
  const hash = location.hash.slice(1) || 'home';
  const [page, id] = hash.split('/');
  return { page: page || 'home', id: id ? parseInt(id) : null };
}

function navigateTo(hash) { location.hash = hash; }

async function renderPage(page, id) {
  const app = document.getElementById('app');
  app.innerHTML = `<div class="loading"><div class="spinner"></div><p>Carregando...</p></div>`;
  try {
    switch (page) {
      case 'home':      await renderHome();           break;
      case 'filmes':    id ? await renderFilmeDetalhe(id) : await renderFilmes(); break;
      case 'series':    id ? await renderSerieDetalhe(id) : await renderSeries(); break;
      case 'login':     renderLogin();                break;
      case 'cadastro':  renderCadastro();             break;
      case 'favoritos': await renderFavoritos();      break;
      case 'comunidade':await renderComunidade();     break;
      case 'cinemas':   await renderCinemas();        break;
      default:          await renderHome();
    }
  } catch (e) {
    console.error(e);
    const dica = e.message === 'Failed to fetch'
      ? `<p>Verifique se as APIs estão rodando:<br>
           <code>C#: ${CONFIG.API}</code><br>
           <code>Java (via proxy C#): ${CONFIG.JAVA_API}</code><br>
           Certifique-se de que o Java esteja rodando na porta 8080.</p>`
      : `<p>${esc(e.message)}</p>`;
    app.innerHTML = `
      <div class="error-page">
        <h2>Erro ao carregar</h2>
        ${dica}
        <a href="#home" class="btn btn-primary" style="margin-top:1.5rem">Voltar ao início</a>
      </div>`;
  }
}

// ── Navbar ─────────────────────────────────────────────────
function renderNavAuth() {
  const el = document.getElementById('nav-auth');
  if (!el) return;
  if (Auth.logado) {
    el.innerHTML = `
      <a href="#favoritos" class="btn-sm btn-sm-outline">⭐ Favoritos</a>
      <span class="user-badge">${esc(Auth.usuario.nickname)}</span>
      <button class="btn-sm btn-sm-outline" onclick="Auth.logout()">Sair</button>`;
  } else {
    el.innerHTML = `
      <a href="#login"    class="btn-sm btn-sm-outline">Entrar</a>
      <a href="#cadastro" class="btn-sm btn-sm-primary">Cadastrar</a>`;
  }
}

// ── Busca da navbar ────────────────────────────────────────
function setupSearch() {
  const form    = document.getElementById('search-form');
  const input   = document.getElementById('search-input');
  const results = document.getElementById('search-results');

  input.addEventListener('input', () => {
    const q = input.value.trim();
    if (!q || q.length < 2) { results.style.display = 'none'; return; }
    debounce(async () => {
      const [filmes, series] = await Promise.all([Api.buscarFilmes(q).catch(() => []), Api.buscarSeries(q).catch(() => [])]);
      const all = [
        ...filmes.map(f => ({ ...f, _tipo: 'filmes' })),
        ...series.map(s => ({ ...s, _tipo: 'series' }))
      ].slice(0, 9);
      if (!all.length) { results.style.display = 'none'; return; }
      results.innerHTML = all.map(item => `
        <a class="sri" href="#${item._tipo}/${item.id}" onclick="results.style.display='none';input.value=''">
          <span class="sri-tipo">${item._tipo === 'filmes' ? '🎬' : '📺'}</span>
          <span>${esc(item.titulo)}</span>
          <span class="sri-nota">⭐ ${(item.notaMedia ?? 0).toFixed(1)}</span>
        </a>`).join('');
      results.style.display = 'block';
    });
  });

  form.addEventListener('submit', e => {
    e.preventDefault();
    results.style.display = 'none';
    input.blur();
  });

  document.addEventListener('click', e => {
    if (!form.contains(e.target)) results.style.display = 'none';
  });
}

// ── Poster helpers ─────────────────────────────────────────
function placeholderImg(titulo) {
  return 'cf/logo.png';
}

async function getPoster(item, tipo = 'movie') {
  if (item.posterUrl && /^https?:\/\//i.test(item.posterUrl)) return item.posterUrl;
  const titulo = fixMojibake(item.titulo);
  if (CONFIG.TMDB_KEY) {
    const p = await Api.tmdb.buscarPoster(titulo, tipo);
    if (p) return p;
  }
  return placeholderImg(titulo);
}

// ── Render Card ────────────────────────────────────────────
async function renderCard(item, tipo) {
  const href    = item._externoLink || `#${tipo === 'filme' ? 'filmes' : 'series'}/${item.id}`;
  const target  = item._externoLink ? ' target="_blank" rel="noopener noreferrer"' : '';
  const imgSrc  = await getPoster(item, tipo === 'filme' ? 'movie' : 'tv');
  const meta2   = item._meta2 ?? (tipo === 'filme' ? `${item.duracaoMinutos ?? '?'} min` : `${item.numTemporadas ?? '?'} temp.`);
  return `
    <a href="${href}" class="card"${target}>
      <div class="card-img-wrap">
        <img src="${imgSrc}" alt="${esc(item.titulo)}" loading="lazy" onerror="this.onerror=null;this.src='cf/logo.png';this.style.objectFit='contain';this.style.padding='18px'">
        <div class="card-overlay"><span class="nota-badge">⭐ ${(item.notaMedia ?? 0).toFixed(1)}</span></div>
      </div>
      <div class="card-info">
        <h3>${esc(item.titulo)}</h3>
        <div class="card-meta"><span>${esc(item.generoNome ?? '')}</span><span>${meta2}</span></div>
      </div>
    </a>`;
}

async function renderCardRow(items, tipo, titulo, href) {
  if (!items.length) return '';
  const cards = await Promise.all(items.map(i => renderCard(i, tipo)));
  return `
    <section class="row-section">
      <div class="row-header">
        <h2>${titulo}</h2>
        <a href="${href}" class="ver-mais">Ver todos →</a>
      </div>
      <div class="cards-row">${cards.join('')}</div>
    </section>`;
}

// ── HOME ───────────────────────────────────────────────────
async function renderHome() {
  const [filmes, series] = await Promise.all([Api.getFilmes(), Api.getSeries()]);
  const featured = filmes[0];

  let heroHtml = '';
  if (featured) {
    const poster = await getPoster(featured, 'movie');
    const tmdbInfo = CONFIG.TMDB_KEY ? await Api.tmdb.buscarInfo(fixMojibake(featured.titulo), 'movie') : null;
    const backdrop = tmdbInfo?.backdrop_path
      ? `https://image.tmdb.org/t/p/w1280${tmdbInfo.backdrop_path}`
      : poster;
    heroHtml = `
      <section class="hero" style="background-image:url('${backdrop}')">
        <div class="hero-content">
          <span class="hero-tag">Em Destaque</span>
          <h1>${esc(featured.titulo)}</h1>
          <div class="hero-meta">
            <span class="stars">⭐ ${(featured.notaMedia ?? 0).toFixed(1)}</span>
            ${featured.generoNome ? `<span>${esc(featured.generoNome)}</span>` : ''}
            ${featured.duracaoMinutos ? `<span>${featured.duracaoMinutos} min</span>` : ''}
          </div>
          <p class="hero-sinopse">${esc((featured.sinopse ?? '').slice(0, 220))}${(featured.sinopse?.length ?? 0) > 220 ? '...' : ''}</p>
          <div class="hero-buttons">
            <a href="#filmes/${featured.id}" class="btn btn-primary">▶ Ver Detalhes</a>
            <button class="btn btn-outline" onclick="addToFav('filme',${featured.id})">+ Favoritar</button>
          </div>
        </div>
      </section>`;
  }

  const rowFilmes = await renderCardRow(filmes.slice(0, 8), 'filme', '🎬 Filmes em Destaque', '#filmes');
  const rowSeries = await renderCardRow(series.slice(0, 6), 'serie', '📺 Séries em Destaque', '#series');
  document.getElementById('app').innerHTML = heroHtml + rowFilmes + rowSeries;
}

// ── FILMES LIST ────────────────────────────────────────────
async function renderFilmes() {
  const [filmes, generos, tmdbP1, tmdbP2, tmdbP3, tmdbP4] = await Promise.all([
    Api.getFilmes(),
    Api.getGeneros(),
    Api.tmdb.catalogo('movie', 1),
    Api.tmdb.catalogo('movie', 2),
    Api.tmdb.catalogo('movie', 3),
    Api.tmdb.catalogo('movie', 4),
  ]);
  const tmdb = [tmdbP1, tmdbP2, tmdbP3, tmdbP4].flat().map(i => tmdbToCard(i, 'movie'));
  const catalogo = mergeByTitle(filmes, tmdb, 120);
  window._catalogoFilmes = catalogo;
  const cards = await Promise.all(catalogo.map(f => renderCard(f, 'filme')));
  const gBtns = generos.map(g => `<button class="genre-btn" data-gid="${g.id}" onclick="filtrarGenero('filme',${g.id},this)">${esc(g.nome)}</button>`).join('');
  document.getElementById('app').innerHTML = `
    <div class="page-content">
      <div class="page-header">
        <h1>🎬 Filmes</h1>
        <div class="search-filter">
          <input type="text" placeholder="Buscar..." oninput="debounce(()=>filtrarBusca('filme',this.value))">
        </div>
      </div>
      <div class="genre-filters">
        <button class="genre-btn active" onclick="resetGeneros('filme',this)">Todos</button>
        ${gBtns}
      </div>
      <div class="cards-grid" id="main-grid">${cards.join('')}</div>
    </div>`;
}

async function renderSeries() {
  const [series, generos, tmdbP1, tmdbP2, tmdbP3, tmdbP4] = await Promise.all([
    Api.getSeries(),
    Api.getGeneros(),
    Api.tmdb.catalogo('tv', 1),
    Api.tmdb.catalogo('tv', 2),
    Api.tmdb.catalogo('tv', 3),
    Api.tmdb.catalogo('tv', 4),
  ]);
  const tmdb = [tmdbP1, tmdbP2, tmdbP3, tmdbP4].flat().map(i => tmdbToCard(i, 'tv'));
  const catalogo = mergeByTitle(series, tmdb, 120);
  window._catalogoSeries = catalogo;
  const cards = await Promise.all(catalogo.map(s => renderCard(s, 'serie')));
  const gBtns = generos.map(g => `<button class="genre-btn" data-gid="${g.id}" onclick="filtrarGenero('serie',${g.id},this)">${esc(g.nome)}</button>`).join('');
  document.getElementById('app').innerHTML = `
    <div class="page-content">
      <div class="page-header"><h1>📺 Séries</h1></div>
      <div class="genre-filters">
        <button class="genre-btn active" onclick="resetGeneros('serie',this)">Todas</button>
        ${gBtns}
      </div>
      <div class="cards-grid" id="main-grid">${cards.join('')}</div>
    </div>`;
}

async function filtrarGenero(tipo, gid, btn) {
  document.querySelectorAll('.genre-btn').forEach(b => b.classList.remove('active'));
  btn.classList.add('active');
  const grid = document.getElementById('main-grid');
  grid.innerHTML = `<div class="loading"><div class="spinner"></div></div>`;
  const items = tipo === 'filme' ? await Api.getFilmesPorGenero(gid) : await Api.getSeriesPorGenero(gid);
  const cards = await Promise.all(items.map(i => renderCard(i, tipo)));
  grid.innerHTML = cards.join('') || '<p class="empty">Nenhum item neste gênero.</p>';
}

async function resetGeneros(tipo, btn) {
  document.querySelectorAll('.genre-btn').forEach(b => b.classList.remove('active'));
  btn.classList.add('active');
  const grid = document.getElementById('main-grid');
  grid.innerHTML = `<div class="loading"><div class="spinner"></div></div>`;
  const items = tipo === 'filme'
    ? (window._catalogoFilmes?.length ? window._catalogoFilmes : await Api.getFilmes())
    : (window._catalogoSeries?.length ? window._catalogoSeries : await Api.getSeries());
  const cards = await Promise.all(items.map(i => renderCard(i, tipo)));
  grid.innerHTML = cards.join('');
}

async function filtrarBusca(tipo, q) {
  const grid = document.getElementById('main-grid');
  if (!q.trim()) {
    const items = tipo === 'filme' ? await Api.getFilmes() : await Api.getSeries();
    const cards = await Promise.all(items.map(i => renderCard(i, tipo)));
    grid.innerHTML = cards.join(''); return;
  }
  const [localItems, tmdbItems] = await Promise.all([
    tipo === 'filme' ? Api.buscarFilmes(q) : Api.buscarSeries(q),
    Api.tmdb.buscarLista(q, tipo === 'filme' ? 'movie' : 'tv')
  ]);
  const extras = tmdbItems.map(i => tmdbToCard(i, tipo === 'filme' ? 'movie' : 'tv'));
  const items = mergeByTitle(localItems, extras, 60);
  const cards = await Promise.all(items.map(i => renderCard(i, tipo)));
  grid.innerHTML = cards.join('') || '<p class="empty">Sem resultados.</p>';
}

// ── DETALHE ────────────────────────────────────────────────
async function renderFilmeDetalhe(id) {
  const [filme, avaliacoes] = await Promise.all([Api.getFilme(id), Api.getAvaliacoesFilme(id)]);
  await renderDetalhe(filme, 'filme', avaliacoes);
}
async function renderSerieDetalhe(id) {
  const [serie, avaliacoes] = await Promise.all([Api.getSerie(id), Api.getAvaliacoesSerie(id)]);
  await renderDetalhe(serie, 'serie', avaliacoes);
}

async function renderDetalhe(item, tipo, avaliacoes) {
  const isFilme = tipo === 'filme';
  const poster  = await getPoster(item, isFilme ? 'movie' : 'tv');
  const tmdbInfo = CONFIG.TMDB_KEY ? await Api.tmdb.buscarInfo(item.titulo, isFilme ? 'movie' : 'tv') : null;

  const metaBadges = [
    item.generoNome ? `<span class="badge">${esc(item.generoNome)}</span>` : '',
    isFilme && item.duracaoMinutos ? `<span class="badge">${item.duracaoMinutos} min</span>` : '',
    !isFilme && item.numTemporadas ? `<span class="badge">${item.numTemporadas} temporada(s)</span>` : '',
    (isFilme ? item.dataLancamento : item.dataEstreia) ? `<span class="badge">${new Date(isFilme ? item.dataLancamento : item.dataEstreia).getFullYear()}</span>` : '',
    `<span class="badge gold">⭐ ${(item.notaMedia ?? 0).toFixed(1)}/10</span>`,
    tmdbInfo?.vote_count ? `<span class="badge">${tmdbInfo.vote_count.toLocaleString('pt-BR')} votos TMDB</span>` : '',
  ].filter(Boolean).join('');

  const avHtml = avaliacoes.length
    ? avaliacoes.map(a => `
        <div class="review-item">
          <div class="review-header">
            <strong>${esc(a.usuarioNickname)}</strong>
            <span class="nota-badge sm">⭐ ${a.nota}/10</span>
            <span class="review-date">${new Date(a.dataCriacao).toLocaleDateString('pt-BR')}</span>
          </div>
          ${a.comentario ? `<p>${esc(a.comentario)}</p>` : ''}
        </div>`).join('')
    : '<p class="empty">Nenhuma avaliação ainda.</p>';

  const reviewForm = Auth.logado
    ? `<div class="review-form">
        <h3>Sua Avaliação</h3>
        <div class="nota-slider">
          <label>Nota: <strong id="nota-val">8</strong> / 10</label>
          <input type="range" min="1" max="10" value="8" id="nota-inp" oninput="document.getElementById('nota-val').textContent=this.value">
        </div>
        <textarea id="coment-inp" placeholder="Deixe um comentário (opcional)..." rows="3"></textarea>
        <button class="btn btn-primary" onclick="enviarAvaliacao('${tipo}',${item.id})">Enviar</button>
       </div>`
    : `<p class="login-prompt"><a href="#login">Entre</a> para avaliar.</p>`;

  document.getElementById('app').innerHTML = `
    <div class="detalhe-page">
      <button class="btn-back" onclick="history.back()">← Voltar</button>
      <div class="detalhe-hero">
        <img src="${poster}" alt="${esc(item.titulo)}" class="detalhe-poster" onerror="this.src='${placeholderImg(item.titulo)}'">
        <div class="detalhe-info">
          <h1>${esc(item.titulo)}</h1>
          <div class="detalhe-badges">${metaBadges}</div>
          ${item.sinopse ? `<p class="detalhe-sinopse">${esc(item.sinopse)}</p>` : ''}
          ${tmdbInfo?.overview && !item.sinopse ? `<p class="detalhe-sinopse">${esc(tmdbInfo.overview)}</p>` : ''}
          <div class="detalhe-actions">
            ${Auth.logado
              ? `<button class="btn btn-primary" onclick="addToFav('${tipo}',${item.id})">⭐ Favoritar</button>`
              : `<a href="#login" class="btn btn-outline">Entre para favoritar</a>`}
          </div>
        </div>
      </div>
      <div class="reviews-section">
        <h2>Avaliações <span class="count">(${avaliacoes.length})</span></h2>
        ${reviewForm}
        <div id="reviews-list">${avHtml}</div>
      </div>
    </div>`;
}

async function enviarAvaliacao(tipo, id) {
  if (!Auth.logado) { navigateTo('login'); return; }
  const nota      = parseInt(document.getElementById('nota-inp').value);
  const comentario = document.getElementById('coment-inp').value.trim();
  const data = { usuarioId: Auth.usuario.id, nota, comentario };
  if (tipo === 'filme') data.filmeId = id; else data.serieId = id;
  try {
    const av = await Api.criarAvaliacao(data);
    const html = `
      <div class="review-item">
        <div class="review-header">
          <strong>${esc(av.usuarioNickname)}</strong>
          <span class="nota-badge sm">⭐ ${av.nota}/10</span>
          <span class="review-date">agora</span>
        </div>
        ${av.comentario ? `<p>${esc(av.comentario)}</p>` : ''}
      </div>`;
    document.getElementById('reviews-list').insertAdjacentHTML('afterbegin', html);
    document.getElementById('nota-inp').value = 8;
    document.getElementById('nota-val').textContent = '8';
    document.getElementById('coment-inp').value = '';
    showToast('Avaliação enviada! ⭐');
  } catch { showToast('Erro ao enviar avaliação.', 'error'); }
}

// ── FAVORITOS ──────────────────────────────────────────────
async function addToFav(tipo, id) {
  if (!Auth.logado) { navigateTo('login'); return; }
  const data = { usuarioId: Auth.usuario.id };
  if (tipo === 'filme') data.filmeId = id; else data.serieId = id;
  try { await Api.adicionarFavorito(data); showToast('Adicionado aos favoritos! ⭐'); }
  catch { showToast('Erro ao favoritar.', 'error'); }
}

async function renderFavoritos() {
  if (!Auth.logado) { navigateTo('login'); return; }
  const favs = await Api.getFavoritosUsuario(Auth.usuario.id);
  let gridHtml = '<div class="empty-state"><p>🎬</p><h3>Nenhum favorito ainda</h3><p><a href="#filmes">Explorar filmes</a></p></div>';
  if (favs.length) {
    const cards = await Promise.all(favs.map(async fav => {
      const titulo = fav.filmeTitulo || fav.serieTitulo || '?';
      const isFilme = !!fav.filmeId;
      const href = `#${isFilme ? 'filmes' : 'series'}/${isFilme ? fav.filmeId : fav.serieId}`;
      const poster = CONFIG.TMDB_KEY ? await Api.tmdb.buscarPoster(titulo, isFilme ? 'movie' : 'tv') : null;
      const img = poster || placeholderImg(titulo);
      return `
        <div class="card fav-card">
          <a href="${href}">
            <div class="card-img-wrap"><img src="${img}" alt="${esc(titulo)}" loading="lazy" onerror="this.src='${placeholderImg(titulo)}'">
              <div class="card-overlay"><span class="nota-badge">${isFilme ? '🎬' : '📺'}</span></div>
            </div>
            <div class="card-info"><h3>${esc(titulo)}</h3>
              <div class="card-meta"><span>${isFilme ? 'Filme' : 'Série'}</span></div>
            </div>
          </a>
          <button class="btn-remover" onclick="removerFavorito(${fav.id})">🗑 Remover</button>
        </div>`;
    }));
    gridHtml = `<div class="cards-grid">${cards.join('')}</div>`;
  }
  document.getElementById('app').innerHTML = `
    <div class="page-content">
      <div class="page-header"><h1>⭐ Meus Favoritos</h1></div>
      ${gridHtml}
    </div>`;
}

async function removerFavorito(id) {
  try { await Api.removerFavorito(id); await renderFavoritos(); showToast('Removido dos favoritos.'); }
  catch { showToast('Erro ao remover.', 'error'); }
}

// ── LOGIN / CADASTRO ───────────────────────────────────────
function renderLogin() {
  document.getElementById('app').innerHTML = `
    <div class="auth-page"><div class="auth-card">
      <h1>🎬 CineFinder</h1>
      <h2>Entrar na sua conta</h2>
      <div class="form-error" id="login-err" style="display:none"></div>
      <div class="form-group"><label>E-mail</label><input type="email" id="l-email" placeholder="seu@email.com" autofocus></div>
      <div class="form-group"><label>Senha</label><input type="password" id="l-senha" placeholder="••••••••" onkeyup="if(event.key==='Enter')doLogin()"></div>
      <button class="btn btn-primary btn-full" onclick="doLogin()">Entrar</button>
      <p class="auth-link">Não tem conta? <a href="#cadastro">Cadastre-se</a></p>
    </div></div>`;
}

async function doLogin() {
  const email = document.getElementById('l-email').value.trim();
  const senha = document.getElementById('l-senha').value;
  const err = document.getElementById('login-err');
  err.style.display = 'none';
  if (!email || !senha) { err.textContent = 'Preencha todos os campos.'; err.style.display = 'block'; return; }
  try {
    const u = await Api.login({ email, senha });
    Auth.salvar(u);
    navigateTo('home');
  } catch { err.textContent = 'E-mail ou senha inválidos.'; err.style.display = 'block'; }
}

function renderCadastro() {
  document.getElementById('app').innerHTML = `
    <div class="auth-page"><div class="auth-card">
      <h1>🎬 CineFinder</h1>
      <h2>Criar conta</h2>
      <div class="form-error"   id="cad-err" style="display:none"></div>
      <div class="form-success" id="cad-ok"  style="display:none"></div>
      <div class="form-group"><label>Nome</label><input type="text"     id="c-nome"     placeholder="Seu nome completo" autofocus></div>
      <div class="form-group"><label>E-mail</label><input type="email"  id="c-email"    placeholder="seu@email.com"></div>
      <div class="form-group"><label>Nickname</label><input type="text" id="c-nick"     placeholder="Como quer ser chamado?"></div>
      <div class="form-group"><label>Senha</label><input type="password"id="c-senha"    placeholder="Mínimo 6 caracteres"></div>
      <button class="btn btn-primary btn-full" onclick="doCadastro()">Criar Conta</button>
      <p class="auth-link">Já tem conta? <a href="#login">Entrar</a></p>
    </div></div>`;
}

async function doCadastro() {
  const nome     = document.getElementById('c-nome').value.trim();
  const email    = document.getElementById('c-email').value.trim();
  const nickname = document.getElementById('c-nick').value.trim();
  const senha    = document.getElementById('c-senha').value;
  const err = document.getElementById('cad-err');
  const ok  = document.getElementById('cad-ok');
  err.style.display = ok.style.display = 'none';
  if (!nome || !email || !nickname || !senha) { err.textContent = 'Preencha todos os campos.'; err.style.display = 'block'; return; }
  try {
    await Api.cadastrar({ nome, email, nickname, senha });
    ok.textContent = 'Conta criada! Redirecionando para o login...';
    ok.style.display = 'block';
    setTimeout(() => navigateTo('login'), 2000);
  } catch { err.textContent = 'Erro: e-mail ou nickname já em uso.'; err.style.display = 'block'; }
}

// ── COMUNIDADE ─────────────────────────────────────────────
async function renderComunidade() {
  const posts = await Api.getPosts();
  const form = Auth.logado
    ? `<div class="post-form">
        <div class="post-form-header">
          <span class="user-avatar">${Auth.usuario.nickname[0].toUpperCase()}</span>
          <span>Publicar como <strong>${esc(Auth.usuario.nickname)}</strong></span>
        </div>
        <textarea id="post-txt" placeholder="Compartilhe sua opinião sobre um filme ou série..." rows="3"></textarea>
        <div class="post-form-error" id="post-err" style="display:none"></div>
        <button class="btn btn-primary" onclick="publicarPost()">Publicar</button>
       </div>`
    : `<div class="login-prompt-box"><p><a href="#login">Entre</a> para participar da comunidade.</p></div>`;

  const postsHtml = posts.length
    ? posts.map(p => `
        <div class="post-item">
          <div class="post-avatar">${(p.usuarioNickname || '?')[0].toUpperCase()}</div>
          <div class="post-body">
            <div class="post-header">
              <strong>${esc(p.usuarioNickname)}</strong>
              <span class="post-date">${new Date(p.dataCriacao).toLocaleDateString('pt-BR',{day:'2-digit',month:'short',year:'numeric',hour:'2-digit',minute:'2-digit'})}</span>
            </div>
            <p class="post-content">${esc(p.conteudo)}</p>
          </div>
        </div>`).join('')
    : '<div class="empty-state"><p>💬</p><h3>Nenhuma publicação ainda.</h3></div>';

  document.getElementById('app').innerHTML = `
    <div class="page-content">
      <div class="page-header"><h1>💬 Comunidade</h1></div>
      ${form}
      <div class="posts-feed" id="posts-feed">${postsHtml}</div>
    </div>`;
}

async function publicarPost() {
  const txt = document.getElementById('post-txt')?.value.trim();
  const err = document.getElementById('post-err');
  if (!txt) { err.textContent = 'Escreva algo antes de publicar.'; err.style.display = 'block'; return; }
  err.style.display = 'none';
  try {
    const p = await Api.criarPost({ usuarioId: Auth.usuario.id, conteudo: txt });
    document.getElementById('post-txt').value = '';
    document.getElementById('posts-feed').insertAdjacentHTML('afterbegin', `
      <div class="post-item">
        <div class="post-avatar">${Auth.usuario.nickname[0].toUpperCase()}</div>
        <div class="post-body">
          <div class="post-header">
            <strong>${esc(p.usuarioNickname)}</strong>
            <span class="post-date">agora</span>
          </div>
          <p class="post-content">${esc(p.conteudo)}</p>
        </div>
      </div>`);
    showToast('Publicado! 🎬');
  } catch { err.textContent = 'Erro ao publicar.'; err.style.display = 'block'; }
}

// ── CINEMAS ────────────────────────────────────────────────
let _map = null;

async function renderCinemas() {
  document.getElementById('app').innerHTML = `
    <div class="page-content">
      <div class="page-header">
        <h1>🎬 Cinemas Próximos</h1>
      </div>
      <div class="cinema-controls">
        <div class="cinema-search-row">
          <input id="cin-cidade" type="text" placeholder="Cidade (ex: São Paulo, Curitiba...)" class="cin-input">
          <select id="cin-raio" class="cin-select">
            <option value="2">2 km</option>
            <option value="5" selected>5 km</option>
            <option value="10">10 km</option>
            <option value="20">20 km</option>
          </select>
          <button class="btn btn-primary" onclick="buscarCinemasPorCidade()">🔍 Buscar</button>
          <button class="btn btn-outline" onclick="buscarCinemasGPS()" id="btn-gps" title="Usar minha localização">📍 Usar GPS</button>
        </div>
        <p class="cinema-info" id="cin-info">Digite uma cidade ou use o GPS para encontrar cinemas próximos.</p>
      </div>
      <div id="cinema-map" class="cinema-map"></div>
      <div id="cinema-list" class="cinema-list"></div>
    </div>`;

  // Inicializa mapa Leaflet centrado no Brasil
  if (_map) { _map.remove(); _map = null; }
  _map = L.map('cinema-map').setView([-15.788, -47.879], 4);
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
    maxZoom: 19,
  }).addTo(_map);
}

async function buscarCinemasPorCidade() {
  const q = document.getElementById('cin-cidade').value.trim();
  if (!q) { showToast('Digite uma cidade.', 'error'); return; }
  const raio = document.getElementById('cin-raio').value;
  await _carregarCinemas(`${CONFIG.JAVA_API}/cinemas/cidade?q=${encodeURIComponent(q)}&raio=${raio}`, `Buscando cinemas em "${q}"...`);
}

function buscarCinemasGPS() {
  const btn = document.getElementById('btn-gps');
  if (!navigator.geolocation) { showToast('GPS não disponível neste navegador.', 'error'); return; }
  btn.textContent = '⏳ Localizando...';
  btn.disabled = true;
  navigator.geolocation.getCurrentPosition(async pos => {
    btn.textContent = '📍 Usar GPS';
    btn.disabled = false;
    const { latitude: lat, longitude: lng } = pos.coords;
    const raio = document.getElementById('cin-raio').value;
    await _carregarCinemas(`${CONFIG.JAVA_API}/cinemas/proximos?lat=${lat}&lng=${lng}&raio=${raio}`, 'Buscando pela sua localização...');
  }, () => {
    btn.textContent = '📍 Usar GPS';
    btn.disabled = false;
    showToast('Não foi possível obter sua localização.', 'error');
  });
}

async function _carregarCinemas(url, msg) {
  const info = document.getElementById('cin-info');
  const list = document.getElementById('cinema-list');
  if (info) info.textContent = msg;
  if (list) list.innerHTML = '<div class="loading"><div class="spinner"></div><p>Consultando OpenStreetMap...</p></div>';

  try {
    const data = await _fetchCinemasComFallback(url);

    if (info) info.textContent = `${data.total} cinema(s) encontrado(s) num raio de ${data.raioKm} km ${data.cidade ? 'em ' + data.cidade : ''}`;

    // Centraliza mapa
    if (_map && data.lat && data.lng) {
      _map.setView([data.lat, data.lng], data.raioKm <= 3 ? 14 : data.raioKm <= 7 ? 13 : 11);
      // Círculo de raio
      L.circle([data.lat, data.lng], {
        radius: data.raioKm * 1000,
        color: '#F5C542', weight: 1.5, fillOpacity: 0.05
      }).addTo(_map);
    }

    // Marcadores no mapa
    const iconCinema = L.divIcon({
      html: '<div class="map-pin">🎬</div>',
      className: '', iconSize: [36, 36], iconAnchor: [18, 18]
    });

    if (list) list.innerHTML = '';

    if (!data.cinemas || data.cinemas.length === 0) {
      if (list) list.innerHTML = '<div class="empty-state"><p>🎬</p><h3>Nenhum cinema encontrado</h3><p>Tente aumentar o raio de busca.</p></div>';
      return;
    }

    data.cinemas.forEach(c => {
      // Marcador
      const marker = L.marker([c.lat, c.lng], { icon: iconCinema }).addTo(_map);
      marker.bindPopup(`
        <div class="popup-cinema">
          <strong>${c.nome}</strong><br>
          ${c.endereco ? `📍 ${c.endereco}<br>` : ''}
          ${c.openingHours ? `🕐 ${c.openingHours}<br>` : ''}
          <a href="${c.googleMapsUrl}" target="_blank" rel="noopener">Ver no Google Maps</a>
        </div>`);

      // Card na lista
      const card = document.createElement('div');
      card.className = 'cinema-card';
      card.innerHTML = `
        <div class="cinema-card-header">
          <span class="cinema-pin">🎬</span>
          <div>
            <h3>${esc(c.nome)}</h3>
            <span class="cinema-dist">${c.distanciaKm} km de distância</span>
          </div>
        </div>
        ${c.endereco ? `<p class="cinema-addr">📍 ${esc(c.endereco)}</p>` : ''}
        ${c.openingHours ? `<p class="cinema-hours">🕐 ${esc(c.openingHours)}</p>` : ''}
        ${c.telefone ? `<p class="cinema-tel">📞 ${esc(c.telefone)}</p>` : ''}
        <div class="cinema-card-actions">
          <a href="${c.googleMapsUrl}" target="_blank" rel="noopener" class="btn-sm btn-sm-outline">Google Maps</a>
          <a href="${c.mapUrl}" target="_blank" rel="noopener" class="btn-sm btn-sm-outline">OpenStreetMap</a>
          ${c.site ? `<a href="${esc(c.site)}" target="_blank" rel="noopener" class="btn-sm btn-sm-outline">Site</a>` : ''}
        </div>`;
      card.addEventListener('click', () => {
        _map.setView([c.lat, c.lng], 16);
        marker.openPopup();
        window.scrollTo({ top: document.getElementById('cinema-map').offsetTop - 80, behavior: 'smooth' });
      });
      if (list) list.appendChild(card);
    });

  } catch (e) {
    if (list) list.innerHTML = `<div class="error-page"><h2>Erro</h2><p>${esc(e.message)}</p></div>`;
    if (info) info.textContent = 'Falha ao carregar cinemas.';
  }
}

async function _fetchCinemasComFallback(url) {
  // Tentativa principal: API Java
  try {
    const res = await fetch(url);
    if (res.ok) {
      return await res.json();
    }
  } catch {
    // segue para fallback direto no frontend
  }

  // Fallback: consulta direta aos serviços OSM
  const parsed = new URL(url, location.origin);
  const isCidade = parsed.pathname.endsWith('/cinemas/cidade');
  const raio = Number(parsed.searchParams.get('raio') || '5');

  if (isCidade) {
    const q = parsed.searchParams.get('q') || '';
    if (!q.trim()) throw new Error('Cidade inválida para busca.');
    return await _buscarCinemasDiretoCidade(q, raio);
  }

  const lat = Number(parsed.searchParams.get('lat'));
  const lng = Number(parsed.searchParams.get('lng'));
  if (!Number.isFinite(lat) || !Number.isFinite(lng)) {
    throw new Error('Coordenadas inválidas para busca de cinemas.');
  }
  return await _buscarCinemasDiretoCoordenadas(lat, lng, raio);
}

async function _buscarCinemasDiretoCidade(cidade, raioKm) {
  const nominatim = `https://nominatim.openstreetmap.org/search?q=${encodeURIComponent(cidade)}&format=json&limit=1`;
  const geoRes = await fetch(nominatim);
  if (!geoRes.ok) throw new Error('Falha ao consultar geocodificação da cidade.');
  const geo = await geoRes.json();
  if (!Array.isArray(geo) || !geo.length) throw new Error('Cidade não encontrada.');

  const lat = Number(geo[0].lat);
  const lng = Number(geo[0].lon);
  const data = await _buscarCinemasDiretoCoordenadas(lat, lng, raioKm);
  data.cidade = cidade;
  return data;
}

async function _buscarCinemasDiretoCoordenadas(lat, lng, raioKm) {
  const metros = Math.max(1, Math.round((Number(raioKm) || 5) * 1000));
  const query = `[out:json][timeout:25];(node["amenity"="cinema"](around:${metros},${lat},${lng});way["amenity"="cinema"](around:${metros},${lat},${lng}););out center tags;`;

  const overpassUrl = `https://overpass-api.de/api/interpreter?data=${encodeURIComponent(query)}`;
  const ovRes = await fetch(overpassUrl);
  if (!ovRes.ok) throw new Error('Falha ao consultar cinemas no OpenStreetMap.');
  const ov = await ovRes.json();

  const cinemas = (ov.elements || []).map(el => {
    const tags = el.tags || {};
    const cLat = Number(el.lat ?? el.center?.lat);
    const cLng = Number(el.lon ?? el.center?.lon);
    return {
      id: el.id,
      nome: tags.name || 'Cinema sem nome',
      lat: cLat,
      lng: cLng,
      endereco: _montarEnderecoTags(tags),
      telefone: tags.phone || null,
      site: tags.website || null,
      openingHours: tags.opening_hours || null,
      distanciaKm: _distanciaKm(lat, lng, cLat, cLng),
      mapUrl: `https://www.openstreetmap.org/?mlat=${cLat}&mlon=${cLng}#map=17/${cLat}/${cLng}`,
      googleMapsUrl: `https://www.google.com/maps/search/?api=1&query=${cLat},${cLng}`,
    };
  }).sort((a, b) => a.distanciaKm - b.distanciaKm);

  return {
    total: cinemas.length,
    lat,
    lng,
    raioKm: Number(raioKm) || 5,
    fonte: 'OpenStreetMap (fallback frontend)',
    cinemas,
  };
}

function _montarEnderecoTags(tags) {
  const partes = [];
  if (tags['addr:street']) {
    let rua = tags['addr:street'];
    if (tags['addr:housenumber']) rua += ', ' + tags['addr:housenumber'];
    partes.push(rua);
  }
  if (tags['addr:suburb']) partes.push(tags['addr:suburb']);
  if (tags['addr:city']) partes.push(tags['addr:city']);
  if (tags['addr:state']) partes.push(tags['addr:state']);
  return partes.length ? partes.join(', ') : null;
}

function _distanciaKm(lat1, lng1, lat2, lng2) {
  if (![lat1, lng1, lat2, lng2].every(Number.isFinite)) return 0;
  const R = 6371;
  const dLat = (lat2 - lat1) * Math.PI / 180;
  const dLng = (lng2 - lng1) * Math.PI / 180;
  const a = Math.sin(dLat / 2) * Math.sin(dLat / 2)
    + Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180)
    * Math.sin(dLng / 2) * Math.sin(dLng / 2);
  const d = R * 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  return Math.round(d * 100) / 100;
}

// ── INIT ───────────────────────────────────────────────────
window.addEventListener('load', () => {
  renderNavAuth();
  setupSearch();
  if (!CONFIG.TMDB_KEY) document.getElementById('tmdb-banner').style.display = 'flex';
  const { page, id } = getRoute();
  renderPage(page, id);
});

window.addEventListener('hashchange', () => {
  const { page, id } = getRoute();
  renderPage(page, id);
  window.scrollTo({ top: 0, behavior: 'smooth' });
});
