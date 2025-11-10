const isPlainObject = (value: unknown): value is Record<string, unknown> =>
  typeof value === 'object' && value !== null && !Array.isArray(value) && !(value instanceof Date);

const formatValue = (value: unknown): string => {
  if (value instanceof Date) {
    return value.toISOString();
  }

  switch (typeof value) {
    case 'boolean':
      return value ? 'true' : 'false';
    case 'number':
    case 'bigint':
      return Number.isFinite(Number(value)) ? String(value) : '';
    case 'string':
      return value;
    default:
      return value != null ? String(value) : '';
  }
};

const appendPair = (segments: string[], key: string, rawValue: unknown): void => {
  if (rawValue === null || rawValue === undefined) {
    return;
  }

  if (Array.isArray(rawValue)) {
    const joined = rawValue
      .map(formatValue)
      .filter(value => value.length > 0)
      .join(',');

    if (joined.length > 0) {
      segments.push(`${encodeURIComponent(key)}=${encodeURIComponent(joined)}`);
    }

    return;
  }

  if (isPlainObject(rawValue)) {
    writeObject(rawValue, key, segments);
    return;
  }

  const formatted = formatValue(rawValue);

  if (formatted.length > 0) {
    segments.push(`${encodeURIComponent(key)}=${encodeURIComponent(formatted)}`);
  }
};

const writeObject = (
  value: Record<string, unknown>,
  prefix: string | null,
  segments: string[]
): void => {
  for (const [key, child] of Object.entries(value)) {
    if (child === undefined || child === null) {
      continue;
    }

    const composedKey = prefix ? `${prefix}_${key}` : key;
    appendPair(segments, composedKey, child);
  }
};

/**
 * Flattens an object graph into a query string, mirroring the C# string extension.
 */
export const toQueryString = (input: unknown): string => {
  if (input === null || typeof input !== 'object') {
    return '';
  }

  const segments: string[] = [];
  writeObject(input as Record<string, unknown>, null, segments);
  return segments.join('&');
};
