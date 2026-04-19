(function(){
  const STORAGE_KEY = 'tinyshop-theme';
  function applyTheme(theme) {
    if (theme === 'dark') document.documentElement.setAttribute('data-theme','dark');
    else document.documentElement.removeAttribute('data-theme');
  }

  function initTheme(){
    const stored = localStorage.getItem(STORAGE_KEY);
    if(stored){ applyTheme(stored); return; }
    // respect system preference
    const prefersDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    applyTheme(prefersDark ? 'dark' : 'light');
  }

  function toggleTheme(){
    const isDark = document.documentElement.getAttribute('data-theme') === 'dark';
    const next = isDark ? 'light' : 'dark';
    // add a temporary class to allow transitions
    document.documentElement.classList.add('theme-transition');
    applyTheme(next);
    localStorage.setItem(STORAGE_KEY, next);
    window.setTimeout(()=> document.documentElement.classList.remove('theme-transition'), 300);
  }

  window.toggleTheme = toggleTheme;
  window.initTheme = initTheme;

  // auto init
  if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', initTheme);
  else initTheme();
})();
