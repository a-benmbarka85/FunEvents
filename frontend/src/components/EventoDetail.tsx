import { useEffect, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { getEvento, getTiposEntrada } from '../lib/api';
import { formatDate, formatPrice } from '../lib/format';
import type { Evento, TipoEntrada } from '../lib/types';

export function EventoDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [evento, setEvento] = useState<Evento | null>(null);
  const [tiposEntrada, setTiposEntrada] = useState<TipoEntrada[]>([]);
  const [tiposError, setTiposError] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) {
      setError('No se indicó el evento.');
      setLoading(false);
      return;
    }

    const eventoId = id;
    let cancelled = false;

    async function cargar() {
      const [eventoResult, tiposResult] = await Promise.allSettled([
        getEvento(eventoId),
        getTiposEntrada(),
      ]);

      if (cancelled) return;

      if (eventoResult.status === 'rejected') {
        console.error('No se pudo cargar el evento', eventoResult.reason);
        setError('No se pudo cargar el evento.');
      } else {
        setEvento(eventoResult.value);
      }

      if (tiposResult.status === 'fulfilled') {
        setTiposEntrada(tiposResult.value.filter((tipo) => tipo.eventoId === eventoId));
      } else {
        console.error('No se pudieron cargar los tipos de entrada', tiposResult.reason);
        setTiposError(true);
      }

      setLoading(false);
    }

    void cargar();

    return () => {
      cancelled = true;
    };
  }, [id]);

  const handleReservar = (tipoEntradaId: string) => {
    navigate(`/reservar?tipoEntradaId=${tipoEntradaId}`);
  };

  if (loading) {
    return (
      <div className="app-container">
        <div className="loading-skeleton">
          <div className="skeleton-row" />
          <div className="skeleton-row" />
          <div className="skeleton-row" />
        </div>
      </div>
    );
  }

  if (!evento) {
    return (
      <div className="app-container">
        <div className="card card--narrow">
          <h2 className="section-title">Evento no encontrado</h2>
          <p>{error ?? 'El evento solicitado no existe o no está disponible.'}</p>
          <button className="refresh-button" onClick={() => navigate('/')} type="button">
            Volver al inicio
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="app-container">
      <header className="app-header app-header--compact">
        <Link to="/" className="back-link">
          ← Volver a eventos
        </Link>
        <h1 className="app-title">{evento.title}</h1>
        <p className="app-subtitle">{formatDate(evento.date, 'long')}</p>
      </header>

      <main className="main-content main-content--start">
        <div className="card card--detail">
          <div className="section-header">
            <h2 className="section-title">Detalle del evento</h2>
          </div>

          <div className="event-card detail-summary">
            <p className="event-title">{evento.title}</p>
            <p><strong>Fecha:</strong> {formatDate(evento.date, 'long')}</p>
            <p><strong>Entradas disponibles:</strong> {evento.availableTickets}</p>
          </div>

          {tiposEntrada.length > 0 && (
            <>
              <div className="section-header section-header--spaced">
                <h2 className="section-title">Tipos de entrada disponibles</h2>
              </div>
              <div className="event-grid">
                {tiposEntrada.map((tipo) => (
                  <article key={tipo.id} className="event-card">
                    <h3 className="event-title">{tipo.nombre}</h3>
                    <p className="event-date">{formatPrice(tipo.precio)}</p>
                    {tipo.descripcion && <p>{tipo.descripcion}</p>}
                    <button
                      className="refresh-button"
                      onClick={() => handleReservar(tipo.id)}
                      type="button"
                    >
                      Seleccionar este tipo
                    </button>
                  </article>
                ))}
              </div>
            </>
          )}

          {tiposError && (
            <div className="error-message">
              <span>No se pudieron cargar los tipos de entrada.</span>
            </div>
          )}

          {!tiposError && tiposEntrada.length === 0 && (
            <div className="error-message">
              <span>No hay tipos de entrada disponibles para este evento.</span>
            </div>
          )}
        </div>
      </main>
    </div>
  );
}