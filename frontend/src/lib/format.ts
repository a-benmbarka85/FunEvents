const shortDate = new Intl.DateTimeFormat('es-ES', {
  weekday: 'short',
  month: 'short',
  day: 'numeric',
});

const longDate = new Intl.DateTimeFormat('es-ES', {
  weekday: 'long',
  year: 'numeric',
  month: 'long',
  day: 'numeric',
});

const price = new Intl.NumberFormat('es-ES', {
  minimumFractionDigits: 2,
  maximumFractionDigits: 2,
});

export function formatDate(value: string, style: 'short' | 'long' = 'short'): string {
  return (style === 'long' ? longDate : shortDate).format(new Date(value));
}

export function formatPrice(value: number): string {
  return `$${price.format(value)}`;
}
