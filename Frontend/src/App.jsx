import { useState, useEffect } from 'react'
import axios from 'axios'
import './App.css'

function App() {
  const [message, setMessage] = useState('')
  const [status, setStatus] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const fetchHello = async () => {
    setLoading(true)
    setError('')
    try {
      const response = await axios.get('/api/hello')
      setMessage(response.data.message)
    } catch (err) {
      setError('Failed to fetch hello message')
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  const fetchStatus = async () => {
    setLoading(true)
    setError('')
    try {
      const response = await axios.get('/api/status')
      setStatus(response.data.status)
    } catch (err) {
      setError('Failed to fetch status')
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="app">
      <h1>My App - React + Vite</h1>
      <div className="controls">
        <button onClick={fetchHello} disabled={loading}>
          {loading ? 'Loading...' : 'Get Hello Message'}
        </button>
        <button onClick={fetchStatus} disabled={loading}>
          {loading ? 'Loading...' : 'Get Status'}
        </button>
      </div>
      {message && <p className="result">Message: {message}</p>}
      {status && <p className="result">Status: {status}</p>}
      {error && <p className="error">{error}</p>}
    </div>
  )
}

export default App
