import { useEffect, useState } from 'react';
import type { FormEvent } from 'react';
import { Link, useSearchParams } from 'react-router-dom';
import { createReserva, getColaboradores, getTipoEntrada, getUsuarios } from '../lib/api';
import { formatPrice } from '../lib/format';
import type { CanalVenta, Colaborador, TipoEntrada, Usuario } from '../lib/types';

const MAX_RETRIES = 4;

export function ReservaPage() {
  const [searchParams] = useSearchParams();
  const tipoEntradaId = searchParams.get('tipoEntradaId') ?? '';

  const [tipoEntrada, setTipoEntrada] = useState<TipoEntrada | null>(null);
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [colaboradores, setColaboradores] = useState<Colaborador[]>([]);
  const [ticketQuantity, setTicketQuantity] = useState(1);
  const [usuarioId, setUsuarioId] = useState('');
  const [colaboradorId, setColaboradorId] = useState('');
  const [canal, setCanal] = useState<CanalVenta>('Online');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);
  const [retryNotice, setRetryNotice] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    let cancelled = false;

    async function cargar() {
      const [tipoResult, usuariosResult, colaboradoresResult] = await Promise.allSettled([
        tipoEntradaId ? getTipoEntrada(tipoEntradaId) : Promise.resolve(null),
        getUsuarios(),
        getColaboradores(),
      ]);

      if (cancelled) return;

      if (tipoResult.status === 'fulfilled' && tipoResult.value) {
        setTipoEntrada(tipoResult.value);
      } else if (!tipoEntradaId) {
        setError('No se indicó el tipo de entrada.');
      } else if (tipoResult.status === 'rejected') {
        console.error('No se pudo cargar el tipo de entrada', tipoResult.reason);
        setError('No se pudo cargar el tipo de entrada seleccionado.');
      }

      if (usuariosResult.status === 'fulfilled') {
        setUsuarios(usuariosResult.value);
      } else {
        console.error('No se pudieron cargar los usuarios', usuariosResult.reason);
      }

      if (colaboradoresResult.status === 'fulfilled') {
        setColaboradores(colaboradoresResult.value);
      } else {
        console.error('No se pudieron cargar los colaboradores', colaboradoresResult.reason);
      }

      setLoading(false);
    }

    void cargar();

    return () => {
      cancelled = true;
    };
  }, [tipoEntradaId]);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (submitting) return;

    setError(null);
    setSuccess(null);

    if (!tipoEntrada) {
      setError('No se pudo cargar el tipo de entrada seleccionado.');
      return;
    }

    if (!usuarioId) {
      setError('Selecciona un usuario.');
      return;
    }

    if (ticketQuantity <= 0) {
      setError('La cantidad debe ser mayor a cero.');
      return;
    }

    if (canal === 'Colaborador' && !colaboradorId) {
      setError('Selecciona el colaborador que emite la venta.');
      return;
    }

    setSubmitting(true);

    try {
      const reserva = await createReserva(
        {
          eventoId: tipoEntrada.eventoId,
          usuarioId,
          tipoEntradaId: tipoEntrada.id,
          ticketQuantity,
          canal,
          colaboradorId: canal === 'Colaborador' ? colaboradorId : undefined,
        },
        {
          retries: MAX_RETRIES,
          baseDelayMs: 300,
          maxDelayMs: 2500,
          onRetry: (attempt, delayMs) =>
            setRetryNotice(
              `Otro cliente tomó las entradas primero. Reintentando (${attempt}/${MAX_RETRIES}) en ${delayMs} ms…`
            ),
        }
      );

      setSuccess(`Reserva creada con ID ${reserva.reservaId}.`);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'No se pudo crear la reserva.');
    } finally {
      setRetryNotice(null);
      setSubmitting(false);
    }
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

  const total = tipoEntrada ? ticketQuantity * tipoEntrada.precio : 0;

  return (
    <div className="app-container">
      <header className="app-header app-header--compact">
        <Link to="/" className="back-link">
          ← Volver a eventos
        </Link>
        <h1 className="app-title">Reservar entradas</h1>
      </header>

      <main className="main-content main-content--start">
        <div className="card card--detail">
          {error && (
            <div className="error-message">
              <span>{error}</span>
            </div>
          )}

          {retryNotice && (
            <div className="notice notice--warning">
              <span>{retryNotice}</span>
            </div>
          )}

          {success && (
            <div className="notice notice--success">
              <span>{success}</span>
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <div className="section-header">
              <h2 className="section-title">Datos de la reserva</h2>
            </div>

            {tipoEntrada && (
              <div className="summary-card">
                <h3 className="event-title">{tipoEntrada.nombre}</h3>
                <p><strong>Precio unitario:</strong> {formatPrice(tipoEntrada.precio)}</p>
                <p><strong>Cantidad:</strong> {ticketQuantity}</p>
                <p><strong>Total:</strong> {formatPrice(total)}</p>
              </div>
            )}

            <div className="field">
              <label className="field-label" htmlFor="usuario">Usuario</label>
              <select
                id="usuario"
                className="field-control"
                value={usuarioId}
                onChange={(event) => setUsuarioId(event.target.value)}
              >
                <option value="">Seleccionar usuario</option>
                {usuarios.map((usuario) => (
                  <option key={usuario.id} value={usuario.id}>
                    {usuario.nombre} ({usuario.email})
                  </option>
                ))}
              </select>
            </div>

            <div className="field">
              <label className="field-label" htmlFor="canal">Canal de venta</label>
              <select
                id="canal"
                className="field-control"
                value={canal}
                onChange={(event) => setCanal(event.target.value as CanalVenta)}
              >
                <option value="Online">Online (portal web)</option>
                <option value="Oficina">Oficina de atención al cliente</option>
                <option value="Colaborador">Colaborador (portal/POS)</option>
              </select>
            </div>

            {canal === 'Colaborador' && (
              <div className="field">
                <label className="field-label" htmlFor="colaborador">Colaborador</label>
                <select
                  id="colaborador"
                  className="field-control"
                  value={colaboradorId}
                  onChange={(event) => setColaboradorId(event.target.value)}
                >
                  <option value="">Seleccionar colaborador</option>
                  {colaboradores.map((colaborador) => (
                    <option key={colaborador.id} value={colaborador.id}>
                      {colaborador.name}
                    </option>
                  ))}
                </select>
              </div>
            )}

            <div className="field">
              <label className="field-label" htmlFor="cantidad">Cantidad de entradas</label>
              <input
                id="cantidad"
                type="number"
                min="1"
                className="field-control"
                value={ticketQuantity}
                onChange={(event) =>
                  setTicketQuantity(Math.max(1, Number.parseInt(event.target.value, 10) || 1))
                }
              />
            </div>

            <button
              className="refresh-button form-submit"
              type="submit"
              disabled={!usuarioId || !tipoEntrada || submitting}
            >
              {submitting ? 'Reservando…' : 'Confirmar reserva'}
            </button>
          </form>

          <div className="form-cancel">
            <Link to="/" className="refresh-button link-button">Cancelar</Link>
          </div>
        </div>
      </main>
    </div>
  );
}