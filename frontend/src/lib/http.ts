export class HttpError extends Error {
  readonly status: number;
  readonly code?: string;

  constructor(message: string, status: number, code?: string) {
    super(message);
    this.name = 'HttpError';
    this.status = status;
    this.code = code;
  }
}

export interface RetryOptions {
  retries?: number;
  baseDelayMs?: number;
  maxDelayMs?: number;
  onRetry?: (attempt: number, delayMs: number) => void;
  signal?: AbortSignal;
}

const CONCURRENCY_CODE = 'concurrency_conflict';

interface ApiErrorBody {
  message?: string;
  code?: string;
}

const sleep = (ms: number) => new Promise<void>((resolve) => setTimeout(resolve, ms));

async function readErrorBody(response: Response): Promise<ApiErrorBody | null> {
  try {
    return (await response.clone().json()) as ApiErrorBody;
  } catch {
    return null;
  }
}

export async function fetchWithRetry(
  input: RequestInfo | URL,
  init?: RequestInit,
  options: RetryOptions = {}
): Promise<Response> {
  const { retries = 3, baseDelayMs = 250, maxDelayMs = 2000, onRetry, signal } = options;

  for (let attempt = 0; ; attempt++) {
    const response = await fetch(input, { ...init, signal: signal ?? init?.signal });

    if (response.status !== 409 || attempt >= retries) return response;

    const body = await readErrorBody(response);
    if (body?.code !== undefined && body.code !== CONCURRENCY_CODE) return response;

    const exponential = Math.min(maxDelayMs, baseDelayMs * 2 ** attempt);
    const delay = Math.round(exponential * (0.5 + Math.random() * 0.5));
    onRetry?.(attempt + 1, delay);

    await sleep(delay);
  }
}

export async function ensureOk(response: Response): Promise<Response> {
  if (response.ok) return response;

  const body = await readErrorBody(response);
  throw new HttpError(
    body?.message ?? `Error HTTP ${response.status}`,
    response.status,
    body?.code
  );
}
