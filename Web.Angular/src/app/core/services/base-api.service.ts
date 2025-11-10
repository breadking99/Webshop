import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Response } from '../../shared/responses/response';
import { ResponseStatus } from '../../shared/enums/response-status';

export abstract class BaseApiService {
  protected readonly baseUrl = environment.apiUrl;

  protected constructor(protected readonly http: HttpClient) { }

  protected toResponse<T>(source: Observable<HttpResponse<T>>): Observable<Response<T>> {
    return source.pipe(
      map(({ status, body }) => ({
        statusCode: status,
        value: body ?? undefined
      })),
      catchError(error => of(this.handleError<T>(error)))
    );
  }

  private handleError<T>(error: unknown): Response<T> {
    if (error instanceof HttpErrorResponse) {
      const message = this.extractMessage(error.error) || error.message;
      return {
        statusCode: error.status || ResponseStatus.UnknownError,
        message
      };
    }

    return {
      statusCode: ResponseStatus.UnknownError,
      message: 'An unexpected error occurred.'
    };
  }

  private extractMessage(content: unknown): string | null {
    if (!content) {
      return null;
    }

    if (typeof content === 'string') {
      return content;
    }

    if (typeof content === 'object' && 'message' in content && typeof content.message === 'string') {
      return content.message;
    }

    return null;
  }
}
