import { authService } from './services/authService.js';

const authSection = document.getElementById('auth-section');
const tasksSection = document.getElementById('tasks-section');
const loginForm = document.getElementById('login-form');
const logoutBtn = document.getElementById('logout-btn');
const authError = document.getElementById('auth-error');
const currentUserSpan = document.getElementById('current-user');
const usernameInput = document.getElementById('username');
const passwordInput = document.getElementById('password');

// Check if user is already logged in
function checkAuth() {
  const token = localStorage.getItem('token');
  if (token) {
    showTasksSection();
  } else {
    showLoginSection();
  }
}

function showLoginSection() {
  authSection.classList.add('active');
  tasksSection.classList.remove('active');
  loginForm.reset();
  authError.style.display = 'none';
}

function showTasksSection() {
  authSection.classList.remove('active');
  tasksSection.classList.add('active');
  const username = localStorage.getItem('username');
  currentUserSpan.textContent = `Welcome, ${username}!`;
}

function hideError() {
  authError.style.display = 'none';
}

function showError(message) {
  authError.textContent = message;
  authError.style.display = 'block';
  setTimeout(hideError, 5000);
}

async function handleLogin(e) {
  e.preventDefault();
  authError.style.display = 'none';

  const username = usernameInput.value.trim();
  const password = passwordInput.value;

  if (!username || !password) {
    showError('Username and password are required');
    return;
  }

  try {
    const { data, error } = await authService.login(username, password);

    if (error) {
      showError(error);
      return;
    }

    // Store token and username
    localStorage.setItem('token', data.token);
    localStorage.setItem('username', data.username);

    // Show tasks section
    showTasksSection();
  } catch (err) {
    showError('Failed to login. Please try again.');
    console.error('Login error:', err);
  }
}

function handleLogout() {
  localStorage.removeItem('token');
  localStorage.removeItem('username');
  showLoginSection();
}

// Event listeners
loginForm.addEventListener('submit', handleLogin);
logoutBtn.addEventListener('click', handleLogout);

// Initialize
checkAuth();
