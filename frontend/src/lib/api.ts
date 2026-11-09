import { ensureOk, fetchWithRetry } from './http';
import type { RetryOptions } from './http';
import type {
  CanalVenta,
  Colaborador,
  Evento,
  ReservaCreada,
  TipoEntrada,
  Usuario,
} from './types';

async function getJson<T>(path: string): Promise<T> {
  const response = await ensureOk(await fetch(path));
  return (await response.json()) as T;
}

export const getEventos = () => getJson<Evento[]>('/api/eventos');

export const getEvento = (id: string) =>
  getJson<Evento>(`/api/eventos/${encodeURIComponent(id)}`);

export const getTiposEntrada = () => getJson<TipoEntrada[]>('/api/tipos-entrada');

export const getTipoEntrada = (id: string) =>
  getJson<TipoEntrada>(`/api/tipos-entrada/${encodeURIComponent(id)}`);

export const getUsuarios = () => getJson<Usuario[]>('/api/usuarios');

export const getColaboradores = () => getJson<Colaborador[]>('/api/colaboradores');

export interface CreateReservaInput {
  eventoId: string;
  usuarioId: string;
  tipoEntradaId: string;
  ticketQuantity: number;
  canal: CanalVenta;
  colaboradorId?: string;
}

export async function createReserva(
  input: CreateReservaInput,
  options?: RetryOptions
): Promise<ReservaCreada> {
  const response = await fetchWithRetry(
    '/api/reservations',
    {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(input),
    },
    options
  );

  return (await (await ensureOk(response)).json()) as ReservaCreada;
}
