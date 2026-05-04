// ============================================================
// AUTH SERVICE — persiste usuário no localStorage
// ============================================================

const Auth = {
  _KEY: 'cf_user',

  get usuario() {
    try { return JSON.parse(localStorage.getItem(this._KEY)); } catch { return null; }
  },
  get logado() { return !!this.usuario; },

  salvar(u) {
    localStorage.setItem(this._KEY, JSON.stringify(u));
    renderNavAuth();
  },
  logout() {
    localStorage.removeItem(this._KEY);
    renderNavAuth();
    navigateTo('home');
  }
};
