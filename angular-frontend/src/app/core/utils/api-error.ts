import { HttpErrorResponse } from '@angular/common/http';

export function getApiErrorMessage(error: unknown, fallback: string): string {
    if (!(error instanceof HttpErrorResponse)) return fallback;

    const body = error.error;
    if (!body) return fallback;

    if (typeof body === 'string') return body;

    if (typeof body.message === 'string' && body.message.trim()) {
        return body.message;
    }

    if (body.errors && typeof body.errors === 'object') {
        const messages = Object.values(body.errors as Record<string, string[]>)
            .flat()
            .filter(Boolean);
        if (messages.length) return messages.join(' ');
    }

    if (typeof body.title === 'string' && body.title !== 'One or more validation errors occurred.') {
        return body.title;
    }

    return fallback;
}
