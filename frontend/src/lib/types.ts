export interface Evento {
  id: string;
  title: string;
  date: string;
  availableTickets: number;
}

export interface TipoEntrada {
  id: string;
  eventoId: string;
  nombre: string;
  precio: number;
  descripcion?: string;
}

export interface Usuario {
  id: string;
  nombre: string;
  email: string;
  rol: string;
}

export interface Colaborador {
  id: string;
  name: string;
  email: string;
}

export type CanalVenta = 'Online' | 'Oficina' | 'Colaborador';

export interface ReservaCreada {
  reservaId: string;
  eventoId: string;
  usuarioId: string;
  tipoEntradaId: string;
  quantityReserved: number;
}
