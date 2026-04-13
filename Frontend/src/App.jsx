import { useEffect, useState } from 'react'
import { fetchMenu } from './api/menuApi'
import './App.css'

function App() {
  const [menu, setMenu] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const controller = new AbortController()

    async function loadMenu() {
      try {
        setLoading(true)
        setError('')

          const data = await fetchMenu(controller.signal)
          setMenu(Array.isArray(data) ? data : [])
      } catch (err) {
        if (err.name !== 'AbortError') {
          setError(err.message || 'Unable to connect to backend API')
        }
      } finally {
        setLoading(false)
      }
    }

    loadMenu()
    return () => controller.abort()
  }, [])

  return (
    <main className="app">
      <h1>Menu</h1>
      <p className="subtitle">Loaded from ASP.NET API: /api/menu</p>

      <section className="card" aria-live="polite">
        {loading && <p>Loading menu...</p>}

        {!loading && !error && (
          <div className="menu-grid">
            {menu.length === 0 && <p>No menu categories found.</p>}

            {menu.map((category) => (
              <article className="category" key={category.id}>
                <h2>{category.name}</h2>

                {category.items?.length ? (
                  <ul className="items">
                    {category.items.map((item) => (
                      <li key={item.id} className="item">
                        <div className="item-line">
                          <strong>{item.name}</strong>
                          <span>${Number(item.price).toFixed(2)}</span>
                        </div>
                        <p>{item.description}</p>
                        {!item.isAvailable && <small>Unavailable</small>}
                      </li>
                    ))}
                  </ul>
                ) : (
                  <p>No items in this category.</p>
                )}
              </article>
            ))}
          </div>
        )}

        {!loading && error && (
          <>
            <p className="error">{error}</p>
            <p>
              Tip: set <code>VITE_API_BASE_URL</code> if your backend runs on a
              different origin during development.
            </p>
          </>
        )}
      </section>
    </main>
  )
}

export default App
