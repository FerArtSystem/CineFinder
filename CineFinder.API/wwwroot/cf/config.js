// ============================================================
// CONFIGURAÇÃO DO CINEFINDER
// ============================================================

const CONFIG = {
  // URL da API C# — conteúdo: filmes, séries, gêneros
  API: 'http://localhost:5004/api',

  // URL do proxy Java no C# — social: usuários, avaliações, favoritos, posts
  // O C# recebe a chamada e repassa internamente para o Java (porta 8080)
  JAVA_API: 'http://localhost:5004/api/java',

  // TMDB API Key
  TMDB_KEY: '880a14a329f1078d5057ae27c2bdd99f',

  // URLs do TMDB (não altere)
  TMDB_IMG: 'https://image.tmdb.org/t/p/w500',
  TMDB_SEARCH: 'https://api.themoviedb.org/3/search',
};

// Conteúdo: filmes, séries, gêneros — C#
function apiUrl(path) {
  return `${CONFIG.API}${path}`;
}

// Social: usuários, avaliações, favoritos, posts — sempre via proxy Java no C#
function socialUrl(path) {
  return `${CONFIG.JAVA_API}${path}`;
}
