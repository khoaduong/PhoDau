# Frontend (React + Vite)

## Requirements

- Node.js 18+

## Run

```bash
npm install
npm run dev
```

## Build

```bash
npm run build
npm run preview
```

## API Configuration

The frontend calls the backend using `VITE_API_BASE_URL`.

- Default: `http://localhost:5000`
- Example: `VITE_API_BASE_URL=https://localhost:5001`

You can set this in a local environment file such as `.env.local`.

## Source Conventions

- API modules are under `src/api/`
- React components are under `src/components/`
- Client state hooks are under `src/state/`
- App entrypoint is `src/main.jsx`
