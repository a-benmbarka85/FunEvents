import { useCallback, useEffect, useState } from 'react';
import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import aspireLogo from '/Aspire.png';
import './App.css';
import { EventoDetail } from './components/EventoDetail';
import { ReservaPage } from './components/ReservaPage';
import { getEventos } from './lib/api';
import { formatDate } from './lib/format';
import type { Evento } from './lib/types';

function EventList() {
  const [eventos, setEventos] = useState<Evento[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const cargarEventos = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      setEventos(await getEventos());
    } catch (err) {
      console.error('No se pudieron cargar los eventos', err);
      setError(err instanceof Error ? err.message : 'No se pudieron cargar los eventos.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void cargarEventos();
  }, [cargarEventos]);

  return (
    <div className="app-container">
      <header className="app-header">
        <a
          href="https://aspire.dev"
          target="_blank"
          rel="noopener noreferrer"
          className="logo-link"
        >
          <img src={aspireLogo} className="logo" alt="Aspire" />
        </a>
        <h1 className="app-title">FunEvents</h1>
        <p className="app-subtitle">Agenda de eventos</p>
      </header>

      <main className="main-content">
        <section className="events-section">
          <div className="card">
            <div className="section-header">
              <h2 className="section-title">Próximos eventos</h2>
              <button
                className="refresh-button"
                onClick={cargarEventos}
                disabled={loading}
                type="button"
              >
                {loading ? 'Cargando…' : 'Refrescar'}
              </button>
            </div>

            {error && (
              <div className="error-message">
                <span>{error}</span>
              </div>
            )}

            {loading && eventos.length === 0 && (
              <div className="loading-skeleton">
                {[...Array(5)].map((_, index) => (
                  <div key={index} className="skeleton-row" />
                ))}
              </div>
            )}

            {eventos.length > 0 && (
              <div className="event-grid event-grid--four">
                {eventos.map((evento) => (
                  <Link key={evento.id} to={`/eventos/${evento.id}`} className="event-link">
                    <article className="event-card">
                      <h3 className="event-date">
                        <time dateTime={evento.date}>{formatDate(evento.date)}</time>
                      </h3>
                      <p className="event-title">{evento.title}</p>
                      <div className="event-tickets">
                        <span className="ticket-value">{evento.availableTickets}</span>
                        <span className="ticket-unit">entradas</span>
                      </div>
                    </article>
                  </Link>
                ))}
              </div>
            )}
          </div>
        </section>
      </main>

      <footer className="app-footer">
        <nav>
          <a href="https://aspire.dev" target="_blank" rel="noopener noreferrer">
            Más información sobre Aspire
          </a>
          <a
            href="https://github.com/microsoft/aspire"
            target="_blank"
            rel="noopener noreferrer"
            className="github-link"
          >
            <img src="/github.svg" alt="" width="24" height="24" />
            <span className="visually-hidden">GitHub</span>
          </a>
        </nav>
      </footer>
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<EventList />} />
        <Route path="/eventos/:id" element={<EventoDetail />} />
        <Route path="/reservar" element={<ReservaPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;